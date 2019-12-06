namespace ClientGui
{
    partial class Form2
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
            this.listButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.joinButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.ninvTextBox = new System.Windows.Forms.TextBox();
            this.slotsTextBox = new System.Windows.Forms.TextBox();
            this.topicTextBox = new System.Windows.Forms.TextBox();
            this.minattendTextBox = new System.Windows.Forms.TextBox();
            this.nslotsTextBox = new System.Windows.Forms.TextBox();
            this.invTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.slotLabel = new System.Windows.Forms.Label();
            this.invitesLabel = new System.Windows.Forms.Label();
            this.addSlotButton = new System.Windows.Forms.Button();
            this.addInviteButton = new System.Windows.Forms.Button();
            this.listView2 = new System.Windows.Forms.ListView();
            this.Topic2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.showSlotsButton = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.Topic = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MinAttendees = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NumSlots = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NumInvitees = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            //
            // listButton
            //
            this.listButton.Location = new System.Drawing.Point(310, 242);
            this.listButton.Name = "listButton";
            this.listButton.Size = new System.Drawing.Size(94, 23);
            this.listButton.TabIndex = 0;
            this.listButton.Text = "List Meetings";
            this.listButton.UseVisualStyleBackColor = true;
            this.listButton.Click += new System.EventHandler(this.listButton_Click);
            //
            // createButton
            //
            this.createButton.Location = new System.Drawing.Point(120, 116);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(100, 23);
            this.createButton.TabIndex = 1;
            this.createButton.Text = "Create Meeting";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            //
            // joinButton
            //
            this.joinButton.Location = new System.Drawing.Point(648, 242);
            this.joinButton.Name = "joinButton";
            this.joinButton.Size = new System.Drawing.Size(221, 23);
            this.joinButton.TabIndex = 2;
            this.joinButton.Text = "Join Meeting on these slot(s)";
            this.joinButton.UseVisualStyleBackColor = true;
            this.joinButton.Click += new System.EventHandler(this.joinButton_Click);
            //
            // closeButton
            //
            this.closeButton.Location = new System.Drawing.Point(429, 265);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(94, 23);
            this.closeButton.TabIndex = 3;
            this.closeButton.Text = "Close Meeting";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            //
            // ninvTextBox
            //
            this.ninvTextBox.Location = new System.Drawing.Point(120, 90);
            this.ninvTextBox.Name = "ninvTextBox";
            this.ninvTextBox.Size = new System.Drawing.Size(100, 20);
            this.ninvTextBox.TabIndex = 5;
            //
            // slotsTextBox
            //
            this.slotsTextBox.Location = new System.Drawing.Point(120, 162);
            this.slotsTextBox.Name = "slotsTextBox";
            this.slotsTextBox.Size = new System.Drawing.Size(100, 20);
            this.slotsTextBox.TabIndex = 6;
            //
            // topicTextBox
            //
            this.topicTextBox.Location = new System.Drawing.Point(120, 12);
            this.topicTextBox.Name = "topicTextBox";
            this.topicTextBox.Size = new System.Drawing.Size(100, 20);
            this.topicTextBox.TabIndex = 7;
            //
            // minattendTextBox
            //
            this.minattendTextBox.Location = new System.Drawing.Point(120, 38);
            this.minattendTextBox.Name = "minattendTextBox";
            this.minattendTextBox.Size = new System.Drawing.Size(100, 20);
            this.minattendTextBox.TabIndex = 8;
            //
            // nslotsTextBox
            //
            this.nslotsTextBox.Location = new System.Drawing.Point(120, 64);
            this.nslotsTextBox.Name = "nslotsTextBox";
            this.nslotsTextBox.Size = new System.Drawing.Size(100, 20);
            this.nslotsTextBox.TabIndex = 9;
            //
            // invTextBox
            //
            this.invTextBox.Location = new System.Drawing.Point(120, 188);
            this.invTextBox.Name = "invTextBox";
            this.invTextBox.Size = new System.Drawing.Size(100, 20);
            this.invTextBox.TabIndex = 10;
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(64, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Topic:";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Min_Attendees:";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Number of Slots:";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Number of Invites:";
            //
            // slotLabel
            //
            this.slotLabel.AutoSize = true;
            this.slotLabel.Location = new System.Drawing.Point(8, 165);
            this.slotLabel.Name = "slotLabel";
            this.slotLabel.Size = new System.Drawing.Size(33, 13);
            this.slotLabel.TabIndex = 15;
            this.slotLabel.Text = "Slots:";
            //
            // invitesLabel
            //
            this.invitesLabel.AutoSize = true;
            this.invitesLabel.Location = new System.Drawing.Point(8, 191);
            this.invitesLabel.Name = "invitesLabel";
            this.invitesLabel.Size = new System.Drawing.Size(41, 13);
            this.invitesLabel.TabIndex = 16;
            this.invitesLabel.Text = "Invites:";
            //
            // addSlotButton
            //
            this.addSlotButton.Location = new System.Drawing.Point(225, 160);
            this.addSlotButton.Name = "addSlotButton";
            this.addSlotButton.Size = new System.Drawing.Size(75, 23);
            this.addSlotButton.TabIndex = 17;
            this.addSlotButton.Text = "Add";
            this.addSlotButton.UseVisualStyleBackColor = true;
            this.addSlotButton.Click += new System.EventHandler(this.addSlotButton_Click);
            //
            // addInviteButton
            //
            this.addInviteButton.Location = new System.Drawing.Point(225, 186);
            this.addInviteButton.Name = "addInviteButton";
            this.addInviteButton.Size = new System.Drawing.Size(75, 23);
            this.addInviteButton.TabIndex = 18;
            this.addInviteButton.Text = "Add";
            this.addInviteButton.UseVisualStyleBackColor = true;
            this.addInviteButton.Click += new System.EventHandler(this.addInviteButton_Click);
            //
            // listView2
            //
            this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Topic2,
            this.Location,
            this.Date});
            this.listView2.FullRowSelect = true;
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(648, 12);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(221, 224);
            this.listView2.TabIndex = 22;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            //
            // Topic2
            //
            this.Topic2.Text = "Topic";
            this.Topic2.Width = 77;
            //
            // Location
            //
            this.Location.Text = "Location";
            this.Location.Width = 70;
            //
            // Date
            //
            this.Date.Text = "Date";
            this.Date.Width = 70;
            //
            // showSlotsButton
            //
            this.showSlotsButton.Location = new System.Drawing.Point(548, 242);
            this.showSlotsButton.Name = "showSlotsButton";
            this.showSlotsButton.Size = new System.Drawing.Size(94, 23);
            this.showSlotsButton.TabIndex = 23;
            this.showSlotsButton.Text = "Show Slot(s)";
            this.showSlotsButton.UseVisualStyleBackColor = true;
            this.showSlotsButton.Click += new System.EventHandler(this.showSlotsButton_Click);
            //
            // listView1
            //
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Topic,
            this.Status,
            this.MinAttendees,
            this.NumSlots,
            this.NumInvitees});
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(310, 12);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(332, 224);
            this.listView1.TabIndex = 24;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            //
            // Topic
            //
            this.Topic.Text = "Meeting Topic";
            this.Topic.Width = 80;
            //
            // MinAttendees
            //
            this.MinAttendees.Text = "Min. Attendees";
            this.MinAttendees.Width = 83;
            //
            // NumSlots
            //
            this.NumSlots.Text = "Nº Slots";
            this.NumSlots.Width = 50;
            //
            // NumInvitees
            //
            this.NumInvitees.Text = "Nº Invitees";
            this.NumInvitees.Width = 64;
            //
            // Status
            //
            this.Status.Text = "Status";
            this.Status.Width = 51;
            //
            // Form2
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 386);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.showSlotsButton);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.addInviteButton);
            this.Controls.Add(this.addSlotButton);
            this.Controls.Add(this.invitesLabel);
            this.Controls.Add(this.slotLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.invTextBox);
            this.Controls.Add(this.nslotsTextBox);
            this.Controls.Add(this.minattendTextBox);
            this.Controls.Add(this.topicTextBox);
            this.Controls.Add(this.slotsTextBox);
            this.Controls.Add(this.ninvTextBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.joinButton);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.listButton);
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button listButton;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Button joinButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox ninvTextBox;
        private System.Windows.Forms.TextBox slotsTextBox;
        private System.Windows.Forms.TextBox topicTextBox;
        private System.Windows.Forms.TextBox minattendTextBox;
        private System.Windows.Forms.TextBox nslotsTextBox;
        private System.Windows.Forms.TextBox invTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label slotLabel;
        private System.Windows.Forms.Label invitesLabel;
        private System.Windows.Forms.Button addSlotButton;
        private System.Windows.Forms.Button addInviteButton;
        public System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Button showSlotsButton;
        private new System.Windows.Forms.ColumnHeader Location;
        private System.Windows.Forms.ColumnHeader Date;
        private System.Windows.Forms.ColumnHeader Topic;
        private System.Windows.Forms.ColumnHeader MinAttendees;
        private System.Windows.Forms.ColumnHeader NumSlots;
        private System.Windows.Forms.ColumnHeader NumInvitees;
        public System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader Topic2;
        private System.Windows.Forms.ColumnHeader Status;
    }
}
