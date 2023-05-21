using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media;
using System.IO;

namespace CodeEditor.LanguageConnect
{
    internal class LanguageConnector
    {
        private Process process;
        private string pathToApp;

        public LanguageConnector(string pathToCompilerApp)
        {
            this.pathToApp = pathToCompilerApp;

            process = new Process();
        }

        public string GetResultCompileCode(string nameFile)
        {
            SetSettingsForProcess();
            process.StartInfo.Arguments = $"-c {nameFile}";
            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
        private void SetSettingsForProcess()
        {
            process.StartInfo.FileName = pathToApp;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
        }
    }
}
