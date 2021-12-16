using CheckersProject;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CheckersGame
{
    public class Board // Leah // 
    {
        Button[,] board;
        private Player startingPlayer;
        private Player currentPlayer;
        public static readonly int SIZE = 8;


        public Board(Button[,] board)
        {
            this.board = board;
        }

        /* Computer is MAX, User is MIN. This method should only be used once */
        public void setStartingPlayer(Player player)
        {
            startingPlayer = player;
            currentPlayer = startingPlayer;
        }

        public Player getStartingPlayer()
        {
            return startingPlayer;
        }

        public void nextPlayersTurn()
        {
            currentPlayer = currentPlayer == Player.MAX ? Player.MIN : Player.MAX;
        }

        public Player getCurrentPlayer()
        {
            return currentPlayer;
        }
        /*
         * calculates and returns a heuristic value for the current board
         */
        internal double heuristicValue()
        {
            throw new NotImplementedException();
        }


        /*
         * returns a boolean that is true if the specified player can move on that piece
         * note: must check that there is a piece in that square
         */
        internal bool canMove(Player player, int col, int row)
        {
            throw new NotImplementedException();
        }


        /*
         * returns a list of boards for every possible next move
         * will call canMove
         */
        internal Board[] allMoves(Player player)
        {
            throw new NotImplementedException();
        }


        /*
         * returns a list of possible moves for the piece specified in that column
         */
        internal List<Board> MakeMove(Player player, int col, int row)
        {
            throw new NotImplementedException();
        }
    }
}