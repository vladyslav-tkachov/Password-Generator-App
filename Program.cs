using System;
using System.IO;

namespace PasswordGeneratorApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                File.WriteAllText("PasswordGeneratorApp_Error.log", ex.ToString());
            }
        }
    }
}
