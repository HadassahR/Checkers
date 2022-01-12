using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckersProject; 

namespace CheckersProject
{
    public class Game
    {
        private Player startingPlayer;
        private Player currentPlayer;
        private Piece computerColor;
        private Piece humanColor;
        private Button originButton;
        private Location originLocation;
        private Location destinationLocation; 
        private Button destinationButton; 
        private bool originClicked;
        private bool destinationClicked; 
        private int computerScore; 
        private int humanScore;
        private int depth; //set this based on difficulty
        public static readonly int MIN_VALUE;
        public static readonly int MAX_VALUE;

        public Game ()
        {
            this.computerScore = 0;
            this.humanScore = 0; 
        }

        public void SetOriginLocation(Location loc)
        {
            originLocation = loc;
        }

        public void SetDestinationLocation(Location loc)
        {
            destinationLocation = loc; 
        }

        public Location GetOriginLocation()
        {
            return originLocation;
        }

        public Location GetDestinationLocation()
        {
            return destinationLocation; 
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
        public Piece GetHumanColor()
        {
            return this.humanColor;
        }
        public void SetHumanColor(string tag)
        {
            this.humanColor = tag.Contains("WHITE") ? Piece.WHITE : Piece.GRAY; 
        }
        public Piece GetComputerColor()
        {
            return this.computerColor;
        }
        public void SetComputerColor(string tag)
        {
            this.computerColor = tag.Contains("WHITE") ? Piece.WHITE : Piece.GRAY;
        }
        public Button GetOriginButton()
        {
            return this.originButton; 
        }        
        public bool IsOriginClicked ()
        {
            return this.originClicked; 
        }
        public void SetOriginClicked(Button btn, bool clicked)
        {
            this.originButton = btn; 
            this.originClicked = clicked; 
        }
        public Button GetDestinationButton()
        {
            return this.destinationButton;
        }
        public bool IsDestinationClicked()
        {
            return this.destinationClicked;
        }
        public void SetDestinationClicked(Button btn, bool clicked)
        {
            this.destinationButton = btn;
            this.destinationClicked = clicked;
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
