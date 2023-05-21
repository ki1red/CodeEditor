using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using CodeEditor.ViewInterfaces;
using CodeEditor.AuxiliaryClasses;
using CodeEditor.LanguageConnect;
using System;

namespace CodeEditor
{
    public partial class MainWindow : Window, IRunable
    {
        private bool isFileSaved = true;
        private CommanderActionsForCode commanderActions = null;
        private LanguageConnector connector;

        public MainWindow()
        {
            InitializeComponent();

            commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
            InitializeTabEdit();
            InitializeKeysInCodeEditor();
            InitializeTabFile();
            InitializeTabView();
            InitializeTabText();
            InitializeTabHelp();
            InitializeTabRun();

            connector = new LanguageConnector("Pascal\\PascalComments.exe");
        }

        private void InitializeTabFile()
        {
            void MENUITEM_CreateFile_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    bool isOpenNewFile = FileWorker.CreateFile();

                    if (isOpenNewFile)
                    {
                        TEXTBOX_WindowCodeEditor.Text = FileWorker.fileContents;

                        commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
                        isFileSaved = true;
                    }
                }
            }

            MENUITEM_CreateFile.Click += MENUITEM_CreateFile_Click;

            void MENUITEM_OpenFile_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    bool isOpenNewFile = FileWorker.OpenFile();

                    if (isOpenNewFile)
                    {
                        TEXTBOX_WindowCodeEditor.Text = FileWorker.fileContents;

                        commanderActions = new CommanderActionsForCode(ref TEXTBOX_WindowCodeEditor);
                        isFileSaved = true;
                    }
                }
            }
            
            MENUITEM_OpenFile.Click += MENUITEM_OpenFile_Click;

            void MENUITEM_SaveFile_Click(object sender, RoutedEventArgs e)
            {
                FileWorker.SaveFile(TEXTBOX_WindowCodeEditor.Text);
                isFileSaved = true;
            }
            
            MENUITEM_SaveFile.Click += MENUITEM_SaveFile_Click;

            void MENUITEM_SaveAsFile_Click(object sender, RoutedEventArgs e)
            {
                FileWorker.SaveAsFile(TEXTBOX_WindowCodeEditor.Text);
                isFileSaved = true;
            }

            MENUITEM_SaveAsFile.Click += MENUITEM_SaveAsFile_Click;

            void MENUITEM_ExitFromProgram_Click(object sender, RoutedEventArgs e)
            {
                bool isClose = IsCurrentFileSaved();
                if (isClose)
                {
                    this.Close();
                }
            }

            MENUITEM_ExitFromProgram.Click += MENUITEM_ExitFromProgram_Click;

            void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                
                bool isClose = IsCurrentFileSaved();
                if (!isClose)
                {
                    e.Cancel = true;
                }
            }

            this.Closing += MainWindow_Closing;
        }
        private void InitializeTabEdit()
        {
            void MENUITEM_UndoAction_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.UndoActions();
                isFileSaved = false;
            }

            MENUITEM_UndoAction.Click += MENUITEM_UndoAction_Click;

            void MENUITEM_RedoAction_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.RedoActions();
                isFileSaved = false;
            }

            MENUITEM_RedoAction.Click += MENUITEM_RedoAction_Click;

            void MENUITEM_CutText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.CutAction();
                isFileSaved = false;
            }

            MENUITEM_CutText.Click += MENUITEM_CutText_Click;

            void MENUITEM_CopyText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.CopyAction();
            }

            MENUITEM_CopyText.Click += MENUITEM_CopyText_Click;

            void MENUITEM_PasteText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.InsertAction();
                isFileSaved = false;
            }

            MENUITEM_PasteText.Click += MENUITEM_PasteText_Click;

            void MENUITEM_DeleteText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.DeleteAction();
                isFileSaved = false;
            }

            MENUITEM_DeleteText.Click += MENUITEM_DeleteText_Click;

            void MENUITEM_SelectAllText_Click(object sender, RoutedEventArgs e)
            {
                commanderActions.SelectAllAction();
            }

            MENUITEM_SelectAllText.Click += MENUITEM_SelectAllText_Click;
        }
        private void InitializeTabView()
        {
            void MENUITEM_IncreaseFontInput_Click(object sender, RoutedEventArgs e)
            {
                TEXTBOX_WindowCodeEditor.FontSize += 10;
            }

            MENUITEM_IncreaseFontInput.Click += MENUITEM_IncreaseFontInput_Click;

            void MENUITEM_IncreaseFontOutput_Click(object sender, RoutedEventArgs e)
            {
                TEXTBOX_WindowOutputerInformation.FontSize += 10;
            }

            MENUITEM_IncreaseFontOutput.Click += MENUITEM_IncreaseFontOutput_Click;

            void MENUITEM_DecreaseFontInput_Click(object sender, RoutedEventArgs e)
            {
                TEXTBOX_WindowCodeEditor.FontSize -= 10;
            }

            MENUITEM_DecreaseFontInput.Click += MENUITEM_DecreaseFontInput_Click;

            void MENUITEM_DecreaseFontOutput_Click(object sender, RoutedEventArgs e)
            {
                TEXTBOX_WindowOutputerInformation.FontSize -= 10;
            }

            MENUITEM_DecreaseFontOutput.Click += MENUITEM_DecreaseFontOutput_Click;
        }
        private void InitializeTabText()
        {
            void MENUITEM_ProblemStatement_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/problem_statement.html");
            }
            MENUITEM_ProblemStatement.Click += MENUITEM_ProblemStatement_Click;

            void MENUITEM_Grammar_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/grammar.html");
            }
            MENUITEM_Grammar.Click += MENUITEM_Grammar_Click;

            void MENUITEM_GrammarClassification_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/grammar_classification.html");
            }
            MENUITEM_GrammarClassification.Click += MENUITEM_GrammarClassification_Click;

            void MENUITEM_AnalizeMethod_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/analize_method.html");
            }
            MENUITEM_AnalizeMethod.Click += MENUITEM_AnalizeMethod_Click;

            void MENUITEM_Diagnostic_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/diagnostics_and_neutralizations.html");
            }
            MENUITEM_Diagnostic.Click += MENUITEM_Diagnostic_Click;

            void MENUITEM_TestExample_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/test_example.html");
            }
            MENUITEM_TestExample.Click += MENUITEM_TestExample_Click;

            void MENUITEM_Bibliography_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/bibliography.html");
            }
            MENUITEM_Bibliography.Click += MENUITEM_Bibliography_Click;

            void MENUITEM_SourceCode_Click(object sender, RoutedEventArgs e)
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "/info/source_code.html");
            }
            MENUITEM_SourceCode.Click += MENUITEM_SourceCode_Click;
        }

        private void InitializeTabHelp()
        {
            void MENUITEM_Help_Click(object sender, RoutedEventArgs e)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/ki1red/CodeEditor",
                    UseShellExecute = true
                });
            }

            MENUITEM_Help.Click += MENUITEM_Help_Click;

            void MENUITEM_About_Click(object sender, RoutedEventArgs e)
            {
                MessageBox.Show("v 1.3", "Info");
            }

            MENUITEM_About.Click += MENUITEM_About_Click;
        }
        private void InitializeTabRun()
        {
            void MENUITEM_Run_Click(object sender, RoutedEventArgs e)
            {
                this.Run();
            }

            MENUITEM_Run.Click += MENUITEM_Run_Click;
        }

        private void InitializeKeysInCodeEditor()
        {
            void TEXTBOX_WindowCodeEditor_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                bool isEdit = false;
                if (Keyboard.Modifiers != ModifierKeys.Control)
                {
                    switch (e.Key)
                    {
                        case Key.Delete:
                            commanderActions.DeleteAction();
                            isEdit = true;
                            break;
                        case Key.Back:
                            commanderActions.BackspaceAction();
                            isEdit = true;
                            break;
                        default:
                            isFileSaved = false;
                            break;
                    }
                }
                else
                {
                    switch (e.Key)
                    {
                        case Key.X:
                            commanderActions.CutAction();
                            isEdit = true;
                            break;
                        case Key.C:
                            commanderActions.CopyAction();
                            isEdit = true;
                            break;
                        case Key.A:
                            commanderActions.CopyAction();
                            isEdit = true;
                            break;
                        case Key.V:
                            commanderActions.InsertAction();
                            isEdit = true;
                            break;
                        case Key.Z:
                            commanderActions.UndoActions();
                            isEdit = true;
                            break;
                        case Key.Y:
                            commanderActions.RedoActions();
                            isEdit = true;
                            break;
                        default:
                            isFileSaved = false;
                            break;
                    }
                }

                if (isEdit)
                {
                    isFileSaved = false;
                    e.Handled = true;
                }
            }

            TEXTBOX_WindowCodeEditor.PreviewKeyDown += TEXTBOX_WindowCodeEditor_PreviewKeyDown;
        }
        private bool IsCurrentFileSaved()
        {
            bool isClose;
            if (!isFileSaved)
            {
                MessageBoxResult resultClick = MessageBox.Show("There is unsaved data. Save them?", "Warning", MessageBoxButton.YesNoCancel);

                switch (resultClick)
                {
                    case MessageBoxResult.Cancel:
                        isClose = false;
                        break;
                    case MessageBoxResult.Yes:
                        isClose = FileWorker.SaveFile(TEXTBOX_WindowCodeEditor.Text);
                        break;
                    case MessageBoxResult.No:
                        isClose = true;
                        break;
                    default:
                        isClose = false;
                        break;
                }
            }
            else
            {
                isClose = true;
            }
            return isClose;
        }
         
        public void Run()
        {
            IsCurrentFileSaved();

            TEXTBOX_WindowOutputerInformation.Text = connector.GetResultCompileCode($"{FileWorker.initialDirectory}\\{FileWorker.nameFile}.{FileWorker.defaultExt}");
        }

    }
}
