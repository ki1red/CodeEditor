using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor.ViewInterfaces
{
    internal interface IActionsOnStack
    {
        void UndoActions();
        void RedoActions();
    }
}
