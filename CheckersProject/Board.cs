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
        private int whitePieces = 0;
        private int grayPieces = 0;
        private Game game;
        public readonly Piece topColor;
        public readonly Piece bottomColor;


        /*
         * An overloaded constructor for Copy
         */
        private Board(Piece topColor, Piece bottomColor)

        {
            squares = new Piece[SIZE, SIZE];
            this.topColor = topColor;
            this.bottomColor = bottomColor;
        }


        

        /*
         * A constructor that takes a 2d array of buttons
         */
        public Board(Button[,] buttons, Game game)
        {
            this.game = game;
            topColor = game.GetComputerColor();
            bottomColor = game.GetHumanColor();
            squares = new Piece[SIZE, SIZE];
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    String square = buttons[row, col].Tag.ToString();
                    squares[row, col] = square.Equals("WHITE") ? Piece.WHITE :
                        square.Equals("WHITEKING") ? Piece.WHITE_KING :
                        square.Equals("GRAY") ? Piece.GRAY :
                        square.Equals("GRAYKING") ? Piece.GRAY_KING : Piece.EMPTY;

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
            this.game = game; 
        }

        /*
         * Function is called to decrement the total number of pieces for a color when a piece is captured
         */
        private void DecrementPieces(Piece capturingPiece)
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
            int value = 0;

            Piece maxColor = topColor;

            value = IsWhite(maxColor) ? whitePieces - grayPieces : grayPieces - whitePieces;
            
            return value;
        }

        public bool CaptureOnBoard(Player player)
        {
            for (int col = 0; col < SIZE; col++)
            {
                for (int row = 0; row < SIZE; row++)
                {
                    if (PieceHasAvailableCapture(new Location(row, col), player))
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public List<Board> GetPossibleCaptures(Piece playersColor)
        {
            throw new NotImplementedException();
        }

        /*
         * returns a list of boards for every possible next move
         */
        public List<Board> GetAllMoves (Player player) 
        {
            List<Board> allmoves = new List<Board>();
            Piece playercolor = (player == Player.MAX) ? topColor : bottomColor; 
            Piece otherPlayer = (player == Player.MAX) ? bottomColor : topColor;
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
                List<Board> movesAbove = CheckAboveOrBelow(square, piece, true); //checking possible moves going up
                AddListToList(movesAbove, theseMoves);
                List<Board> movesBelow = CheckAboveOrBelow(square, piece, false); //checking possible moves going down
                AddListToList(movesBelow, theseMoves);
            }
            else if (SameColor(piece, topColor)) //moving down
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
        }

        /*
         * check if moves are possible above or below and returns the boards of possible moves 
         * TODO: combine right and left
         */
        public List<Board> CheckAboveOrBelow(Location location, Piece playerPiece, bool above)
        {
            List<Board> moves = new List<Board>();
            int col = location.col;
            int row = location.row;
            bool firstRow = (row == 0);
            bool lastRow = (row == SIZE - 1);
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
                Piece left = firstCol ? Piece.NULL : squares[checkingRow, col - 1];
                Piece right = lastCol ? Piece.NULL : squares[checkingRow, col + 1];
                if (!firstCol) //check left side
                {
                    if (left != Piece.NULL)
                    {
                        Location moveto = new Location(col - 1, checkingRow);
                        moves.Add(MakeMoveCopy(location, moveto, playerPiece));
                    }
                    //else
                    //{
                    //    int jumpingRow = above ? row - 2 : row + 2;
                    //    if ((col - 2 >= 0) && (jumpingRow >= 0) && (jumpingRow < SIZE)) //check for jump
                    //    {
                    //        Location middle = new Location(col - 1, checkingRow);
                    //        Location end = new Location(col - 2, jumpingRow);
                    //        Board jumpedBoard = MakeJumpCopy(location, middle, end, playerPiece);
                    //        if (jumpedBoard != null)  
                    //        {
                    //            moves.Add(jumpedBoard);
                    //        }
                    //    }
                    //}
                }

                if (!lastCol) //check right side
                {
                    if (right != Piece.NULL)
                    {
                        Location moveto = new Location(col + 1, checkingRow);
                        moves.Add(MakeMoveCopy(location, moveto, playerPiece));
                    }
                    //else
                    //{
                    //    int jumpingRow = above ? row - 2 : row + 2;
                    //    if ((col + 2 < SIZE) && (jumpingRow < SIZE) && (jumpingRow >= 0))
                    //    {
                    //        Location middle = new Location(col + 1, checkingRow);
                    //        Location end = new Location(col + 2, jumpingRow);
                    //        Board jumpedBoard = MakeJumpCopy(location, middle, end, playerPiece);
                    //        if (jumpedBoard != null)
                    //        {
                    //            moves.Add(jumpedBoard); 
                    //        }
                    //    }
                    //}
                }
            }

            return moves;
        }

        /* 
        * returns a copy of the board with the new move
        */
        public Board MakeMoveCopy(Location starting, Location ending, Piece color)
        {
            Board movedBoard = this.Copy();
            movedBoard.squares[starting.row, starting.col] = Piece.EMPTY;
            if (ending.row == 0 || ending.row == SIZE - 1)
            {
                movedBoard.squares[ending.row, ending.col] = GetKing(color);
            }
            else
            {
                movedBoard.squares[ending.row, ending.col] = color;
            }

            return movedBoard;
        }

        /*
         * This method makes a move on the board
         */
        public void MakeMove(Location starting, Location ending, Piece color)
        {

            squares[starting.row, starting.col] = Piece.EMPTY;
            if (ending.row == 0 || ending.row == SIZE - 1)
            {
                squares[ending.row, ending.col] = GetKing(color);
            }
            else
            {
                squares[ending.row, ending.col] = color;
            }

        }

        /*
        * checks if a jump is possible and returns a copy of the board with the new move if it is
        */
        public Board MakeJumpCopy(Location starting, Location middle, Location end, Piece startingPiece)
        {
            Board jumpedBoard = null;
            Piece middlePiece = squares[middle.row, middle.col];
            Piece endPiece = squares[end.row, end.col];
            if (!SameColor(middlePiece, startingPiece) && (endPiece == Piece.EMPTY)) //checks that a jump is possible 
            {
                jumpedBoard = this.Copy();
                jumpedBoard.squares[starting.row, starting.col] = Piece.EMPTY;
                jumpedBoard.squares[middle.row, middle.col] = Piece.EMPTY;

                if (end.row == 0 || end.row == SIZE - 1)
                {
                    jumpedBoard.squares[end.row, end.col] = GetKing(startingPiece);
                }
                else
                {
                    jumpedBoard.squares[end.row, end.col] = startingPiece;
                }

                jumpedBoard.DecrementPieces(startingPiece);
            }

            return jumpedBoard;
        }

        /*
         * This method makes a jump on the board if the capture is legal
         */
        public void MakeJump(Location starting, Location middle, Location end, Piece startingPiece)
        {
            Piece middlePiece = squares[middle.row, middle.col];
            Piece endPiece = squares[end.row, end.col];
            if (!SameColor(middlePiece, startingPiece) && (endPiece == Piece.EMPTY)) //checks that a jump is possible 
            {
                squares[starting.row, starting.col] = Piece.EMPTY;
                squares[middle.row, middle.col] = Piece.EMPTY;

                if (end.row == 0 || end.row == SIZE - 1)
                {
                    squares[end.row, end.col] = GetKing(startingPiece);
                }
                else
                {
                    squares[end.row, end.col] = startingPiece;
                }

                DecrementPieces(startingPiece);
            }
        }

        /*
        * Makes a deep copy of the board
        */
        public Board Copy()
        {
            Board copiedBoard = new Board(topColor, bottomColor);
            for(int row = 0; row < SIZE; row++)
            {
                for(int col = 0; col < SIZE; col++)
                {
                    copiedBoard.squares[row, col] = this.squares[row, col];
                }
            }
            copiedBoard.whitePieces = this.whitePieces;
            copiedBoard.grayPieces = this.grayPieces;
            copiedBoard.game = this.game;

            return copiedBoard;
        }


        public bool isKing(Piece piece)
        {
            return piece.Equals(Piece.GRAY_KING) || piece.Equals(Piece.WHITE_KING);
        }
        public bool IsLegal(Location origin, Location destination, Player player)
        {
            Piece currPiece = squares[origin.row, origin.col];
            
            if (squares[origin.row, origin.col] == Piece.EMPTY ||
                squares[destination.row, destination.col] != Piece.EMPTY)
            {
                return false;
            }

            if (player.Equals(Player.MIN) || (player.Equals(Player.MAX) && isKing(currPiece))) // player is human
            {
                if (destination.row == origin.row - 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1)
                    && !PieceHasAvailableCapture(origin, player)) // regular move
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col + 2 &&
                         (player.Equals(Player.MIN) && SameColor(squares[origin.row - 1, origin.col + 1], game.GetComputerColor()) ||
                          (player.Equals(Player.MAX) && SameColor(squares[origin.row - 1, origin.col + 1], game.GetHumanColor()))))
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col - 2 && 
                         ((player.Equals(Player.MIN) && SameColor(squares[origin.row - 1, origin.col - 1], game.GetComputerColor()) ||
                          (player.Equals(Player.MAX) && SameColor(squares[origin.row - 1, origin.col - 1], game.GetHumanColor())))))
                if (destination.row == origin.row - 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1)) // regular move
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col + 2 && SameColor(squares[origin.row + 1, origin.col + 1], game.GetComputerColor())) // single capture - right (Hadassah - figure out tags)
                {
                    return true;
                }
                else if (destination.row == origin.row - 2 && destination.col == origin.col - 2 && SameColor(squares[origin.row + 1, origin.col - 1], (game.GetComputerColor()))) // single capture - left
                {
                    return true;
                }
            }
            if (player.Equals(Player.MAX) || (player.Equals(Player.MIN) && isKing(currPiece))) // player is computer
            {
                if (destination.row == origin.row + 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1)
                    && !PieceHasAvailableCapture(origin, player))
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col + 2 &&
                         (player.Equals(Player.MIN) && SameColor(squares[origin.row + 1, origin.col + 1], game.GetComputerColor()) ||
                          (player.Equals(Player.MAX) && SameColor(squares[origin.row + 1, origin.col + 1], game.GetHumanColor()))))
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col - 2 &&
                         (player.Equals(Player.MIN) && SameColor(squares[origin.row + 1, origin.col + 1], game.GetComputerColor()) ||
                          (player.Equals(Player.MAX) && SameColor(squares[origin.row + 1, origin.col + 1], game.GetHumanColor()))))
                if (destination.row == origin.row + 1 && (destination.col == origin.col - 1 || destination.col == origin.col + 1))
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col + 2 && SameColor(squares[origin.row - 1, origin.col + 1], game.GetHumanColor()))
                {
                    return true;
                }
                else if (destination.row == origin.row + 2 && destination.col == origin.col - 2 && SameColor(squares[origin.row - 1, origin.col - 1], game.GetHumanColor())) // single capture - left
                {
                    return true;
                }
            }
            return false;
        }

        public bool PieceHasAvailableCapture(Location origin, Player player)
        {
            if (player.Equals(Player.MIN) && origin.row - 2 > 0)
            {
                if (IsLegal(origin, new Location(origin.col + 2, origin.row - 2), player) 
                    && origin.col + 2 < 8)
                {
                    return true; // jump to right
                }
                if (IsLegal(origin, new Location(origin.col - 2, origin.row - 2), player)
                    && origin.col - 2 > -1)
                {
                    return true; // jump to left
                }
            }

            if (player.Equals(Player.MAX) && origin.row + 2 < 8)
            {
                if (IsLegal(origin, new Location(origin.col + 2, origin.row + 2), player)
                    && origin.col + 2 < 8)
                {
                    return true; // jump to right
                }
                if (IsLegal(origin, new Location(origin.col - 2, origin.row + 2), player) 
                    && origin.col - 2 > -1)
                {
                    return true; // jump to left
                }
            }
            return false; 
        }

        /*
        * If either player has no more peices on the board, this returns true
        * TODO: call this in alpha beta
        */
        public bool GameOver()
        {
            return this.whitePieces == 0 || this.grayPieces == 0;
        }

        /*

         * A method that returns the winner of the game
         */
        public Player GetWinner()
        {
            Piece computerColor = game.GetComputerColor();
            Piece winnerColor = whitePieces == 0 ? Piece.GRAY : Piece.WHITE;
            Player winner = winnerColor == computerColor ? Player.MAX : Player.MIN;
            return winner;
        }
        /*
         * A method to add one list to another
         */
        private void AddListToList(List<Board> listFrom, List<Board> listTo)
        {
            foreach(Board item in listFrom)
            {
                listTo.Add(item);
            }
        }

        /*
         * Takes 2 Piece enums and returns true if they are the same color 
         */
        private bool SameColor(Piece piece1, Piece piece2)
        {
            bool sameColor = (piece1 == Piece.GRAY || piece1 == Piece.GRAY_KING) && //piece1 is gray
                (piece2 == Piece.GRAY || piece2 == Piece.GRAY_KING) ? true : //piece2 is gray
                (piece1 == Piece.WHITE || piece1 == Piece.WHITE_KING) && //piece1 is white
                (piece2 == Piece.WHITE || piece2 == Piece.WHITE_KING) ? true : false; //piece2 is white

           return sameColor;
        }

        /*
         * Returns true if the enum is a white piece
         */
        private bool IsWhite(Piece piece)
        {
            return piece == Piece.WHITE || piece == Piece.WHITE_KING;
        }

        /*
         * Returns true if the enum is a gray piece
         */
        private bool IsGray(Piece piece)
        {
            return !IsWhite(piece) && !(piece == Piece.NULL || piece == Piece.EMPTY);
        }

        /*
         * Returns the king piece of that color
         */
        private Piece GetKing(Piece piece)
        {
            Piece king= Piece.NULL;
            if (IsWhite(piece))
            {
                king = Piece.WHITE_KING;
            }
            else if (IsGray(piece))
            {
                king = Piece.GRAY_KING;
            }
            return king;
        }

    }
}