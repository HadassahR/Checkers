using CheckersProject;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CheckersGame
{
    // TO DO: PossibleCapture()
    public class Board // Leah // 
    {
        public static readonly int SIZE = 8;
        Piece[,] squares = new Piece[SIZE, SIZE];
        public static readonly Piece topColor; //TODO: this
        private int whitePieces = 0;
        private int grayPieces = 0;
        private Game game;

        private Board()
        {
            squares = new Piece[SIZE, SIZE];
        }

        public Board(Button[,] button)
        {
            squares = new Piece[SIZE, SIZE];
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    String square = button[row, col].Tag.ToString();
                    squares[row, col] = square.Equals("white") ? Piece.WHITE :
                        square.Equals("whiteKing") ? Piece.WHITE_KING :
                        square.Equals("gray") ? Piece.GRAY :
                        square.Equals("grayKing") ? Piece.GRAY_KING : Piece.EMPTY;

                    if (squares[row, col] == Piece.GRAY || squares[row, col] == Piece.GRAY_KING)
                    {
                        grayPieces++;
                    }
                    else if (squares[row, col] == Piece.WHITE || squares[row, col] == Piece.WHITE_KING)
                    {
                        whitePieces++;
                    }
                }
            }
        }

        /*
         * Function is called to decrement the total number of pieces for a color when a piece is captured
         * TODO: change to MAX and MIN
         */
        private void decrementPieces(Piece capturingPiece)
        {
            if (IsWhite(capturingPiece))
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
         * TODO: change playerColor and otherColor
         */
        public List<Board> GetAllMoves (Player player) // ****
        {
            List<Board> allmoves = new List<Board>();
            Piece playercolor = (player == Player.MIN) ? Piece.GRAY : Piece.WHITE; 
            Piece otherPlayer = (player == Player.MIN) ? Piece.WHITE : Piece.GRAY;
            for (int col = 0; col < SIZE; col++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    Piece piece = squares[col, row];
                    if (!(piece == Piece.EMPTY) || SameColor(piece, otherPlayer)) //check that there is a piece of the other color there
                    {
                        continue;
                    }
                    else
                    {
                        Location square = new Location(col, row);
                        List<Board> theseMoves = MovesForThisPiece(square, otherPlayer);
                        AddListToList(theseMoves, allmoves);
                    }
                }
            }
            return allmoves;
        }

        /*
        * generate a list of all possible moves for this piece
        */
        public List<Board> MovesForThisPiece(Location square, Piece otherColor)
        {
            List<Board> theseMoves = new List<Board>();
            Piece piece = squares[square.row, square.col];
            if (piece == Piece.WHITE_KING || piece == Piece.GRAY_KING)
            {
                List<Board> movesAbove = CheckAboveOrBelow(square, piece, true); 
                AddListToList(movesAbove, theseMoves);
                List<Board> movesBelow = CheckAboveOrBelow(square, piece, false);
                AddListToList(movesBelow, theseMoves);
            }
            else if (SameColor(piece, topColor)) //moving down ** Where is topColor declared?
            {
                List<Board> movesBelow = CheckAboveOrBelow(square, piece, false);
                AddListToList(movesBelow, theseMoves);
            }
            else //moving up
            {
                List<Board> movesAbove = CheckAboveOrBelow(square, piece, true);
                AddListToList(movesAbove, theseMoves);
            }
            return theseMoves;
            //}
        }

        /*
         * check if moves are possible above and returns the boards of possible moves 
         * TODO: combine right and left
         */
        public List<Board> CheckAboveOrBelow(Location location, Piece playerPiece, bool above)
        {
            List<Board> moves = new List<Board>();
            int col = location.col;
            int row = location.row;
            bool firstRow = (row == 0);
            bool lastRow = (row == SIZE-1);
            if (!(above && firstRow) && !(!above && lastRow)) //not first or lastrow
            {
                bool firstCol = false;
                bool lastCol = false;
                if (row == 0)
                {
                    firstCol = true;
                }
                if (row == SIZE - 1)
                {
                    lastCol = true;
                }
                int checkingRow = above ? row - 1 : row + 1;
                Piece left = firstCol ? Piece.NULL : squares[checkingRow, col -1];
                Piece right = lastCol ? Piece.NULL : squares[checkingRow, col + 1];
                if (!firstCol) //check left side
                {
                    if (left != Piece.NULL) 
                    {
                        Location moveto = new Location(col - 1, checkingRow);
                        moves.Add(MakeMove(location, moveto, playerPiece));
                    }
                    else
                    {
                        int jumpingRow = above ? row - 2 : row + 2;
                        if ((col - 2 >= 0) && (jumpingRow >= 0) && (jumpingRow < SIZE)) //check for jump
                        {
                            Location middle = new Location(col - 1, checkingRow);
                            Location end = new Location(col - 2, jumpingRow);
                            Board jumpedBoard = MakeJump(location, middle, end, playerPiece);
                            if (jumpedBoard != null)  
                            {
                                moves.Add(jumpedBoard);
                            }
                        }
                    }
                }
                if (!lastCol) //check right side
                {
                    if (right != Piece.NULL) 
                    {
                        Location moveto = new Location(col + 1, checkingRow);
                        moves.Add(MakeMove(location, moveto, playerPiece)); 
                     }
                    else
                    {
                        int jumpingRow = above ? row - 2 : row + 2;
                        if ((col + 2 < SIZE) && (jumpingRow < SIZE) && (jumpingRow >= 0))
                        {
                            Location middle = new Location(col + 1, checkingRow);
                            Location end = new Location(col + 2, jumpingRow);
                            Board jumpedBoard = MakeJump(location, middle, end, playerPiece);
                            if (jumpedBoard != null)
                            {
                                moves.Add(jumpedBoard); 
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
        public Board MakeMove(Location starting, Location ending, Piece color)
        {
            Board movedBoard = this.Copy();
            movedBoard.squares[starting.row, starting.col] = Piece.EMPTY;
            movedBoard.squares[ending.row, ending.col] = color; 
            return movedBoard;
        }

        /*
       * checks if a jump is possible and returns a copy of the board with the new move if it is
       */
        public Board MakeJump(Location starting, Location middle, Location end, Piece startingPiece)
        {
            Board jumpedBoard = null;
            Piece middlePiece = squares[middle.row, middle.col];
            Piece endPiece = squares[end.row, end.col];
            if (!SameColor(middlePiece, startingPiece) && (endPiece == Piece.EMPTY)) //checks that a jump is possible 
            {
                jumpedBoard = this.Copy();
                jumpedBoard.squares[starting.row, starting.col] = Piece.EMPTY;
                jumpedBoard.squares[middle.row, middle.col] = Piece.EMPTY;
                jumpedBoard.squares[end.row, end.col] = startingPiece;
                jumpedBoard.decrementPieces(startingPiece);
            }
            return jumpedBoard;
        }

        /*
        * Makes a deep copy of the board
        */
        public Board Copy()
        {
            Board copiedBoard = new Board();
            for(int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col < SIZE; col++)
                {
                    copiedBoard.squares[row, col] = this.squares[row, col];
                }
            }
            copiedBoard.whitePieces = this.whitePieces;
            copiedBoard.grayPieces = this.grayPieces;
            return copiedBoard;
        }

        public bool IsLegal(Location origin, Location destination, Player player)
        {
            if (squares[origin.row, origin.col] == Piece.EMPTY || !(squares[destination.row, destination.col] == Piece.EMPTY))
            {
                return false;
            }
            if (player.Equals(Player.MIN)) // player is human
            {
                if (destination.row == origin.row + 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1)) // regular move
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col + 2 && squares[origin.row + 1, origin.col + 1].Text.Equals((game.GetComputerColor()))) // single capture - right (Hadassah - figure out tags)
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col - 2 && squares[origin.row + 1, origin.col - 1].Text.Equals((game.GetComputerColor()))) // single capture - left
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
                else if (destination.row == origin.row - 2 && destination.col == origin.col + 2 && squares[origin.row - 1, origin.col + 1].Text.Equals((game.GetHumanColor()))) // single capture - right (Hadassah - figure out tags)
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col - 2 && squares[origin.row - 1, origin.col - 1].Text.Equals((game.GetHumanColor()))) // single capture - left
                {
                    return true;
                }
            }
            return false;
        }

        public bool AnotherCapture()
        {
            // This will check if there as opponents piece to the right or left. If its legal (with current origina and new destination, 
            // it will see if its an option to move and will move. If it is legal, another jump is ture. If not, its false. If true, second jump will be taken
            return false; 
        }

        /*
        * If either player has no more peices on the board, this returns true
        * TODO: call this in alpha beta
        */
        public bool gameOver()
        {
            return this.whitePieces == 0 || this.grayPieces == 0;
        }

        public Player GetWinner()
        {
            throw new NotImplementedException(); 
        }

        private void AddListToList(List<Board> listFrom, List<Board> listTo)
        {
            foreach(Board item in listFrom)
            {
                listTo.Add(item);
            }
        }

        private bool SameColor(Piece piece1, Piece piece2)
        {
            bool sameColor = (piece1 == Piece.GRAY || piece1 == Piece.GRAY_KING) && //piece1 is gray
                (piece2 == Piece.GRAY || piece2 == Piece.GRAY_KING) ? true : //piece2 is gray
                (piece1 == Piece.WHITE || piece1 == Piece.WHITE_KING) && //piece1 is white
                (piece2 == Piece.WHITE || piece2 == Piece.WHITE_KING) ? true : false; //piece2 is white

           return sameColor;
        }

        private bool IsWhite(Piece piece)
        {
            return piece == Piece.WHITE || piece == Piece.WHITE_KING;
        }

        private bool IsGray(Piece piece)
        {
            return !IsWhite(piece) && !(piece == Piece.NULL || piece == Piece.EMPTY);
        }

    }
}