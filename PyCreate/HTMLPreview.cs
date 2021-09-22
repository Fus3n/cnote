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
    public partial class HTMLPreview : Form
    {
        public HTMLPreview()
        {
            InitializeComponent();
            webBrowser1.AllowNavigation = true;
            webBrowser1.AllowWebBrowserDrop = true;



        }


        public void LoadDoc(string f)
        {
            try
            {
                webBrowser1.DocumentText = FileReader(f);
               
              
            }
            catch
            {
                webBrowser1.DocumentText = f;
            }
            
            
        }


        private string FileReader(string path)
        {
            StreamReader sr = new StreamReader(path);
            var content = sr.ReadToEnd();
            sr.Close();
            return content;
        }

        private void HTMLPreview_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void HTMLPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
