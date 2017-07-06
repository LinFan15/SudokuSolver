using System;

namespace SudokuSolver
{
    /// <summary>
    /// This type contains the horizontal and vertical dimensions of the blocks in the sudoku.
    /// </summary>
    class BlockSize
    {
        public int Horizontal { get; private set; }
        public int Vertical { get; private set; }

        /// <summary>
        /// Constructs a new instance of the BlockSize class, calculating the horizontal and vertical dimensions of the blocks in the sudoku
        /// on the basis of the provided gridSize.
        /// </summary>
        /// <param name="gridSize">The 'gridSize' equals the amount of cells in a block, row or column of the sudoku.</param>
        public BlockSize(int gridSize)
        {
            // The horizontal and vertical dimensions of the blocks in the sudoku are defined as 'Vertical' * 'Horizontal' = gridSize, where 'Vertical' and 'Horizontal'
            // are the nearest integers to the square root of gridSize which still result in the equation being valid.
            // Because of this, we start looking for a valid product at the square root of gridSize itself.
            Vertical = (int)Math.Round(Math.Sqrt(gridSize));

            // Call to SetSizes(gridSize) to have it calculate the correct horizontal and vertical dimensions of the blocks in the sudoku,
            // given the 'gridSize', as well as the square root of the 'gridSize', through 'Vertical'.
            SetSizes(gridSize);
        }

        /// <summary>
        /// Calculate the correct horizontal and vertical dimensions of the blocks in the sudoku,
        /// by recursively calling itself until a valid product has been found.
        /// </summary>
        /// <param name="gridSize">The 'gridSize' equals the amount of cells in a block, row or column of the sudoku.</param>
        public void SetSizes(int gridSize)
        {
            // If 'gridSize' / 'Vertical' doesn't leave a remainder, we have found the first factor of the product we are looking for, in 'Vertical'.
            if (gridSize % Vertical == 0)
            {
                // Knowing that 'Vertical' contains the first factor of the product we are looking for,
                // we can use it to deduce the second factor of said product, and store the integer we find in 'Horizontal'.
                Horizontal = gridSize / Vertical;
                return;
            }

            // If we haven't found the correct value for 'Vertical' yet, subtract 1 from 'Vertical' and try again.
            // We don't have to check higher values for 'Vertical', 
            // since if one factor in the product we are looking for is higher than the square root of gridSize, the other one will automatically be lower.
            // The value for 'Vertical', in this case, will be the lower one.
            Vertical--;
            SetSizes(gridSize);
        }
    }
}
