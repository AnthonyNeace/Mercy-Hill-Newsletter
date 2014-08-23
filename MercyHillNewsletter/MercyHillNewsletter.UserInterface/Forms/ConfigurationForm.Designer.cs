namespace MercyHillNewsletter.UserInterface
{
    partial class ConfigurationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewsletterUrl = new System.Windows.Forms.TextBox();
            this.txtWriteDirectory = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNewsletterFeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Newsletter Url";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Write To Directory";
            // 
            // txtNewsletterUrl
            // 
            this.txtNewsletterUrl.Location = new System.Drawing.Point(132, 53);
            this.txtNewsletterUrl.Name = "txtNewsletterUrl";
            this.txtNewsletterUrl.Size = new System.Drawing.Size(420, 20);
            this.txtNewsletterUrl.TabIndex = 2;
            // 
            // txtWriteDirectory
            // 
            this.txtWriteDirectory.Location = new System.Drawing.Point(132, 79);
            this.txtWriteDirectory.Name = "txtWriteDirectory";
            this.txtWriteDirectory.Size = new System.Drawing.Size(420, 20);
            this.txtWriteDirectory.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(402, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Update (Session)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNewsletterFeed
            // 
            this.txtNewsletterFeed.Location = new System.Drawing.Point(132, 27);
            this.txtNewsletterFeed.Name = "txtNewsletterFeed";
            this.txtNewsletterFeed.Size = new System.Drawing.Size(420, 20);
            this.txtNewsletterFeed.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Newsletter Feed";
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 151);
            this.Controls.Add(this.txtNewsletterFeed);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtWriteDirectory);
            this.Controls.Add(this.txtNewsletterUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ConfigurationForm";
            this.Text = "Configuration";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewsletterUrl;
        private System.Windows.Forms.TextBox txtWriteDirectory;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtNewsletterFeed;
        private System.Windows.Forms.Label label3;
    }
}