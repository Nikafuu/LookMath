namespace LookMath
{
    partial class LookMath
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPicture = new System.Windows.Forms.PictureBox();
            this.MenuPicture = new System.Windows.Forms.Button();
            this.GroupFunctions = new System.Windows.Forms.FlowLayoutPanel();
            this.Coordinate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MainPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPicture
            // 
            this.MainPicture.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MainPicture.Location = new System.Drawing.Point(0, 0);
            this.MainPicture.Margin = new System.Windows.Forms.Padding(4);
            this.MainPicture.Name = "MainPicture";
            this.MainPicture.Size = new System.Drawing.Size(887, 447);
            this.MainPicture.TabIndex = 0;
            this.MainPicture.TabStop = false;
            this.MainPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.MainPicture_Paint);
            this.MainPicture.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainPicture_MouseDown);
            this.MainPicture.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainPicture_MouseMove);
            this.MainPicture.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainPicture_MouseUp);
            // 
            // MenuPicture
            // 
            this.MenuPicture.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.MenuPicture.Font = new System.Drawing.Font("Segoe Script", 12F, System.Drawing.FontStyle.Bold);
            this.MenuPicture.Location = new System.Drawing.Point(12, 12);
            this.MenuPicture.Name = "MenuPicture";
            this.MenuPicture.Size = new System.Drawing.Size(59, 52);
            this.MenuPicture.TabIndex = 1;
            this.MenuPicture.Text = ">>";
            this.MenuPicture.UseVisualStyleBackColor = false;
            this.MenuPicture.Click += new System.EventHandler(this.MenuPicture_Click);
            // 
            // GroupFunctions
            // 
            this.GroupFunctions.AutoScroll = true;
            this.GroupFunctions.Location = new System.Drawing.Point(0, 0);
            this.GroupFunctions.Name = "GroupFunctions";
            this.GroupFunctions.Size = new System.Drawing.Size(400, 447);
            this.GroupFunctions.TabIndex = 4;
            this.GroupFunctions.Visible = false;
            // 
            // Coordinate
            // 
            this.Coordinate.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Coordinate.Location = new System.Drawing.Point(758, 402);
            this.Coordinate.Name = "Coordinate";
            this.Coordinate.Size = new System.Drawing.Size(118, 39);
            this.Coordinate.TabIndex = 5;
            this.Coordinate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LookMath
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 450);
            this.Controls.Add(this.Coordinate);
            this.Controls.Add(this.GroupFunctions);
            this.Controls.Add(this.MenuPicture);
            this.Controls.Add(this.MainPicture);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "LookMath";
            this.Text = "LookMath";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.LookMath_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.MainPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainPicture;
        private System.Windows.Forms.Button MenuPicture;
        private System.Windows.Forms.FlowLayoutPanel GroupFunctions;
        private System.Windows.Forms.Label Coordinate;
    }
}

