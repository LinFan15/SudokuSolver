using System.Collections.Generic;

namespace SudokuSolver
{
    /// <summary>
    /// This class holds a list of type SudokuEntry and provides custom methods for interacting with that list.
    /// </summary>
    class SudokuEntries
    {
        public List<SudokuEntry> Entries { get; private set; }

        /// <summary>
        /// Constructs a new instance of the SudokuEntries class.
        /// </summary>
        public SudokuEntries()
        {
            Entries = new List<SudokuEntry>();
        }

        /// <summary>
        /// Adds a new SudokuEntry to the 'Entries' list, provided the new SudokuEntry is valid for the current sudoku.
        /// </summary>
        /// <param name="value">The 'value' the new SudokuEntry should take.</param>
        /// <param name="blockIndex">The 'blockIndex' for the new SudokuEntry.</param>
        /// <param name="columnIndex">The 'columnIndex' for the new SudokuEntry.</param>
        /// <param name="rowIndex">The 'rowIndex' for the new SudokuEntry.</param>
        /// <param name="initialRead">
        /// Is true when the SudokuEntry is being added through Sudoku.ReadEntries(),
        /// which means that the value for this SudokuEntry was added by the user, so it should be locked.
        /// </param>
        /// <returns>True if the new SudokuEntry is valid and was added successfully, otherwise false.</returns>
        public bool AddEntry(int value, int blockIndex, int columnIndex, int rowIndex, bool initialRead)
        {
            // A value in a sudoku must always be >=1, so a value of 0 is invalid.
            if(value == 0)
            {
                return false;
            }

            SudokuEntry sudokuEntry = new SudokuEntry(value, blockIndex, rowIndex, columnIndex, initialRead);

            // If sudokuEntry is valid for the current sudoku, add it to the 'Entries' list and return true, otherwise return false. 
            if (EntryIsValid(sudokuEntry))
            {
                Entries.Add(sudokuEntry);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a SudokuEntry from the 'Entries' list.
        /// </summary>
        /// <param name="columnIndex">The columnIndex of the SudokuEntry that needs to be removed.</param>
        /// <param name="rowIndex">The rowIndex of the SudokuEntry that needs to be removed.</param>
        /// <returns>True if the SudokuEntry was removed successfully, otherwise false.</returns>
        public bool RemoveEntry(int columnIndex, int rowIndex)
        {
            SudokuEntry sudokuEntry = FindEntry(columnIndex, rowIndex);

            // If there isn't a SudokuEntry for the specified rowIndex and columnIndex,
            // or the found SudokuEntry is locked, we can't remove it, so return false.
            if (sudokuEntry == null || sudokuEntry.Locked)
            {
                return false;
            }
            else
            {
                Entries.Remove(sudokuEntry);
                return true;
            }
        }

        /// <summary>
        /// Finds the SudokuEntry in 'Entries' for the specified columnIndex and rowIndex.
        /// </summary>
        /// <param name="columnIndex">The columnIndex of the SudokuEntry that we want to find.</param>
        /// <param name="rowIndex">The rowIndex of the SudokuEntry that we want to find.</param>
        /// <returns>The SudokuEntry we want to find, if it exists, otherwise null.</returns>
        public SudokuEntry FindEntry(int columnIndex, int rowIndex)
        {
            foreach(SudokuEntry sudokuEntry in Entries)
            {
                if(sudokuEntry.ColumnIndex == columnIndex && sudokuEntry.RowIndex == rowIndex)
                {
                    return sudokuEntry;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks whether a SudokuEntry is locked or not.
        /// </summary>
        /// <param name="columnIndex">The columnIndex of the SudokuEntry that we want to check.</param>
        /// <param name="rowIndex">The rowIndex of the SudokuEntry that we want to check.</param>
        /// <returns>True if the SudokuEntry for the given rowIndex and columnIndex is locked, otherwise false.</returns>
        public bool EntryIsLocked(int columnIndex, int rowIndex)
        {
            SudokuEntry sudokuEntry = FindEntry(columnIndex, rowIndex);

            // If there isn't a SudokuEntry for the specified rowIndex and columnIndex, it çan't be locked.
            if (sudokuEntry == null || sudokuEntry.Locked == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if a given SudokuEntry is valid for the current sudoku.
        /// </summary>
        /// <param name="sudokuEntry">The SudokuEntry that we want to check.</param>
        /// <returns>True if the given SudokuEntry is valid for the current sudoku, otherwise false.</returns>
        private bool EntryIsValid(SudokuEntry sudokuEntry)
        {
            foreach(SudokuEntry entry in Entries)
            {
                // First, we check if there is a SudokuEntry with the same value as sudokuEntry in the same block, column or row as sudokuEntry.
                // If there is, sudokuEntry is invalid, so we return false.
                if (entry.Value == sudokuEntry.Value)
                {
                    if (entry.BlockIndex == sudokuEntry.BlockIndex)
                    {
                        return false;
                    }
                    if (entry.ColumnIndex == sudokuEntry.ColumnIndex)
                    {
                        return false;
                    }
                    if (entry.RowIndex == sudokuEntry.RowIndex)
                    {
                        return false;
                    }
                }

                // Second, we check whether there already is a SudokuEntry at the same location as sudokuEntry
                // and if there is, and it's locked, sudokuEntry is also invalid, so we return false.
                if (entry.ColumnIndex == sudokuEntry.ColumnIndex && entry.RowIndex == sudokuEntry.RowIndex && entry.Locked)
                {
                    return false;
                }
            }

            return true;
        }

    }
}