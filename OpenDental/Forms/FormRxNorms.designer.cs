namespace OpenDental{
	partial class FormRxNorms {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.labelCodeOrDesc = new System.Windows.Forms.Label();
			this.textCode = new System.Windows.Forms.TextBox();
			this.gridMain = new OpenDental.UI.ODGrid();
			this.butExact = new OpenDental.UI.Button();
			this.butSearch = new OpenDental.UI.Button();
			this.butNone = new OpenDental.UI.Button();
			this.butOK = new OpenDental.UI.Button();
			this.butCancel = new OpenDental.UI.Button();
			this.checkIgnore = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// labelCodeOrDesc
			// 
			this.labelCodeOrDesc.Location = new System.Drawing.Point(3, 10);
			this.labelCodeOrDesc.Name = "labelCodeOrDesc";
			this.labelCodeOrDesc.Size = new System.Drawing.Size(172, 16);
			this.labelCodeOrDesc.TabIndex = 21;
			this.labelCodeOrDesc.Text = "Code or Description";
			this.labelCodeOrDesc.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// textCode
			// 
			this.textCode.Location = new System.Drawing.Point(178, 7);
			this.textCode.Name = "textCode";
			this.textCode.Size = new System.Drawing.Size(100, 20);
			this.textCode.TabIndex = 20;
			// 
			// gridMain
			// 
			this.gridMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gridMain.HScrollVisible = false;
			this.gridMain.Location = new System.Drawing.Point(26, 34);
			this.gridMain.Name = "gridMain";
			this.gridMain.ScrollValue = 0;
			this.gridMain.Size = new System.Drawing.Size(642, 599);
			this.gridMain.TabIndex = 4;
			this.gridMain.Title = "RxNorm Codes";
			this.gridMain.TranslationName = null;
			this.gridMain.CellDoubleClick += new OpenDental.UI.ODGridClickEventHandler(this.gridMain_CellDoubleClick);
			// 
			// butExact
			// 
			this.butExact.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butExact.Autosize = true;
			this.butExact.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butExact.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butExact.CornerRadius = 4F;
			this.butExact.Location = new System.Drawing.Point(365, 5);
			this.butExact.Name = "butExact";
			this.butExact.Size = new System.Drawing.Size(75, 24);
			this.butExact.TabIndex = 24;
			this.butExact.Text = "Exact";
			this.butExact.Click += new System.EventHandler(this.butExact_Click);
			// 
			// butSearch
			// 
			this.butSearch.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butSearch.Autosize = true;
			this.butSearch.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butSearch.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butSearch.CornerRadius = 4F;
			this.butSearch.Location = new System.Drawing.Point(284, 5);
			this.butSearch.Name = "butSearch";
			this.butSearch.Size = new System.Drawing.Size(75, 24);
			this.butSearch.TabIndex = 22;
			this.butSearch.Text = "Similar";
			this.butSearch.Click += new System.EventHandler(this.butSearch_Click);
			// 
			// butNone
			// 
			this.butNone.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.butNone.Autosize = true;
			this.butNone.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butNone.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butNone.CornerRadius = 4F;
			this.butNone.Location = new System.Drawing.Point(26, 660);
			this.butNone.Name = "butNone";
			this.butNone.Size = new System.Drawing.Size(75, 24);
			this.butNone.TabIndex = 3;
			this.butNone.Text = "&None";
			this.butNone.Click += new System.EventHandler(this.butNone_Click);
			// 
			// butOK
			// 
			this.butOK.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butOK.Autosize = true;
			this.butOK.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butOK.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butOK.CornerRadius = 4F;
			this.butOK.Location = new System.Drawing.Point(512, 660);
			this.butOK.Name = "butOK";
			this.butOK.Size = new System.Drawing.Size(75, 24);
			this.butOK.TabIndex = 3;
			this.butOK.Text = "&OK";
			this.butOK.Click += new System.EventHandler(this.butOK_Click);
			// 
			// butCancel
			// 
			this.butCancel.AdjustImageLocation = new System.Drawing.Point(0, 0);
			this.butCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.butCancel.Autosize = true;
			this.butCancel.BtnShape = OpenDental.UI.enumType.BtnShape.Rectangle;
			this.butCancel.BtnStyle = OpenDental.UI.enumType.XPStyle.Silver;
			this.butCancel.CornerRadius = 4F;
			this.butCancel.Location = new System.Drawing.Point(593, 660);
			this.butCancel.Name = "butCancel";
			this.butCancel.Size = new System.Drawing.Size(75, 24);
			this.butCancel.TabIndex = 2;
			this.butCancel.Text = "&Cancel";
			this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
			// 
			// checkIgnore
			// 
			this.checkIgnore.AutoSize = true;
			this.checkIgnore.Location = new System.Drawing.Point(486, 9);
			this.checkIgnore.Name = "checkIgnore";
			this.checkIgnore.Size = new System.Drawing.Size(101, 17);
			this.checkIgnore.TabIndex = 25;
			this.checkIgnore.Text = "Ignore Numbers";
			this.checkIgnore.UseVisualStyleBackColor = true;
			// 
			// FormRxNorms
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(693, 702);
			this.Controls.Add(this.checkIgnore);
			this.Controls.Add(this.butExact);
			this.Controls.Add(this.butSearch);
			this.Controls.Add(this.labelCodeOrDesc);
			this.Controls.Add(this.textCode);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.butNone);
			this.Controls.Add(this.butOK);
			this.Controls.Add(this.butCancel);
			this.Name = "FormRxNorms";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "RxNorms";
			this.Load += new System.EventHandler(this.FormRxNorms_Load);
			this.Shown += new System.EventHandler(this.FormRxNorms_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private OpenDental.UI.Button butOK;
		private OpenDental.UI.Button butCancel;
		private UI.Button butNone;
		private UI.ODGrid gridMain;
		private UI.Button butSearch;
		private System.Windows.Forms.Label labelCodeOrDesc;
		private System.Windows.Forms.TextBox textCode;
		private UI.Button butExact;
		private System.Windows.Forms.CheckBox checkIgnore;
	}
}