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
        private Game game; 


        public Board(Button[,] button)
        {
            this.buttons = button;
        }

        /*
         * calculates and returns a heuristic value for the current board
         */
        public double HeuristicValue()
        {
            int whitePieces = 0;
            int grayPieces = 0;
            for (int col = 0; col < SIZE; col++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    String squareStatus = buttons[col, row].Tag.ToString();
                    if (squareStatus == "white")
                    {
                        whitePieces++;
                    }
                    else if (squareStatus == "gray")
                    {
                        grayPieces++;
                    }
                }
            }
            return whitePieces - grayPieces;
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
        public List<Board> movesforthispiece(Location location, string othercolor)
        {
            List<Board> thesemoves = new List<Board>();
            //check if king, when uncomment, make the next if an else if
            //string piece = board[square.col, square.row];
            //if (piece == "king")
            //{
            //    thesemoves.add(checkabove);
            //    thesemoves.add(checkbelow);
            //}
            if (playercolor == topcolor) //moving down
            {
                thesemoves.add(checkbelow(square, othercolor));
            }
            else //moving up
            {
                thesemoves.add(checkabove(square, othercolor));
            }
            return thesemoves;
        }

        /*
         * check if moves are possible above and returns the boards of possible moves 
         */
        public List<Board> checkabove(Location location, playercolor, othercolor)
        {
            list<board> moves = new list<board>();
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
                if (row = size - 1)
                {
                    lastrow = true;
                }
                string left = firstrow ? null : getpiece(board[col - 1, row - 1]);
                string right = lastrow ? null : getpiece(board[col - 1, row + 1]);
                if (!firstrow) //check left side
                {
                    if (!left)
                    {
                        location moveto = new location(col - 1, row - 1);
                        moves.add(makemove(location, moveto, playercolor));
                    }
                    else
                    {
                        if (col - 2 > 0 && row - 2 > 0)
                        {
                            location middle = new location(col - 1, row - 1);
                            location end = new location(col - 2, row - 2);
                            board jumpedboard = makejump(location, middle, end, playercolor);
                            if (jumpedboard)
                            {
                                moves.add(jumpedboard);
                            }
                        }
                    }
                }
                if (!lastrow) //check right side
                {
                    if (!right)
                    {
                        location moveto = new location(col - 1, row + 1);
                        moves.add(makemove(location, moveto, playercolor))
                    }
                    else
                    {
                        if (col - 2 > 0 && row + 2 < size)
                        {
                            location middle = new location(col - 1, row + 1);
                            location end = new location(col - 2, row + 2);
                            board jumpedboard = makejump(location, middle, end, playercolor);
                            if (jumpedboard)
                            {
                                moves.add(jumpedboard)
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
                if (row = SIZE - 1)
                {
                    lastrow = true;
                }
                string left = firstrow ? null : GetPiece(board[col + 1, row - 1]);
                string right = lastrow ? null : GetPiece(board[col + 1, row + 1]);
                if (!firstrow) //check left side
                {
                    if (!left)
                    {
                        location moveto = new location(col + 1, row - 1);
                        moves.add(makemove(location, moveto, playercolor));
                    }
                    else
                    {
                        if (col + 2 > 0 && row - 2 > 0)
                        {
                            location middle = new location(col + 1, row - 1);
                            location end = new location(col + 2, row - 2);
                            board jumpedboard = makejump(location, middle, end, playercolor);
                            if (jumpedboard)
                            {
                                moves.add(jumpedboard);
                            }
                        }
                    }
                }
                if (!lastrow) //check right side
                {
                    if (!right)
                    {
                        location moveto = new location(col + 1, row + 1);
                        moves.add(makemove(location, moveto, playercolor))
                    }
                    else
                    {
                        if (col - 2 > 0 && row + 2 < size)
                        {
                            location middle = new location(col + 1, row + 1);
                            location end = new location(col + 2, row + 2);
                            board jumpedboard = makejump(location, middle, end, playercolor);
                            if (jumpedboard)
                            {
                                moves.add(jumpedboard);
                            }
                        }
                    }
                }
            }
            return moves;
        }

        /* 
        * returns a copy of the board with the new move
        */
        public board makemove(location starting, location ending, string color)
        {
            board movedboard = this.copy();
            movedboard[col, row].tag = "none";
            movedboard[destcol, destrow].tag = color;
            return movedboard;
        }

        /*
       * checks if a jump is possible and returns a copy of the board with the new move if it is
       */
        public Board MakeJump(Location starting, Location middle, Location end, string color)
        {
            Board jumpedboard = null;
            string middlecolor = GetPiece(Board[middle.col, middle.row]));
            string endpiece = getpiece(board[end.col, end.row]);
            if ((middle != color) && (!endpiece)) //checks that a jump is possible
            {
                jumpedboard = this.copy;
                jumpedboard[starting.col, starting.row].tag = "none";
                jumpedboard[middle.col, middle.row].tag = "none";
                jumpedboard[end.col, end.row].tag = color;
            }
            return jumpedboard;
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
        public String GetPiece(Button square)
        {
            String piece = square.Tag.ToString();
            if (piece == "none")
            {
                piece = null;
            }
            return piece;
        }

        public bool IsLegal (Location origin, Location destination, Player player)
        {
            if (buttons[origin.row, origin.col].Tag.Equals("none") ||! buttons[destination.row, destination.col].Tag.Equals("none"))
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
        }

        public bool AnotherCapture()
        {
            // This will check if there as opponents piece to the right or left. If its legal (with current origina and new destnation, 
            // it will see if its an option to move and will move. If it is legal, another jump is ture. If not, its false. If true, second jump will be taken
        }
      
    }
}