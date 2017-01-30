namespace ArduinoNetDemo
{
    partial class Form1
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
            this.btnFlash = new System.Windows.Forms.Button();
            this.lblKnob = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnFlash
            // 
            this.btnFlash.Location = new System.Drawing.Point(226, 137);
            this.btnFlash.Name = "btnFlash";
            this.btnFlash.Size = new System.Drawing.Size(136, 23);
            this.btnFlash.TabIndex = 0;
            this.btnFlash.Text = "Turn On LED";
            this.btnFlash.UseVisualStyleBackColor = true;
            this.btnFlash.Click += new System.EventHandler(this.btnFlash_Click);
            // 
            // lblKnob
            // 
            this.lblKnob.AutoSize = true;
            this.lblKnob.Location = new System.Drawing.Point(226, 209);
            this.lblKnob.Name = "lblKnob";
            this.lblKnob.Size = new System.Drawing.Size(0, 13);
            this.lblKnob.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 298);
            this.Controls.Add(this.lblKnob);
            this.Controls.Add(this.btnFlash);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFlash;
        private System.Windows.Forms.Label lblKnob;
    }
}

