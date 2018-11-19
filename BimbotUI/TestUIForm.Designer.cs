namespace Bimbot.BimbotUI
{
   partial class TestUIForm
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
         this.listing1 = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.listing2 = new System.Windows.Forms.ListView();
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.listing3 = new System.Windows.Forms.ListView();
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.SuspendLayout();
         // 
         // listing1
         // 
         this.listing1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
         this.listing1.FullRowSelect = true;
         this.listing1.HideSelection = false;
         this.listing1.Location = new System.Drawing.Point(12, 21);
         this.listing1.MultiSelect = false;
         this.listing1.Name = "listing1";
         this.listing1.ShowGroups = false;
         this.listing1.Size = new System.Drawing.Size(245, 417);
         this.listing1.TabIndex = 4;
         this.listing1.UseCompatibleStateImageBehavior = false;
         this.listing1.View = System.Windows.Forms.View.Details;
         this.listing1.SelectedIndexChanged += new System.EventHandler(this.listing1_SelectedIndexChanged);
         // 
         // columnHeader1
         // 
         this.columnHeader1.Tag = "";
         this.columnHeader1.Text = "Name";
         this.columnHeader1.Width = 238;
         // 
         // listing2
         // 
         this.listing2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
         this.listing2.FullRowSelect = true;
         this.listing2.HideSelection = false;
         this.listing2.Location = new System.Drawing.Point(277, 21);
         this.listing2.MultiSelect = false;
         this.listing2.Name = "listing2";
         this.listing2.ShowGroups = false;
         this.listing2.Size = new System.Drawing.Size(245, 417);
         this.listing2.TabIndex = 4;
         this.listing2.UseCompatibleStateImageBehavior = false;
         this.listing2.View = System.Windows.Forms.View.Details;
         this.listing2.SelectedIndexChanged += new System.EventHandler(this.listing2_SelectedIndexChanged);
         // 
         // columnHeader2
         // 
         this.columnHeader2.Tag = "";
         this.columnHeader2.Text = "Name";
         this.columnHeader2.Width = 238;
         // 
         // listing3
         // 
         this.listing3.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
         this.listing3.FullRowSelect = true;
         this.listing3.HideSelection = false;
         this.listing3.Location = new System.Drawing.Point(547, 21);
         this.listing3.MultiSelect = false;
         this.listing3.Name = "listing3";
         this.listing3.ShowGroups = false;
         this.listing3.Size = new System.Drawing.Size(241, 417);
         this.listing3.TabIndex = 4;
         this.listing3.UseCompatibleStateImageBehavior = false;
         this.listing3.View = System.Windows.Forms.View.Details;
         this.listing3.SelectedIndexChanged += new System.EventHandler(this.listing3_SelectedIndexChanged);
         // 
         // columnHeader3
         // 
         this.columnHeader3.Tag = "";
         this.columnHeader3.Text = "Name";
         this.columnHeader3.Width = 238;
         // 
         // TestUIForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(800, 450);
         this.Controls.Add(this.listing3);
         this.Controls.Add(this.listing2);
         this.Controls.Add(this.listing1);
         this.Name = "TestUIForm";
         this.Text = "TestUIForm";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ListView listing1;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ListView listing2;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ListView listing3;
      private System.Windows.Forms.ColumnHeader columnHeader3;
   }
}