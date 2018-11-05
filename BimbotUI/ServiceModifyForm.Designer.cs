namespace Bimbot.BimbotUI
{
   partial class ServiceModifyForm
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
         this.listActiveServices = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.serviceTrigger = new System.Windows.Forms.ComboBox();
         this.changeButton = new System.Windows.Forms.Button();
         this.deleteButton = new System.Windows.Forms.Button();
         this.serviceToken = new System.Windows.Forms.TextBox();
         this.label9 = new System.Windows.Forms.Label();
         this.serviceSoid = new System.Windows.Forms.TextBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.serviceUrl = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.serviceDescription = new System.Windows.Forms.TextBox();
         this.serviceName = new System.Windows.Forms.TextBox();
         this.label8 = new System.Windows.Forms.Label();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.listActiveServices);
         this.groupBox1.Controls.Add(this.serviceTrigger);
         this.groupBox1.Controls.Add(this.changeButton);
         this.groupBox1.Controls.Add(this.deleteButton);
         this.groupBox1.Controls.Add(this.serviceToken);
         this.groupBox1.Controls.Add(this.label9);
         this.groupBox1.Controls.Add(this.serviceSoid);
         this.groupBox1.Controls.Add(this.label10);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.serviceUrl);
         this.groupBox1.Controls.Add(this.label6);
         this.groupBox1.Controls.Add(this.label7);
         this.groupBox1.Controls.Add(this.serviceDescription);
         this.groupBox1.Controls.Add(this.serviceName);
         this.groupBox1.Controls.Add(this.label8);
         this.groupBox1.Location = new System.Drawing.Point(9, 10);
         this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
         this.groupBox1.Size = new System.Drawing.Size(564, 491);
         this.groupBox1.TabIndex = 11;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Configured services";
         // 
         // listActiveServices
         // 
         this.listActiveServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
         this.listActiveServices.FullRowSelect = true;
         this.listActiveServices.HideSelection = false;
         this.listActiveServices.Location = new System.Drawing.Point(11, 18);
         this.listActiveServices.MultiSelect = false;
         this.listActiveServices.Name = "listActiveServices";
         this.listActiveServices.ShowGroups = false;
         this.listActiveServices.Size = new System.Drawing.Size(279, 458);
         this.listActiveServices.TabIndex = 31;
         this.listActiveServices.UseCompatibleStateImageBehavior = false;
         this.listActiveServices.View = System.Windows.Forms.View.Details;
         this.listActiveServices.SelectedIndexChanged += new System.EventHandler(this.ListActive_SelectedIndexChanged);
         // 
         // columnHeader1
         // 
         this.columnHeader1.Tag = "";
         this.columnHeader1.Text = "Name";
         this.columnHeader1.Width = 100;
         // 
         // columnHeader2
         // 
         this.columnHeader2.Tag = "";
         this.columnHeader2.Text = "State";
         this.columnHeader2.Width = 277;
         // 
         // serviceTrigger
         // 
         this.serviceTrigger.FormattingEnabled = true;
         this.serviceTrigger.Items.AddRange(new object[] {
            "button click | 1",
            "save project | 2"});
         this.serviceTrigger.Location = new System.Drawing.Point(307, 200);
         this.serviceTrigger.Name = "serviceTrigger";
         this.serviceTrigger.Size = new System.Drawing.Size(239, 21);
         this.serviceTrigger.TabIndex = 30;
         this.serviceTrigger.SelectedIndexChanged += new System.EventHandler(this.ServiceTrigger_SelectedIndexChanged);
         // 
         // changeButton
         // 
         this.changeButton.Location = new System.Drawing.Point(310, 423);
         this.changeButton.Name = "changeButton";
         this.changeButton.Size = new System.Drawing.Size(130, 23);
         this.changeButton.TabIndex = 29;
         this.changeButton.Text = "Change";
         this.changeButton.UseVisualStyleBackColor = true;
         this.changeButton.Click += new System.EventHandler(this.ChangeButton_Click);
         // 
         // deleteButton
         // 
         this.deleteButton.Location = new System.Drawing.Point(310, 453);
         this.deleteButton.Name = "deleteButton";
         this.deleteButton.Size = new System.Drawing.Size(130, 23);
         this.deleteButton.TabIndex = 12;
         this.deleteButton.Text = "Delete";
         this.deleteButton.UseVisualStyleBackColor = true;
         this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
         // 
         // serviceToken
         // 
         this.serviceToken.Location = new System.Drawing.Point(310, 285);
         this.serviceToken.Margin = new System.Windows.Forms.Padding(2);
         this.serviceToken.Name = "serviceToken";
         this.serviceToken.Size = new System.Drawing.Size(237, 20);
         this.serviceToken.TabIndex = 28;
         this.serviceToken.TextChanged += new System.EventHandler(this.ServiceToken_TextChanged);
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Location = new System.Drawing.Point(307, 270);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(44, 13);
         this.label9.TabIndex = 27;
         this.label9.Text = "Token: ";
         // 
         // serviceSoid
         // 
         this.serviceSoid.Location = new System.Drawing.Point(310, 242);
         this.serviceSoid.Margin = new System.Windows.Forms.Padding(2);
         this.serviceSoid.Name = "serviceSoid";
         this.serviceSoid.Size = new System.Drawing.Size(237, 20);
         this.serviceSoid.TabIndex = 26;
         this.serviceSoid.TextChanged += new System.EventHandler(this.ServiceSoid_TextChanged);
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Location = new System.Drawing.Point(307, 227);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(34, 13);
         this.label10.TabIndex = 25;
         this.label10.Text = "Soid: ";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(307, 184);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(46, 13);
         this.label1.TabIndex = 23;
         this.label1.Text = "Trigger: ";
         // 
         // hostName
         // 
         this.serviceUrl.Location = new System.Drawing.Point(310, 156);
         this.serviceUrl.Margin = new System.Windows.Forms.Padding(2);
         this.serviceUrl.Name = "hostName";
         this.serviceUrl.ReadOnly = true;
         this.serviceUrl.Size = new System.Drawing.Size(237, 20);
         this.serviceUrl.TabIndex = 22;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(307, 61);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(66, 13);
         this.label6.TabIndex = 20;
         this.label6.Text = "Description: ";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(307, 17);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(41, 13);
         this.label7.TabIndex = 19;
         this.label7.Text = "Name: ";
         // 
         // serviceDescription
         // 
         this.serviceDescription.Location = new System.Drawing.Point(310, 76);
         this.serviceDescription.Margin = new System.Windows.Forms.Padding(2);
         this.serviceDescription.Multiline = true;
         this.serviceDescription.Name = "serviceDescription";
         this.serviceDescription.ReadOnly = true;
         this.serviceDescription.Size = new System.Drawing.Size(237, 54);
         this.serviceDescription.TabIndex = 17;
         // 
         // serviceName
         // 
         this.serviceName.Location = new System.Drawing.Point(310, 32);
         this.serviceName.Margin = new System.Windows.Forms.Padding(2);
         this.serviceName.Name = "serviceName";
         this.serviceName.ReadOnly = true;
         this.serviceName.Size = new System.Drawing.Size(237, 20);
         this.serviceName.TabIndex = 18;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(307, 141);
         this.label8.Name = "label8";
         this.label8.Size = new System.Drawing.Size(35, 13);
         this.label8.TabIndex = 16;
         this.label8.Text = "Host: ";
         // 
         // ServiceModifyForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(584, 521);
         this.Controls.Add(this.groupBox1);
         this.Name = "ServiceModifyForm";
         this.Text = "BimBotSetupForm";
         this.Load += new System.EventHandler(this.ServiceSetupForm_Load);
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.TextBox serviceToken;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.TextBox serviceSoid;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox serviceUrl;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox serviceDescription;
      private System.Windows.Forms.TextBox serviceName;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Button changeButton;
      private System.Windows.Forms.Button deleteButton;
      private System.Windows.Forms.ComboBox serviceTrigger;
      private System.Windows.Forms.ListView listActiveServices;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
   }
}