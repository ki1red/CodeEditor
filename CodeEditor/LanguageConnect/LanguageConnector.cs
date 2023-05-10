using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Media;
using System.IO;

namespace Lab_1.LanguageConnect
{
    internal class LanguageConnector : ICaller
    {
        private Process process;
        private string textCode;
        private string pathToApp;
        private static Dictionary<string, char> flags = new Dictionary<string, char> { {"chech errors",'e'},{"compile and read",'c'},{"get golors for code",'g'} };
        //private static string pathToColors;

        public LanguageConnector(string pathToCompilerApp)
        {
            this.pathToApp = pathToCompilerApp;
            //pathToColors = pathToColorsFile;

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

        //public Dictionary<string, Color> GetColorsForCode()
        //{
        //    SetSettingsForProcess();
        //    process.StartInfo.Arguments = $"{flags["get golors for code"]} \"{textCode}\"";
        //    process.Start();
        //    string result = process.StandardOutput.ReadToEnd();
        //    process.WaitForExit();

        //    Dictionary<string, string> nameAndType = GetNamesAndTypes(result);

        //    if (nameAndType == null)
        //        return null;

        //    string colors = File.ReadAllText(pathToColors);
        //    Dictionary<string, Color> typeAndColor = GetTypesAndColors(colors);

        //    Dictionary<string, Color> nameAndColor = new Dictionary<string, Color>();
        //    foreach (string name in nameAndType.Keys)
        //    {
        //        string type = nameAndType[name];
        //        Color color = typeAndColor[type];
        //        nameAndColor.Add(name, color);
        //    }
        //    return nameAndColor;
        //}

        public void UpdateTextCode(string textCodeFromCodeEditor)
        {
            textCode = textCodeFromCodeEditor;
        }

        private Dictionary<string, Color> GetTypesAndColors(string textFromFile)
        {
            Dictionary<string, Color> typeAndColor = new Dictionary<string, Color>();

            string[] lines = textFromFile.Split('\n');
            foreach (string line in lines)
            {
                string[] trgb = line.Split(' '); // TYPE RED GREEN BLUE

                string type = trgb[0];
                byte red = Convert.ToByte(trgb[1]);
                byte green = Convert.ToByte(trgb[2]);
                byte blue = Convert.ToByte(trgb[3]);
                Color color = Color.FromArgb(255, red, green, blue);

                typeAndColor.Add(type, color);
            }

            return typeAndColor;
        }
        private Dictionary<string, string> GetNamesAndTypes(string textFromApp)
        {
            textFromApp = textFromApp.Replace("\r\n", string.Empty);
            if (textFromApp.Length == 0) return null;

            Dictionary<string, string> nameAndType = new Dictionary<string, string>();

            string[] lines = textFromApp.Split('\n');
            foreach (string line in lines)
            {
                string[] nt = line.Split(' '); // NAME TYPE

                string name = nt[0];
                if (!nameAndType.ContainsKey(name))
                {
                    string type = nt[1];
                    nameAndType.Add(name, type);
                }
            }

            return nameAndType;
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
