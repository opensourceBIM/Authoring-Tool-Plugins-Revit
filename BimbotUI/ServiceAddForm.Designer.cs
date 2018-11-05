namespace Bimbot.BimbotUI
{
   partial class ServiceAddForm
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
         this.buttonRegister = new System.Windows.Forms.Button();
         this.newTrigger = new System.Windows.Forms.ComboBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.buttonAdd = new System.Windows.Forms.Button();
         this.newToken = new System.Windows.Forms.TextBox();
         this.newSoid = new System.Windows.Forms.TextBox();
         this.serviceProvider = new System.Windows.Forms.TextBox();
         this.userName = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.serviceName = new System.Windows.Forms.TextBox();
         this.button1 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // buttonRegister
         // 
         this.buttonRegister.Location = new System.Drawing.Point(232, 83);
         this.buttonRegister.Name = "buttonRegister";
         this.buttonRegister.Size = new System.Drawing.Size(109, 23);
         this.buttonRegister.TabIndex = 22;
         this.buttonRegister.Text = "Register service";
         this.buttonRegister.UseVisualStyleBackColor = true;
         this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click_1);
         // 
         // newTrigger
         // 
         this.newTrigger.FormattingEnabled = true;
         this.newTrigger.Items.AddRange(new object[] {
            "button click | 1",
            "save project | 2"});
         this.newTrigger.Location = new System.Drawing.Point(67, 112);
         this.newTrigger.Name = "newTrigger";
         this.newTrigger.Size = new System.Drawing.Size(274, 21);
         this.newTrigger.TabIndex = 15;
         this.newTrigger.SelectedIndexChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(11, 164);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(44, 13);
         this.label5.TabIndex = 21;
         this.label5.Text = "Token: ";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 114);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(46, 13);
         this.label3.TabIndex = 16;
         this.label3.Text = "Trigger: ";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(10, 140);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(34, 13);
         this.label4.TabIndex = 20;
         this.label4.Text = "Soid: ";
         // 
         // buttonAdd
         // 
         this.buttonAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonAdd.Location = new System.Drawing.Point(232, 196);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(109, 23);
         this.buttonAdd.TabIndex = 17;
         this.buttonAdd.Text = "OK";
         this.buttonAdd.UseVisualStyleBackColor = true;
         this.buttonAdd.Click += new System.EventHandler(this.AddServiceClick);
         // 
         // newToken
         // 
         this.newToken.Location = new System.Drawing.Point(67, 162);
         this.newToken.Margin = new System.Windows.Forms.Padding(2);
         this.newToken.Name = "newToken";
         this.newToken.Size = new System.Drawing.Size(274, 20);
         this.newToken.TabIndex = 18;
         this.newToken.TextChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // newSoid
         // 
         this.newSoid.Location = new System.Drawing.Point(67, 138);
         this.newSoid.Margin = new System.Windows.Forms.Padding(2);
         this.newSoid.Name = "newSoid";
         this.newSoid.Size = new System.Drawing.Size(274, 20);
         this.newSoid.TabIndex = 19;
         this.newSoid.TextChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // serviceProvider
         // 
         this.serviceProvider.Location = new System.Drawing.Point(67, 33);
         this.serviceProvider.Margin = new System.Windows.Forms.Padding(2);
         this.serviceProvider.Name = "serviceProvider";
         this.serviceProvider.ReadOnly = true;
         this.serviceProvider.Size = new System.Drawing.Size(274, 20);
         this.serviceProvider.TabIndex = 27;
         // 
         // userName
         // 
         this.userName.Location = new System.Drawing.Point(67, 57);
         this.userName.Margin = new System.Windows.Forms.Padding(2);
         this.userName.Name = "userName";
         this.userName.ReadOnly = true;
         this.userName.Size = new System.Drawing.Size(274, 20);
         this.userName.TabIndex = 26;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(10, 35);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(52, 13);
         this.label6.TabIndex = 28;
         this.label6.Text = "Provider: ";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(12, 9);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(49, 13);
         this.label2.TabIndex = 24;
         this.label2.Text = "Service: ";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(11, 59);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(35, 13);
         this.label1.TabIndex = 29;
         this.label1.Text = "User: ";
         // 
         // serviceName
         // 
         this.serviceName.Location = new System.Drawing.Point(67, 9);
         this.serviceName.Margin = new System.Windows.Forms.Padding(2);
         this.serviceName.Name = "serviceName";
         this.serviceName.ReadOnly = true;
         this.serviceName.Size = new System.Drawing.Size(274, 20);
         this.serviceName.TabIndex = 27;
         // 
         // button1
         // 
         this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.button1.Location = new System.Drawing.Point(67, 196);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(109, 23);
         this.button1.TabIndex = 17;
         this.button1.Text = "Cancel";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // ServiceAddForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(352, 231);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.userName);
         this.Controls.Add(this.serviceName);
         this.Controls.Add(this.serviceProvider);
         this.Controls.Add(this.buttonRegister);
         this.Controls.Add(this.newTrigger);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.buttonAdd);
         this.Controls.Add(this.newToken);
         this.Controls.Add(this.newSoid);
         this.Name = "ServiceAddForm";
         this.Text = "Register the service and add";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button buttonRegister;
      private System.Windows.Forms.ComboBox newTrigger;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button buttonAdd;
      private System.Windows.Forms.TextBox newToken;
      private System.Windows.Forms.TextBox newSoid;
      private System.Windows.Forms.TextBox serviceProvider;
      private System.Windows.Forms.TextBox userName;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox serviceName;
      private System.Windows.Forms.Button button1;
   }
}