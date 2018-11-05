namespace Bimbot.BimbotUI
{
   partial class ServiceAddFormOld
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
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.buttonRegister = new System.Windows.Forms.Button();
         this.listAvailableServices = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.newTrigger = new System.Windows.Forms.ComboBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.buttonAdd = new System.Windows.Forms.Button();
         this.newToken = new System.Windows.Forms.TextBox();
         this.newSoid = new System.Windows.Forms.TextBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.comboBox2 = new System.Windows.Forms.ComboBox();
         this.comboBox1 = new System.Windows.Forms.ComboBox();
         this.label8 = new System.Windows.Forms.Label();
         this.label7 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.trackBar2 = new System.Windows.Forms.TrackBar();
         this.trackBar1 = new System.Windows.Forms.TrackBar();
         this.label1 = new System.Windows.Forms.Label();
         this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.button1 = new System.Windows.Forms.Button();
         this.groupBox2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
         this.SuspendLayout();
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.button1);
         this.groupBox2.Controls.Add(this.buttonRegister);
         this.groupBox2.Controls.Add(this.listAvailableServices);
         this.groupBox2.Controls.Add(this.newTrigger);
         this.groupBox2.Controls.Add(this.label5);
         this.groupBox2.Controls.Add(this.label3);
         this.groupBox2.Controls.Add(this.label4);
         this.groupBox2.Controls.Add(this.buttonAdd);
         this.groupBox2.Controls.Add(this.newToken);
         this.groupBox2.Controls.Add(this.newSoid);
         this.groupBox2.Location = new System.Drawing.Point(11, 11);
         this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
         this.groupBox2.Size = new System.Drawing.Size(392, 481);
         this.groupBox2.TabIndex = 13;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Available Services";
         // 
         // buttonRegister
         // 
         this.buttonRegister.Location = new System.Drawing.Point(271, 347);
         this.buttonRegister.Name = "buttonRegister";
         this.buttonRegister.Size = new System.Drawing.Size(109, 23);
         this.buttonRegister.TabIndex = 11;
         this.buttonRegister.Text = "Register  service";
         this.buttonRegister.UseVisualStyleBackColor = true;
         this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
         // 
         // listAvailableServices
         // 
         this.listAvailableServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
         this.listAvailableServices.FullRowSelect = true;
         this.listAvailableServices.HideSelection = false;
         this.listAvailableServices.Location = new System.Drawing.Point(14, 20);
         this.listAvailableServices.MultiSelect = false;
         this.listAvailableServices.Name = "listAvailableServices";
         this.listAvailableServices.ShowGroups = false;
         this.listAvailableServices.Size = new System.Drawing.Size(366, 321);
         this.listAvailableServices.TabIndex = 3;
         this.listAvailableServices.UseCompatibleStateImageBehavior = false;
         this.listAvailableServices.View = System.Windows.Forms.View.Details;
         this.listAvailableServices.SelectedIndexChanged += new System.EventHandler(this.listAvailableServices_SelectedIndexChanged);
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
         this.columnHeader2.Text = "Description";
         this.columnHeader2.Width = 277;
         // 
         // newTrigger
         // 
         this.newTrigger.FormattingEnabled = true;
         this.newTrigger.Items.AddRange(new object[] {
            "button click | 1",
            "save project | 2"});
         this.newTrigger.Location = new System.Drawing.Point(106, 376);
         this.newTrigger.Name = "newTrigger";
         this.newTrigger.Size = new System.Drawing.Size(274, 21);
         this.newTrigger.TabIndex = 5;
         this.newTrigger.SelectedIndexChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(11, 428);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(44, 13);
         this.label5.TabIndex = 10;
         this.label5.Text = "Token: ";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(12, 378);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(46, 13);
         this.label3.TabIndex = 6;
         this.label3.Text = "Trigger: ";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(10, 404);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(34, 13);
         this.label4.TabIndex = 9;
         this.label4.Text = "Soid: ";
         // 
         // buttonAdd
         // 
         this.buttonAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.buttonAdd.Location = new System.Drawing.Point(271, 451);
         this.buttonAdd.Name = "buttonAdd";
         this.buttonAdd.Size = new System.Drawing.Size(109, 23);
         this.buttonAdd.TabIndex = 7;
         this.buttonAdd.Text = "Add service";
         this.buttonAdd.UseVisualStyleBackColor = true;
         this.buttonAdd.Click += new System.EventHandler(this.AddServiceClick);
         // 
         // newToken
         // 
         this.newToken.Location = new System.Drawing.Point(106, 426);
         this.newToken.Margin = new System.Windows.Forms.Padding(2);
         this.newToken.Name = "newToken";
         this.newToken.Size = new System.Drawing.Size(274, 20);
         this.newToken.TabIndex = 8;
         this.newToken.TextChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // newSoid
         // 
         this.newSoid.Location = new System.Drawing.Point(106, 402);
         this.newSoid.Margin = new System.Windows.Forms.Padding(2);
         this.newSoid.Name = "newSoid";
         this.newSoid.Size = new System.Drawing.Size(274, 20);
         this.newSoid.TabIndex = 8;
         this.newSoid.TextChanged += new System.EventHandler(this.EnableDisableAddButton);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.comboBox2);
         this.groupBox1.Controls.Add(this.comboBox1);
         this.groupBox1.Controls.Add(this.label8);
         this.groupBox1.Controls.Add(this.label7);
         this.groupBox1.Controls.Add(this.label6);
         this.groupBox1.Controls.Add(this.label2);
         this.groupBox1.Controls.Add(this.trackBar2);
         this.groupBox1.Controls.Add(this.trackBar1);
         this.groupBox1.Controls.Add(this.label1);
         this.groupBox1.Controls.Add(this.checkedListBox1);
         this.groupBox1.Controls.Add(this.checkBox1);
         this.groupBox1.Location = new System.Drawing.Point(409, 13);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(163, 479);
         this.groupBox1.TabIndex = 14;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "groupBox1";
         // 
         // comboBox2
         // 
         this.comboBox2.FormattingEnabled = true;
         this.comboBox2.Location = new System.Drawing.Point(10, 303);
         this.comboBox2.Name = "comboBox2";
         this.comboBox2.Size = new System.Drawing.Size(121, 21);
         this.comboBox2.TabIndex = 6;
         // 
         // comboBox1
         // 
         this.comboBox1.FormattingEnabled = true;
         this.comboBox1.Location = new System.Drawing.Point(10, 263);
         this.comboBox1.Name = "comboBox1";
         this.comboBox1.Size = new System.Drawing.Size(121, 21);
         this.comboBox1.TabIndex = 6;
         // 
         // label8
         // 
         this.label8.AutoSize = true;
         this.label8.Location = new System.Drawing.Point(7, 287);
         this.label8.Name = "label8";
         this.label8.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.label8.Size = new System.Drawing.Size(65, 13);
         this.label8.TabIndex = 5;
         this.label8.Text = "Output type:";
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(7, 247);
         this.label7.Name = "label7";
         this.label7.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.label7.Size = new System.Drawing.Size(57, 13);
         this.label7.TabIndex = 5;
         this.label7.Text = "Input type:";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(7, 184);
         this.label6.Name = "label6";
         this.label6.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.label6.Size = new System.Drawing.Size(54, 13);
         this.label6.TabIndex = 5;
         this.label6.Text = "Max Price";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 136);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(68, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "Minimal price";
         // 
         // trackBar2
         // 
         this.trackBar2.Location = new System.Drawing.Point(10, 203);
         this.trackBar2.Name = "trackBar2";
         this.trackBar2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.trackBar2.Size = new System.Drawing.Size(104, 45);
         this.trackBar2.TabIndex = 4;
         this.trackBar2.Tag = "";
         // 
         // trackBar1
         // 
         this.trackBar1.Location = new System.Drawing.Point(10, 152);
         this.trackBar1.Name = "trackBar1";
         this.trackBar1.Size = new System.Drawing.Size(104, 45);
         this.trackBar1.TabIndex = 3;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 25);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(49, 13);
         this.label1.TabIndex = 2;
         this.label1.Text = "Category";
         // 
         // checkedListBox1
         // 
         this.checkedListBox1.FormattingEnabled = true;
         this.checkedListBox1.Items.AddRange(new object[] {
            "Analysis",
            "Simulation"});
         this.checkedListBox1.Location = new System.Drawing.Point(6, 42);
         this.checkedListBox1.Name = "checkedListBox1";
         this.checkedListBox1.Size = new System.Drawing.Size(150, 79);
         this.checkedListBox1.TabIndex = 1;
         this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.Location = new System.Drawing.Point(7, 1);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(90, 17);
         this.checkBox1.TabIndex = 0;
         this.checkBox1.Text = "Filter services";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // button1
         // 
         this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.button1.Location = new System.Drawing.Point(156, 451);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(109, 23);
         this.button1.TabIndex = 12;
         this.button1.Text = "Cancel";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // ServiceAddFormOld
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(584, 500);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.groupBox2);
         this.Name = "ServiceAddFormOld";
         this.Text = "ServiceAddForm";
         this.Load += new System.EventHandler(this.ServiceSetupForm_Load);
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.ListView listAvailableServices;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ComboBox newTrigger;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button buttonAdd;
      private System.Windows.Forms.TextBox newToken;
      private System.Windows.Forms.TextBox newSoid;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.CheckedListBox checkedListBox1;
      private System.Windows.Forms.CheckBox checkBox1;
      private System.Windows.Forms.ComboBox comboBox2;
      private System.Windows.Forms.ComboBox comboBox1;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TrackBar trackBar2;
      private System.Windows.Forms.TrackBar trackBar1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Button buttonRegister;
      private System.Windows.Forms.Button button1;
   }
}