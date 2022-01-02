using CheckersProject;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CheckersGame
{
    // TO DO: PossibleCapture()
    public class Board // Leah // 
    {
        Button[,] buttons;
        
        public static readonly int SIZE = 8;
        public static readonly String topColor;
        private int whitePieces;
        private int grayPieces;
        private Game game;
        public Board(Button[,] button)
        {
            this.buttons = button;
            this.whitePieces = countPieces("white");
            this.grayPieces = countPieces("gray");
        }

        /*
         * This is called from the constructor and sets the pieces values for the class
         */
        private int countPieces(String color)
        {
            int pieces = 0;
            for (int col = 0; col < SIZE; col++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    String squareStatus = buttons[col, row].Tag.ToString();
                    if (squareStatus == color)
                    {
                        pieces++;
                    }
                }
            }
            return pieces;
        }

        /*
         * Function is called to decrement the total number of pieces for a color when a piece is captured
         */
        private void decrementPieces(String capturingColor)
        {
            if (capturingColor.Equals("white"))
            {
                this.grayPieces--;
            }
            else
            {
                this.whitePieces--;
            }
        }

        /*
        * calculates and returns a heuristic value for the current board
        */
        public double HeuristicValue()
        {
            return this.whitePieces - this.grayPieces;
        }

        public bool PossibleCapture ()
        {
            throw new NotImplementedException(); 
            return true; 
        }

        /*
         * returns a list of boards for every possible next move
         */
        public List<Board> GetAllMoves (Player player) // ****
        {
            List<Board> allmoves = new List<Board>();
            string playercolor = (player == Player.MIN) ? "gray" : "white";
            string othercolor = (playercolor == "gray") ? "white" : "gray";
            for (int col = 0; col < SIZE; col++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    string piece = GetPiece(buttons[col, row]);
                    /** if (!piece || piece == othercolor) //check that there is a piece there that is this color
                    {
                        continue;
                    }
                    else
                    {
                        location square = new location(col, row);
                        list<board> thesemoves = movesforthispiece(square, othercolor);
                        allmoves.add(thesemoves); //might need to make sure not null

                    } need leah to explain **/
                }
            }
            return allmoves;
        }

        /*
        * generate a list of all possible moves for this piece
        */
        public List<Board> MovesForThisPiece(Location square, string otherColor)
        {
            List<Board> theseMoves = new List<Board>();
            //check if king, when uncomment, make the next if an else if
            //string piece = board[square.col, square.row];
            //if (piece == "king")
            //{
            //    thesemoves.add(checkabove);
            //    thesemoves.add(checkbelow);
            //}
            if (playerColor == topColor) //moving down ** Where is playerColor declared?
            {
                theseMoves.Add(CheckBelow(square, otherColor));
            }
            else //moving up
            {
                theseMoves.Add(CheckAbove(square, otherColor));
            }
            return theseMoves;
            //}
        }

        /*
         * check if moves are possible above and returns the boards of possible moves 
         */
        public List<Board> checkabove(Location location, string playerColor, string otherColor)
        {
            List<Board> moves = new List<Board>();
            int col = location.col;
            int row = location.row;
            if (!(col == 0)) //not first row
            {
                bool firstrow = false;
                bool lastrow = false;
                if (row == 0)
                {
                    firstrow = true;
                }
                if (row == SIZE - 1)
                {
                    lastrow = true;
                }
                string left = firstrow ? null : GetPiece(buttons[col - 1, row - 1]);
                string right = lastrow ? null : GetPiece(buttons[col - 1, row + 1]);
                if (!firstrow) //check left side
                {
                    if (!left) // fix this - its not a condition
                    {
                        Location moveto = new Location(col - 1, row - 1);
                        moves.Add(MakeMove(location, moveto, playerColor));
                    }
                    else
                    {
                        if (col - 2 > 0 && row - 2 > 0)
                        {
                            Location middle = new Location(col - 1, row - 1);
                            Location end = new Location(col - 2, row - 2);
                            Board jumpedBoard = MakeJump(location, middle, end, playerColor);
                            if (jumpedBoard) // how is this a condition? 
                            {
                                moves.Add(jumpedBoard);
                            }
                        }
                    }
                }
                if (!lastrow) //check right side
                {
                    if (!right) // how is this a condition?
                    {
                        Location moveto = new Location(col - 1, row + 1);
                        moves.Add(MakeMove(location, moveto, playerColor)); 
                     }
                    else
                    {
                        if (col - 2 > 0 && row + 2 < SIZE)
                        {
                            Location middle = new Location(col - 1, row + 1);
                            Location end = new Location(col - 2, row + 2);
                            Board jumpedboard = MakeJump(location, middle, end, playerColor);
                            if (jumpedBoard)
                            {
                                moves.Add(jumpedboard); 
                            }
                        }
                    }
                }
            }
            return moves;
        }

        /*
       * check if moves are possible below and returns the boards of possible moves 
       */
        public List<Board> CheckBelow(Location location, string otherColor)
        {
            List<Board> moves = new List<Board>();
            int col = location.col;
            int row = location.row;
            if (!(col == SIZE)) //not last row
            {
                bool firstrow = false;
                bool lastrow = false;
                if (row == 0)
                {
                    firstrow = true;
                }
                if (row == SIZE - 1)
                {
                    lastrow = true;
                }
                string left = firstrow ? null : GetPiece(Board[col + 1, row - 1]);
                string right = lastrow ? null : GetPiece(Board[col + 1, row + 1]);
                if (!firstrow) //check left side
                {
                    if (!left) // see similar comments for CheckAbove
                    {
                        Location moveto = new Location(col + 1, row - 1);
                        moves.Add(MakeMove(location, moveto, playerColor));
                    }
                    else
                    {
                        if (col + 2 > 0 && row - 2 > 0)
                        {
                            Location middle = new Location(col + 1, row - 1);
                            Location end = new Location(col + 2, row - 2);
                            Board jumpedboard = MakeJump(location, middle, end, playerColor);
                            if (jumpedBoard)
                            {
                                moves.Add(jumpedboard);
                            }
                        }
                    }
                }
                if (!lastrow) //check right side
                {
                    if (!right)
                    {
                        Location moveTo = new Location(col + 1, row + 1);
                        moves.Add(MakeMove(location, moveTo, playerColor))
                     }
                    else
                    {
                        if (col - 2 > 0 && row + 2 < SIZE)
                        {
                            Location middle = new Location(col + 1, row + 1);
                            Location end = new Location(col + 2, row + 2);
                            Board jumpedboard = MakeJump(location, middle, end, playerColor);
                            if (jumpedBoard)
                            {
                                moves.Add(jumpedboard);
                            }
                        }
                    }
                }
            }
            return moves;
        }
        // can check above and check below call one method

        /* 
        * returns a copy of the board with the new move
        */
        public Board MakeMove(Location starting, Location ending, string color)
        {
            Board movedBoard = this.Copy();
            movedBoard[starting.col, starting.row].Tag = "none";
            movedBoard[ending.col, ending.row].Tag = color; // should be row, col not col, row
            return movedBoard;
        }

        // /*
        //* checks if a jump is possible and returns a copy of the board with the new move if it is
        //*/
        public Board MakeJump(Location starting, Location middle, Location end, string color)
        {
            Board jumpedBoard = null;
            string middleColor = GetPiece(buttons[middle.col, middle.row]));
            string endPiece = GetPiece(buttons[end.col, end.row]);
            if ((middle != color) && (!endPiece)) //checks that a jump is possible *** .equals() for middle and what condition are you checking on end piece? 
            {
                jumpedBoard = this.copy;
                jumpedBoard[starting.col, starting.row].tag = "none";
                jumpedBoard[middle.col, middle.row].tag = "none";
                jumpedBoard[end.col, end.row].tag = color;
                jumpedBoard.decrementPieces(color);
            }
            return jumpedBoard;
        }

        /*
        * TODO
        * Makes a deep copy of the board
        */
        public Board Copy()
        {
            throw new NotImplementedException(); 
        }

        /*
        * returns the color of the square and null if empty
        */
        public bool IsLegal(Location origin, Location destination, Player player)
        {
            if (buttons[origin.row, origin.col].Tag.Equals("none") || !buttons[destination.row, destination.col].Tag.Equals("none"))
            {
                return false;
            }
            if (player.Equals(Player.MIN)) // player is human
            {
                if (destination.row == origin.row + 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1)) // regular move
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col + 2 && buttons[origin.row + 1, origin.col + 1].Text.Equals((game.GetComputerColor()))) // single capture - right (Hadassah - figure out tags)
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col - 2 && buttons[origin.row + 1, origin.col - 1].Text.Equals((game.GetComputerColor()))) // single capture - left
                {
                    return true;
                }
            }
            if (player.Equals(Player.MAX)) // player is computer
            {
                if (destination.row == origin.row - 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1))
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col + 2 && buttons[origin.row - 1, origin.col + 1].Text.Equals((game.GetHumanColor()))) // single capture - right (Hadassah - figure out tags)
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col - 2 && buttons[origin.row - 1, origin.col - 1].Text.Equals((game.GetHumanColor()))) // single capture - left
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnotherCapture(Board button, Location currentPiece, Player player)
        {
            if (player.Equals(Player.MIN))
            {
                if (IsLegal(currentPiece, new Location(buttons[currentPiece.col + 2, currentPiece.row + 2]), player))
                {

                }
            }
            // This will check if there as opponents piece to the right or left. If its legal (with current origina and new destination, 
            // it will see if its an option to move and will move. If it is legal, another jump is ture. If not, its false. If true, second jump will be taken
            return false; 
        }

        /*
        * If either player has no more peices on the board, this returns true
        * TODO: store the number of white and gray pieces in the class, call this in alpha beta
        */
        

        public bool gameOver()
        {
            return this.whitePieces == 0 || this.grayPieces == 0;
        }

        public Player GetWinner()
        {
            throw new NotImplementedException(); 
        }

    }
}