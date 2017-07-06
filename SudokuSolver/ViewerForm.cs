using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    /// <summary>
    /// This class represents the window of the application which allows the user to view the sudoku and edit it's values.
    /// </summary>
    public partial class ViewerForm : Form
    {
        internal Sudoku Sudoku;

        /// <summary>
        /// Initializes and renders the form, using the gridSize to determine how to dynamically add and
        /// style form controls.
        /// </summary>
        /// <param name="gridSize">The 'gridSize' equals the amount of cells in a block, row or column of the sudoku.</param>
        public ViewerForm(int gridSize)
        {
            InitializeComponent();

            Sudoku = new Sudoku(gridSize);

            // sudokuColumnCount equals the amount of columns needed in the top-level TableLayoutPanel
            // to be able to show all blocks in the sudoku.
            int sudokuColumnCount = Sudoku.GridSize / Sudoku.BlockSize.Horizontal;

            // The width of a column in a TableLayoutPanel is defined as a percentage, so we divide 100 by sudokuColumnCount.
            float sudokuColumnWidth = 100 / sudokuColumnCount;


            // sudokuRowCount equals the amount of rows needed in the top-level TableLayoutPanel
            // to be able to show all blocks in the sudoku.
            int sudokuRowCount = Sudoku.GridSize / Sudoku.BlockSize.Vertical;

            // The height of a row in a TableLayoutPanel is defined as a percentage, so we divide 100 by sudokuRowCount.
            float sudokuRowHeight = 100 / sudokuRowCount;


            // 'Width' and 'Height' are builtin properties of the 'Form' class, representing the width and height of the form.
            // The formulas for them are defined as follows:
            // Width = (int)(((width of numericUpDown) + (extra required width per digit) * (Math.Floor(Math.Log10(Sudoku.GridSize)) + 1)) * Sudoku.GridSize + 
            //      sudokuColumnCount * ((total horizontal margin along the outside of all numericUpDowns in a single block) + 
            //      (horizontal margin between two numericUpDowns in the same block) * (Sudoku.BlockSize.Horizontal - 1)) + 22);
            //
            // Height = ((height of numericUpDown) * Sudoku.GridSize + sudokuRowCount * ((total vertical margin along the outside of all numericUpDowns in a single block) +
            //      (vertical margin between two numericUpDowns in the same block) * (Sudoku.BlockSize.Vertical - 1))) + (height of progressBar);
            Width = (int)((22 + 25 * (Math.Floor(Math.Log10(Sudoku.GridSize)) + 1)) * Sudoku.GridSize + sudokuColumnCount * (12 + 6 * (Sudoku.BlockSize.Horizontal - 1)) + 22);
            Height = (41 * Sudoku.GridSize + sudokuRowCount * (15 + 9 * (Sudoku.BlockSize.Vertical - 1))) + 71;


            // Create the top-level TableLayoutPanel with the specified attributes.
            TableLayoutPanel mainSudokuContainer = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.OutsetDouble,

                ColumnCount = sudokuColumnCount,
                RowCount = sudokuRowCount
            };

            // Set the width for all columns in the top-level TableLayoutPanel.
            for (int i = 0; i < sudokuColumnCount; i++)
            {
                mainSudokuContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, sudokuColumnWidth));
            }

            // Set the height for all rows in the top-level TableLayoutPanel.
            for (int i = 0; i < sudokuRowCount; i++)
            {
                mainSudokuContainer.RowStyles.Add(new RowStyle(SizeType.Percent, sudokuRowHeight));
            }

            // Each cell in the top-level TableLayoutPanel makes up one block of the sudoku;
            // here we fill each of these blocks with another TableLayoutPanel, which will contain the actual cells of the sudoku.
            for (int i = 0; i < Sudoku.GridSize; i++)
            {
                mainSudokuContainer.Controls.Add(GenerateSudokuBlockContainer(i+1));
            }

            // Add the top-level TableLayoutPanel with all of it's child controls to the ViewerForm.
            sudokuContainerPanel.Controls.Add(mainSudokuContainer);
        }

        /// <summary>
        /// Creates a fully styled TableLayoutPanel with necessary child controls, which will make up one block of the sudoku.
        /// </summary>
        /// <param name="containerNumber">The number of the new TableLayoutPanel, which is used to name it correctly.</param>
        /// <returns>A new TableLayoutPanel, which is ready to be used as a block in the sudoku.</returns>
        private TableLayoutPanel GenerateSudokuBlockContainer(int containerNumber)
        {
            // The width of a column in a TableLayoutPanel is defined as a percentage, so we divide 100 by Sudoku.BlockSize.Horizontal.
            float blockColumnWidth = 100 / Sudoku.BlockSize.Horizontal;

            // The height of a row in a TableLayoutPanel is defined as a percentage, so we divide 100 by Sudoku.BlockSize.Vertical.
            float blockRowHeight = 100 / Sudoku.BlockSize.Vertical;


            // Create the new TableLayoutPanel with the specified attributes.
            TableLayoutPanel sudokuBlockContainer = new TableLayoutPanel()
            {
                Name = $"sudokuBlockContainer{containerNumber}",
                Dock = DockStyle.Fill,

                ColumnCount = Sudoku.BlockSize.Horizontal,
                RowCount = Sudoku.BlockSize.Vertical
            };

            // Set the width for all columns in the new TableLayoutPanel.
            for (int i = 0; i < Sudoku.BlockSize.Horizontal; i++)
            {
                sudokuBlockContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, blockColumnWidth));
            }

            // Set the height for all rows in the new TableLayoutPanel.
            for (int i = 0; i < Sudoku.BlockSize.Vertical; i++)
            {
                sudokuBlockContainer.RowStyles.Add(new RowStyle(SizeType.Percent, blockRowHeight));
            }

            // Each cell in the new TableLayoutPanel constitutes one cell of the sudoku;
            // here we fill each of these cells with a numericUpDown control, which will contain the value for it's respective cell of the sudoku.
            for (int i = 0; i < Sudoku.GridSize; i++)
            {
                sudokuBlockContainer.Controls.Add(new NumericUpDown()
                {
                    Name = $"numeric{i + 1}",
                    Font = new Font(FontFamily.GenericSansSerif, 18),
                    TextAlign = HorizontalAlignment.Center,

                    // Set width of numericUpDown control, dependent on the number of digits we need to show.
                    Width = (int)(22 + 25 * (Math.Floor(Math.Log10(Sudoku.GridSize)) + 1)),

                    // Set the 'Text' property to an empty string, so the user doesn't see a bunch of zeroes when generating a new sudoku.
                    Text = "",
                    Minimum = 0,
                    
                    // Dynamically set the 'Maximum' to account for sudokus of different sizes.
                    Maximum = Sudoku.GridSize
                });
            }
            // Return the new TableLayoutPanel with it's child controls.
            return sudokuBlockContainer;
        }
    }
}
