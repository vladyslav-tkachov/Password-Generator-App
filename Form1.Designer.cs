namespace PasswordGeneratorApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Panel panelOutput;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelSpacer;

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonClear;

        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonCopy;

        private System.Windows.Forms.CheckBox chkReveal;
        private System.Windows.Forms.FlowLayoutPanel actionPanel;
        private System.Windows.Forms.Button button1;     // Generate / Lookup
        private System.Windows.Forms.Button buttonSave;  // Save (create mode only)

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolTip toolTip2;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ------- Root form -------
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;   // DPI-aware sizing
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Text = "Password Generator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.BackColor = System.Drawing.Color.White;

            // Compact default that fits everything; minimum guards against squeeze
            this.ClientSize = new System.Drawing.Size(520, 230);
            this.MinimumSize = new System.Drawing.Size(520, 250);

            // ------- ToolTips -------
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);

            // ------- StatusStrip -------
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SizingGrip = false;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.statusLabel });
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Ready";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;

            // ===== Row 1: Input (AutoSize, no fixed height) =====
            this.panelInput = new System.Windows.Forms.Panel();
            this.panelInput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInput.AutoSize = true;
            this.panelInput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelInput.Padding = new System.Windows.Forms.Padding(12, 10, 12, 4);

            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox1.PlaceholderText = "Enter length (8–128) or a keyword";
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.textBox1.Location = new System.Drawing.Point(12, 10);
            this.textBox1.Width = 380; // width is recalculated by anchor on resize
            this.textBox1.TabIndex = 0;

            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonClear.Text = "Clear";
            this.buttonClear.AutoSize = true;                                // let it size to text/DPI
            this.buttonClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClear.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.buttonClear.Location = new System.Drawing.Point(0, 8);       // x is aligned on Resize
            this.buttonClear.Click += new System.EventHandler(this.ButtonClear_Click);
            this.toolTip1.SetToolTip(this.buttonClear, "Clear fields");

            this.panelInput.Resize += (s, e) =>
            {
                this.buttonClear.Left = this.panelInput.ClientSize.Width - 12 - this.buttonClear.Width;
                this.textBox1.Width = this.buttonClear.Left - 12 - this.textBox1.Left;
            };

            this.panelInput.Controls.Add(this.textBox1);
            this.panelInput.Controls.Add(this.buttonClear);

            // ===== Row 2: Output =====
            this.panelOutput = new System.Windows.Forms.Panel();
            this.panelOutput.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOutput.AutoSize = true;
            this.panelOutput.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelOutput.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);

            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox2.ReadOnly = true;
            this.textBox2.UseSystemPasswordChar = true;
            this.textBox2.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.textBox2.Location = new System.Drawing.Point(12, 6);
            this.textBox2.Width = 380;
            this.textBox2.TabIndex = 1;

            this.buttonCopy = new System.Windows.Forms.Button();
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.AutoSize = true;                                  // DPI-safe
            this.buttonCopy.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonCopy.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.buttonCopy.Location = new System.Drawing.Point(0, 4);
            this.buttonCopy.Click += new System.EventHandler(this.ButtonCopy_Click);
            this.toolTip2.SetToolTip(this.buttonCopy, "Copy password to clipboard");

            this.panelOutput.Resize += (s, e) =>
            {
                this.buttonCopy.Left = this.panelOutput.ClientSize.Width - 12 - this.buttonCopy.Width;
                this.textBox2.Width = this.buttonCopy.Left - 12 - this.textBox2.Left;
            };

            this.panelOutput.Controls.Add(this.textBox2);
            this.panelOutput.Controls.Add(this.buttonCopy);

            // ===== Row 3: Bottom (Reveal + Actions) =====
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBottom.AutoSize = true;
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.Padding = new System.Windows.Forms.Padding(12, 2, 12, 6);

            this.chkReveal = new System.Windows.Forms.CheckBox();
            this.chkReveal.Text = "Show";
            this.chkReveal.AutoSize = true;
            this.chkReveal.Location = new System.Drawing.Point(12, 8);
            this.chkReveal.TabIndex = 2;

            this.actionPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.actionPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.actionPanel.WrapContents = false;
            this.actionPanel.AutoSize = true;                                 // grow to content (buttons)
            this.actionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.actionPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;

            this.button1 = new System.Windows.Forms.Button();
            this.button1.Text = "Generate";
            this.button1.AutoSize = true;                                     // DPI-safe
            this.button1.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.button1.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.button1.TabIndex = 3;
            this.button1.Click += new System.EventHandler(this.Button1_Click);

            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonSave.Text = "Save";
            this.buttonSave.AutoSize = true;                                  // DPI-safe
            this.buttonSave.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            this.buttonSave.Visible = false;
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);

            this.actionPanel.Controls.Add(this.button1);
            this.actionPanel.Controls.Add(this.buttonSave);

            this.panelBottom.Controls.Add(this.chkReveal);
            this.panelBottom.Controls.Add(this.actionPanel);

            this.panelBottom.Resize += (s, e) =>
            {
                // right-align the action panel within bottom row
                this.actionPanel.Left = this.panelBottom.ClientSize.Width - 12 - this.actionPanel.Width;
                this.actionPanel.Top = 4; // small top padding
            };

            // ===== Flexible spacer (fills remaining space) =====
            this.panelSpacer = new System.Windows.Forms.Panel();
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Fill;

            // ------- Compose -------
            this.Controls.Add(this.panelSpacer);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelOutput);
            this.Controls.Add(this.panelInput);
            this.Controls.Add(this.statusStrip);

            // Enter triggers Generate/Lookup
            this.AcceptButton = this.button1;
        }
    }
}
