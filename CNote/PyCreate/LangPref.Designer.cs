
namespace CNote
{
    partial class LangPref
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.browser_radio = new System.Windows.Forms.RadioButton();
            this.default_radio = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.open_file_select = new System.Windows.Forms.Button();
            this.python_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.browser_radio);
            this.groupBox1.Controls.Add(this.default_radio);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.open_file_select);
            this.groupBox1.Controls.Add(this.python_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(536, 109);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Paths";
            // 
            // browser_radio
            // 
            this.browser_radio.AutoSize = true;
            this.browser_radio.Location = new System.Drawing.Point(166, 51);
            this.browser_radio.Name = "browser_radio";
            this.browser_radio.Size = new System.Drawing.Size(63, 17);
            this.browser_radio.TabIndex = 6;
            this.browser_radio.TabStop = true;
            this.browser_radio.Text = "Browser";
            this.browser_radio.UseVisualStyleBackColor = true;
            this.browser_radio.CheckedChanged += new System.EventHandler(this.browser_radio_CheckedChanged);
            // 
            // default_radio
            // 
            this.default_radio.AutoSize = true;
            this.default_radio.Location = new System.Drawing.Point(101, 51);
            this.default_radio.Name = "default_radio";
            this.default_radio.Size = new System.Drawing.Size(59, 17);
            this.default_radio.TabIndex = 5;
            this.default_radio.TabStop = true;
            this.default_radio.Text = "Default";
            this.default_radio.UseVisualStyleBackColor = true;
            this.default_radio.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(7, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "HTML Preview";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "More coming soon";
            // 
            // open_file_select
            // 
            this.open_file_select.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_file_select.Location = new System.Drawing.Point(488, 19);
            this.open_file_select.Name = "open_file_select";
            this.open_file_select.Size = new System.Drawing.Size(32, 23);
            this.open_file_select.TabIndex = 2;
            this.open_file_select.Text = "...";
            this.open_file_select.UseVisualStyleBackColor = true;
            this.open_file_select.Click += new System.EventHandler(this.open_file_select_Click);
            // 
            // python_path
            // 
            this.python_path.BackColor = System.Drawing.SystemColors.ControlLight;
            this.python_path.Location = new System.Drawing.Point(79, 19);
            this.python_path.Name = "python_path";
            this.python_path.Size = new System.Drawing.Size(403, 20);
            this.python_path.TabIndex = 1;
            this.python_path.TextChanged += new System.EventHandler(this.python_path_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Python Path";
            // 
            // LangPref
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 144);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LangPref";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Language Preference";
            this.Load += new System.EventHandler(this.LangPref_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox python_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button open_file_select;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton browser_radio;
        private System.Windows.Forms.RadioButton default_radio;
        private System.Windows.Forms.Label label3;
    }
}