﻿//using Lab_1.ViewInterfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Controls;

//namespace Lab_1.AuxiliaryClasses
//{
//    public class CommanderActionsForCode : IActionsOnCode, IActionsOnStack
//    {
//        private ColoredTextBox textBox;
//        private string previousText = "";
//        private Stack<string> undoStack = new Stack<string>();
//        private Stack<string> redoStack = new Stack<string>();

//        public CommanderActionsForCode(ref ColoredTextBox textBoxFromCodeEditor)
//        {
//            textBox = textBoxFromCodeEditor;
//            previousText = textBox.Text;
//            textBox.TextChanged += OnTextChanged;
//        }

//        private void OnTextChanged(object sender, TextChangedEventArgs e)
//        {
//            if (!undoStack.Contains(textBox.Text))
//            {
//                redoStack.Clear();
//                undoStack.Push(previousText);
//                previousText = textBox.Text;
//            }
//        }

//        public void UndoActions()
//        {
//            if (undoStack.Count > 0)
//            {
//                int positionCursorInString = textBox.CaretIndex;

//                var lastState = undoStack.Pop();
//                textBox.TextChanged -= OnTextChanged;
//                redoStack.Push(textBox.Text);
//                textBox.Text = lastState;
//                textBox.TextChanged += OnTextChanged;
//                previousText = textBox.Text;

//                textBox.CaretIndex = positionCursorInString;
//            }
//        }

//        public void RedoActions()
//        {
//            if (redoStack.Count > 0)
//            {
//                int positionCursorInString = textBox.CaretIndex;

//                var nextState = redoStack.Pop();
//                textBox.TextChanged -= OnTextChanged;
//                undoStack.Push(textBox.Text);
//                textBox.Text = nextState;
//                textBox.TextChanged += OnTextChanged;
//                previousText = textBox.Text;

//                textBox.CaretIndex = positionCursorInString;
//            }
//        }

//        public void CopyAction()
//        {
//            if (textBox.SelectionLength > 0)
//            {
//                textBox.Copy();
//            }
//        }

//        public void CutAction()
//        {
//            if (textBox.SelectionLength > 0)
//            {
//                undoStack.Push(textBox.Text);
//                redoStack.Clear();
//                textBox.Cut();
//            }
//        }

//        public void DeleteAction()
//        {
//            if (textBox.SelectionLength > 0)
//            {
//                undoStack.Push(textBox.Text);
//                redoStack.Clear();
//                textBox.SelectedText = "";
//            }
//            else if (textBox.CaretIndex != textBox.Text.Length)
//            {
//                undoStack.Push(textBox.Text);
//                redoStack.Clear();


//                int positionCursorInString = textBox.CaretIndex;
//                int indexTheElement = positionCursorInString;
//                string originalString = textBox.Text;

//                textBox.Text = originalString.Remove(indexTheElement, 1);
//                textBox.CaretIndex = positionCursorInString;
//            }
//        }

//        public void BackspaceAction()
//        {
//            if (textBox.SelectionLength > 0)
//            {
//                undoStack.Push(textBox.Text);
//                redoStack.Clear();
//                RichTextBox t = new RichTextBox();

//                textBox.SelectedText = "";
//            }
//            else if (textBox.CaretIndex != 0)
//            {
//                undoStack.Push(textBox.Text);
//                redoStack.Clear();


//                int positionCursorInString = textBox.CaretIndex;
//                int indexTheElement = positionCursorInString - 1;
//                string originalString = textBox.Text;

//                textBox.Text = originalString.Remove(indexTheElement, 1);
//                textBox.CaretIndex = positionCursorInString - 1;
//            }
//        }

//        public void InsertAction()
//        {
//            undoStack.Push(textBox.Text);
//            redoStack.Clear();
//            textBox.Paste();
//        }

//        public void SelectAllAction()
//        {
//            textBox.SelectAll();
//        }
//    }

//}
using Lab_1.ViewInterfaces;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Lab_1.AuxiliaryClasses
{
    public class CommanderActionsForCode : IActionsOnCode, IActionsOnStack
    {
        private TextBox textBox;
        private TextInMomentTime previousText;
        private Stack<TextInMomentTime> undoStack = new Stack<TextInMomentTime>();
        private Stack<TextInMomentTime> redoStack = new Stack<TextInMomentTime>();

        public CommanderActionsForCode(ref TextBox textBoxFromCodeEditor)
        {
            textBox = textBoxFromCodeEditor;
            previousText = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
            textBox.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
            if (!undoStack.Contains(textInMomentTime))
            {
                redoStack.Clear();
                undoStack.Push(previousText);
                previousText = textInMomentTime;
            }
        }

        public void UndoActions()
        {
            if (undoStack.Count > 0)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                int positionCursorInString = textBox.CaretIndex;

                var lastState = undoStack.Pop();
                textBox.TextChanged -= OnTextChanged;
                redoStack.Push(textInMomentTime);
                textBox.Text = lastState.Text;
                textBox.CaretIndex = lastState.CursorPosition;
                textBox.TextChanged += OnTextChanged;
                previousText = textInMomentTime;
            }
        }

        public void RedoActions()
        {
            if (redoStack.Count > 0)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                int positionCursorInString = textBox.CaretIndex;

                var nextState = redoStack.Pop();
                textBox.TextChanged -= OnTextChanged;
                undoStack.Push(textInMomentTime);
                textBox.Text = nextState.Text;
                textBox.CaretIndex = nextState.CursorPosition;
                textBox.TextChanged += OnTextChanged;
                previousText = textInMomentTime;
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
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                undoStack.Push(textInMomentTime);
                redoStack.Clear();
                textBox.Cut();
            }
        }

        public void DeleteAction()
        {
            if (textBox.SelectionLength > 0)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                undoStack.Push(textInMomentTime);
                redoStack.Clear();
                textBox.SelectedText = "";
            }
            else if (textBox.CaretIndex != textBox.Text.Length)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                undoStack.Push(textInMomentTime);
                redoStack.Clear();


                int positionCursorInString = textBox.CaretIndex;
                int indexTheElement = positionCursorInString;
                string originalString = textBox.Text;

                textBox.Text = originalString.Remove(indexTheElement, 1);
                textBox.CaretIndex = positionCursorInString;
            }
        }

        public void BackspaceAction()
        {
            if (textBox.SelectionLength > 0)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                undoStack.Push(textInMomentTime);
                redoStack.Clear();

                textBox.SelectedText = "";
            }
            else if (textBox.CaretIndex != 0)
            {
                TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
                undoStack.Push(textInMomentTime);
                redoStack.Clear();


                int positionCursorInString = textBox.CaretIndex;
                int indexTheElement = positionCursorInString - 1;
                string originalString = textBox.Text;

                textBox.Text = originalString.Remove(indexTheElement, 1);
                textBox.CaretIndex = positionCursorInString - 1;
            }
        }

        public void InsertAction()
        {
            TextInMomentTime textInMomentTime = new TextInMomentTime(textBox.Text, textBox.CaretIndex);
            undoStack.Push(textInMomentTime);
            redoStack.Clear();
            textBox.Paste();
        }

        public void SelectAllAction()
        {
            textBox.SelectAll();
        }
    }

}

