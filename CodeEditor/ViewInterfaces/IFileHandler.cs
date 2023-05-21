using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor.ViewInterfaces
{
    public interface IFileHandler
    {
        bool CreateFile();
        bool OpenFile();
        bool SaveFile(string data);
        bool SaveAsFile(string data);
    }
}
