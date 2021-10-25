
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
            this.open_file_select = new System.Windows.Forms.Button();
            this.python_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.browser_radio = new System.Windows.Forms.RadioButton();
            this.default_radio = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.apply_btn = new System.Windows.Forms.Button();
            this.cancel_btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.custom_cmd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.html_help = new System.Windows.Forms.Button();
            this.python_help = new System.Windows.Forms.Button();
            this.customcmd_help = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.open_file_select);
            this.groupBox1.Controls.Add(this.python_path);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(548, 62);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Python";
            // 
            // open_file_select
            // 
            this.open_file_select.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_file_select.Location = new System.Drawing.Point(510, 16);
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
            this.python_path.Location = new System.Drawing.Point(120, 19);
            this.python_path.Name = "python_path";
            this.python_path.Size = new System.Drawing.Size(383, 20);
            this.python_path.TabIndex = 1;
            this.python_path.TextChanged += new System.EventHandler(this.python_path_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Python Executable";
            // 
            // browser_radio
            // 
            this.browser_radio.AutoSize = true;
            this.browser_radio.Location = new System.Drawing.Point(165, 15);
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
            this.default_radio.Location = new System.Drawing.Point(100, 15);
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
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "HTML Preview";
            // 
            // apply_btn
            // 
            this.apply_btn.Enabled = false;
            this.apply_btn.Location = new System.Drawing.Point(33, 18);
            this.apply_btn.Name = "apply_btn";
            this.apply_btn.Size = new System.Drawing.Size(75, 23);
            this.apply_btn.TabIndex = 7;
            this.apply_btn.Text = "Apply";
            this.apply_btn.UseVisualStyleBackColor = true;
            this.apply_btn.Click += new System.EventHandler(this.apply_btn_Click);
            // 
            // cancel_btn
            // 
            this.cancel_btn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel_btn.Location = new System.Drawing.Point(114, 18);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.cancel_btn.TabIndex = 8;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.default_radio);
            this.groupBox2.Controls.Add(this.browser_radio);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(5, 80);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(548, 47);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HTML";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cancel_btn);
            this.panel1.Controls.Add(this.apply_btn);
            this.panel1.Location = new System.Drawing.Point(378, 233);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 52);
            this.panel1.TabIndex = 9;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.custom_cmd);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(5, 133);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(548, 62);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Command";
            // 
            // custom_cmd
            // 
            this.custom_cmd.AccessibleDescription = "If you wanna run files other then python put the Command here the result will be " +
    "showen in Ouput panel ";
            this.custom_cmd.AccessibleName = "Custom Run command";
            this.custom_cmd.BackColor = System.Drawing.SystemColors.ControlLight;
            this.custom_cmd.Location = new System.Drawing.Point(82, 11);
            this.custom_cmd.Multiline = true;
            this.custom_cmd.Name = "custom_cmd";
            this.custom_cmd.Size = new System.Drawing.Size(457, 45);
            this.custom_cmd.TabIndex = 1;
            this.custom_cmd.TextChanged += new System.EventHandler(this.custom_cmd_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 30);
            this.label2.TabIndex = 0;
            this.label2.Text = "Custom Run \r\nCommand";
            // 
            // html_help
            // 
            this.html_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.html_help.Location = new System.Drawing.Point(559, 96);
            this.html_help.Name = "html_help";
            this.html_help.Size = new System.Drawing.Size(23, 25);
            this.html_help.TabIndex = 2;
            this.html_help.Text = "?";
            this.html_help.UseVisualStyleBackColor = true;
            this.html_help.Click += new System.EventHandler(this.html_help_Click);
            // 
            // python_help
            // 
            this.python_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.python_help.Location = new System.Drawing.Point(559, 32);
            this.python_help.Name = "python_help";
            this.python_help.Size = new System.Drawing.Size(23, 25);
            this.python_help.TabIndex = 7;
            this.python_help.Text = "?";
            this.python_help.UseVisualStyleBackColor = true;
            this.python_help.Click += new System.EventHandler(this.python_help_Click);
            // 
            // customcmd_help
            // 
            this.customcmd_help.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customcmd_help.Location = new System.Drawing.Point(559, 153);
            this.customcmd_help.Name = "customcmd_help";
            this.customcmd_help.Size = new System.Drawing.Size(23, 25);
            this.customcmd_help.TabIndex = 10;
            this.customcmd_help.Text = "?";
            this.customcmd_help.UseVisualStyleBackColor = true;
            this.customcmd_help.Click += new System.EventHandler(this.customcmd_help_Click);
            // 
            // LangPref
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 286);
            this.Controls.Add(this.customcmd_help);
            this.Controls.Add(this.html_help);
            this.Controls.Add(this.python_help);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LangPref";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Build Configuration";
            this.Load += new System.EventHandler(this.LangPref_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox python_path;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button open_file_select;
        private System.Windows.Forms.RadioButton browser_radio;
        private System.Windows.Forms.RadioButton default_radio;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button apply_btn;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox custom_cmd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button html_help;
        private System.Windows.Forms.Button python_help;
        private System.Windows.Forms.Button customcmd_help;
    }
}