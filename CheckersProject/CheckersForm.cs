using CheckersGame;
using System;
using System.Drawing;
using System.Windows.Forms;
using WECPOFLogic;

namespace CheckersProject
{
    public partial class CheckersForm : Form
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CheckersForm));
        Button[,] buttons;
        Board board;
        Game game;
        private string emptySquare = "NONE";
        private string whiteSquare = "WHITE";
        private string graySquare = "GRAY"; 
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
        }
        private void InitializeNoneCheckers()
        {
            foreach (var btn in buttons)
            {
                btn.BackgroundImage = Properties.Resources.checkerNone;
                btn.Tag = emptySquare;
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
                InitializeTop(Properties.Resources.checkerGray, whiteSquare);
                InitializeBottom(Properties.Resources.checkerWhite, graySquare);
                game.SetStartingPlayer(Player.MIN);
                UpdateTurn(Player.MIN); 
            }
            else if (radioComputer.Checked)
            {
                InitializeTop(Properties.Resources.checkerWhite, graySquare);
                InitializeBottom(Properties.Resources.checkerGray, whiteSquare);
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
            board = new Board(buttons);
        }
        private void SquareOnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            bool pieceOfCurrentPlayer = game.GetCurrentPlayer().Equals(Player.MAX) && game.GetComputerColor().ToString().Equals(btn.Tag.ToString().ToUpper()) || (game.GetCurrentPlayer().Equals(Player.MIN) && game.GetHumanColor().ToString().Equals(btn.Tag.ToString().ToUpper()) ? true : false);
            bool emptySquare = btn.Tag.Equals(this.emptySquare);

            if (!emptySquare && pieceOfCurrentPlayer && !game.IsOriginClicked())
            {
                btn.BackColor = Color.Cyan;
                chooseDestination.Visible = true;
                cancel.Visible = true;
                game.SetOriginClicked(btn, true);
            }
            else if (emptySquare && !game.IsDestinationClicked())
            {
                btn.BackColor = Color.Magenta;
                chooseDestination.Visible = false;
                cancel.Visible = true;
                game.SetDestinationClicked(btn, true);

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
            Piece color = game.GetCurrentPlayer().Equals(Player.MIN) ? game.GetHumanColor() : game.GetComputerColor(); 

            if (board.IsLegal(origin, destination, game.GetCurrentPlayer()))
            {
                MoveChecker(game.GetCurrentPlayer(), color, origin, destination);
            }
            else
            {
                MessageBox.Show("Illegal Move");
                ResetRound(); 
            }
        }
        private void MoveChecker(Player player, Piece color, Location origin, Location destination)
        {
            buttons[destination.row, destination.col].Tag = buttons[origin.row, origin.col].Tag;
            buttons[destination.row, destination.col].BackgroundImage = color == Piece.GRAY ? Properties.Resources.checkerWhite : Properties.Resources.checkerGray; 
            buttons[origin.row, origin.col].Tag = emptySquare; 
            buttons[origin.row, origin.col].BackgroundImage = Properties.Resources.checkerNone;

            board.MakeMove(origin, destination, color);


            while (board.AnotherCapture(destination, player) != 0)
            {
                if (board.AnotherCapture(destination, player) == 1)
                {
                    // Make that move
                }
                else if (board.AnotherCapture(destination, player) == 2)
                {
                    MessageBoxManager.Yes = "Right";
                    MessageBoxManager.No = "Left";
                    DialogResult result = MessageBox.Show("Select RIGHT or LEFT Capture", "Another Capture", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        // right jump
                    }
                    else
                    {
                        // left jump
                    }
                }
                if (board.AnotherCapture(destination, player) == 0)
                {
                    EndOfTurn();
                }
            }

            if (board.GameOver())
            {
                EndGame();
            }

        }
        private void EndOfTurn()
        {
            game.NextPlayersTurn();
            UpdateTurn(game.GetCurrentPlayer());
            ResetRound();
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            ResetRound(); 
        }
        private void ResetRound()
        {
            cancel.Visible = false;
            move.Visible = false;
            chooseDestination.Visible = false;
            if (!game.IsOriginClicked().Equals(null))
            {
                game.GetOriginButton().BackColor = Color.Red;
            }

            if (!game.IsDestinationClicked().Equals(null))
            {
                game.GetDestinationButton().BackColor = Color.Red;

            }
            game.SetOriginClicked(null, false);
            game.SetDestinationClicked(null, false);
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
