namespace LanguageConverter
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOPen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_XML = new System.Windows.Forms.Button();
            this.btnSrc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOPen
            // 
            this.btnOPen.Location = new System.Drawing.Point(637, 52);
            this.btnOPen.Name = "btnOPen";
            this.btnOPen.Size = new System.Drawing.Size(75, 23);
            this.btnOPen.TabIndex = 0;
            this.btnOPen.Text = "文件";
            this.btnOPen.UseVisualStyleBackColor = true;
            this.btnOPen.Click += new System.EventHandler(this.BtnOPen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog";
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(167, 50);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(455, 25);
            this.txtFile.TabIndex = 1;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(314, 336);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "测试";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "程序集目录：";
            // 
            // btn_XML
            // 
            this.btn_XML.Location = new System.Drawing.Point(304, 123);
            this.btn_XML.Name = "btn_XML";
            this.btn_XML.Size = new System.Drawing.Size(75, 23);
            this.btn_XML.TabIndex = 4;
            this.btn_XML.Text = "转换XML文件";
            this.btn_XML.UseVisualStyleBackColor = true;
            this.btn_XML.Click += new System.EventHandler(this.Btn_XML_Click);
            // 
            // btnSrc
            // 
            this.btnSrc.Location = new System.Drawing.Point(416, 123);
            this.btnSrc.Name = "btnSrc";
            this.btnSrc.Size = new System.Drawing.Size(75, 23);
            this.btnSrc.TabIndex = 7;
            this.btnSrc.Text = "生成源文件";
            this.btnSrc.UseVisualStyleBackColor = true;
            this.btnSrc.Click += new System.EventHandler(this.btnSrc_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 423);
            this.Controls.Add(this.btnSrc);
            this.Controls.Add(this.btn_XML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnOPen);
            this.Name = "FrmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOPen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_XML;
        private System.Windows.Forms.Button btnSrc;
    }
}

