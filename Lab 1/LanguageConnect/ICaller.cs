using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab_1.LanguageConnect
{
    internal interface ICaller
    {
        string GetResultCompileCode();
        Dictionary<string, Color> GetColorsForCode();
    }
}
