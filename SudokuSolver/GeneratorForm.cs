using System;
using System.Windows.Forms;

namespace SudokuSolver
{
    /// <summary>
    /// This class represents the main window of the application.
    /// </summary>
    public partial class GeneratorForm : Form
    {
        private ViewerForm SudokuViewer;

        /// <summary>
        /// Initializes and renders the form.
        /// </summary>
        public GeneratorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens a new ViewerForm when the 'Generate Sudoku' button is clicked.
        /// </summary>
        /// <param name="sender">Contains data about the form control on which the event fired.</param>
        /// <param name="e">Contains data relevant to the triggered event.</param>
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            // If another ViewerForm is still open, close it.
            if(SudokuViewer != null)
            {
                SudokuViewer.Close();
            }

            // Create a new ViewerForm, getting the size the sudoku will have to be from the 'Grid Size:' numericUpDown control on the GeneratorForm,
            // and show the new ViewerForm.
            SudokuViewer = new ViewerForm((int)gridSizeNumeric.Value);
            SudokuViewer.Show();
        }

        /// <summary>
        /// Read the values entered by the user from the ViewerForm and put them in a SudokuEntries datastructure,
        /// solve the sudoku for the read values and write the full solution back to the ViewerForm.
        /// </summary>
        /// <param name="sender">Contains data about the form control on which the event fired.</param>
        /// <param name="e">Contains data relevant to the triggered event.</param>
        private void SolveButton_Click(object sender, EventArgs e)
        {
            // If no ViewerForm currently exists or is rendered to the screen (i.e. it has been closed by the user),
            // notify the user a sudoku needs to be generated first.
            if(SudokuViewer == null || SudokuViewer.Created == false)
            {
                MessageBox.Show("Please generate a sudoku first.");
                return;
            }

            // Reset the progress bar on the ViewerForm, in case the same ViewerForm is used multiple times consecutively
            // to solve multiple sudokus.
            (SudokuViewer.Controls[1] as ProgressBar).Value = 0;

            try
            {
                // Read the values entered by the user from the ViewerForm.
                SudokuViewer.Sudoku.ReadEntries(SudokuViewer);
                
                // Solve the sudoku for the given values. The ViewerForm is being passed as an argument, so we can control the progress bar
                // from the Solve() function.
                SudokuViewer.Sudoku.Solve(SudokuViewer);

                // Write the full solution back to the ViewerForm.
                SudokuViewer.Sudoku.WriteEntries(SudokuViewer);
            }
            // If an error occurs while executing the functions in the above 'try' block (i.e. the user entered an invalid value in the sudoku),
            // show the user the relevant error message.
            catch (InvalidSudokuException error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
