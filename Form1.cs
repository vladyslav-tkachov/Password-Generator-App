using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace PasswordGeneratorApp
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<string, string> vtspDict =
            new Dictionary<string, string> { };

        private const string StoreFileName = "vault.dat";
        private readonly string _storePath;

        public Form1()
        {
            InitializeComponent();

            _storePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "PasswordGeneratorApp",
                StoreFileName);

            BootstrapData();
            WireUiState();
        }

        private void BootstrapData()
        {
            var seeded = new Dictionary<string, string>
            {
                { "defaultSecret", "QMlob&y%138Sls74e1hoy9sQa$" }
            };

            foreach (var kv in seeded)
                if (!vtspDict.ContainsKey(kv.Key))
                    vtspDict[kv.Key] = kv.Value;

            try
            {
                if (File.Exists(_storePath))
                {
                    var json = DecryptString(File.ReadAllBytes(_storePath));
                    var fromDisk = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (fromDisk != null)
                    {
                        foreach (var kv in fromDisk)
                            vtspDict[kv.Key] = kv.Value;
                    }
                }
            }
            catch
            {
                SetStatus("Local vault could not be read. Continuing with defaults.", isWarning: true);
            }
        }

        private void WireUiState()
        {
            textBox2.ReadOnly = true;
            textBox2.UseSystemPasswordChar = true;
            buttonSave.Visible = false;
            chkReveal.CheckedChanged += (s, e) =>
            {
                textBox2.UseSystemPasswordChar = !chkReveal.Checked;
            };

            this.AcceptButton = button1;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            ResetCreateMode();
            var input = (textBox1.Text ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Enter a length (8–128) or a keyword.", "Input Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (vtspDict.ContainsKey(input))
            {
                textBox2.Text = vtspDict[input];
                SetStatus($"Loaded saved password for “{input}”.");
                return;
            }

            if (int.TryParse(input, out int length))
            {
                if (length < 8 || length > 128)
                {
                    MessageBox.Show("Password length must be between 8 and 128 characters!",
                        "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                textBox2.Text = GeneratePassword(length);
                SetStatus($"Generated {length}-char password.");
                return;
            }

            EnterCreateModeFor(input);
        }

        private void EnterCreateModeFor(string key)
        {
            textBox2.ReadOnly = false;
            textBox2.Clear();
            textBox2.PlaceholderText = $"Type password to save for key “{key}”";
            buttonSave.Tag = key;
            buttonSave.Visible = true;
            textBox2.Focus();
            SetStatus($"“{key}” not found. Enter a password and click Save.");
        }

        private void ResetCreateMode()
        {
            textBox2.ReadOnly = true;
            buttonSave.Visible = false;
            buttonSave.Tag = null;
            textBox2.PlaceholderText = string.Empty;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            var key = buttonSave.Tag as string;
            if (string.IsNullOrWhiteSpace(key))
                return;

            var value = (textBox2.Text ?? string.Empty).Trim();
            if (value.Length < 8)
            {
                MessageBox.Show("Minimum 8 characters for a saved password.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            var overwriting = vtspDict.ContainsKey(key);
            vtspDict[key] = value;

            try
            {
                PersistStore();
                SetStatus(overwriting
                    ? $"Updated saved password for “{key}”."
                    : $"Saved new password for “{key}”.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not save. {ex.Message}", "Persistence Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ResetCreateMode();
            textBox2.ReadOnly = true;
        }

        private void PersistStore()
        {
            var dir = Path.GetDirectoryName(_storePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            var json = JsonSerializer.Serialize(vtspDict);
            var encrypted = EncryptString(json);
            File.WriteAllBytes(_storePath, encrypted);
        }

        private string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int idx = RandomNumberGenerator.GetInt32(chars.Length);
                sb.Append(chars[idx]);
            }
            return sb.ToString();
        }


        private void ButtonCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                Clipboard.SetText(textBox2.Text);
                SetStatus("Copied to clipboard.");
            }
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            ResetCreateMode();
            SetStatus("Cleared.");
        }

        private void SetStatus(string message, bool isWarning = false)
        {
            statusLabel.Text = message;
            statusLabel.ForeColor = isWarning ? System.Drawing.Color.DarkGoldenrod : System.Drawing.Color.DimGray;
        }

        private static byte[] EncryptString(string plaintext)
        {
            var data = Encoding.UTF8.GetBytes(plaintext);
            var entropy = Encoding.UTF8.GetBytes("PasswordGeneratorApp::v1");
            return ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
        }

        private static string DecryptString(byte[] ciphertext)
        {
            var entropy = Encoding.UTF8.GetBytes("PasswordGeneratorApp::v1");
            var data = ProtectedData.Unprotect(ciphertext, entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(data);
        }
    }
}
