namespace Bimbot.Forms
{
   partial class ShowResultsForm
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
         this.groupBox6 = new System.Windows.Forms.GroupBox();
         this.viewpointImage = new System.Windows.Forms.PictureBox();
         this.topicTitle = new System.Windows.Forms.TextBox();
         this.markupTabControl = new System.Windows.Forms.TabControl();
         this.tabTopic = new System.Windows.Forms.TabPage();
         this.topicItems = new System.Windows.Forms.ListView();
         this.Item = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.tabComments = new System.Windows.Forms.TabPage();
         this.commentItems = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.commentList = new System.Windows.Forms.ListView();
         this.tabViewpoints = new System.Windows.Forms.TabPage();
         this.viewpointItems = new System.Windows.Forms.ListView();
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.viewpointList = new System.Windows.Forms.ListView();
         this.label7 = new System.Windows.Forms.Label();
         this.topicDescription = new System.Windows.Forms.TextBox();
         this.resultList = new System.Windows.Forms.ListView();
         this.label1 = new System.Windows.Forms.Label();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.serviceLastRun = new System.Windows.Forms.TextBox();
         this.serviceName = new System.Windows.Forms.TextBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.groupBox6.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.viewpointImage)).BeginInit();
         this.markupTabControl.SuspendLayout();
         this.tabTopic.SuspendLayout();
         this.tabComments.SuspendLayout();
         this.tabViewpoints.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBox6
         // 
         this.groupBox6.Controls.Add(this.viewpointImage);
         this.groupBox6.Controls.Add(this.topicTitle);
         this.groupBox6.Controls.Add(this.markupTabControl);
         this.groupBox6.Controls.Add(this.label7);
         this.groupBox6.Controls.Add(this.topicDescription);
         this.groupBox6.Location = new System.Drawing.Point(294, 95);
         this.groupBox6.Name = "groupBox6";
         this.groupBox6.Size = new System.Drawing.Size(562, 468);
         this.groupBox6.TabIndex = 37;
         this.groupBox6.TabStop = false;
         this.groupBox6.Text = "Issue";
         // 
         // viewpointImage
         // 
         this.viewpointImage.Location = new System.Drawing.Point(333, 16);
         this.viewpointImage.Name = "viewpointImage";
         this.viewpointImage.Size = new System.Drawing.Size(219, 127);
         this.viewpointImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.viewpointImage.TabIndex = 36;
         this.viewpointImage.TabStop = false;
         // 
         // topicTitle
         // 
         this.topicTitle.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
         this.topicTitle.Location = new System.Drawing.Point(41, 16);
         this.topicTitle.Name = "topicTitle";
         this.topicTitle.ReadOnly = true;
         this.topicTitle.Size = new System.Drawing.Size(285, 20);
         this.topicTitle.TabIndex = 11;
         // 
         // markupTabControl
         // 
         this.markupTabControl.Controls.Add(this.tabTopic);
         this.markupTabControl.Controls.Add(this.tabComments);
         this.markupTabControl.Controls.Add(this.tabViewpoints);
         this.markupTabControl.Location = new System.Drawing.Point(9, 127);
         this.markupTabControl.Name = "markupTabControl";
         this.markupTabControl.SelectedIndex = 0;
         this.markupTabControl.Size = new System.Drawing.Size(547, 335);
         this.markupTabControl.TabIndex = 37;
         this.markupTabControl.SelectedIndexChanged += new System.EventHandler(this.viewpoints_SelectedIndexChanged);
         // 
         // tabTopic
         // 
         this.tabTopic.Controls.Add(this.topicItems);
         this.tabTopic.Location = new System.Drawing.Point(4, 22);
         this.tabTopic.Name = "tabTopic";
         this.tabTopic.Padding = new System.Windows.Forms.Padding(3);
         this.tabTopic.Size = new System.Drawing.Size(539, 309);
         this.tabTopic.TabIndex = 0;
         this.tabTopic.Text = "Topic";
         this.tabTopic.UseVisualStyleBackColor = true;
         // 
         // topicItems
         // 
         this.topicItems.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
         this.topicItems.AutoArrange = false;
         this.topicItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Item,
            this.Value});
         this.topicItems.GridLines = true;
         this.topicItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
         this.topicItems.LabelWrap = false;
         this.topicItems.Location = new System.Drawing.Point(6, 6);
         this.topicItems.MultiSelect = false;
         this.topicItems.Name = "topicItems";
         this.topicItems.ShowGroups = false;
         this.topicItems.Size = new System.Drawing.Size(256, 297);
         this.topicItems.TabIndex = 4;
         this.topicItems.UseCompatibleStateImageBehavior = false;
         this.topicItems.View = System.Windows.Forms.View.Details;
         // 
         // Item
         // 
         this.Item.Text = "Item";
         // 
         // Value
         // 
         this.Value.Text = "Value";
         // 
         // tabComments
         // 
         this.tabComments.Controls.Add(this.commentItems);
         this.tabComments.Controls.Add(this.commentList);
         this.tabComments.Location = new System.Drawing.Point(4, 22);
         this.tabComments.Name = "tabComments";
         this.tabComments.Padding = new System.Windows.Forms.Padding(3);
         this.tabComments.Size = new System.Drawing.Size(539, 309);
         this.tabComments.TabIndex = 1;
         this.tabComments.Text = "Comments";
         this.tabComments.UseVisualStyleBackColor = true;
         // 
         // commentItems
         // 
         this.commentItems.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
         this.commentItems.AutoArrange = false;
         this.commentItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
         this.commentItems.GridLines = true;
         this.commentItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
         this.commentItems.LabelWrap = false;
         this.commentItems.Location = new System.Drawing.Point(182, 6);
         this.commentItems.MultiSelect = false;
         this.commentItems.Name = "commentItems";
         this.commentItems.ShowGroups = false;
         this.commentItems.Size = new System.Drawing.Size(351, 297);
         this.commentItems.TabIndex = 33;
         this.commentItems.UseCompatibleStateImageBehavior = false;
         this.commentItems.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "Item";
         this.columnHeader1.Width = 32;
         // 
         // columnHeader2
         // 
         this.columnHeader2.Text = "Value";
         this.columnHeader2.Width = 315;
         // 
         // commentList
         // 
         this.commentList.HideSelection = false;
         this.commentList.Location = new System.Drawing.Point(6, 6);
         this.commentList.Name = "commentList";
         this.commentList.Size = new System.Drawing.Size(170, 297);
         this.commentList.TabIndex = 32;
         this.commentList.UseCompatibleStateImageBehavior = false;
         this.commentList.View = System.Windows.Forms.View.List;
         this.commentList.SelectedIndexChanged += new System.EventHandler(this.commentList_SelectedIndexChanged);
         // 
         // tabViewpoints
         // 
         this.tabViewpoints.BackColor = System.Drawing.Color.White;
         this.tabViewpoints.Controls.Add(this.viewpointItems);
         this.tabViewpoints.Controls.Add(this.viewpointList);
         this.tabViewpoints.Location = new System.Drawing.Point(4, 22);
         this.tabViewpoints.Name = "tabViewpoints";
         this.tabViewpoints.Padding = new System.Windows.Forms.Padding(3);
         this.tabViewpoints.Size = new System.Drawing.Size(539, 309);
         this.tabViewpoints.TabIndex = 2;
         this.tabViewpoints.Text = "Viewpoints";
         // 
         // viewpointItems
         // 
         this.viewpointItems.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
         this.viewpointItems.AutoArrange = false;
         this.viewpointItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
         this.viewpointItems.GridLines = true;
         this.viewpointItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
         this.viewpointItems.LabelWrap = false;
         this.viewpointItems.Location = new System.Drawing.Point(194, 6);
         this.viewpointItems.MultiSelect = false;
         this.viewpointItems.Name = "viewpointItems";
         this.viewpointItems.ShowGroups = false;
         this.viewpointItems.Size = new System.Drawing.Size(339, 297);
         this.viewpointItems.TabIndex = 34;
         this.viewpointItems.UseCompatibleStateImageBehavior = false;
         this.viewpointItems.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader3
         // 
         this.columnHeader3.Text = "Item";
         this.columnHeader3.Width = 32;
         // 
         // columnHeader4
         // 
         this.columnHeader4.Text = "Value";
         this.columnHeader4.Width = 303;
         // 
         // viewpointList
         // 
         this.viewpointList.Location = new System.Drawing.Point(6, 6);
         this.viewpointList.Name = "viewpointList";
         this.viewpointList.Size = new System.Drawing.Size(183, 297);
         this.viewpointList.TabIndex = 31;
         this.viewpointList.UseCompatibleStateImageBehavior = false;
         this.viewpointList.View = System.Windows.Forms.View.List;
         // 
         // label7
         // 
         this.label7.AutoSize = true;
         this.label7.Location = new System.Drawing.Point(5, 19);
         this.label7.Name = "label7";
         this.label7.Size = new System.Drawing.Size(30, 13);
         this.label7.TabIndex = 10;
         this.label7.Text = "Title:";
         // 
         // topicDescription
         // 
         this.topicDescription.Location = new System.Drawing.Point(9, 42);
         this.topicDescription.Multiline = true;
         this.topicDescription.Name = "topicDescription";
         this.topicDescription.ReadOnly = true;
         this.topicDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.topicDescription.Size = new System.Drawing.Size(317, 79);
         this.topicDescription.TabIndex = 3;
         // 
         // resultList
         // 
         this.resultList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.resultList.Location = new System.Drawing.Point(12, 33);
         this.resultList.Name = "resultList";
         this.resultList.Size = new System.Drawing.Size(274, 530);
         this.resultList.TabIndex = 36;
         this.resultList.UseCompatibleStateImageBehavior = false;
         this.resultList.View = System.Windows.Forms.View.List;
         this.resultList.SelectedIndexChanged += new System.EventHandler(this.resultList_SelectedIndexChanged);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 12);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(148, 13);
         this.label1.TabIndex = 35;
         this.label1.Text = "Results/Issues from services: ";
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.serviceLastRun);
         this.groupBox3.Controls.Add(this.serviceName);
         this.groupBox3.Controls.Add(this.label5);
         this.groupBox3.Controls.Add(this.label6);
         this.groupBox3.Location = new System.Drawing.Point(293, 12);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(563, 77);
         this.groupBox3.TabIndex = 38;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Service";
         // 
         // serviceLastRun
         // 
         this.serviceLastRun.Location = new System.Drawing.Point(60, 47);
         this.serviceLastRun.Name = "serviceLastRun";
         this.serviceLastRun.ReadOnly = true;
         this.serviceLastRun.Size = new System.Drawing.Size(497, 20);
         this.serviceLastRun.TabIndex = 7;
         // 
         // serviceName
         // 
         this.serviceName.Location = new System.Drawing.Point(60, 21);
         this.serviceName.Name = "serviceName";
         this.serviceName.ReadOnly = true;
         this.serviceName.Size = new System.Drawing.Size(497, 20);
         this.serviceName.TabIndex = 5;
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(6, 50);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(48, 13);
         this.label5.TabIndex = 6;
         this.label5.Text = "Last run:";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(6, 24);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(38, 13);
         this.label6.TabIndex = 4;
         this.label6.Text = "Name:";
         // 
         // ShowResultsForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(870, 573);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.groupBox6);
         this.Controls.Add(this.resultList);
         this.Controls.Add(this.label1);
         this.Name = "ShowResultsForm";
         this.Text = "ShowResultsForm";
         this.Load += new System.EventHandler(this.ServiceSetupForm_Load);
         this.groupBox6.ResumeLayout(false);
         this.groupBox6.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.viewpointImage)).EndInit();
         this.markupTabControl.ResumeLayout(false);
         this.tabTopic.ResumeLayout(false);
         this.tabComments.ResumeLayout(false);
         this.tabViewpoints.ResumeLayout(false);
         this.groupBox3.ResumeLayout(false);
         this.groupBox3.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox6;
      private System.Windows.Forms.PictureBox viewpointImage;
      private System.Windows.Forms.TextBox topicTitle;
      private System.Windows.Forms.TabControl markupTabControl;
      private System.Windows.Forms.TabPage tabTopic;
      private System.Windows.Forms.ListView topicItems;
      private System.Windows.Forms.ColumnHeader Item;
      private System.Windows.Forms.ColumnHeader Value;
      private System.Windows.Forms.TabPage tabComments;
      private System.Windows.Forms.ListView commentItems;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ListView commentList;
      private System.Windows.Forms.TabPage tabViewpoints;
      private System.Windows.Forms.ListView viewpointItems;
      private System.Windows.Forms.ColumnHeader columnHeader3;
      private System.Windows.Forms.ColumnHeader columnHeader4;
      private System.Windows.Forms.ListView viewpointList;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.TextBox topicDescription;
      private System.Windows.Forms.ListView resultList;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.TextBox serviceLastRun;
      private System.Windows.Forms.TextBox serviceName;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
   }
}