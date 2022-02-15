
namespace QRform
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.QRBox = new System.Windows.Forms.PictureBox();
            this.CorrectionComboBox = new System.Windows.Forms.ComboBox();
            this.CreateButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.CopyButton = new System.Windows.Forms.Button();
            this.RequestLabel = new System.Windows.Forms.Label();
            this.RequestTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.QRBox)).BeginInit();
            this.SuspendLayout();
            // 
            // QRBox
            // 
            this.QRBox.Location = new System.Drawing.Point(12, 12);
            this.QRBox.Name = "QRBox";
            this.QRBox.Size = new System.Drawing.Size(420, 420);
            this.QRBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.QRBox.TabIndex = 0;
            this.QRBox.TabStop = false;
            // 
            // CorrectionComboBox
            // 
            this.CorrectionComboBox.FormattingEnabled = true;
            this.CorrectionComboBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.CorrectionComboBox.Items.AddRange(new object[] {
            "L",
            "M",
            "Q",
            "H"});
            this.CorrectionComboBox.Location = new System.Drawing.Point(514, 243);
            this.CorrectionComboBox.Name = "CorrectionComboBox";
            this.CorrectionComboBox.Size = new System.Drawing.Size(200, 21);
            this.CorrectionComboBox.TabIndex = 1;
            this.CorrectionComboBox.TabStop = false;
            this.CorrectionComboBox.Text = "Choose correction level";
            // 
            // CreateButton
            // 
            this.CreateButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CreateButton.Location = new System.Drawing.Point(503, 330);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(224, 30);
            this.CreateButton.TabIndex = 2;
            this.CreateButton.TabStop = false;
            this.CreateButton.Text = "Create QR-code";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(503, 366);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(224, 30);
            this.SaveButton.TabIndex = 3;
            this.SaveButton.TabStop = false;
            this.SaveButton.Text = "Save QR-code";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // CopyButton
            // 
            this.CopyButton.Location = new System.Drawing.Point(503, 402);
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new System.Drawing.Size(224, 30);
            this.CopyButton.TabIndex = 4;
            this.CopyButton.TabStop = false;
            this.CopyButton.Text = "Copy QR-code";
            this.CopyButton.UseVisualStyleBackColor = true;
            this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
            // 
            // RequestLabel
            // 
            this.RequestLabel.AutoSize = true;
            this.RequestLabel.Location = new System.Drawing.Point(500, 80);
            this.RequestLabel.Name = "RequestLabel";
            this.RequestLabel.Size = new System.Drawing.Size(49, 13);
            this.RequestLabel.TabIndex = 5;
            this.RequestLabel.Text = "Your text";
            // 
            // RequestTextBox
            // 
            this.RequestTextBox.Location = new System.Drawing.Point(503, 96);
            this.RequestTextBox.Multiline = true;
            this.RequestTextBox.Name = "RequestTextBox";
            this.RequestTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RequestTextBox.Size = new System.Drawing.Size(224, 68);
            this.RequestTextBox.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 451);
            this.Controls.Add(this.RequestTextBox);
            this.Controls.Add(this.RequestLabel);
            this.Controls.Add(this.CopyButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.CorrectionComboBox);
            this.Controls.Add(this.QRBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QR-coder";
            ((System.ComponentModel.ISupportInitialize)(this.QRBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox QRBox;
        private System.Windows.Forms.ComboBox CorrectionComboBox;
        private System.Windows.Forms.Button CreateButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button CopyButton;
        private System.Windows.Forms.Label RequestLabel;
        private System.Windows.Forms.TextBox RequestTextBox;
    }
}

