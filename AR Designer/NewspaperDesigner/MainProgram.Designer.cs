namespace NewspaperDesigner
{
    partial class MainProgram
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainProgram));
			this.rbMenu = new Janus.Windows.Ribbon.Ribbon();
			this.dropDownCommand1 = new Janus.Windows.Ribbon.DropDownCommand();
			this.rbTab1 = new Janus.Windows.Ribbon.RibbonTab();
			this.File = new Janus.Windows.Ribbon.RibbonGroup();
			this.btnNew = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnOpen = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnSave = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnSaveAs = new Janus.Windows.Ribbon.ButtonCommand();
			this.ribbonGroup1 = new Janus.Windows.Ribbon.RibbonGroup();
			this.btnClone = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnDelete = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnUndo = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnRedo = new Janus.Windows.Ribbon.ButtonCommand();
			this.ribbonGroup2 = new Janus.Windows.Ribbon.RibbonGroup();
			this.btnBack = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnNext = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnSavePage = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnNewPage = new Janus.Windows.Ribbon.ButtonCommand();
			this.cbcPages = new Janus.Windows.Ribbon.ComboBoxCommand();
			this.btnZoomIn = new Janus.Windows.Ribbon.ButtonCommand();
			this.btnZoomOut = new Janus.Windows.Ribbon.ButtonCommand();
			this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
			this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.buttonBar1 = new Janus.Windows.ButtonBar.ButtonBar();
			this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
			this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
			this.panel2 = new System.Windows.Forms.Panel();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lbObjectName = new System.Windows.Forms.Label();
			this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
			this.buttonCommand3 = new Janus.Windows.Ribbon.ButtonCommand();
			this.buttonCommand4 = new Janus.Windows.Ribbon.ButtonCommand();
			this.panelMidScreen = new System.Windows.Forms.Panel();
			this.panelInnerBounder = new System.Windows.Forms.Panel();
			this.panelInnerScreen = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.rbMenu)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
			this.uiPanel0.SuspendLayout();
			this.uiPanel0Container.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.buttonBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
			this.uiPanel1.SuspendLayout();
			this.uiPanel1Container.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
			this.panelMidScreen.SuspendLayout();
			this.panelInnerBounder.SuspendLayout();
			this.SuspendLayout();
			// 
			// rbMenu
			// 
			this.rbMenu.ControlBoxMenu.LeftCommands.AddRange(new Janus.Windows.Ribbon.CommandBase[] {
            this.dropDownCommand1});
			this.rbMenu.Location = new System.Drawing.Point(0, 0);
			this.rbMenu.Name = "rbMenu";
			this.rbMenu.Size = new System.Drawing.Size(992, 146);
			// 
			// 
			// 
			this.rbMenu.SuperTipComponent.AutoPopDelay = 2000;
			this.rbMenu.SuperTipComponent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbMenu.SuperTipComponent.ImageList = null;
			this.rbMenu.TabIndex = 0;
			this.rbMenu.Tabs.AddRange(new Janus.Windows.Ribbon.RibbonTab[] {
            this.rbTab1});
			// 
			// dropDownCommand1
			// 
			this.dropDownCommand1.Key = "dropDownCommand1";
			this.dropDownCommand1.Name = "dropDownCommand1";
			this.dropDownCommand1.Text = "&Exit";
			this.dropDownCommand1.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.dropDownCommand1_Click);
			// 
			// rbTab1
			// 
			this.rbTab1.Groups.AddRange(new Janus.Windows.Ribbon.RibbonGroup[] {
            this.File,
            this.ribbonGroup1,
            this.ribbonGroup2});
			this.rbTab1.Key = "ribbonTab1";
			this.rbTab1.Name = "rbTab1";
			this.rbTab1.Selected = true;
			this.rbTab1.Text = "Menu";
			// 
			// File
			// 
			this.File.Commands.AddRange(new Janus.Windows.Ribbon.CommandBase[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAs});
			this.File.Key = "ribbonGroup1";
			this.File.Name = "File";
			this.File.Text = "Newspaper";
			// 
			// btnNew
			// 
			this.btnNew.Key = "buttonCommand1";
			this.btnNew.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnNew.LargeImage")));
			this.btnNew.Name = "btnNew";
			this.btnNew.Text = "New";
			this.btnNew.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.Key = "buttonCommand2";
			this.btnOpen.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnOpen.LargeImage")));
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Text = "Load";
			this.btnOpen.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnLoad_Click);
			// 
			// btnSave
			// 
			this.btnSave.Enabled = false;
			this.btnSave.Key = "buttonCommand5";
			this.btnSave.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSave.LargeImage")));
			this.btnSave.Name = "btnSave";
			this.btnSave.Text = "Save";
			this.btnSave.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnSave_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.Enabled = false;
			this.btnSaveAs.Key = "buttonCommand1";
			this.btnSaveAs.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.LargeImage")));
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Text = "Save As";
			this.btnSaveAs.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.buttonCommand1_Click);
			// 
			// ribbonGroup1
			// 
			this.ribbonGroup1.Commands.AddRange(new Janus.Windows.Ribbon.CommandBase[] {
            this.btnClone,
            this.btnDelete,
            this.btnUndo,
            this.btnRedo});
			this.ribbonGroup1.Key = "ribbonGroup1";
			this.ribbonGroup1.Name = "ribbonGroup1";
			this.ribbonGroup1.Text = "Edit";
			// 
			// btnClone
			// 
			this.btnClone.Enabled = false;
			this.btnClone.Key = "buttonCommand5";
			this.btnClone.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnClone.LargeImage")));
			this.btnClone.Name = "btnClone";
			this.btnClone.Text = "Clone";
			this.btnClone.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnClone_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Enabled = false;
			this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
			this.btnDelete.Key = "buttonCommand1";
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnDelete_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Enabled = false;
			this.btnUndo.Key = "buttonCommand5";
			this.btnUndo.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnUndo.LargeImage")));
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Text = "Undo";
			this.btnUndo.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnUndo_Click);
			// 
			// btnRedo
			// 
			this.btnRedo.Enabled = false;
			this.btnRedo.Key = "buttonCommand6";
			this.btnRedo.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnRedo.LargeImage")));
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Text = "Redo";
			this.btnRedo.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnRedo_Click);
			// 
			// ribbonGroup2
			// 
			this.ribbonGroup2.Commands.AddRange(new Janus.Windows.Ribbon.CommandBase[] {
            this.btnBack,
            this.btnNext,
            this.btnSavePage,
            this.btnNewPage,
            this.cbcPages,
            this.btnZoomIn,
            this.btnZoomOut});
			this.ribbonGroup2.Key = "ribbonGroup2";
			this.ribbonGroup2.Name = "ribbonGroup2";
			this.ribbonGroup2.Text = "Page";
			// 
			// btnBack
			// 
			this.btnBack.Enabled = false;
			this.btnBack.Key = "buttonCommand6";
			this.btnBack.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnBack.LargeImage")));
			this.btnBack.Name = "btnBack";
			this.btnBack.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Large;
			this.btnBack.Text = "Back page";
			this.btnBack.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btcBack_Click);
			// 
			// btnNext
			// 
			this.btnNext.Enabled = false;
			this.btnNext.Key = "buttonCommand5";
			this.btnNext.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnNext.LargeImage")));
			this.btnNext.Name = "btnNext";
			this.btnNext.SizeStyle = Janus.Windows.Ribbon.CommandSizeStyle.Large;
			this.btnNext.Text = "Next page";
			this.btnNext.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btcNext_Click);
			// 
			// btnSavePage
			// 
			this.btnSavePage.Enabled = false;
			this.btnSavePage.Key = "buttonCommand5";
			this.btnSavePage.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnSavePage.LargeImage")));
			this.btnSavePage.Name = "btnSavePage";
			this.btnSavePage.Text = "Save Page";
			this.btnSavePage.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.bcSavePage_Click);
			// 
			// btnNewPage
			// 
			this.btnNewPage.Enabled = false;
			this.btnNewPage.Key = "buttonCommand2";
			this.btnNewPage.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnNewPage.LargeImage")));
			this.btnNewPage.Name = "btnNewPage";
			this.btnNewPage.Text = "New Page";
			this.btnNewPage.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnNewPage_Click);
			// 
			// cbcPages
			// 
			// 
			// 
			// 
			this.cbcPages.ComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbcPages.ComboBox.Location = new System.Drawing.Point(532, 60);
			this.cbcPages.ComboBox.Name = "";
			this.cbcPages.ComboBox.Size = new System.Drawing.Size(60, 21);
			this.cbcPages.ComboBox.TabIndex = 0;
			this.cbcPages.ComboBox.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
			this.cbcPages.ComboBox.SelectedIndexChanged += new System.EventHandler(this.cbcPages_ComboBox_SelectedIndexChanged);
			this.cbcPages.ControlWidth = 60;
			this.cbcPages.Key = "comboBoxCommand1";
			this.cbcPages.Name = "cbcPages";
			this.cbcPages.Style = Janus.Windows.Ribbon.CommandStyle.ImageAndText;
			this.cbcPages.Text = "";
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.Enabled = false;
			this.btnZoomIn.Key = "buttonCommand2";
			this.btnZoomIn.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.LargeImage")));
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Text = "Zoom In";
			this.btnZoomIn.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnZoomIn_Click);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.Enabled = false;
			this.btnZoomOut.Key = "buttonCommand2";
			this.btnZoomOut.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.LargeImage")));
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Text = "Zoom out";
			this.btnZoomOut.Click += new Janus.Windows.Ribbon.CommandEventHandler(this.btnZoomOut_Click);
			// 
			// uiPanelManager1
			// 
			this.uiPanelManager1.ContainerControl = this;
			this.uiPanel0.Id = new System.Guid("f8dbdcd5-fc8c-44c5-8efd-621b533fbb4b");
			this.uiPanelManager1.Panels.Add(this.uiPanel0);
			this.uiPanel1.Id = new System.Guid("908a9308-f59e-4d33-b1d5-d6c4043802c6");
			this.uiPanelManager1.Panels.Add(this.uiPanel1);
			this.uiPanel2.Id = new System.Guid("24b6e699-fd8a-4e19-a318-d647c1830809");
			this.uiPanelManager1.Panels.Add(this.uiPanel2);
			// 
			// Design Time Panel Info:
			// 
			this.uiPanelManager1.BeginPanelInfo();
			this.uiPanelManager1.AddDockPanelInfo(new System.Guid("f8dbdcd5-fc8c-44c5-8efd-621b533fbb4b"), Janus.Windows.UI.Dock.PanelDockStyle.Left, new System.Drawing.Size(178, 534), true);
			this.uiPanelManager1.AddDockPanelInfo(new System.Guid("908a9308-f59e-4d33-b1d5-d6c4043802c6"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(220, 534), true);
			this.uiPanelManager1.AddDockPanelInfo(new System.Guid("24b6e699-fd8a-4e19-a318-d647c1830809"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(588, 534), true);
			this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("f8dbdcd5-fc8c-44c5-8efd-621b533fbb4b"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("908a9308-f59e-4d33-b1d5-d6c4043802c6"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("24b6e699-fd8a-4e19-a318-d647c1830809"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
			this.uiPanelManager1.EndPanelInfo();
			// 
			// uiPanel0
			// 
			this.uiPanel0.InnerContainer = this.uiPanel0Container;
			this.uiPanel0.Location = new System.Drawing.Point(3, 149);
			this.uiPanel0.Name = "uiPanel0";
			this.uiPanel0.Size = new System.Drawing.Size(178, 534);
			this.uiPanel0.TabIndex = 4;
			this.uiPanel0.Text = "Toolbox";
			this.uiPanel0.SizeChanged += new System.EventHandler(this.uiPanel0_SizeChanged);
			// 
			// uiPanel0Container
			// 
			this.uiPanel0Container.Controls.Add(this.buttonBar1);
			this.uiPanel0Container.Location = new System.Drawing.Point(1, 23);
			this.uiPanel0Container.Name = "uiPanel0Container";
			this.uiPanel0Container.Size = new System.Drawing.Size(172, 510);
			this.uiPanel0Container.TabIndex = 0;
			// 
			// buttonBar1
			// 
			this.buttonBar1.GroupAppearance = Janus.Windows.ButtonBar.GroupAppearance.Standard;
			this.buttonBar1.ItemsStateStyles.SelectedFormatStyle.ForeColor = System.Drawing.Color.White;
			this.buttonBar1.Location = new System.Drawing.Point(2, 3);
			this.buttonBar1.Name = "buttonBar1";
			this.buttonBar1.Size = new System.Drawing.Size(166, 498);
			this.buttonBar1.TabIndex = 3;
			this.buttonBar1.Text = "buttonBar1";
			this.buttonBar1.UnfoldEffect = Janus.Windows.ButtonBar.UnfoldEffect.None;
			// 
			// uiPanel1
			// 
			this.uiPanel1.InnerContainer = this.uiPanel1Container;
			this.uiPanel1.Location = new System.Drawing.Point(769, 149);
			this.uiPanel1.Name = "uiPanel1";
			this.uiPanel1.Size = new System.Drawing.Size(220, 534);
			this.uiPanel1.TabIndex = 4;
			this.uiPanel1.Text = "Properties";
			// 
			// uiPanel1Container
			// 
			this.uiPanel1Container.Controls.Add(this.panel2);
			this.uiPanel1Container.Controls.Add(this.panel1);
			this.uiPanel1Container.Location = new System.Drawing.Point(5, 23);
			this.uiPanel1Container.Name = "uiPanel1Container";
			this.uiPanel1Container.Size = new System.Drawing.Size(214, 510);
			this.uiPanel1Container.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.propertyGrid1);
			this.panel2.Location = new System.Drawing.Point(1, 38);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(210, 466);
			this.panel2.TabIndex = 4;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.Size = new System.Drawing.Size(210, 466);
			this.propertyGrid1.TabIndex = 3;
			this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.lbObjectName);
			this.panel1.Location = new System.Drawing.Point(1, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(211, 32);
			this.panel1.TabIndex = 3;
			// 
			// lbObjectName
			// 
			this.lbObjectName.AutoSize = true;
			this.lbObjectName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbObjectName.ForeColor = System.Drawing.Color.Blue;
			this.lbObjectName.Location = new System.Drawing.Point(7, 9);
			this.lbObjectName.Name = "lbObjectName";
			this.lbObjectName.Size = new System.Drawing.Size(139, 14);
			this.lbObjectName.TabIndex = 0;
			this.lbObjectName.Text = "(Media Object Name)";
			// 
			// uiPanel2
			// 
			this.uiPanel2.Location = new System.Drawing.Point(181, 149);
			this.uiPanel2.Name = "uiPanel2";
			this.uiPanel2.Size = new System.Drawing.Size(588, 534);
			this.uiPanel2.TabIndex = 4;
			this.uiPanel2.Text = "Designed Panel";
			this.uiPanel2.SizeChanged += new System.EventHandler(this.uiPanel2_SizeChanged);
			// 
			// buttonCommand3
			// 
			this.buttonCommand3.Key = "buttonCommand3";
			this.buttonCommand3.Name = "buttonCommand3";
			this.buttonCommand3.Text = "buttonCommand3";
			// 
			// buttonCommand4
			// 
			this.buttonCommand4.Key = "buttonCommand4";
			this.buttonCommand4.Name = "buttonCommand4";
			this.buttonCommand4.Text = "buttonCommand4";
			// 
			// panelMidScreen
			// 
			this.panelMidScreen.Controls.Add(this.panelInnerBounder);
			this.panelMidScreen.Location = new System.Drawing.Point(187, 175);
			this.panelMidScreen.Name = "panelMidScreen";
			this.panelMidScreen.Size = new System.Drawing.Size(576, 508);
			this.panelMidScreen.TabIndex = 2;
			this.panelMidScreen.SizeChanged += new System.EventHandler(this.uiPanelMidScreen_SizeChanged);
			// 
			// panelInnerBounder
			// 
			this.panelInnerBounder.AutoScroll = true;
			this.panelInnerBounder.AutoScrollMinSize = new System.Drawing.Size(20, 20);
			this.panelInnerBounder.Controls.Add(this.panelInnerScreen);
			this.panelInnerBounder.Location = new System.Drawing.Point(131, 3);
			this.panelInnerBounder.Name = "panelInnerBounder";
			this.panelInnerBounder.Size = new System.Drawing.Size(304, 502);
			this.panelInnerBounder.TabIndex = 2;
			this.panelInnerBounder.SizeChanged += new System.EventHandler(this.panelInnerBounder_SizeChanged);
			// 
			// panelInnerScreen
			// 
			this.panelInnerScreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.panelInnerScreen.Location = new System.Drawing.Point(6, 8);
			this.panelInnerScreen.Name = "panelInnerScreen";
			this.panelInnerScreen.Size = new System.Drawing.Size(276, 474);
			this.panelInnerScreen.TabIndex = 0;
			this.panelInnerScreen.SizeChanged += new System.EventHandler(this.panelInnerScreen_SizeChanged);
			this.panelInnerScreen.Click += new System.EventHandler(this.uiPanelInner_Click);
			// 
			// MainProgram
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(992, 686);
			this.Controls.Add(this.panelMidScreen);
			this.Controls.Add(this.uiPanel2);
			this.Controls.Add(this.uiPanel1);
			this.Controls.Add(this.uiPanel0);
			this.Controls.Add(this.rbMenu);
			this.Name = "MainProgram";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Newspaper Designer";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainProgram_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.rbMenu)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
			this.uiPanel0.ResumeLayout(false);
			this.uiPanel0Container.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.buttonBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
			this.uiPanel1.ResumeLayout(false);
			this.uiPanel1Container.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
			this.panelMidScreen.ResumeLayout(false);
			this.panelInnerBounder.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.Ribbon.Ribbon rbMenu;
        private Janus.Windows.Ribbon.RibbonTab rbTab1;
        private Janus.Windows.Ribbon.RibbonGroup File;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
		private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
		private Janus.Windows.UI.Dock.UIPanel uiPanel2;
		private Janus.Windows.Ribbon.ButtonCommand btnNew;
        private Janus.Windows.Ribbon.ButtonCommand btnSaveAs;
        private Janus.Windows.Ribbon.ButtonCommand btnOpen;
        private Janus.Windows.Ribbon.DropDownCommand dropDownCommand1;
        private Janus.Windows.Ribbon.ButtonCommand buttonCommand3;
        private Janus.Windows.Ribbon.ButtonCommand buttonCommand4;
        private Janus.Windows.Ribbon.RibbonGroup ribbonGroup2;
        private Janus.Windows.Ribbon.ButtonCommand btnNext;
        private Janus.Windows.Ribbon.ButtonCommand btnBack;
        private Janus.Windows.Ribbon.ComboBoxCommand cbcPages;
        private Janus.Windows.Ribbon.ButtonCommand btnSave;
        private Janus.Windows.Ribbon.ButtonCommand btnSavePage;
		private Janus.Windows.ButtonBar.ButtonBar buttonBar1;
		private Janus.Windows.Ribbon.RibbonGroup ribbonGroup1;
		private Janus.Windows.Ribbon.ButtonCommand btnClone;
		private Janus.Windows.Ribbon.ButtonCommand btnUndo;
		private Janus.Windows.Ribbon.ButtonCommand btnRedo;
		private System.Windows.Forms.Panel panelMidScreen;
		private System.Windows.Forms.Panel panelInnerBounder;
		private System.Windows.Forms.Panel panelInnerScreen;
		private Janus.Windows.Ribbon.ButtonCommand btnNewPage;
		private Janus.Windows.Ribbon.ButtonCommand btnZoomOut;
		private Janus.Windows.Ribbon.ButtonCommand btnZoomIn;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lbObjectName;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private Janus.Windows.Ribbon.ButtonCommand btnDelete;
    }
}

