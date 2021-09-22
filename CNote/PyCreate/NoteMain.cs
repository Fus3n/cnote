/*
 Copyright 2021 Fusen
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using ColorsNew;
using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CNote
{

    public partial class NoteMain : Form
    {
        private Process pProcess;
        
        private TextStyle greenstyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        private TextStyle graystyle = new TextStyle(Brushes.Gray, null, FontStyle.Bold);
        private TextStyle pyPurple = new TextStyle(Brushes.MediumPurple, null, FontStyle.Bold);
        private TextStyle pyOrange = new TextStyle(Brushes.Orange, null, FontStyle.Bold);
        private TextStyle pyOrangeRed = new TextStyle(Brushes.OrangeRed, null, FontStyle.Bold);
        private TextStyle pyBlue = new TextStyle(Brushes.CornflowerBlue, null, FontStyle.Bold);


        //for python dynamic autocomplete
        //used to store items and remove them if they are removed from editor
        private List<string> NewAddedItems = new List<string>();

        private bool pyDoIndent; //python do auto indent if regex pattern found
        private bool TextChangedFCTB; //check if text changed used to add * in the title 
        private bool isPythonLang; //check if python is selected in language

        private HTMLPreview hm = new HTMLPreview();
        private Thread proc_thread;//Python Procces Thread
        CNoteUtils util; 

        string strOutput;
        private string currFileName;
        private string currFilePath;
        private string[] args;

        private List<string> lns;


        public NoteMain()
        {
            InitializeComponent();
            util = new CNoteUtils();
            intitThread();

            args = Environment.GetCommandLineArgs();


        }

        public void intitThread()
        {
            proc_thread = new Thread(startCmdPython);
        }


        private void NoteMain_Load(object sender, EventArgs e)
        {
            if (SplitContainer.Panel2Collapsed)
            {

            }
            cmdout.Zoom = 122;
            txt_file.Checked = true;
            languageChanger.Text = "Text File";

            //int da = 0;
            //foreach(var i in args)
            //{
            //    fctb_main.AppendText(da.ToString() + " "+ i + "\n");
            //    da++;
            //}

            if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
            {
                FileOpener(args[1]);
            }

            menu_bar2.Renderer = new ToolBarTheme_Light();

            fctb_main.AllowDrop = true;

            fctb_main.Zoom = 122;



            SplitContainer.Panel2Collapsed = true;

            InitSettings();

        }


        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }


        //init some settings
        private void InitSettings()
        {

            string pyPath = util.GetPythonPath();
            if (string.IsNullOrEmpty(pyPath))
                util.AOUSettings("pypath", "python");
            else
                util.AOUSettings("pypath", pyPath);


            string html_pref = util.GetSettings("htmlpref");

            if (string.IsNullOrEmpty(html_pref))
                util.AOUSettings("htmlpref", "default");
            else
                util.AOUSettings("htmlpref", html_pref);



            if (util.GetSettings("zoom") != null)
                fctb_main.Zoom = Int16.Parse(util.GetSettings("zoom"));


            if (util.GetSettings("theme") == null)
            {
                util.AOUSettings("theme", "light");
            }
            else
            {
                if (util.GetSettings("theme") == "dark")
                {
                    DarkMode();
                }
                else
                {
                    LightMode();
                }

            }
        }

        //Create New File
        private void new_tools_Click(object sender, EventArgs e)
        {
            fctb_main.Clear();
            currFileName = "";
            currFilePath = "";
            TextChangedFCTB = false;
            pyDoIndent = false;
        }

        //Open File Dialog Depen
        private void OpenDlg()
        {
            System.Windows.Forms.OpenFileDialog of = new System.Windows.Forms.OpenFileDialog();

            if (cshapr_lang.Checked)
                of.Filter = "CSharp File |*.cs|Any File|*.*";
            else if (vb_lang.Checked)
                of.Filter = "VB File |*.vb|Any File|*.*";
            else if (html_lang.Checked)
                of.Filter = "HTML File |*.html|Any File|*.*";
            else if (php_lang.Checked)
                of.Filter = "PHP File |*.php|Any File|*.*";
            else if (js_lang.Checked)
                of.Filter = "JavaScript File |*.js|Any File|*.*";
            else if (xml_lang.Checked)
                of.Filter = "XML File |*.xml|Any File|*.*";
            else if (sql_lang.Checked)
                of.Filter = "SQL File |*.sql|Any File|*.*";
            else if (lua_lang.Checked)
                of.Filter = "LUA File |*.lua|Any File|*.*";
            else if (python_lang.Checked)
                of.Filter = "Python File |*.py|Any File|*.*";
            else if (txt_file.Checked)
                of.Filter = "Text File |*.txt|Any File|*.*";
            else
                of.Filter = util.allFileFilter;

            if (of.ShowDialog() == DialogResult.OK)
            {

                StreamReader sr = new StreamReader(of.FileName);
                currFilePath = of.FileName;
                currFileName = Path.GetFileName(currFilePath);
                TextChangedFCTB = false;
                validateLang();
                fctb_main.Text = sr.ReadToEnd();
                sr.Close();
                FileSetup();

            }
        }

        private void FileSetup()
        {
            validateLang();
            this.Text = currFileName + " - CNote";

            if (!txt_file.Checked)
            {
                fctb_main.SelectAll();
                fctb_main.Cut();
                fctb_main.Paste();
                Clipboard.Clear();
                SaveFileAll();

            }

            if (currFileName.EndsWith(".py"))
            {

                lns = fctb_main.Lines.ToList();

                new Thread(() =>
                {
                    foreach (string line in lns)
                    {
                        PyDynamicAC(line + "\n");

                    }
                }).Start();


            }


            fctb_main.SelectionStart = 1;
            SaveFileAll();
        }


        //opens files....
        private void FileOpener(string filename)
        {
            try
            {
                StreamReader sr = new StreamReader(filename);

                fctb_main.Text = sr.ReadToEnd();
                sr.Close();
                currFilePath = filename;
                currFileName = Path.GetFileName(filename);
                sr.Close();
                TextChangedFCTB = false;
                FileSetup();

            }
            catch
            {
                //pass
            }

        }


        //reads files......maybe?
        private string FileReader(string path)
        {
            StreamReader sr = new StreamReader(path);
            var content = sr.ReadToEnd();
            sr.Close();
            return content;
        }



        //opens new file 
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text))
            {
                if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text))
                {
                    DialogResult res = MessageBox.Show($"Do you want to save changes to {currFileName}?", "CNote", MessageBoxButtons.YesNoCancel);
                    if (res == DialogResult.No)
                    {
                        OpenDlg();
                    }
                    else if (res == DialogResult.Yes)
                    {
                        FileWriter();
                        OpenDlg();
                    }

                }
            }
            else
            {
                OpenDlg();
            }

        }


        //saves file :)
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileAll();
        }

        //function to save all file if cant then open save file dialog
        private void SaveFileAll()
        {
            try
            {
                StreamWriter sw = new StreamWriter(currFilePath);
                sw.Write(fctb_main.Text);
                this.Text = currFileName + " - CNote";
                TextChangedFCTB = false;
                sw.Close();
            }
            catch { SaveDlg(); }
        }


        //yup and this one writes files
        void FileWriter()
        {
            try
            {
                StreamWriter sw = new StreamWriter(currFilePath);
                sw.Write(fctb_main.Text);
                sw.Close();
            }
            catch
            {
                SaveDlg();
            }
        }


        //SaveDlg
        private void SaveDlg()
        {
            System.Windows.Forms.SaveFileDialog of = new System.Windows.Forms.SaveFileDialog();
            if (cshapr_lang.Checked)
                of.Filter = "CSharp File |*.cs|Any File|*.*";
            else if (vb_lang.Checked)
                of.Filter = "VB File |*.vb|Any File|*.*";
            else if (html_lang.Checked)
                of.Filter = "HTML File |*.html|Any File|*.*";
            else if (php_lang.Checked)
                of.Filter = "PHP File |*.php|Any File|*.*";
            else if (js_lang.Checked)
                of.Filter = "JavaScript File |*.js|Any File|*.*";
            else if (xml_lang.Checked)
                of.Filter = "XML File |*.xml|Any File|*.*";
            else if (sql_lang.Checked)
                of.Filter = "SQL File |*.sql|Any File|*.*";
            else if (lua_lang.Checked)
                of.Filter = "LUA File |*.lua|Any File|*.*";
            else if (python_lang.Checked)
                of.Filter = "Python File |*.py|Any File|*.*";
            else if (txt_file.Checked)
                of.Filter = "Text File |*.txt|Any File|*.*";
            else
                of.Filter = util.allFileFilter;


            if (of.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(of.FileName);
                sw.Write(fctb_main.Text);
                sw.Close();
                currFilePath = of.FileName;
                currFileName = Path.GetFileName(of.FileName);
                this.Text = currFileName + " - CNote";
                validateLang();

                fctb_main.SelectAll();
                fctb_main.Cut();
                fctb_main.Paste();
                Clipboard.Clear();
                SaveFileAll();
            }
        }


        //saves as file dialog
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveDlg();

        }


        //exit..
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //uh
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();
        }

        //hm
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        //yea 
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }

        //about that..
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }

        // redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Redo();
        }


        //Theme Options

        private void lightm_toggle_Click(object sender, EventArgs e)
        {
            LightMode();
        }

        private void darkm_toggle_Click(object sender, EventArgs e)
        {
            DarkMode();
        }

        private void LightMode()
        {
            lightm_toggle.Checked = true;
            darkm_toggle.Checked = false;
            fctb_main.BackColor = Color.FromArgb(254, 251, 243);
            fctb_main.ForeColor = Color.FromArgb(0, 0, 0);
            fctb_main.IndentBackColor = Color.WhiteSmoke;
            cmdout.BackColor = Color.WhiteSmoke;

            menu_bar2.BackColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(0, 0, 0);
            cmdout.ForeColor = Color.Black;


            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.White;
                    a.ForeColor = Color.Black;
                }
            }

            menu_bar2.Renderer = new ToolBarTheme_Light();
            menu_bar2.Refresh();
            util.AOUSettings("theme", "light");

        }

        private void DarkMode()
        {

            darkm_toggle.Checked = true;
            lightm_toggle.Checked = false;
            fctb_main.BackColor = Color.FromArgb(23, 16, 16);
            fctb_main.IndentBackColor = Color.FromArgb(23, 16, 16);
            menu_bar2.BackColor = Color.FromArgb(23, 16, 16);
            cmdout.BackColor = Color.FromArgb(23, 16, 16);

            fctb_main.ForeColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(254, 251, 243);
            cmdout.ForeColor = Color.LightSteelBlue;



            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.FromArgb(23, 16, 16);
                    a.ForeColor = Color.FromArgb(254, 251, 243);
                }
            }

            menu_bar2.Renderer = new ToolBarTheme_Dark();
            menu_bar2.Refresh();
            util.AOUSettings("theme", "dark");
        }

        //Edit Option


        //find dailog
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.ShowFindDialog();
        }


        //goto line dialog
        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.ShowGoToDialog();
        }

        //find and replace
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.ShowReplaceDialog();
        }


        //temporary font selection
        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                fctb_main.Font = fd.Font;//change current font and size

            }
        }



        //Context Menu Items
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.SelectAll();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Cut();

        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Paste();
        }


        //reset Language.Custom in fastcoloredtextbox
        private void resetCustoms()
        {
            isPythonLang = false;
        }



        //word wrap
        private void WrapdisableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.WordWrap = false;
            autoCheck(sender);

        }

        private void WrapenableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.WordWrap = true;
            autoCheck(sender);

        }


        //Auto Functions
        void autoCheck(object sender)
        {
            var currentItem = sender as ToolStripMenuItem;

            ((ToolStripMenuItem)currentItem.OwnerItem).DropDownItems
            .OfType<ToolStripMenuItem>().ToList()
            .ForEach(item =>
            {
                item.Checked = false;
            });
            currentItem.Checked = true;
        }

        void autocheckstrip(object sender)
        {

            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            foreach (var it in languageChanger.DropDownItems)
            {
                try
                {
                    items.Add((ToolStripMenuItem)it);
                }
                catch
                {
                    //pass
                }
            }

            foreach (var item in items)
            {
                item.Checked = false;
            }

             ((ToolStripMenuItem)sender).Checked = true;

        }

        private void python_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            isPythonLang = true;
            fctb_main.Language = Language.Custom;
            languageChanger.Text = "Python";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.python_items;
        }


        //Change Syntext Highlighting and autocomplete list to respective selected language
        private void cshapr_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.CSharp;
            languageChanger.Text = "#C";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.cshapr_items;
        }

        private void vb_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.VB;
            languageChanger.Text = "VB";
            AutoCompMenu1.Enabled = false;
        }

        private void php_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.PHP;
            languageChanger.Text = "PHP";
            AutoCompMenu1.Enabled = false;
        }

        private void html_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.HTML;
            languageChanger.Text = "HTML";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.html_items;

        }

        private void xml_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.XML;
            languageChanger.Text = "XML";
            AutoCompMenu1.Enabled = false;
        }

        private void lua_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.Lua;
            languageChanger.Text = "Lua";
            AutoCompMenu1.Enabled = false;
        }

        private void sql_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.SQL;
            languageChanger.Text = "SQL";
            AutoCompMenu1.Enabled = false;
        }

        private void js_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.JS;
            languageChanger.Text = "JavaScript";
            AutoCompMenu1.Enabled = false;
        }

        private void txt_file_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.Custom;
            languageChanger.Text = "Text File";
            AutoCompMenu1.Enabled = false;
        }




        //Validate which language being written or opened in editor
        private void validateLang()
        {
            switch (Path.GetExtension(currFileName))
            {
                case ".cs":
                    autocheckstrip(cshapr_lang);
                    resetCustoms();
                    fctb_main.Language = Language.CSharp;
                    languageChanger.Text = "C#";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.cshapr_items;
                    break;
                case ".py":
                    autocheckstrip(python_lang);
                    isPythonLang = true;
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "Python";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.python_items;
                    foreach (string line in fctb_main.Lines)
                    {
                        PyDynamicAC(line);
                    }
                    break;

                case ".xml":
                    autocheckstrip(xml_lang);
                    resetCustoms();
                    fctb_main.Language = Language.XML;
                    languageChanger.Text = "XML";
                    AutoCompMenu1.Enabled = false;

                    break;


                case ".html":
                    autocheckstrip(html_lang);
                    resetCustoms();
                    fctb_main.Language = Language.HTML;
                    languageChanger.Text = "HTML";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.html_items;

                    break;

                case ".sql":
                    autocheckstrip(sql_lang);
                    resetCustoms();
                    fctb_main.Language = Language.SQL;
                    languageChanger.Text = "SQL";
                    AutoCompMenu1.Enabled = false;

                    break;

                case ".lua":
                    autocheckstrip(lua_lang);
                    resetCustoms();
                    fctb_main.Language = Language.Lua;
                    languageChanger.Text = "Lua";
                    AutoCompMenu1.Enabled = false;

                    break;

                case ".js":
                    autocheckstrip(html_lang);
                    resetCustoms();
                    fctb_main.Language = Language.JS;
                    languageChanger.Text = "JavaScript";
                    AutoCompMenu1.Enabled = false;

                    break;

                case ".vb":
                    autocheckstrip(vb_lang);
                    resetCustoms();
                    fctb_main.Language = Language.VB;
                    languageChanger.Text = "VB";
                    AutoCompMenu1.Enabled = false;

                    break;

                case ".txt":
                    autocheckstrip(txt_file);
                    resetCustoms();
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "Text File";
                    AutoCompMenu1.Enabled = false;

                    break;


            }

        }


        //Get basic info for statusStrip
        private string GetEditInfo()
        {
            var info = $"Line {fctb_main.Selection.FromLine + 1} Zoom {fctb_main.Zoom}";
            return info;
        }


        //Run File is Runnable
        private void run_tools_Click_1(object sender, EventArgs e)
        {
            if (fctb_main.Language == Language.HTML)
            {
                if (util.GetSettings("htmlpref") == "default")
                {
                    if (string.IsNullOrEmpty(currFilePath))
                    {
                        hm.LoadDoc(fctb_main.Text);
                        hm.Show();
                    }
                    else
                    {
                        hm.LoadDoc(currFilePath);
                        hm.Show();
                    }
                }
                else if (util.GetSettings("htmlpref") == "browser")
                {
                    Process.Start(currFilePath);
                }


            }

            else if (python_lang.Checked)
            {
                if (!string.IsNullOrEmpty(currFilePath))
                {
                    if (!proc_thread.IsAlive)
                    {
                        cmdout.Text = "";
                        SplitContainer.Panel2Collapsed = false;
                        try
                        {
                            proc_thread.Start();
                        }
                        catch
                        {
                            MessageBox.Show("Script already running", "CNOTE");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Script already running", "CNOTE");
                    }
                }
                else
                    SaveDlg();
                

            }

        }


        //Run Python File
        private void startCmdPython()
        {
            try
            {
                pProcess = new Process();

          
                string strCommand = util.GetSettings("pypath");
                //strCommand is path and file name of command to run
                pProcess.StartInfo.FileName = strCommand;

                //strCommandParameters are parameters to pass to program
                pProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(currFilePath);
                pProcess.StartInfo.Arguments = currFileName;

                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardError = true;
                //Set output of program to be written to process output stream
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.EnableRaisingEvents = true;


                pProcess.StartInfo.CreateNoWindow = true;



                //Start the process
                pProcess.Start();

                //Get program output
                strOutput = pProcess.StandardOutput.ReadToEnd();
                strOutput += pProcess.StandardError.ReadToEnd();
                pProcess.Exited += pProcess_Exited;
                //Wait for process to finish


                this.Invoke((MethodInvoker)delegate
                {
                    if (!string.IsNullOrEmpty(strOutput))
                    {
                        cmdout.Text = strOutput;
                        cmdout.GoEnd();
                       



                    }

                });
                pProcess.Kill();


                pProcess.WaitForExit();

            }

            catch
            {
   
            }

        }


        private void pProcess_Exited(object sender, EventArgs e)
        {
            stop_proc();
        }

        private void stop_proc()
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (pProcess.HasExited == false)
                    pProcess.Kill();
                cmdout.Text += $"Procces Exited with Code {pProcess.ExitCode}";

                intitThread();

            });
        }

        // Get Info in StatusTool
        private void fctb_main_Click(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
        }


        private void fctb_main_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (fctb_main.Language == Language.HTML) //Html Live Preview
            {
                if (string.IsNullOrEmpty(currFilePath))
                {
                    hm.LoadDoc(fctb_main.Text); // add the new value to Browser control from editor if file not saved
                }
                else
                {
                    hm.LoadDoc(currFilePath); // add the new value to Browser control from saved file
                }
            }

            if (fctb_main.IsChanged)
            {
                // put * in title if file not saved after change

                TextChangedFCTB = true;
                if (string.IsNullOrEmpty(currFilePath)) //check if file is saved or opend if not then adds untitled
                {
                    this.Text = "*" + "untitled" + " - CNote";
                    currFileName = "untitled";
                }
                else
                {
                    this.Text = "*" + currFileName + " - CNote"; //if opened add * before filename
                }

            }
            else
            {
                this.Text = currFileName + " - CNote"; //if already saved add normal title
            }



            if (isPythonLang)// check if python language selected or python file opened
            {
                PyDynamicAC(e.ChangedRange.Text);
                

                PythonSynHighligt(e); //custom syn highlight ofc
                

                if (e.ChangedRange.Text.EndsWith("):")) //Find ':' for pythons auto indent
                {
                    pyDoIndent = true;
                }

            }
        }



        //adding value to autocomplete dynamicly
        private void PyDynamicAC(string e)
        {


            //Match result_var = Regex.Match(e, @"(^.*?(?=\=)"); // find any value before '='

            Match result_var = Regex.Match(e, @"(\b[a-zA-Z]+)(\s?=)"); // find any value before '='

            Match result_import = Regex.Match(e, @"\bimport\s([a-zA-Z_0-9]+)\s?"); // find any value after 'import'

            Match result_from = Regex.Match(e, @"\bfrom\s([a-zA-Z_0-9]+)\s"); // find any value after 'from'

            Match result_as = Regex.Match(e, @"\bas\s([a-zA-Z_0-9]*)"); // find any value after 'as'

            //Find variables
            if (result_var.Success)
            {
                var str = result_var.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                }
            }

            //find Imports
            if (result_import.Success)
            {
                var str = result_import.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                }
            }

            //find froms
            if (result_from.Success)
            {
                var str = result_from.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                }
            }


            //find as
            if (result_as.Success)
            {
                var str = result_as.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                } 
            }


            if (NewAddedItems.Count != 0)
            {

                //Removing values/variables from autocompletemenu that where removed from file Dynamicly
                foreach (var it in NewAddedItems)
                {
                    if (AutoCompMenu1.Items.Contains(it) && !fctb_main.Text.Contains(it) && !string.IsNullOrWhiteSpace("it")) //if value contains in autocompmenu and not in editor
                    {
                        // reinserting values to AutocompMenu array without removed values
                        List<string> newl = new List<string>(); //temp list of string 
                        foreach (var Item in AutoCompMenu1.Items)
                        {
                            newl.Add(Item); // adding all previous values from Autocompletemenu
                        }

                        newl.Remove(it); // remove the value that where removed from file

                        AutoCompMenu1.Items = newl.ToArray(); // reinsert new values back

                    }
                }
            }

        }


        //custom python syn highlight
        private void PythonSynHighligt(TextChangedEventArgs e)
        {

            Regex reg = new Regex(@"#.*$", RegexOptions.Multiline);
            Regex reg_func = new Regex(@"\.([a-zA-Z]+(\(\))?)", RegexOptions.Multiline);

            


            e.ChangedRange.ClearStyle(graystyle);
            e.ChangedRange.ClearStyle(pyOrange);
            e.ChangedRange.ClearStyle(pyOrangeRed);
            e.ChangedRange.ClearStyle(pyPurple);
            e.ChangedRange.ClearStyle(greenstyle);
            e.ChangedRange.ClearStyle(pyBlue);



            e.ChangedRange.SetStyle(graystyle, reg);
            e.ChangedRange.SetStyle(pyOrange, util.pyHLSecOrange);
            e.ChangedRange.SetStyle(pyOrangeRed, util.pyHLSecOrangeRed);
            e.ChangedRange.SetStyle(pyBlue, util.pyHLSecFuncs);

            e.ChangedRange.SetStyle(pyBlue, reg_func);


            e.ChangedRange.SetStyle(pyPurple, util.pyHLmains);

            e.ChangedRange.SetStyle(greenstyle, util.pyHLstr);


        }


        //save zoom value on zoom change
        private void fctb_main_ZoomChanged(object sender, EventArgs e)
        {
            stat_txt.Text = GetEditInfo();
            util.AOUSettings("zoom", $"{fctb_main.Zoom}");
        }

        //word wrap
        private void wrap_tool_Click(object sender, EventArgs e)
        {
            if (wrap_tool.Checked)
            {
                fctb_main.WordWrap = false;
                wrap_tool.Checked = false;
            }
            else
            {
                fctb_main.WordWrap = true;
                wrap_tool.Checked = true;
            }

        }

        private void cmdoutHide_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = true;
        }

        private void cmdoutSelectAll_Click(object sender, EventArgs e)
        {
            cmdout.SelectAll();
        }

        //Show Cmd output panel
        private void showOutPanel_Click(object sender, EventArgs e)
        {
            SplitContainer.Panel2Collapsed = false;
        }


        //Auto indent for Python 
        private void fctb_main_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                stat_txt.Text = GetEditInfo();
                if (python_lang.Checked && pyDoIndent)
                {
                    var line = fctb_main.Selection.FromLine;
                    fctb_main.Selection.tb.InsertText("\t");
                    pyDoIndent = false;
                }

            }
        }


        //Lang pref (settings)
        private void languagePreferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LangPref lp = new LangPref();

            lp.Show();
        }


        //Drag files 
        private void fctb_main_DragDrop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop);
            if (data != null)
            {
                var filenames = data as string[];
                if (filenames.Length > 0)
                {
                    currFilePath = filenames[0];
                    currFileName = Path.GetFileName(filenames[0]);
                    fctb_main.Text = FileReader(filenames[0]);
                    this.Text = currFileName + " - CNote";
                    fctb_main.ClearStylesBuffer();
                    validateLang();
                }

            }
        }

        private void fctb_main_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        // save dialog if any contenet has changed without saving
        private void NoteMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text))
            {
                DialogResult res = MessageBox.Show($"Do you want to save changes to {currFileName}?", "Title", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (res == DialogResult.Yes)
                {
                    FileWriter();
                }

            }


        }


        //reset zoom and save it
        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.Zoom = 122;
            util.AOUSettings("zoom", "122");

        }


        //Create New Window instance
        private void newWindowtool_Click(object sender, EventArgs e)
        {
            NoteMain newMain = new NoteMain();
            newMain.Show();
        }

        //close current window
        private void closeWindowtool_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // Kill Any script  Procces if running
        private void cancelRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stop_proc();
        }


        private void cmdout_clear_Click(object sender, EventArgs e)
        {
            cmdout.Clear();
        }

        private void cmdout_paste_Click(object sender, EventArgs e)
        {
            cmdout.Paste();
        }

        private void cmdout_cpy_Click(object sender, EventArgs e)
        {
            cmdout.Copy();
            cmdout.Text = "cmd>";
        }

        private void aboutCNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutNote an = new AboutNote();
            an.ShowDialog();

        }

    }

}

