using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab_1.ViewInterfaces
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
