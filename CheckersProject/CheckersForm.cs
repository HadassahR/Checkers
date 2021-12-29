using CheckersGame;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersProject
{
    public partial class CheckersForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckersForm));
        Button[,] buttons;
        Board board;
        Game game; 
        const int BOARD_SIZE = 8;
        public CheckersForm()
        {
            InitializeComponent();
        }
        private void CheckerForm_Load(object sender, EventArgs e)
        {
            buttons = new Button[BOARD_SIZE, BOARD_SIZE] {
                { square1 , square2, square3, square4, square5, square6, square7, square8       },
                { square9, square10, square11, square12, square13, square14, square15, square16 },
                { square17, square18, square19, square20, square21, square22, square23, square24},
                { square25, square26, square27, square28, square29, square30, square31, square32},
                { square33, square34, square35, square36, square37, square38, square39, square40},
                { square41, square42, square43, square44, square45, square46, square47, square48},
                { square49, square50, square51, square52, square53, square54, square55, square56},
                { square57, square58, square59, square60, square61, square62, square63, square64}
            };
            board = new Board(buttons);
            game = new Game(); 
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
            foreach (var btn in buttons)
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
                InitializeTop(Properties.Resources.checkerGray, "White");
                InitializeBottom(Properties.Resources.checkerWhite, "Gray");
                game.SetStartingPlayer(Player.MIN);
                UpdateTurn(Player.MIN); 
            }
            else if (radioComputer.Checked)
            {
                InitializeTop(Properties.Resources.checkerWhite, "Gray");
                InitializeBottom(Properties.Resources.checkerGray, "White");
                game.SetStartingPlayer(Player.MAX);
                UpdateTurn(Player.MIN); 
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
                        buttons[col, row].BackgroundImage = checker;
                        buttons[col, row].Tag = color;
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
                        buttons[col, row].BackgroundImage = checker;
                        buttons[col, row].Tag = color;
                    }
                }
            }
        }
        private void SquareOnClick(object sender, EventArgs e)
        {
            Button btn = (Button) sender;
            bool pieceOfCurrentPlayer = game.GetCurrentPlayer().Equals(Player.MAX) && game.GetComputerColor().ToString().Equals(btn.Tag.ToString()) ? true
                : game.GetCurrentPlayer().Equals(Player.MIN) && game.GetHumanColor().ToString().Equals(btn.Tag.ToString()) ? true : false;
            
            if (!btn.Tag.Equals("none") && pieceOfCurrentPlayer && !game.IsOriginClicked()) // Must belong to current player
            {
                for (var r = 0; r < BOARD_SIZE; r++)
                {
                    for (var c = 0; c < BOARD_SIZE; c++)
                    {
                        if (buttons[r, c].Name == btn.Name)
                        {
                            btn.BackColor = Color.Cyan;
                            chooseDestination.Visible = true; // remember to switch to false in cancel and once move is done
                            cancel.Visible = true;
                            game.SetOriginClicked(btn, true);
                        }
                    }
                }
            } else if (btn.Tag.Equals("none") && !game.IsDestinationClicked())
                for (var r = 0; r < BOARD_SIZE; r++)
                {
                    for (var c = 0; c < BOARD_SIZE; c++)
                    {
                        if (buttons[r, c].Name == btn.Name)
                        {
                            btn.BackColor = Color.Magenta;
                            chooseDestination.Visible = false; // remember to switch to false in cancel and once move is done
                            cancel.Visible = true;
                            game.SetDestinationClicked(btn, true);
                        }
                    }
                }
            if (game.IsOriginClicked() && game.IsDestinationClicked())
            {
                move.Visible = true;
            }
        }
        private void MoveClick(object sender, EventArgs e)
        {
            Location origin = null;
            Location destination = null; 
            for (var r = 0; r < BOARD_SIZE; r++)
            {
                for (var c = 0; c < BOARD_SIZE; c++)
                {
                    if (buttons[r, c] == game.GetOriginButton())
                    {
                        origin = new Location(r, c);
                    } else if (buttons[r, c] == game.GetDestinationButton())
                    {
                        destination = new Location(r, c); 
                    }
                }
            }
            MoveChecker(game.GetCurrentPlayer(), origin, destination); 
        }
        private void MoveChecker(Player player, Location origin, Location destination)
        {
            buttons[destination.row, destination.col].Tag = buttons[origin.row, origin.col].Tag.ToString();
            if (player == Player.MIN)
            {
                bool gray = game.GetHumanColor().Equals("Gray"); 
                buttons[destination.row, destination.col].BackgroundImage = gray ? Properties.Resources.checkerGray : Properties.Resources.checkerWhite;  
            } else
            {
                bool gray = game.GetComputerColor().Equals("Gray");
                buttons[destination.row, destination.col].BackgroundImage = gray ? Properties.Resources.checkerGray : Properties.Resources.checkerWhite;
            }

            buttons[origin.row, origin.col].Tag = "none"; 
            buttons[origin.row, origin.col].BackgroundImage = Properties.Resources.checkerNone;
            if (player == Player.MIN)
            {
                game.IncreaseHumanScore(1); // clarify this
            } else
            {
                game.IncreaseComputerScore(1); 
            }
            game.NextPlayersTurn();
            UpdateTurn(game.GetCurrentPlayer()); 
            UpdateScoreDisplay(); 
            game.SetOriginClicked(null, false); 
            game.SetDestinationClicked(null, false); 
            //if (board.IsGameOver())
            //{
            //    EndGame(); 
            //}
            ResetColors();
            cancel.Visible = false;
            move.Visible = false; 
        }
        private void UpdateScoreDisplay()
        {
            computerScore.Text = game.GetComputerScore();
            youScore.Text = game.GetHumanScore(); 
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            ResetColors();
            cancel.Visible = false; 
        }
        private void ResetColors()
        {
            for (var r = 0; r < BOARD_SIZE; r++)
            {
                for (var c = 0; c < BOARD_SIZE; c++)
                {
                    if (buttons[r, c].BackColor == Color.Cyan || buttons[r, c].BackColor == Color.Magenta)
                    {
                        buttons[r, c].BackColor = Color.Red;
                        chooseDestination.Visible = false;
                        game.SetOriginClicked(null, false);
                        game.SetDestinationClicked(null, false);
                    }
                }
            }
        }
        private void UpdateTurn(Player player)
        {
            currentTurn.Text = player == Player.MIN ? "Your turn!" : "Computer's Turn!"; 
        }
        private void EndGame()
        {
            throw new NotImplementedException();
        }
    }
}
