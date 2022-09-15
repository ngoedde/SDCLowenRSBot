using System.ComponentModel;
using System.Windows.Forms;

namespace RSBot.Views.Controls
{
    [ToolboxItem(false)]
    public partial class CosControlBase : UserControl
    {

        private SDUI.Controls.Label label1;
        protected SDUI.Controls.Label lblPetName;
        private Panel panel1;
        protected SDUI.Controls.Label labelLevel;
        protected SDUI.Controls.ProgressBar progressHP;

        public MiniCosControl MiniCosControl { get; private set; }

        public CosControlBase()
        {
            MiniCosControl = new();
            MiniCosControl.Dock = DockStyle.Left;
            InitializeComponent();
        }

        public virtual void Initialize()
        {

        }

        public virtual void Reset()
        {
            progressHP.Value = 0;
            MiniCosControl.Hp.Value = 0;

            progressHP.Maximum = 0;
            MiniCosControl.Hp.Maximum = 0;
        }

        private void InitializeComponent()
        {
            this.label1 = new();
            this.lblPetName = new();
            this.progressHP = new();
            this.panel1 = new();
            this.labelLevel = new();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new(14, 44);
            this.label1.Name = "label1";
            this.label1.Size = new(26, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "HP:";
            // 
            // lblPetName
            // 
            this.lblPetName.AutoSize = true;
            this.lblPetName.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblPetName.Font = new("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPetName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblPetName.Location = new(0, 0);
            this.lblPetName.Name = "lblPetName";
            this.lblPetName.Size = new(81, 15);
            this.lblPetName.TabIndex = 19;
            this.lblPetName.Text = "No pet found";
            // 
            // progressHP
            // 
            this.progressHP.BackColor = System.Drawing.Color.Transparent;
            this.progressHP.DrawHatch = false;
            this.progressHP.ForeColor = System.Drawing.Color.Firebrick;
            this.progressHP.Gradient = new System.Drawing.Color[] {
        System.Drawing.Color.Maroon,
        System.Drawing.Color.Red};
            this.progressHP.HatchType = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            this.progressHP.Location = new(48, 45);
            this.progressHP.Maximum = ((long)(100));
            this.progressHP.Name = "progressHP";
            this.progressHP.PercentIndices = 2;
            this.progressHP.Radius = 0;
            this.progressHP.ShowAsPercent = false;
            this.progressHP.ShowValue = true;
            this.progressHP.Size = new(180, 16);
            this.progressHP.TabIndex = 18;
            this.progressHP.Text = "0 / 100";
            this.progressHP.Value = ((long)(0));
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelLevel);
            this.panel1.Controls.Add(this.lblPetName);
            this.panel1.Location = new(48, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new(180, 21);
            this.panel1.TabIndex = 21;
            // 
            // labelLevel
            // 
            this.labelLevel.AutoSize = true;
            this.labelLevel.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelLevel.Font = new("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labelLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelLevel.Location = new(81, 0);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new(0, 15);
            this.labelLevel.TabIndex = 20;
            // 
            // CosControlBase
            // 
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressHP);
            this.Font = new("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "CosControlBase";
            this.Size = new(243, 79);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
