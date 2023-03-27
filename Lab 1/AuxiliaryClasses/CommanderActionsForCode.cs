using Lab_1.ViewInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab_1.AuxiliaryClasses
{
    public class CommanderActionsForCode : IActionsOnCode, IActionsOnStack
    {
        private TextBox textBox;
        private string previousText = "";
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        public CommanderActionsForCode(ref TextBox textBoxFromCodeEditor)
        {
            textBox = textBoxFromCodeEditor;
            previousText = textBox.Text;
            textBox.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!undoStack.Contains(textBox.Text))
            {
                redoStack.Clear();
                undoStack.Push(previousText);
                previousText = textBox.Text;
            }
        }

        public void UndoActions()
        {
            if (undoStack.Count > 0)
            {
                var lastState = undoStack.Pop();
                textBox.TextChanged -= OnTextChanged;
                redoStack.Push(textBox.Text);
                textBox.Text = lastState;
                textBox.TextChanged += OnTextChanged;
                previousText = textBox.Text;
            }
        }

        public void RedoActions()
        {
            if (redoStack.Count > 0)
            {
                var nextState = redoStack.Pop();
                textBox.TextChanged -= OnTextChanged;
                undoStack.Push(textBox.Text);
                textBox.Text = nextState;
                textBox.TextChanged += OnTextChanged;
                previousText = textBox.Text;
            }
        }

        public void CopyAction()
        {
            if (textBox.SelectionLength > 0)
            {
                textBox.Copy();
            }
        }

        public void CutAction()
        {
            if (textBox.SelectionLength > 0)
            {
                undoStack.Push(textBox.Text);
                redoStack.Clear();
                textBox.Cut();
            }
        }

        public void DeleteAction()
        {
            if (textBox.SelectionLength > 0)
            {
                undoStack.Push(textBox.Text);
                redoStack.Clear();
                textBox.SelectedText = "";
            }
        }

        public void InsertAction()
        {
            undoStack.Push(textBox.Text);
            redoStack.Clear();
            textBox.Paste();
        }

        public void SelectAllAction()
        {
            textBox.SelectAll();
        }
    }
}
