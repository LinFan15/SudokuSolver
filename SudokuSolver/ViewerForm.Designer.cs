namespace SudokuSolver
{
    partial class ViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerForm));
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.sudokuContainerPanel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.mainProgressBar.Location = new System.Drawing.Point(0, 441);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(485, 23);
            this.mainProgressBar.TabIndex = 0;
            // 
            // sudokuContainerPanel
            // 
            this.sudokuContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sudokuContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.sudokuContainerPanel.Name = "sudokuContainerPanel";
            this.sudokuContainerPanel.Size = new System.Drawing.Size(485, 441);
            this.sudokuContainerPanel.TabIndex = 1;
            // 
            // ViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(485, 464);
            this.Controls.Add(this.sudokuContainerPanel);
            this.Controls.Add(this.mainProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sudoku Solver";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar mainProgressBar;
        private System.Windows.Forms.Panel sudokuContainerPanel;
    }
}