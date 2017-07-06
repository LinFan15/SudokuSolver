using System;
using System.Windows.Forms;

namespace SudokuSolver
{
    /// <summary>
    /// Represents the sudoku itself and contains all data relevant to the sudoku.
    /// </summary>
    class Sudoku
    {
        public int GridSize { get; private set; }
        public BlockSize BlockSize { get; private set; }
        public SudokuEntries EntriesList { get; private set; }

        /// <summary>
        /// Constructs a new instance of the Sudoku class, given the necessary arguments.
        /// </summary>
        /// <param name="gridSize">The 'gridSize' equals the amount of cells in a block, row or column of the sudoku.</param>
        public Sudoku(int gridSize)
        {
            GridSize = gridSize;
            BlockSize = new BlockSize(GridSize);
            EntriesList = new SudokuEntries();
        }

        /// <summary>
        /// Read all values entered by the user from the ViewerForm.
        /// </summary>
        /// <param name="sudokuViewer">The current ViewerForm instance.</param>
        public void ReadEntries(ViewerForm sudokuViewer)
        {
            // Clear the SudokuEntries.Entries list, so the user can solve multiple sudokus without having to restart the application.
            EntriesList.Entries.Clear();

            // Iterate over all numericUpDown controls in the ViewerForm and 
            // attempt to add a SudokuEntry to 'EntriesList' for all of them.
            for (int i = 0; i < GridSize; i++)
            {
                for (int i2 = 0; i2 < GridSize; i2++)
                {
                    Control sudokuControl = sudokuViewer.Controls[0].Controls[0].Controls[i].Controls[i2];

                    // If 'sudokuControl.Text' is empty, the user didn't set a value for this cell in the sudoku, so set value to 0.
                    int value = sudokuControl.Text == "" ? 0 : Convert.ToInt32((sudokuControl as NumericUpDown).Value);

                    int blockIndex = i;
                    int columnIndex = ((blockIndex % (GridSize / BlockSize.Horizontal)) * BlockSize.Horizontal) + (i2 % BlockSize.Horizontal);
                    int rowIndex = (int)(((Math.Floor((float)(blockIndex / (GridSize / BlockSize.Horizontal)))) * BlockSize.Vertical) +
                        (Math.Floor((float)(i2 / BlockSize.Horizontal))));

                    bool result = EntriesList.AddEntry(value, blockIndex, columnIndex, rowIndex, true);

                    // If 'EntriesList.AddEntry()' returns false and 'value' != 0, the value the user entered is invalid, so throw an exception.
                    if (result == false && value != 0)
                    {
                        throw new InvalidSudokuException("There are one or more invalid entries in the sudoku.");
                    }
                }
            }
        }

        /// <summary>
        /// Write the solution to the sudoku back to the ViewerForm.
        /// </summary>
        /// <param name="sudokuViewer">The current ViewerForm instance.</param>
        public void WriteEntries(ViewerForm sudokuViewer)
        {
            foreach (SudokuEntry sudokuEntry in EntriesList.Entries)
            {
                int indexInBlock = (sudokuEntry.ColumnIndex - (sudokuEntry.BlockIndex * BlockSize.Horizontal)) + (sudokuEntry.RowIndex * BlockSize.Horizontal);
                Control numericControl = sudokuViewer.Controls[0].Controls[0].Controls[sudokuEntry.BlockIndex].Controls[indexInBlock];

                // Explicitly set 'numericControl.Text' to be equal to 'sudokuEntry.Value',
                // because this doesn't always happen automatically when setting the value of 'numericControl'.
                numericControl.Text = sudokuEntry.Value.ToString();
                (numericControl as NumericUpDown).Value = sudokuEntry.Value;
            }
        }

        /// <summary>
        /// Solve the sudoku.
        /// </summary>
        /// <param name="sudokuViewer">The current ViewerForm instance.</param>
        public void Solve(ViewerForm sudokuViewer)
        {
            // Here 'i' represents the columnIndex, while 'i2' respresents the rowIndex. 
            for (int i = 0; i < GridSize; i++)
            {
                for (int i2 = 0; i2 < GridSize; i2++)
                {
                    int blockIndex = Convert.ToInt32((Math.Floor((double)(i / BlockSize.Horizontal))) +
                        ((Math.Floor((double)(i2 / BlockSize.Vertical))) * (GridSize / BlockSize.Horizontal)));

                    SudokuEntry currentEntry = EntriesList.FindEntry(i, i2);

                    // If there isn't a SudokuEntry at this location yet, set current value to 0.
                    int value = currentEntry == null ? 0 : currentEntry.Value;

                    // If the SudokuEntry at this location is locked, continue to the next location by incrementing the inner for-loop.
                    if (currentEntry != null && currentEntry.Locked)
                    {
                        continue;
                    }

                    // If there already is a SudokuEntry at this location, which is unlocked, remove it from the 'EntriesList'
                    // so we don't end up with multiple instances of SudokuEntry for the same location.
                    if (value > 0)
                    {
                        EntriesList.RemoveEntry(i, i2);
                    }

                   // Here we increment 'value' so we don't end up in an endless loop where Solve() constantly finds the same value for the same location
                   // after backtracking.
                    if (Solve((value + 1), blockIndex, i, i2) == false)
                    {
                        // If we couldn't find a valid value for this location, backtrack.
                        int[] vars = DetermineNext(i, i2);
                        i = vars[0];
                        i2 = vars[1];
                    }

                    // Update the progress bar to reflect current progress.
                    (sudokuViewer.Controls[1] as ProgressBar).Value = (int)Math.Round((100d / ((double)GridSize * (double)GridSize)) * ((double)(i + 1) * (double)(GridSize)));
                }
            }
            // Set the progress bar to 100% when we are finished, to account for the user entering a sudoku
            // where the final column is already completely solved. 
            (sudokuViewer.Controls[1] as ProgressBar).Value = 100;
        }

        /// <summary>
        /// Recursively finds the next valid value for a new SudokuEntry at the specified location.
        /// </summary>
        /// <param name="value">The next value to check.</param>
        /// <param name="blockIndex">The 'blockIndex' for the new SudokuEntry.</param>
        /// <param name="columnIndex">The 'columnIndex' for the new SudokuEntry.</param>
        /// <param name="rowIndex">The 'rowIndex' for the new SudokuEntry.</param>
        /// <returns>True if it successfully found a valid value and added a new SudokuEntry with that value, otherwise false.</returns>
        private bool Solve(int value, int blockIndex, int columnIndex, int rowIndex)
        {
            // When value > GridSize, we have checked all possible values, so return false.
            if (value > GridSize)
            {
                return false;
            }

            // We found a valid value and successfully added a SudokuEntry with that value to 'EntriesList', so return true.
            if(EntriesList.AddEntry(value, blockIndex, columnIndex, rowIndex, false))
            {
                return true;
            }

            // Keep trying until we either find a valid value or run out of possible values.
            return Solve((value + 1), blockIndex, columnIndex, rowIndex);
        }

        /// <summary>
        /// Determine the next values for 'i' and 'i2', in order to backtrack to the previous SudokuEntry that isn't locked.
        /// </summary>
        /// <param name="i">Value of 'i' in the outer for-loop of Solve(ViewerForm).</param>
        /// <param name="i2">Value of 'i2' in the inner for-loop of Solve(ViewerForm).</param>
        /// <returns>An array of integers, with new values for 'i' and 'i2'.</returns>
        private int[] DetermineNext(int i, int i2)
        {
            // If i2 == 0, we need to decrement i.
            if (i2 == 0)
            {

                if (i == 0)
                {
                    // If i == 0, we have tried all possible combinations of values for the current sudoku, so we can't find a solution.
                    throw new InvalidSudokuException("No solution was found to the entered sudoku.");
                }
                else
                {
                    i--;

                    // Set i2 to be the at the last iteration of 'i', so we still only go one step back when we change i.
                    i2 = GridSize - 1;

                    // If the SudokuEntry for the new 'i' and 'i2' is locked, continue looking.
                    if (EntriesList.EntryIsLocked(i, i2))
                    {
                        return DetermineNext(i, i2);
                    }
                    else
                    {
                        // Decrement i2 to compensate for the inner for-loop of Solve(ViewerForm), which will immediately increment it again.
                        return new int[] { i, (i2 - 1) };
                    }
                }
            }
            else
            {
                // If i2 > 0, we need to decrement i2.
                i2--;

                // If the SudokuEntry for the new 'i' and 'i2' is locked, continue looking.
                if (EntriesList.EntryIsLocked(i, i2))
                {
                    return DetermineNext(i, i2);
                }
                else
                {
                    // Decrement i2 to compensate for the inner for-loop of Solve(ViewerForm), which will immediately increment it again.
                    return new int[] { i, (i2 - 1) };
                }
            }
        }
    }

    /// <summary>
    /// This exception is thrown when the user entered an invalid sudoku.
    /// </summary>
    class InvalidSudokuException : Exception
    {
        /// <summary>
        /// Constructs a new instance of the exception, without an additional message.
        /// </summary>
        public InvalidSudokuException()
            : base() {
        }

        /// <summary>
        /// Constructs a new instance of the exception, with an additional message.
        /// </summary>
        /// <param name="message"></param>
        public InvalidSudokuException(string message)
            : base(message) {
        }
    }
}
