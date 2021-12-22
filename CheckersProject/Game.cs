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
        private Button clickedOrigin;
        private Button clickedDestination; 
        private bool originClicked;
        private bool destinationClicked; 
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
        public string GetHumanColor()
        {
            return this.humanColor;
        }
        public void SetHumanColor(String tag)
        {
            this.humanColor = tag;
        }
        public string GetComputerColor()
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
            clickedOrigin = button; 
            // When cancelled this needs to be reset
        }
        public Button GetClickedOrigin()
        {
            return this.clickedOrigin; 
        }        
        public Button GetClickedDestination()
        {
            return this.clickedDestination; 
        }
        public void ResetClickedOrigin ()
        {
            clickedOrigin = null; 
        }
        public bool OriginClicked ()
        {
            return this.originClicked; 
        }
        public void SetOriginClicked(bool clicked)
        {
            this.originClicked = clicked; 
        }
        public void SetDestinationClicked(Button btn)
        {
            this.clickedDestination = btn; 
        }
        public void ResetDestinationClicked()
        {
            this.clickedDestination = null;
        }
        public void SetDestinationClicked(bool clicked)
        {
            this.destinationClicked = clicked;
        }

        public bool GetDestinationClicked()
        {
            return this.destinationClicked; 
        }
        public void IncreaseComputerScore (int points)
        {
            this.computerScore += points;
        }        
        public void IncreaseHumanScore (int points)
        {
            this.humanScore += points;
        }
        public string GetComputerScore()
        {
            return this.computerScore.ToString(); 
        }
        public string GetHumanScore()
        {
            return this.humanScore.ToString(); 
        }


    }
}
