using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CNote
{
    public partial class AboutNote : Form
    {
        public AboutNote()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/Fus3n/cnote");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
