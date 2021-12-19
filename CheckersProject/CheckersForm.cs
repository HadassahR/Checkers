using CheckersGame;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersProject
{
    public partial class CheckersForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckersForm));
        Button[,] button;
        Board board;
        Game game; 
        const int BOARD_SIZE = 8;
        public CheckersForm()
        {
            InitializeComponent();
        }

        private void CheckerForm_Load(object sender, EventArgs e)
        {
            button = new Button[BOARD_SIZE, BOARD_SIZE] {
                { square1 , square2, square3, square4, square5, square6, square7, square8       },
                { square9, square10, square11, square12, square13, square14, square15, square16 },
                { square17, square18, square19, square20, square21, square22, square23, square24},
                { square25, square26, square27, square28, square29, square30, square31, square32},
                { square33, square34, square35, square36, square37, square38, square39, square40},
                { square41, square42, square43, square44, square45, square46, square47, square48},
                { square49, square50, square51, square52, square53, square54, square55, square56},
                { square57, square58, square59, square60, square61, square62, square63, square64}
            };
            game = new Game();
            board = new Board(button);

        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            InitializeNoneCheckers();
            InitializeBoard();
            startingPlayer.Visible = false;
            level.Visible = false;
            var startBtn = (Button)sender;
            startBtn.Visible = false;
            score.Visible = true; 
        }
        private void InitializeNoneCheckers()
        {
            foreach (var btn in button)
            {
                btn.BackgroundImage = Properties.Resources.checkerNone;
                btn.Tag = "none";
                btn.BackgroundImageLayout = ImageLayout.Stretch;
                if (btn.BackColor == Color.Black)
                {
                    btn.Enabled = false;
                } else
                {
                    btn.Click += new EventHandler(SquareOnClick);
                }
            }
        }
        private void InitializeBoard()
        {
            if (radioYou.Checked)
            {
                InitializeTop(Properties.Resources.checkerGray, "white");
                InitializeBottom(Properties.Resources.checkerWhite, "gray");
                game.SetStartingPlayer(Player.MIN);
            }
            else if (radioComputer.Checked)
            {
                InitializeTop(Properties.Resources.checkerWhite, "gray");
                InitializeBottom(Properties.Resources.checkerGray, "white");
                game.SetStartingPlayer(Player.MAX);
            }
        }
        private void InitializeTop(Bitmap checker, string color)
        {
            game.SetComputerColor(color); 
            int lastColumn = 2;
            int boardSize = 8;
            for (int col = 0; col <= lastColumn; col++)
            {
                for (int row = 0; row < boardSize; row++)
                {
                    if ((col + row) % 2 == 0)
                    {
                        button[col, row].BackgroundImage = checker;
                        button[col, row].Tag = color;
                    }
                }
            }
        }
        private void InitializeBottom(Bitmap checker, string color)
        {
            game.SetHumanColor(color); 
            int startingColumn = 5;

            for (int col = startingColumn; col < BOARD_SIZE; col++)
            {
                for (int row = 0; row < BOARD_SIZE; row++)
                {
                    if ((col + row) % 2 == 0)
                    {
                        button[col, row].BackgroundImage = checker;
                        button[col, row].Tag = color;
                    }
                }
            }
        }
        private void SquareOnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            bool pieceOfCurrentPlayer = game.GetCurrentPlayer().Equals(Player.MAX) && game.GetComputerColor().ToString().Equals(btn.Tag.ToString()) ? true
                : game.GetCurrentPlayer().Equals(Player.MIN) && game.GetHumanColor().ToString().Equals(btn.Tag.ToString()) ? true : false;

            if (!btn.Tag.Equals("none") && pieceOfCurrentPlayer) // Must belong to current player
            {
                for (var r = 0; r < BOARD_SIZE; r++)
                {
                    for (var c = 0; c < BOARD_SIZE; c++)
                    {
                        if (button[r, c].Name == btn.Name)
                        {
                            btn.BackColor = Color.Cyan;
                            chooseDestination.Visible = true; // remember to switch to false in cancel and once move is done
                            cancel.Visible = true;
                            game.SetPieceClicked(btn); 
                        }
                    }
                }

            }
        }
        private void DestinationCheckerOnClick()
        {
            throw new NotImplementedException();
        }
        private void MoveChecker()
        {
            throw new NotImplementedException();
            // Get location of origin checker
            // Get location of destination square
            // Set image and tag of origin to none
            // Set location and tag of destination to new one
            // Update score in GUI
            // Switch to next player
            game.NextPlayersTurn();
            UpdateScore(); // figure out how to update
            game.ResetClickedPiece(); 
        }
        private void UpdateScore()
        {
            computerScore.Text = game.GetComputerScore();
            youScore.Text = game.GetHumanScore(); 
        }
        private void ValidateMove()
        {
            throw new NotImplementedException(); 
            // calls a method in board? 
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            for (var r = 0; r < BOARD_SIZE; r++)
            {
                for (var c = 0; c < BOARD_SIZE; c++)
                {
                    if (button[r, c].BackColor == Color.Cyan) 
                    {
                        button[r,c].BackColor = Color.Red;
                        chooseDestination.Visible = false; // remember to switch to false. Cancel option
                        cancel.Visible = false;
                        game.ResetClickedPiece(); 
                    }
                }
            }
        }
    }
}
