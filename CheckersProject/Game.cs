using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckersProject; 

namespace CheckersProject
{
    class Game
    {
        private Player startingPlayer;
        private Player currentPlayer;
        private String computerColor;
        private String humanColor;
        private Button clickedPiece;
        private int computerScore; 
        private int humanScore; 

        public Game ()
        {
            this.computerScore = 0;
            this.humanScore = 0; 
        }
        public void SetStartingPlayer(Player player)
        {
            startingPlayer = player;
            currentPlayer = startingPlayer;
        }
        public Player GetStartingPlayer()
        {
            return startingPlayer;
        }
        public void NextPlayersTurn()
        {
            currentPlayer = currentPlayer == Player.MAX ? Player.MIN : Player.MAX;
        }
        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }
        public String GetHumanColor()
        {
            return this.humanColor;
        }
        public void SetHumanColor(String tag)
        {
            this.humanColor = tag;
        }
        public String GetComputerColor()
        {
            return this.computerColor;
        }
        public void SetComputerColor(String tag)
        {
            this.computerColor = tag;
        }
        public void SetPieceClicked(Button button)
        {
            // Need to check if this piece is legal to move
            clickedPiece = button; 
            // When cancelled this needs to be reset
        }
        public Button GetClickedPiece()
        {
            return this.clickedPiece; 
        }
        public void ResetClickedPiece ()
        {
            clickedPiece = null; 
        }
        public void IncreaseComputerScore (int points)
        {
            this.computerScore += points;
        }        
        public void IncreaseHumanScore (int points)
        {
            this.humanScore += points;
        }
        public String GetComputerScore()
        {
            return this.computerScore.ToString(); 
        }
        public String GetHumanScore()
        {
            return this.humanScore.ToString(); 
        }
    }
}
