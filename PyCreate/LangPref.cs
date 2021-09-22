using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            apply_btn.Enabled = false;
            if (!string.IsNullOrEmpty(util.GetSettings("pypath")))
            {
                python_path.AppendText(util.GetSettings("pypath"));
            }

            if (util.GetSettings("htmlpref") == "default")
                default_radio.Checked = true;
            else if (util.GetSettings("htmlpref") == "browser")
                browser_radio.Checked = true;

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
