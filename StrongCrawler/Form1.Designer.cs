namespace StrongCrawler
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.searchBtn = new System.Windows.Forms.Button();
            this.DetailTxt = new System.Windows.Forms.TextBox();
            this.cityListBox = new System.Windows.Forms.ListBox();
            this.HotelListBox = new System.Windows.Forms.ListBox();
            this.previousBtn = new System.Windows.Forms.Button();
            this.nextBtn = new System.Windows.Forms.Button();
            this.skipTxtbox = new System.Windows.Forms.TextBox();
            this.skipBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // searchBtn
            // 
            this.searchBtn.Location = new System.Drawing.Point(62, 485);
            this.searchBtn.Name = "searchBtn";
            this.searchBtn.Size = new System.Drawing.Size(75, 23);
            this.searchBtn.TabIndex = 0;
            this.searchBtn.Text = "查看";
            this.searchBtn.UseVisualStyleBackColor = true;
            this.searchBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // DetailTxt
            // 
            this.DetailTxt.Location = new System.Drawing.Point(608, 12);
            this.DetailTxt.Multiline = true;
            this.DetailTxt.Name = "DetailTxt";
            this.DetailTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DetailTxt.Size = new System.Drawing.Size(429, 447);
            this.DetailTxt.TabIndex = 2;
            this.DetailTxt.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // cityListBox
            // 
            this.cityListBox.DisplayMember = "CityName";
            this.cityListBox.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cityListBox.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.cityListBox.FormattingEnabled = true;
            this.cityListBox.ItemHeight = 20;
            this.cityListBox.Items.AddRange(new object[] {
            "北京华滨国际大酒店"});
            this.cityListBox.Location = new System.Drawing.Point(62, 13);
            this.cityListBox.Name = "cityListBox";
            this.cityListBox.Size = new System.Drawing.Size(227, 444);
            this.cityListBox.TabIndex = 3;
            this.cityListBox.ValueMember = "Uri";
            this.cityListBox.SelectedIndexChanged += new System.EventHandler(this.cityListBox_SelectedIndexChanged);
            // 
            // HotelListBox
            // 
            this.HotelListBox.DisplayMember = "HotelName";
            this.HotelListBox.FormattingEnabled = true;
            this.HotelListBox.ItemHeight = 12;
            this.HotelListBox.Location = new System.Drawing.Point(295, 13);
            this.HotelListBox.Name = "HotelListBox";
            this.HotelListBox.Size = new System.Drawing.Size(307, 448);
            this.HotelListBox.TabIndex = 4;
            this.HotelListBox.ValueMember = "Uri";
            this.HotelListBox.SelectedIndexChanged += new System.EventHandler(this.HotelListBox_SelectedIndexChanged);
            // 
            // previousBtn
            // 
            this.previousBtn.Enabled = false;
            this.previousBtn.Location = new System.Drawing.Point(608, 485);
            this.previousBtn.Name = "previousBtn";
            this.previousBtn.Size = new System.Drawing.Size(75, 23);
            this.previousBtn.TabIndex = 5;
            this.previousBtn.Text = "上一页";
            this.previousBtn.UseVisualStyleBackColor = true;
            this.previousBtn.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // nextBtn
            // 
            this.nextBtn.Enabled = false;
            this.nextBtn.Location = new System.Drawing.Point(705, 485);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(75, 23);
            this.nextBtn.TabIndex = 6;
            this.nextBtn.Text = "下一页";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // skipTxtbox
            // 
            this.skipTxtbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.skipTxtbox.Location = new System.Drawing.Point(862, 485);
            this.skipTxtbox.Name = "skipTxtbox";
            this.skipTxtbox.Size = new System.Drawing.Size(48, 21);
            this.skipTxtbox.TabIndex = 7;
            this.skipTxtbox.Text = "1";
            this.skipTxtbox.Leave += new System.EventHandler(this.skipTxtbox_Leave);
            // 
            // skipBtn
            // 
            this.skipBtn.Enabled = false;
            this.skipBtn.Location = new System.Drawing.Point(949, 485);
            this.skipBtn.Name = "skipBtn";
            this.skipBtn.Size = new System.Drawing.Size(49, 23);
            this.skipBtn.TabIndex = 8;
            this.skipBtn.Text = "跳转";
            this.skipBtn.UseVisualStyleBackColor = true;
            this.skipBtn.Click += new System.EventHandler(this.skipBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(916, 490);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "页";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 520);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skipBtn);
            this.Controls.Add(this.skipTxtbox);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.previousBtn);
            this.Controls.Add(this.HotelListBox);
            this.Controls.Add(this.cityListBox);
            this.Controls.Add(this.DetailTxt);
            this.Controls.Add(this.searchBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button searchBtn;
        private System.Windows.Forms.TextBox DetailTxt;
        private System.Windows.Forms.ListBox cityListBox;
        private System.Windows.Forms.ListBox HotelListBox;
        private System.Windows.Forms.Button previousBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.TextBox skipTxtbox;
        private System.Windows.Forms.Button skipBtn;
        private System.Windows.Forms.Label label1;
    }
}

