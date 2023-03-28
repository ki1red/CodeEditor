using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media;

namespace Lab_1.LanguageConnect
{
    internal class LanguageConnector : ICaller
    {
        private Process process;
        private string textCode;
        private string pathToApp;
        private static Dictionary<string, char> flags = new Dictionary<string, char> { {"chech errors",'e'},{"compile and read",'c'},{"get golors for code",'g'} };

        public LanguageConnector(string pathToCompilerApp)
        {
            this.pathToApp = pathToCompilerApp;

            process = new Process();
        }

        public string GetResultCompileCode()
        {
            SetSettingsForProcess();
            process.StartInfo.Arguments = $"{flags["compile and read"]} \"{textCode}\"";
            process.Start();

            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        public Dictionary<string, Color> GetColorsForCode()
        {
            throw new NotImplementedException();
        }

        public void UpdateTextCode(string textCodeFromCodeEditor)
        {
            textCode = textCodeFromCodeEditor;
        }

        private void SetSettingsForProcess()
        {
            process.StartInfo.FileName = pathToApp;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
        }
    }
}
