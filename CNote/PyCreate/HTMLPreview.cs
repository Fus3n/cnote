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
