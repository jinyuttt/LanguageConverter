namespace TestClient
{
    partial class Form1
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
            this.uC_Test1 = new TestClient.UC_Test();
            this.SuspendLayout();
            // 
            // uC_Test1
            // 
            this.uC_Test1.Location = new System.Drawing.Point(12, 12);
            this.uC_Test1.Name = "uC_Test1";
            this.uC_Test1.Size = new System.Drawing.Size(776, 426);
            this.uC_Test1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uC_Test1);
            this.Name = "Form1";
            this.Text = "测试窗口";
            this.ResumeLayout(false);

        }

        #endregion

        private UC_Test uC_Test1;
    }
}

