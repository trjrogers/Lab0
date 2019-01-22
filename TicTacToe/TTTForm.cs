using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class TTTForm : Form
    {
        public TTTForm()
        {
            InitializeComponent();
        }

        const string USER_SYMBOL = "X";
        const string COMPUTER_SYMBOL = "O";
        const string EMPTY = "";

        const int SIZE = 5;

        // constants for the 2 diagonals
        const int TOP_LEFT_TO_BOTTOM_RIGHT = 1;
        const int TOP_RIGHT_TO_BOTTOM_LEFT = 2;

        // constants for IsWinner
        const int NONE = -1;
        const int ROW = 1;
        const int COLUMN = 2;
        const int DIAGONAL = 3;

        // counter to track number of squares used in game
        int counter = 0;

        // This method takes a row and column as parameters and 
        // returns a reference to a label on the form in that position
        private Label GetSquare(int row, int column)
        {
            int labelNumber = row * SIZE + column + 1;
            return (Label)(this.Controls["label" + labelNumber.ToString()]);
        }

        // This method does the "reverse" process of GetSquare
        // It takes a label on the form as it's parameter and
        // returns the row and column of that square as output parameters
        private void GetRowAndColumn(Label l, out int row, out int column)
        {
            int position = int.Parse(l.Name.Substring(5));
            row = (position - 1) / SIZE;
            column = (position - 1) % SIZE;
        }

        // This method takes a row (in the range of 0 - 4) and returns true if 
        // the row on the form contains 5 Xs or 5 Os.
        // Use it as a model for writing IsColumnWinner
        private bool IsRowWinner(int row)
        {
            Label square = GetSquare(row, 0);
            string symbol = square.Text;
            for (int col = 1; col < SIZE; col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyRowWinner()
        {
            return false;
        }

        //starts at first row of clicked column and goes through each row detecting if each row shares the same symbol
        //if every row shares same symbol, returns true, else false
        private bool IsColumnWinner(int col)
        {
            Label square = GetSquare(0, col);
            string symbol = square.Text;
            for (int row = 1; row < SIZE; row++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }

            return true;
        }

        private bool IsAnyColumnWinner()
        {
            return false;
        }

        //increments through rows and columns to detect if top left to bottom right diag shares same symbols
        //if it returns true, it's a win for whoever clicked
        private bool IsDiagonal1Winner()
        {
            Label square = GetSquare(0, 0);
            string symbol = square.Text;
            for (int row = 1, col = 1; col < SIZE; row++, col++)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }
        //sames as IsDiagonal1Winner(), but from top right to bottom left
        private bool IsDiagonal2Winner()
        {
            Label square = GetSquare(0, (SIZE - 1));
            string symbol = square.Text;
            for (int row = 1, col = SIZE - 2; row < SIZE; row++, col--)
            {
                square = GetSquare(row, col);
                if (symbol == EMPTY || square.Text != symbol)
                    return false;
            }
            return true;
        }

        private bool IsAnyDiagonalWinner()
        {
            return false;
        }

        //counter keeps track of number of squares that have been used
        //if counter is equal to the total number of squares, then the board is full
        private bool IsFull()
        {
            if (counter == SIZE * SIZE)
            {
                return true;
            }
            else return false;
        }


        // This method determines if any row, column or diagonal on the board is a winner.
        // It returns true or false and the output parameters will contain appropriate values
        // when the method returns true.  See constant definitions at top of form.
        private bool IsWinner(out int whichDimension, out int whichOne)
        {
            // rows
            for (int row = 0; row < SIZE; row++)
            {
                if (IsRowWinner(row))
                {
                    whichDimension = ROW;
                    whichOne = row;
                    return true;
                }
            }
            // columns
            for (int column = 0; column < SIZE; column++)
            {
                if (IsColumnWinner(column))
                {
                    whichDimension = COLUMN;
                    whichOne = column;
                    return true;
                }
            }
            // diagonals
            if (IsDiagonal1Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_LEFT_TO_BOTTOM_RIGHT;
                return true;
            }
            if (IsDiagonal2Winner())
            {
                whichDimension = DIAGONAL;
                whichOne = TOP_RIGHT_TO_BOTTOM_LEFT;
                return true;
            }
            whichDimension = NONE;
            whichOne = NONE;
            return false;
        }

        // I wrote this method to show you how to call IsWinner
        private bool IsTie()
        {
            int winningDimension, winningValue;
            return (IsFull() && !IsWinner(out winningDimension, out winningValue));
        }

        // This method takes an integer in the range 0 - 4 that represents a column
        // as it's parameter and changes the font color of that cell to red.
        private void HighlightColumn(int col)
        {
            for (int row = 0; row < SIZE; row++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method changes the font color of the top right to bottom left diagonal to red
        // I did this diagonal because it's harder than the other one
        private void HighlightDiagonal2()
        {
            for (int row = 0, col = SIZE - 1; row < SIZE; row++, col--)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        // This method will highlight either diagonal, depending on the parameter that you pass
        private void HighlightDiagonal(int whichDiagonal)
        {
            if (whichDiagonal == TOP_LEFT_TO_BOTTOM_RIGHT)
                HighlightDiagonal1();
            else
                HighlightDiagonal2();

        }

        // highlights the row of the received argument
        private void HighlightRow(int row)
        {
            for (int col = 0; col < SIZE; col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }
        //highlights diagonal1
        private void HighlightDiagonal1()
        {
            for (int row = 0, col = 0; row < SIZE; row++, col++)
            {
                Label square = GetSquare(row, col);
                square.Enabled = true;
                square.ForeColor = Color.Red;
            }
        }

        //highlights winning streak based on which dimension and which value
        //names who won
        private void HighlightWinner(string player, int winningDimension, int winningValue)
        {
            switch (winningDimension)
            {
                case ROW:
                    HighlightRow(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case COLUMN:
                    HighlightColumn(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
                case DIAGONAL:
                    HighlightDiagonal(winningValue);
                    resultLabel.Text = (player + " wins!");
                    break;
            }
        }


        // increments through square matrix and sets text to black and assigns text value to empty string
        private void ResetSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.ForeColor = Color.Black;
                    square.Text = EMPTY;
                }
            }
        }

        //dictates what the computer needs to do on their turn
        //uses generator to determine random unused square to pick for comp's turn
        //sets text to comp symbol and disables square
        private void MakeComputerMove()
        {
            Label square;

            Random randomGenerator = new Random();

            do
            {
                int row = randomGenerator.Next(0, SIZE);
                int col = randomGenerator.Next(0, SIZE);
                square = GetSquare(row, col);
            } while (square.Text != EMPTY);

            square.Text = COMPUTER_SYMBOL;
            DisableSquare(square);
        }

        // Setting the enabled property changes the look and feel of the cell.
        // Instead, this code removes the event handler from each square.
        // Use it when someone wins or the board is full to prevent clicking a square.
        private void DisableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    DisableSquare(square);
                }
            }
        }

        // Inside the click event handler you have a reference to the label that was clicked
        // Use this method (and pass that label as a parameter) to disable just that one square
        private void DisableSquare(Label square)
        {
            square.Click -= new System.EventHandler(this.label_Click);
        }

        // You'll need this method to allow the user to start a new game
        private void EnableAllSquares()
        {
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    Label square = GetSquare(row, col);
                    square.Click += new System.EventHandler(this.label_Click);
                }
            }
        }

        /*
         *Calculate square based on clicked label, sets text for that label to user symbol, disables square and incrememnts counter
         *Determines if there is a winner
         * If there is not and the board is not full, comp turn
         * If there is, disables all squares and highlights winner
         * If there is no winner and the board is full, it's a tie
             */
        private void label_Click(object sender, EventArgs e)
        {
            int winningDimension = NONE;
            int winningValue = NONE;

            Label clickedLabel = (Label)sender;
            Label l = clickedLabel;
            // gets row/column of clicked label
            GetRowAndColumn(l, out int row, out int column);
            Label square = GetSquare(row, column);

            clickedLabel.Text = USER_SYMBOL;
            DisableSquare(square);
            counter++;


            // checks for a win
            if (IsWinner(out winningDimension, out winningValue))
            {
                DisableAllSquares();
                HighlightWinner("The User", winningDimension, winningValue);
            }
            //if no winner and board is not full, comp turn
            else if (!IsFull())
            {
                MakeComputerMove();
                if (IsWinner(out winningDimension, out winningValue))
                {
                    DisableAllSquares();
                    HighlightWinner("The Computer", winningDimension, winningValue);
                }
            }
            //if no winner and board is full, tie
            else IsTie();
        }

        private void newGameButton_Click(object sender, EventArgs e)
        {
            //resets squares, enables them, empties result label and resets counter
            ResetSquares();
            EnableAllSquares();
            resultLabel.Text = EMPTY;
            counter = 0;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
