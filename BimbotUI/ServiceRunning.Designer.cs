namespace Bimbot.Forms
{
   partial class ServiceRunning
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
         this.progressBar1 = new System.Windows.Forms.ProgressBar();
         this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
         this.SuspendLayout();
         // 
         // progressBar1
         // 
         this.progressBar1.Location = new System.Drawing.Point(12, 113);
         this.progressBar1.Name = "progressBar1";
         this.progressBar1.Size = new System.Drawing.Size(272, 23);
         this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
         this.progressBar1.TabIndex = 0;
         // 
         // fileSystemWatcher1
         // 
         this.fileSystemWatcher1.EnableRaisingEvents = true;
         this.fileSystemWatcher1.SynchronizingObject = this;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 21);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(35, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "label1";
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(296, 162);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.progressBar1);
         this.Name = "Form1";
         this.Text = "Service running";
         ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ProgressBar progressBar1;
      private System.IO.FileSystemWatcher fileSystemWatcher1;
      private System.Windows.Forms.Label label1;
   }
}