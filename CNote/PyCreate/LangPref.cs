/*
 Copyright 2021 Fusen
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.IO;
using System.Windows.Forms;

namespace CNote
{
    public partial class LangPref : Form
    {
        CNoteUtils util;
        public LangPref()
        {
            InitializeComponent();
            util = new CNoteUtils();
        }
       
        private void LangPref_Load(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(util.GetSettings("pypath")))
            {
                python_path.AppendText(util.GetSettings("pypath"));
            }

            if (util.GetSettings("htmlpref") == "default")
                default_radio.Checked = true;
            else if (util.GetSettings("htmlpref") == "browser")
                browser_radio.Checked = true;
            apply_btn.Enabled = false;
        }

        private void python_path_TextChanged(object sender, EventArgs e)
        {
            apply_btn.Enabled = true;
        }

        private void open_file_select_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "exe File |*.exe";

            if (of.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(of.FileName))
                {
                    python_path.Text = of.FileName;
                    util.AOUSettings("pypath", of.FileName);
                }
                   
               
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (default_radio.Checked)
                util.AOUSettings("htmlpref","default");
            else
                util.AOUSettings("htmlpref", "browser");
        }

        private void browser_radio_CheckedChanged(object sender, EventArgs e)
        {
            if (browser_radio.Checked)
                util.AOUSettings("htmlpref", "browser");
            else
                util.AOUSettings("htmlpref", "default");
        }

        private void apply_btn_Click(object sender, EventArgs e)
        {
            if (File.Exists(python_path.Text))
            {
                util.AOUSettings("pypath", python_path.Text);
                apply_btn.Enabled = false;
            }
            else
            {
                MessageBox.Show("The Path Does not exists", "CNote");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
        
        }
    }
}
