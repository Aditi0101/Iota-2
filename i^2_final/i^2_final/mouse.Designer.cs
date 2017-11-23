namespace i_2_final
{
    partial class mouse
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
            ReleaseData();
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
            this.captureImageBox = new Emgu.CV.UI.ImageBox();
            this.Color4ImageBox = new Emgu.CV.UI.ImageBox();
            this.cannyImageBox = new Emgu.CV.UI.ImageBox();
            this.smoothedGrayscaleImageBox = new Emgu.CV.UI.ImageBox();
            this.grayscaleImageBox = new Emgu.CV.UI.ImageBox();
            this.ExtraImageBox = new Emgu.CV.UI.ImageBox();
            this.captureButton = new System.Windows.Forms.Button();
            this.horizontal = new System.Windows.Forms.Button();
            this.vertical = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Color4ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothedGrayscaleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grayscaleImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExtraImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // captureImageBox
            // 
            this.captureImageBox.Location = new System.Drawing.Point(12, 47);
            this.captureImageBox.Name = "captureImageBox";
            this.captureImageBox.Size = new System.Drawing.Size(371, 320);
            this.captureImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.captureImageBox.TabIndex = 2;
            this.captureImageBox.TabStop = false;
            // 
            // Color4ImageBox
            // 
            this.Color4ImageBox.Location = new System.Drawing.Point(401, 384);
            this.Color4ImageBox.Name = "Color4ImageBox";
            this.Color4ImageBox.Size = new System.Drawing.Size(382, 330);
            this.Color4ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.Color4ImageBox.TabIndex = 3;
            this.Color4ImageBox.TabStop = false;
            // 
            // cannyImageBox
            // 
            this.cannyImageBox.Location = new System.Drawing.Point(12, 384);
            this.cannyImageBox.Name = "cannyImageBox";
            this.cannyImageBox.Size = new System.Drawing.Size(371, 330);
            this.cannyImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.cannyImageBox.TabIndex = 4;
            this.cannyImageBox.TabStop = false;
            // 
            // smoothedGrayscaleImageBox
            // 
            this.smoothedGrayscaleImageBox.Location = new System.Drawing.Point(799, 47);
            this.smoothedGrayscaleImageBox.Name = "smoothedGrayscaleImageBox";
            this.smoothedGrayscaleImageBox.Size = new System.Drawing.Size(389, 320);
            this.smoothedGrayscaleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.smoothedGrayscaleImageBox.TabIndex = 5;
            this.smoothedGrayscaleImageBox.TabStop = false;
            // 
            // grayscaleImageBox
            // 
            this.grayscaleImageBox.Location = new System.Drawing.Point(401, 47);
            this.grayscaleImageBox.Name = "grayscaleImageBox";
            this.grayscaleImageBox.Size = new System.Drawing.Size(382, 320);
            this.grayscaleImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.grayscaleImageBox.TabIndex = 6;
            this.grayscaleImageBox.TabStop = false;
            // 
            // ExtraImageBox
            // 
            this.ExtraImageBox.Location = new System.Drawing.Point(806, 384);
            this.ExtraImageBox.Name = "ExtraImageBox";
            this.ExtraImageBox.Size = new System.Drawing.Size(382, 330);
            this.ExtraImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ExtraImageBox.TabIndex = 7;
            this.ExtraImageBox.TabStop = false;
            // 
            // captureButton
            // 
            this.captureButton.Location = new System.Drawing.Point(86, 12);
            this.captureButton.Name = "captureButton";
            this.captureButton.Size = new System.Drawing.Size(96, 23);
            this.captureButton.TabIndex = 8;
            this.captureButton.Text = "start";
            this.captureButton.UseVisualStyleBackColor = true;
            this.captureButton.Click += new System.EventHandler(this.captureButton_Click);
            // 
            // horizontal
            // 
            this.horizontal.Location = new System.Drawing.Point(236, 12);
            this.horizontal.Name = "horizontal";
            this.horizontal.Size = new System.Drawing.Size(96, 23);
            this.horizontal.TabIndex = 9;
            this.horizontal.Text = "horizontal";
            this.horizontal.UseVisualStyleBackColor = true;
            this.horizontal.Click += new System.EventHandler(this.horizontal_Click);
            // 
            // vertical
            // 
            this.vertical.Location = new System.Drawing.Point(372, 12);
            this.vertical.Name = "vertical";
            this.vertical.Size = new System.Drawing.Size(96, 23);
            this.vertical.TabIndex = 10;
            this.vertical.Text = "vertical";
            this.vertical.UseVisualStyleBackColor = true;
            this.vertical.Click += new System.EventHandler(this.vertical_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(545, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "red";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(939, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "blue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 368);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "green";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(596, 368);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "yellow";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(894, 370);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "label6";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(503, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(178, 22);
            this.button1.TabIndex = 17;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // mouse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 736);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vertical);
            this.Controls.Add(this.horizontal);
            this.Controls.Add(this.captureButton);
            this.Controls.Add(this.ExtraImageBox);
            this.Controls.Add(this.grayscaleImageBox);
            this.Controls.Add(this.smoothedGrayscaleImageBox);
            this.Controls.Add(this.cannyImageBox);
            this.Controls.Add(this.Color4ImageBox);
            this.Controls.Add(this.captureImageBox);
            this.Name = "mouse";
            this.Text = "mouse";
            ((System.ComponentModel.ISupportInitialize)(this.captureImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Color4ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothedGrayscaleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grayscaleImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExtraImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox captureImageBox;
        private Emgu.CV.UI.ImageBox Color4ImageBox;
        private Emgu.CV.UI.ImageBox cannyImageBox;
        private Emgu.CV.UI.ImageBox smoothedGrayscaleImageBox;
        private Emgu.CV.UI.ImageBox grayscaleImageBox;
        private Emgu.CV.UI.ImageBox ExtraImageBox;
        private System.Windows.Forms.Button captureButton;
        private System.Windows.Forms.Button horizontal;
        private System.Windows.Forms.Button vertical;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;


    }
}

