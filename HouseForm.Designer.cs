namespace houseOmatic
{
    partial class HouseForm
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
            // Dispose of components.
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            // Dispose of the house layout we created.
            if (disposing && (layout != null))
            {
                layout.Dispose();
            }
            // Call parent class Dispose.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HouseForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.modeLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itemSelect = new System.Windows.Forms.ToolStripComboBox();
            this.openDlg = new System.Windows.Forms.OpenFileDialog();
            this.dataRows = new System.Data.DataSet();
            this.itemGrid = new System.Windows.Forms.DataGridView();
            this.dgvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvXPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvZPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvYPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRotation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPitch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRoll = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvScale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCrated = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator,
            this.modeLabel,
            this.toolStripSeparator1,
            this.itemSelect});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(784, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(56, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.Enabled = false;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(51, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // modeLabel
            // 
            this.modeLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modeLabel.ForeColor = System.Drawing.Color.Blue;
            this.modeLabel.Name = "modeLabel";
            this.modeLabel.Size = new System.Drawing.Size(58, 22);
            this.modeLabel.Text = "File Mode";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // itemSelect
            // 
            this.itemSelect.DropDownWidth = 300;
            this.itemSelect.Enabled = false;
            this.itemSelect.Name = "itemSelect";
            this.itemSelect.Size = new System.Drawing.Size(300, 25);
            this.itemSelect.SelectedIndexChanged += new System.EventHandler(this.itemSelect_SelectedIndexChanged);
            // 
            // openDlg
            // 
            this.openDlg.AddExtension = false;
            this.openDlg.SupportMultiDottedExtensions = true;
            this.openDlg.Title = "Select EverQuest 2 house layout file to load...";
            // 
            // dataRows
            // 
            this.dataRows.DataSetName = "House Contents";
            // 
            // itemGrid
            // 
            this.itemGrid.AllowUserToAddRows = false;
            this.itemGrid.AllowUserToOrderColumns = true;
            this.itemGrid.AutoGenerateColumns = false;
            this.itemGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.itemGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.itemGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvName,
            this.dgvXPos,
            this.dgvZPos,
            this.dgvYPos,
            this.dgvRotation,
            this.dgvPitch,
            this.dgvRoll,
            this.dgvScale,
            this.dgvCrated});
            this.itemGrid.DataSource = this.dataRows;
            this.itemGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemGrid.Location = new System.Drawing.Point(0, 25);
            this.itemGrid.Name = "itemGrid";
            this.itemGrid.RowHeadersVisible = false;
            this.itemGrid.RowHeadersWidth = 14;
            this.itemGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.itemGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.itemGrid.Size = new System.Drawing.Size(784, 337);
            this.itemGrid.TabIndex = 1;
            this.itemGrid.Visible = false;
            this.itemGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.itemGrid_CellValidating);
            this.itemGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.itemGrid_CellValueChanged);
            this.itemGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.itemGrid_CurrentCellDirtyStateChanged);
            this.itemGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.itemGrid_DataError);
            this.itemGrid.SelectionChanged += new System.EventHandler(this.itemGrid_SelectionChanged);
            // 
            // dgvName
            // 
            this.dgvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvName.DataPropertyName = "Name";
            this.dgvName.FillWeight = 95F;
            this.dgvName.HeaderText = "Name";
            this.dgvName.Name = "dgvName";
            this.dgvName.ReadOnly = true;
            // 
            // dgvXPos
            // 
            this.dgvXPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvXPos.DataPropertyName = "XPos";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvXPos.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvXPos.FillWeight = 90F;
            this.dgvXPos.HeaderText = "Left/Right";
            this.dgvXPos.MaxInputLength = 15;
            this.dgvXPos.Name = "dgvXPos";
            this.dgvXPos.ToolTipText = "Position across the width of the building.";
            this.dgvXPos.Width = 80;
            // 
            // dgvZPos
            // 
            this.dgvZPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvZPos.DataPropertyName = "ZPos";
            this.dgvZPos.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvZPos.FillWeight = 90F;
            this.dgvZPos.HeaderText = "Up/Down";
            this.dgvZPos.MaxInputLength = 15;
            this.dgvZPos.Name = "dgvZPos";
            this.dgvZPos.ToolTipText = "Height within the building.";
            this.dgvZPos.Width = 79;
            // 
            // dgvYPos
            // 
            this.dgvYPos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvYPos.DataPropertyName = "YPos";
            this.dgvYPos.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvYPos.FillWeight = 90F;
            this.dgvYPos.HeaderText = "Fwd/Back";
            this.dgvYPos.MaxInputLength = 15;
            this.dgvYPos.Name = "dgvYPos";
            this.dgvYPos.ToolTipText = "Distance away from the entrance of the building.";
            this.dgvYPos.Width = 82;
            // 
            // dgvRotation
            // 
            this.dgvRotation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvRotation.DataPropertyName = "XRot";
            this.dgvRotation.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRotation.FillWeight = 90F;
            this.dgvRotation.HeaderText = "Rotate";
            this.dgvRotation.MaxInputLength = 9;
            this.dgvRotation.Name = "dgvRotation";
            this.dgvRotation.ToolTipText = "Controls which way the object is facing.";
            this.dgvRotation.Width = 64;
            // 
            // dgvPitch
            // 
            this.dgvPitch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvPitch.DataPropertyName = "ZRot";
            this.dgvPitch.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPitch.FillWeight = 90F;
            this.dgvPitch.HeaderText = "Pitch";
            this.dgvPitch.MaxInputLength = 9;
            this.dgvPitch.Name = "dgvPitch";
            this.dgvPitch.ToolTipText = "Controls the object\'s rotation relative to the floor/ceiling.";
            this.dgvPitch.Width = 56;
            // 
            // dgvRoll
            // 
            this.dgvRoll.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvRoll.DataPropertyName = "YRot";
            this.dgvRoll.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRoll.FillWeight = 90F;
            this.dgvRoll.HeaderText = "Roll";
            this.dgvRoll.MaxInputLength = 9;
            this.dgvRoll.Name = "dgvRoll";
            this.dgvRoll.ToolTipText = "Controls the object\'s roll.";
            this.dgvRoll.Width = 50;
            // 
            // dgvScale
            // 
            this.dgvScale.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvScale.DataPropertyName = "Scale";
            this.dgvScale.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvScale.FillWeight = 75F;
            this.dgvScale.HeaderText = "Scale";
            this.dgvScale.MaxInputLength = 7;
            this.dgvScale.Name = "dgvScale";
            this.dgvScale.ToolTipText = "Controls how large the object will appear.";
            this.dgvScale.Width = 59;
            // 
            // dgvCrated
            // 
            this.dgvCrated.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvCrated.DataPropertyName = "Crated";
            this.dgvCrated.FalseValue = "false";
            this.dgvCrated.FillWeight = 20F;
            this.dgvCrated.HeaderText = "Crated";
            this.dgvCrated.Name = "dgvCrated";
            this.dgvCrated.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvCrated.ToolTipText = "Check to hide the object by putting it into the moving crate!";
            this.dgvCrated.TrueValue = "true";
            this.dgvCrated.Width = 63;
            // 
            // HouseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.itemGrid);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HouseForm";
            this.Text = "houseOmatic";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.OpenFileDialog openDlg;
        private System.Data.DataSet dataRows;
        private System.Windows.Forms.DataGridView itemGrid;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripComboBox itemSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvXPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvZPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvYPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRotation;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvPitch;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvRoll;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvScale;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvCrated;
        private System.Windows.Forms.ToolStripLabel modeLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

