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

        //total all files 1775 lines
        private Process pProcess;

       

        /// form1
        List<NoteMain> ins_list = new List<NoteMain>();
        private int curr_ins_index = 0;

        //for python dynamic autocomplete
        //used to store items and remove them if they are removed from editor
        private List<string> NewAddedItems = new List<string>();

        private bool pyDoIndent; //python do auto indent if regex pattern found
        private bool TextChangedFCTB; //check if text changed used to add * in the title 
        private bool isPythonLang; //check if python is selected in language
        private bool isCppLang; //check if c++ file is opend;
        private bool isPyLibLoaded; //check if python modules loaded to autocomp list
        private bool isOpenFirstTime = false;
        private bool isFileRunning;

        private HTMLPreview hm = new HTMLPreview();
        private Thread proc_thread;//Python Procces Thread
        CNoteUtils util;

        string strOutput;
        private string currFileName;
        private string currFilePath;
        private string[] args;


        private List<string> lns;

        

        Regex reg_c_comment = new Regex(@"//.*$", RegexOptions.Multiline);
        Regex reg_c_user_func = new Regex(@"\b[a-zA-Z]+(\[\d?\])?\d?.?(\s+)(\w+\d?.?)(\s+)?", RegexOptions.Multiline);
        Regex reg_c_func_calls = new Regex(@"([a-zA-Z0-9_]+(\(\)))", RegexOptions.Multiline);

        Regex reg = new Regex(@"#.*$", RegexOptions.Multiline);
        Regex reg_func = new Regex(@"\.([a-zA-Z0-9_]+(\(\))?)", RegexOptions.Multiline);
        Regex reg_user_func = new Regex(@"\bdef\s([a-zA-Z0-9_]+)", RegexOptions.Multiline);
        Regex reg_user_class = new Regex(@"\bclass\s([a-zA-Z0-9_]+)", RegexOptions.Multiline);


        private Keys lastkeycode;
        public NoteMain()
        {
            InitializeComponent();
            util = new CNoteUtils();
           
            intitThread();

            args = Environment.GetCommandLineArgs();


        }

        public void intitThread()
        {
            if (isPythonLang)
                proc_thread = new Thread(startCmd);
            else
                proc_thread = new Thread(RunFile);
        }

        public string[] removeDup(string[] arr)
        {
            string[] q = arr.Distinct().ToArray();

            return q;
        }
        private void NoteMain_Load(object sender, EventArgs e)
        {
            util.cshapr_items = removeDup(util.cshapr_items);
            opencnt_folder.Enabled = false;
            run_strip_menu.Enabled = false;

            cmdout.Zoom = 122;
            txt_file.Checked = true;
            languageChanger.Text = "Text File";

            if (args.Length >= 2 && !string.IsNullOrEmpty(args[1]))
            {
                FileOpener(args[1]);
            }

            menu_bar2.Renderer = new ToolBarTheme_Light();

            fctb_main.AllowDrop = true;

        

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
                fctb_main.Zoom = Int32.Parse(util.GetSettings("zoom"));


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
            currFileName = "untitled";
            currFilePath = "";
            TextChangedFCTB = false;
            pyDoIndent = false;
            opencnt_folder.Enabled = false;
            Text = currFileName + " - CNote";
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
            isOpenFirstTime = true;
            opencnt_folder.Enabled = true;

            validateLang();
            this.Text = currFileName + " - CNote";

            if (!txt_file.Checked)
            {
                string prevclip = Clipboard.GetText();
                fctb_main.SelectAll();
                fctb_main.Cut();
                fctb_main.Paste();
                Clipboard.Clear();
                if (!string.IsNullOrEmpty(prevclip))
                    Clipboard.SetText(prevclip);
                //SaveFileAll();

            }

            if (currFileName.EndsWith(".cs"))
            {

                lns = fctb_main.Lines.ToList();

                new Thread(() =>
                {
                    foreach (string line in lns)
                    {
                        GetAllMethodNames(line + "\n");

                    }
                }).Start();

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
            //SaveFileAll();
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
                isOpenFirstTime = false;
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
                TextChangedFCTB = false;
                FileSetup();
                isOpenFirstTime = false;
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
            this.Close();
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
            AutoCompMenu1.Colors.BackColor = Color.WhiteSmoke;

            menu_bar2.BackColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(0, 0, 0);
            cmdout.ForeColor = Color.Black;

            AutoCompMenu1.Colors.ForeColor = Color.Black;
            AutoCompMenu1.Colors.HighlightingColor = Color.DarkOrange;



            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.White;
                    a.ForeColor = Color.Black;
                }
            }


            foreach (ToolStripItem item in theme_colorstrip.DropDownItems)
            {
                item.BackColor = Color.White;
                item.ForeColor = Color.Black;
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
            AutoCompMenu1.Colors.BackColor = Color.FromArgb(44, 57, 75);

            fctb_main.ForeColor = Color.FromArgb(254, 251, 243);
            menu_bar2.ForeColor = Color.FromArgb(254, 251, 243);
            cmdout.ForeColor = Color.LightSteelBlue;

            AutoCompMenu1.Colors.ForeColor = Color.LightSteelBlue;
            AutoCompMenu1.Colors.HighlightingColor = Color.White;





            foreach (ToolStripMenuItem item in menu_bar2.Items)
            {
                foreach (ToolStripItem a in item.DropDownItems)
                {
                    a.BackColor = Color.FromArgb(23, 16, 16);
                    a.ForeColor = Color.FromArgb(254, 251, 243);


                }

            }

            foreach (ToolStripItem item in theme_colorstrip.DropDownItems)
            {
                item.BackColor = Color.FromArgb(23, 16, 16);
                item.ForeColor = Color.FromArgb(254, 251, 243);
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
            isCppLang = false;

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


        //Change Syntext Highlighting and autocomplete list to respective selected language
        private void python_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            intitThread();
            LoadLibsPy();
            resetCustoms();
            isPythonLang = true;
            fctb_main.Language = Language.Custom;
            languageChanger.Text = "Python";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.python_items;
            run_strip_menu.Enabled = true;
            fctb_main.CommentPrefix = "#";
           
        }

        private void cshapr_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.CSharp;
            languageChanger.Text = "C#";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.cshapr_items;
            run_strip_menu.Enabled = true;
            fctb_main.CommentPrefix = "//";
        }
        private void cpp_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            isCppLang = true;
            fctb_main.Language = Language.Custom;
            languageChanger.Text = "C++";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.cpp_items;
            run_strip_menu.Enabled = true;
            fctb_main.CommentPrefix = "//";
        }

        private void vb_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.VB;
            languageChanger.Text = "VB";
            AutoCompMenu1.Enabled = false;
            run_strip_menu.Enabled = true;
            fctb_main.CommentPrefix = "' ";
        }

        private void php_lang_Click(object sender, EventArgs e)
        {
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.PHP;
            languageChanger.Text = "PHP";
            run_strip_menu.Enabled = false;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "//";
        }

        private void html_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.HTML;
            languageChanger.Text = "HTML";
            AutoCompMenu1.Enabled = true;
            AutoCompMenu1.Items = util.html_items;
            run_strip_menu.Enabled = true;
            fctb_main.CommentPrefix = "<!--";

        }

        private void xml_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.XML;
            languageChanger.Text = "XML";
            run_strip_menu.Enabled = false;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "";
        }

        private void lua_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.Lua;
            languageChanger.Text = "Lua";
            run_strip_menu.Enabled = true;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "--";
        }

        private void sql_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.SQL;
            languageChanger.Text = "SQL";
            run_strip_menu.Enabled = true;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "--";
        }

        private void js_lang_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.JS;
            languageChanger.Text = "JavaScript";
            run_strip_menu.Enabled = true;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "//";
        }

        private void txt_file_Click(object sender, EventArgs e)
        {
            intitThread();
            autocheckstrip(sender);
            resetCustoms();
            fctb_main.Language = Language.Custom;
            languageChanger.Text = "Text File";
            run_strip_menu.Enabled = false;
            AutoCompMenu1.Enabled = false;
            fctb_main.CommentPrefix = "";
        }

 


        private void LoadLibsPy()
        {
           if (isPyLibLoaded == false)
            {
                if ((!string.IsNullOrEmpty(util.GetPythonPath()) && util.GetPythonPath().Contains("\\"))) // check if saved python path contains slash 
                {

                    string import_path = $"{Path.GetDirectoryName(util.GetPythonPath())}\\Lib\\site-packages\\";
                    string import_path_inbuilt = $"{Path.GetDirectoryName(util.GetPythonPath())}\\Lib\\";
                    if (Directory.Exists(import_path))
                    {
                        List<string> tempLi = new List<string>();
                        string[] directories_user = Directory.GetDirectories(import_path);
                        string[] directories_inbuilt = Directory.GetFiles(import_path_inbuilt);


                        foreach (var item in util.python_items) // reasign old values from complete menu
                        {
                            tempLi.Add(item);
                        }

                        foreach (var dir in directories_user) // add the new upadated values|| !num_range.Any(dir.Contains) || !dir.Contains('.')
                        {
                            string foldername = Path.GetFileName(dir);
                            if (!foldername.Contains("dist-info") && !foldername.Contains("egg-info") && !foldername.Contains('_') && !foldername.Contains('~'))
                            {
                                tempLi.Add(foldername);
                            }
                        }

                        foreach (var file in directories_inbuilt) // add the new upadated values|| !num_range.Any(dir.Contains) || !dir.Contains('.')
                        {
                            string filename = Path.GetFileName(file);

                            if (!filename.Contains("dist-info") && !filename.Contains("egg-info") && !filename.Contains('_') && !filename.Contains('~') && filename.EndsWith(".py") && !filename.EndsWith(".pyo"))
                            {
                                tempLi.Add(filename.Replace(".py", string.Empty));
                            }

                        }


                        util.python_items = tempLi.ToArray();
                        isPyLibLoaded = true;

                    }

                }
            }
           
        }


        //Validate which language being written or opened in editor
        private void validateLang()
        {
            switch (Path.GetExtension(currFileName))
            {
                case ".cpp":
                    intitThread();
                    autocheckstrip(cpp_lang);
                    resetCustoms();
                    isCppLang = true;
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "C++";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.cpp_items;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "//";
                    foreach (string line in fctb_main.Lines)
                    {
                        GetAllMethodNames(line);
                    }
                    break;

                case ".cs":
                    intitThread();
                    autocheckstrip(cshapr_lang);
                    resetCustoms();
                    fctb_main.Language = Language.CSharp;
                    languageChanger.Text = "C#";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.cshapr_items;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "//";
                    foreach (string line in fctb_main.Lines)
                    {
                        GetAllMethodNames(line);
                    }
                    break;
                case ".py":
                    intitThread();
                    autocheckstrip(python_lang);
                    LoadLibsPy();
                    isPythonLang = true;
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "Python";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.python_items;
                    fctb_main.CommentPrefix = "#";
                    foreach (string line in fctb_main.Lines)
                    {
                        PyDynamicAC(line);
                    }
                    run_strip_menu.Enabled = true;
                    break;
                   
                case ".xml":
                    intitThread();
                    autocheckstrip(xml_lang);
                    resetCustoms();
                    fctb_main.Language = Language.XML;
                    languageChanger.Text = "XML";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = false;
                    fctb_main.CommentPrefix = "";
                    break;


                case ".html":
                    intitThread();
                    autocheckstrip(html_lang);
                    resetCustoms();
                    fctb_main.Language = Language.HTML;
                    languageChanger.Text = "HTML";
                    AutoCompMenu1.Enabled = true;
                    AutoCompMenu1.Items = util.html_items;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "<!---->";
                    break;

                case ".sql":
                    intitThread();
                    autocheckstrip(sql_lang);
                    resetCustoms();
                    fctb_main.Language = Language.SQL;
                    languageChanger.Text = "SQL";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "--";
                    break;

                case ".lua":
                    autocheckstrip(lua_lang);
                    resetCustoms();
                    fctb_main.Language = Language.Lua;
                    languageChanger.Text = "Lua";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "--";
                    break;

                case ".js":
                    intitThread();
                    autocheckstrip(html_lang);
                    resetCustoms();
                    fctb_main.Language = Language.JS;
                    languageChanger.Text = "JavaScript";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "//";

                    break;

                case ".vb":
                    intitThread();
                    autocheckstrip(vb_lang);
                    resetCustoms();
                    fctb_main.Language = Language.VB;
                    languageChanger.Text = "VB";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = true;
                    fctb_main.CommentPrefix = "' ";
                    break;

                case ".php":
                    intitThread();
                    autocheckstrip(vb_lang);
                    resetCustoms();
                    fctb_main.Language = Language.VB;
                    languageChanger.Text = "VB";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = false;
                    fctb_main.CommentPrefix = "//";
                    break;

                case ".txt":
                    intitThread();
                    autocheckstrip(txt_file);
                    resetCustoms();
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "Text File";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = false;
                    fctb_main.CommentPrefix = "";
                    break;

                default:
                    intitThread();
                    autocheckstrip(txt_file);
                    resetCustoms();
                    fctb_main.Language = Language.Custom;
                    languageChanger.Text = "Text File";
                    AutoCompMenu1.Enabled = false;
                    run_strip_menu.Enabled = false;
                    fctb_main.CommentPrefix = "";
                    break;
            }

        }


        //Get basic info for statusStrip
        private string GetEditInfo()
        {
            return $"Line {fctb_main.Selection.FromLine + 1} Zoom {fctb_main.Zoom}";

        }


        //Run File is Runnable
        private void run_tools_Click_1(object sender, EventArgs e)
        {

            RunClicked();
            stat_txt.Text = GetEditInfo();

        }

        private void RunClicked()
        {
            if (fctb_main.Language == Language.HTML)
            {
                
                if (util.GetSettings("htmlpref") == "default")
                {
                    if (string.IsNullOrEmpty(currFilePath) && !File.Exists(currFilePath))
                    {
                        SaveDlg();
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
                        cmdout.Text += "Running";
                        SplitContainer.Panel2Collapsed = false;
                        try
                        {
                            proc_thread.Start();
                        }
                        catch
                        {
                            stop_proc();
                            proc_thread.Start();
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
            else
            {
                if (!string.IsNullOrEmpty(currFilePath))
                {
                    if (!proc_thread.IsAlive)
                    {
                        cmdout.Text = "";
                        cmdout.Text += "Running";
                        SplitContainer.Panel2Collapsed = false;
                                                
                        try
                        {
                            proc_thread.Start();
                        }
                        catch
                        {
                            MessageBox.Show("Thread Error: File already running", "CNOTE");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Thread Error isAlive File already running", "CNOTE");
                    }
                }
                else
                    SaveDlg();
            }

        }

        //Run Other Files
        void RunFile()
        {
            // cmd path @"C:\Windows\System32\cmd.exe";

            if (!string.IsNullOrEmpty(util.GetSettings("customCommand")))
            {
                isFileRunning = true;
                pProcess = new Process();

                string fileName = @"C:\Windows\System32\cmd.exe";

                //strCommand is path and file name of command to run
                pProcess.StartInfo.FileName = fileName;
                //strCommandParameters are parameters to pass to program
                pProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(currFilePath);
                string formedCMD = util.GetSettings("customCommand");
                formedCMD = formedCMD.Replace("%1", currFileName);
                formedCMD = formedCMD.Replace("%bin", Path.GetFileNameWithoutExtension(currFileName));

                pProcess.StartInfo.RedirectStandardError = true;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.Arguments = "/c" + formedCMD;
                pProcess.EnableRaisingEvents = true;

               

                //Start the process
                pProcess.Start();


                while (!pProcess.StandardOutput.EndOfStream)
                {
                    string line = pProcess.StandardOutput.ReadLine();
                    this.Invoke((MethodInvoker)delegate
                    {
                        cmdout.Text += "\n" + line;
                        cmdout.GoEnd();

                    });
                }

                strOutput = pProcess.StandardOutput.ReadToEnd();
                strOutput += pProcess.StandardError.ReadToEnd();
                pProcess.Exited += pProcess_Exited;


                this.Invoke((MethodInvoker)delegate
                {
                    if (!string.IsNullOrEmpty(strOutput))
                    {
                        cmdout.Text = strOutput;
                        cmdout.GoEnd();


                    }

                });


                pProcess.WaitForExit();
                
                

                Invoke((MethodInvoker)delegate
                {
                    isFileRunning = false;
                });
                
                if (!pProcess.HasExited)
                    pProcess.Kill();

            }
            else
            {
                SplitContainer.Panel2Collapsed = false;
                cmdout.Text = "Configuration is Empty";
                cmdout.AppendText("Exited");
                cmdout.GoEnd();
                
            }
           
        }


        //Run Python File
        private void startCmd()
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


                //pProcess.StartInfo.CreateNoWindow = true;

                //Start the process
                pProcess.Start();

                while (!pProcess.StandardOutput.EndOfStream)
                {
                    string line = pProcess.StandardOutput.ReadLine();
                    this.Invoke((MethodInvoker)delegate
                    {
                        cmdout.Text += "\n" + line;
                        cmdout.GoEnd();

                    });
                }


                //Get program output
                //strOutput = pProcess.StandardOutput.ReadToEnd();
                //strOutput += pProcess.StandardError.ReadToEnd();
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
                //e
            }

        }

        private void pProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                if (!string.IsNullOrEmpty(strOutput))
                {
                    cmdout.Text = e.Data;
                }

            });

        }

        private void pProcess_Exited(object sender, EventArgs e)
        {
            stop_proc();
        }

        private void stop_proc()
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    if (pProcess.HasExited == false)
                        pProcess.Kill();

      
                    cmdout.Text += $"\nProcces Exited with Code {pProcess.ExitCode}";
                    intitThread();

                });

            }
            catch
            {
                //pass
            }

            stat_txt.Text = GetEditInfo();

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
                // put '*' in title if file not saved after change

                TextChangedFCTB = true;
                if (string.IsNullOrEmpty(currFilePath)) //check if file is saved or opend if not then adds untitled
                {
                    if (string.IsNullOrEmpty(currFileName))
                    {
                        currFileName = "untitled";
                        this.Text = currFileName + " - CNote";
                    }
                    else
                    {
                        currFileName = "untitled";
                        this.Text = "*"+ currFileName + " - CNote";
                    }
                   
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


           
            if (fctb_main.Language == Language.CSharp && lastkeycode != Keys.Back)
            {
                GetAllMethodNames(e.ChangedRange.Text);
           
            }


            if (isPythonLang)// check if python language selected or python file opened
            {
                PyDynamicAC(e.ChangedRange.Text);


                PythonSynHighligt(e); //custom syn highlight ofc

                Match result_indent = Regex.Match(e.ChangedRange.Text, @"\b[a-zA-z0-9_]+(:|\([a-zA-Z0-9_,\+:\->]*\):$)");//Find ':' or ':()' for pythons auto indent


                if (result_indent.Success)
                {
                    pyDoIndent = true;
                }


            }else if (isCppLang)
            {
                CSynHighligt(e);
            }



        }


        // ------ CSHARP STUFF ------- //

        public void GetAllMethodNames(string e)
        {
            //Match result_var = Regex.Match(e, @"(^.*?(?=\=)"); // find any string before '='

            Match result_var_references = Regex.Match(e, @"\b[a-zA-Z_0-9_]+\s([a-zA-Z_0-9_]+)(\s?[=;])"); // find any variable references


            Match result_class_funcs = Regex.Match(e, @"\b[a-zA-Z_0-9_]+\s([a-zA-Z_0-9_]+)(\(\))"); // find any class/funcion refrences

            // find any variable references
            if (result_var_references.Success)
            {
                var str = result_var_references.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);

                }
            }

            // find any class/funcion refrences
            if (result_class_funcs.Success)
            {
                var str = result_class_funcs.Groups[1].Value;
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
                    if (AutoCompMenu1.Items.Contains(it) && fctb_main.Text.Contains(it) == false && !string.IsNullOrWhiteSpace(it)) //if value contains in autocompmenu and not in editor and not an empty space/null 
                    {
                       
                        List<string> clean_list = new List<string>(); //temp list of string 
                        foreach (var Item in AutoCompMenu1.Items)
                        {
                            clean_list.Add(Item); // adding all previous values from Autocompletemenu
                        }

                        clean_list.Remove(it); // remove the value that where removed from file
                        AutoCompMenu1.Items = clean_list.ToArray(); // reinsert new values back

                    }
                }
            }
  
        }

        private void CSynHighligt(TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(util.pyfuncParam);
            e.ChangedRange.ClearStyle(util.pynums);

            e.ChangedRange.ClearStyle(util.pyOrange);
            e.ChangedRange.ClearStyle(util.pyOrangeRed);
            e.ChangedRange.ClearStyle(util.pyPurple);
            e.ChangedRange.ClearStyle(util.pyBlue);
            e.ChangedRange.ClearStyle(util.pyLightGreen);
            e.ChangedRange.ClearStyle(util.pyclassorange);
            e.ChangedRange.ClearStyle(util.pyops);
            e.ChangedRange.ClearStyle(util.graystyle);
            e.ChangedRange.ClearStyle(util.greenstyle);


            e.ChangedRange.SetStyle(util.pyfuncParam, util.CHLparams);
            e.ChangedRange.SetStyle(util.pynums, util.CHLnums);
            e.ChangedRange.SetStyle(util.pyOrange, util.CHLSecOrange);
            e.ChangedRange.SetStyle(util.pyOrangeRed, util.CHLSecOrangeRed);
            //e.ChangedRange.SetStyle(util.pyBlue, util.CHLSecFuncs);

            e.ChangedRange.SetStyle(util.pyBlue, reg_func);
            e.ChangedRange.SetStyle(util.pyBlue, reg_c_func_calls);
            e.ChangedRange.SetStyle(util.pyLightGreen, reg_c_user_func);

            e.ChangedRange.SetStyle(util.pyclassorange, reg_user_class);

            e.ChangedRange.SetStyle(util.pyPurple, util.CHLmains);
            e.ChangedRange.SetStyle(util.pyPurple, util.CHLinclues);

            e.ChangedRange.SetStyle(util.pyops, util.CHLops);


            e.ChangedRange.SetStyle(util.graystyle, reg_c_comment);
            e.ChangedRange.SetStyle(util.greenstyle, util.CHLstr);



        }


        // -------- PYTHON STUFF --------- //

        //adding value to autocomplete dynamicly
        private void PyDynamicAC(string e)
        {
            
            //Match result_var = Regex.Match(e, @"(^.*?(?=\=)"); // find any string before '='

            Match result_var = Regex.Match(e, @"(\b[a-zA-Z_]+)(\s?=)"); // find any string before '='

            Match result_import = Regex.Match(e, @"\bimport\s([a-zA-Z_0-9_]+)\s"); // find any value string 'import'

            Match result_from = Regex.Match(e, @"\bfrom\s([a-zA-Z_0-9_/.]+)\s"); // find any value string 'from'

            Match result_as = Regex.Match(e, @"\bas\s([a-zA-Z_0-9_]*)\s"); // find any value string 'as'

            Match result_func = Regex.Match(e, @"\bdef\s([a-zA-Z0-9_]+)\(\)"); // find any value string 'def'

            Match result_class = Regex.Match(e, @"\bclass\s([a-zA-Z0-9_]+)(:|\(\))"); // find any string after 'class'

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
                    PyDynamicImport(str);
                   
                }
            }

            //find froms
            if (result_from.Success)
            {
                string str = result_from.Groups[1].Value;
                if (str.Contains("."))
                    str = str.Split('.').First();

                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                    PyDynamicImport(str);
    

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

            //find defs
            if (result_func.Success)
            {
                var str = result_func.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    NewAddedItems.Add(str);
                    AutoCompMenu1.AddItem(str);
                }
            }

            //find classes
            if (result_class.Success)
            {
                var str = result_class.Groups[1].Value;
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
                    if (AutoCompMenu1.Items.Contains(it) && fctb_main.Text.Contains(it) == false && !string.IsNullOrWhiteSpace(it)) //if value contains in autocompmenu and not in editor
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
            e.ChangedRange.ClearStyle(util.pyfuncParam);
            e.ChangedRange.ClearStyle(util.pynums);

            e.ChangedRange.ClearStyle(util.pyOrange);
            e.ChangedRange.ClearStyle(util.pyOrangeRed);
            e.ChangedRange.ClearStyle(util.pyPurple);
            e.ChangedRange.ClearStyle(util.pyBlue);
            e.ChangedRange.ClearStyle(util.pyLightGreen);
            e.ChangedRange.ClearStyle(util.pyclassorange);
            e.ChangedRange.ClearStyle(util.pyops);
            e.ChangedRange.ClearStyle(util.graystyle);
            e.ChangedRange.ClearStyle(util.greenstyle);
           

            e.ChangedRange.SetStyle(util.pyfuncParam, util.pyHLparams);
            e.ChangedRange.SetStyle(util.pynums, util.pyHLnums);
            e.ChangedRange.SetStyle(util.pyOrange, util.pyHLSecOrange);
            e.ChangedRange.SetStyle(util.pyOrangeRed, util.pyHLSecOrangeRed);
            e.ChangedRange.SetStyle(util.pyBlue, util.pyHLSecFuncs);

            e.ChangedRange.SetStyle(util.pyBlue, reg_func);
            e.ChangedRange.SetStyle(util.pyLightGreen, reg_user_func);
            e.ChangedRange.SetStyle(util.pyclassorange, reg_user_class);

            e.ChangedRange.SetStyle(util.pyPurple, util.pyHLmains);

            e.ChangedRange.SetStyle(util.pyops, util.pyHLops);


            e.ChangedRange.SetStyle(util.graystyle, reg);
            e.ChangedRange.SetStyle(util.greenstyle, util.pyHLstr);
           


        }

        private void PyDynamicAC_Other(string e)
        {

            //Match result_var = Regex.Match(e, @"(^.*?(?=\=)"); // find any string before '='

            Match result_var = Regex.Match(e, @"(\b[a-zA-Z_]+)(\s?=)"); // find any string before '='

            Match result_import = Regex.Match(e, @"\bimport\s([a-zA-Z_0-9_]+)\s"); // find any value string 'import'

            Match result_from = Regex.Match(e, @"\bfrom\s([a-zA-Z_0-9_]+)\s"); // find any value string 'from'

            Match result_as = Regex.Match(e, @"\bas\s([a-zA-Z_0-9_]*)\s"); // find any value string 'as'

            Match result_func = Regex.Match(e, @"\bdef\s([a-zA-Z0-9_]+)\(\)"); // find any value string 'def'

            Match result_class = Regex.Match(e, @"\bclass\s([a-zA-Z0-9_]+)(:|\(\))"); // find any string after 'class'

            //Find variables
            if (result_var.Success)
            {
                var str = result_var.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    if (!AutoCompMenu1.Items.Contains(str))
                        AutoCompMenu1.AddItem(str);
                }
            }

            //find Imports
            if (result_import.Success)
            {
                var str = result_import.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    if (!AutoCompMenu1.Items.Contains(str))
                        AutoCompMenu1.AddItem(str);
                }
            }

            //find as
            if (result_as.Success)
            {
                var str = result_as.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {

                    if (!AutoCompMenu1.Items.Contains(str))
                        AutoCompMenu1.AddItem(str);
                }
            }

            //find defs
            if (result_func.Success)
            {
                var str = result_func.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                    if (!AutoCompMenu1.Items.Contains(str))
                        AutoCompMenu1.AddItem(str);
                }
            }

            //find classes
            if (result_class.Success)
            {
                var str = result_class.Groups[1].Value;
                if (!AutoCompMenu1.Items.Contains(str)) //if found string not inside autocompletemenu then add
                {
                   
                    AutoCompMenu1.AddItem(str);

                }
            }
  
    
        }

        // -------- END python syn --------- //

        private void PyDynamicImport(string filename)
        {
            if (!string.IsNullOrEmpty(util.GetSettings("pypath")))
            {
                string filePath = Path.GetDirectoryName(util.GetSettings("pypath")) + "\\Lib\\" + filename + ".py";
                if (File.Exists(filePath))
                {
                    try
                    {
                        var lines = File.ReadLines(filePath);
                        foreach (var line in lines)
                        {
                            PyDynamicAC_Other(line);
                            Debug.WriteLine(line);
                        }
                    }
                    catch
                    {}

                }
               
            }
          
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
            SplitContainer.Panel2Collapsed = SplitContainer.Panel2Collapsed == true ? false : true;
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
                    FileSetup();
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

            if (TextChangedFCTB && !string.IsNullOrEmpty(fctb_main.Text) && isOpenFirstTime == false)
            {
                DialogResult res = MessageBox.Show($"Do you want to save changes to {currFileName}?", "CNote", MessageBoxButtons.YesNoCancel);
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
            ins_list.Add(newMain);
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
            stat_txt.Text = GetEditInfo();
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
        }

        private void aboutCNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutNote an = new AboutNote();
            an.ShowDialog();

        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fctb_main.Undo();
        }

        private void run_strip_menu_Click(object sender, EventArgs e)
        {
            RunClicked();
        }

        private void opencnt_folder_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currFilePath) && Directory.Exists(Path.GetDirectoryName(currFilePath)))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = Path.GetDirectoryName(currFilePath),
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
        }


        //Auto indent for Python and info
        private void fctb_main_KeyPressed(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                stat_txt.Text = GetEditInfo();
                if (python_lang.Checked && pyDoIndent)
                {
                    var line = fctb_main.Selection.FromLine;
                    fctb_main.Selection.tb.InsertText("\t");
                    pyDoIndent = false;
                }
            }
            if (e.KeyChar == (char)Keys.Back)
            {
                stat_txt.Text = GetEditInfo();

            }



        }

        private void fctb_main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                stat_txt.Text = GetEditInfo();

            }

            if (e.KeyCode == Keys.Down)
            {
                stat_txt.Text = GetEditInfo();

            }

            if (e.Control)
            {
                stat_txt.Text = GetEditInfo();
            }
        }

        private void fctb_main_KeyUp(object sender, KeyEventArgs e)
        {
            lastkeycode = e.KeyCode;

            if (e.Control)
            {
                stat_txt.Text = GetEditInfo();
            }


        }

        private void commentSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fctb_main.CommentSelected();
        }

        private void cmdout_KeyDown(object sender, KeyEventArgs e)
        {
            lastkeycode = e.KeyCode;
            if (e.Control && e.KeyCode  == Keys.C)
            {
                cmdout.Copy();
            }
            if (e.Control && e.KeyCode == Keys.P)
            {
                cmdout.Paste();
            }
        }

        private void GetPreviousWindow()
        {

            if (curr_ins_index == 0)
            {
                if (ins_list.Count == 0)
                {
                    Application.OpenForms[0].BringToFront();
                    
                }
                else
                {
                    ins_list[curr_ins_index].BringToFront();
                    curr_ins_index++;
                }

            }
            else
            {
                try
                {
                    ins_list[curr_ins_index + 1].BringToFront();
                }
                catch
                {
                    var form1 = Application.OpenForms[0];
                    form1.BringToFront();
                    curr_ins_index--;
                }
            }

        }

        private void prev_window_tool_Click(object sender, EventArgs e)
        {
            GetPreviousWindow();
        }

        private void build_config_Click(object sender, EventArgs e)
        {
            LangPref lp = new LangPref();
            lp.ShowDialog();
        }


        private void AutoCompMenu1_Selected(object sender, AutocompleteMenuNS.SelectedEventArgs e)
        {
            //snippets for python
            if (isPythonLang)
            {
                if (e.Item.Text == "forrange")
                {
                    fctb_main.ClearCurrentLine();
                    fctb_main.InsertText("\nfor i in range(10):\n\tpass");

                }
                if (e.Item.Text == "ifmain")
                {
                    fctb_main.ClearCurrentLine();
                    fctb_main.InsertText("\nif __name__ == '__main__':\n\tmain()");
                }


            }else if (fctb_main.Language == Language.CSharp && !isCppLang)
            {
                if (e.Item.Text == "forr")
                {
                    fctb_main.ClearCurrentLine();
                    fctb_main.InsertText("\nfor (int i = 0; i < length; i++)\n{\n\t\n}");

                }
                if (e.Item.Text == "cmain")
                {
                    fctb_main.ClearCurrentLine();
                    string className = string.IsNullOrEmpty(currFileName) ? "Hello" : Path.GetFileNameWithoutExtension(currFileName);
                    string nameSpace = "HelloWorld";
                    fctb_main.InsertText($"\nnamespace {nameSpace}\n{{\n\tclass {className} {{\n\t\tstatic void Main(string[] args)\n\t\t{{\n\t\t\tSystem.Console.WriteLine(\"Hello World!\");\n\t\t}}\n\t}}\n}}");



                }
            }

        }

       
    }

}

