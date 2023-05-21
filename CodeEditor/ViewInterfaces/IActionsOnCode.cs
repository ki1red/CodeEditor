using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CodeEditor.ViewInterfaces
{
    internal interface IActionsOnCode
    {
        void CutAction();
        void CopyAction();
        void DeleteAction();
        void InsertAction();
        void SelectAllAction();
    }
}
