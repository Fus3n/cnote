
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
            this.open_file_select = new System.Windows.Forms.Button();
            this.python_path = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.apply_btn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
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
            this.groupBox1.Text = "Paths";
            // 
            // browser_radio
            // 
            this.browser_radio.AutoSize = true;
            this.browser_radio.Location = new System.Drawing.Point(161, 77);
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
            this.default_radio.Location = new System.Drawing.Point(96, 77);
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
            this.label3.Location = new System.Drawing.Point(2, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "HTML Preview";
            // 
            // open_file_select
            // 
            this.open_file_select.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_file_select.Location = new System.Drawing.Point(509, 19);
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
            this.label1.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Python Executable";
            // 
            // apply_btn
            // 
            this.apply_btn.Location = new System.Drawing.Point(390, 96);
            this.apply_btn.Name = "apply_btn";
            this.apply_btn.Size = new System.Drawing.Size(75, 23);
            this.apply_btn.TabIndex = 7;
            this.apply_btn.Text = "Apply";
            this.apply_btn.UseVisualStyleBackColor = true;
            this.apply_btn.Click += new System.EventHandler(this.apply_btn_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(471, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LangPref
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 131);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.apply_btn);
            this.Controls.Add(this.browser_radio);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.default_radio);
            this.Controls.Add(this.label3);
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
            this.PerformLayout();

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
        private System.Windows.Forms.Button button2;
    }
}