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
        public Board board;
        public Game game;
        private string emptySquare = "NONE";
        private string whiteSquare = "WHITE";
        private string graySquare = "GRAY";
        private string whiteKing = "WHITE_KING";
        private string grayKing = "GRAY_KING"; 
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
            radioYou.Checked = true; 
            lvlEasy.Checked = true; 
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
            if (lvlMedium.Checked)
            {
                game.SetDepth(3); 
            } 
            else if (lvlHard.Checked)
            {
                game.SetDepth(4); 
            } else
            {
                game.SetDepth(2); 
            }
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
                UpdateTurn(Player.MAX);
                ComputerMove(); 
            }
        }
        private void ComputerMove()
        {
            board = AlphaBeta.callingFunction(board, game.GetDepth(), -1, 1, Player.MAX);
            Piece[,] squares = board.GetSquares();
            for (int row = 0; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    if (squares[row, col] == Piece.EMPTY)
                    {
                        buttons[row, col].BackgroundImage = Properties.Resources.checkerNone;
                        buttons[row, col].Tag = emptySquare;
                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else if (squares[row, col] == Piece.WHITE)
                    {
                        buttons[row, col].BackgroundImage = Properties.Resources.checkerWhite;
                        buttons[row, col].Tag = whiteSquare;
                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    } 
                    else if (squares[row, col] == Piece.GRAY)
                    {
                        buttons[row, col].BackgroundImage = Properties.Resources.checkerWhite;
                        buttons[row, col].Tag = graySquare;
                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    } 
                    else if (squares[row, col] == Piece.WHITE_KING)
                    {
                        buttons[row, col].BackgroundImage = Properties.Resources.checkerWhiteKing;
                        buttons[row, col].Tag = whiteKing;
                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    } 
                    else if (squares[row, col] == Piece.GRAY_KING)
                    {
                        buttons[row, col].BackgroundImage = Properties.Resources.checkerGrayKing;
                        buttons[row, col].Tag = grayKing;
                        buttons[row, col].BackgroundImageLayout = ImageLayout.Stretch;
                    }
                }
            }
            UpdateTurn(Player.MIN); 
        }
        private void InitializeTop(Bitmap checker, string color)
        {
            game.SetComputerColor(color); 
            int lastColumn = 2;
            int boardSize = 8;
            for (int row = 0; row <= lastColumn; row++)
            {
                for (int col = 0; col < boardSize; col++)
                {
                    if ((col + row) % 2 == 0)
                    {
                        buttons[row, col].BackgroundImage = checker;
                        buttons[row, col].Tag = color;
                    }
                }
            }
        }
        private void InitializeBottom(Bitmap checker, string color)
        {
            game.SetHumanColor(color); 
            int startingColumn = 5;

            for (int row = startingColumn; row < BOARD_SIZE; row++)
            {
                for (int col = 0; col < BOARD_SIZE; col++)
                {
                    if ((col + row) % 2 == 0)
                    {
                        buttons[row, col].BackgroundImage = checker;
                        buttons[row, col].Tag = color;
                    }
                }
            }
            board = new Board(buttons, game);
        }
        private void SquareOnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            bool pieceOfCurrentPlayer = game.GetCurrentPlayer().Equals(Player.MAX) && game.GetComputerColor().ToString().Equals(btn.Tag.ToString()) || (game.GetCurrentPlayer().Equals(Player.MIN) && game.GetHumanColor().ToString().Equals(btn.Tag.ToString()) ? true : false);
            bool emptySquare = btn.Tag.Equals(this.emptySquare);

            // This finds location of current button
            Location buttonLocation = null;
            for (var r = 0; r < BOARD_SIZE; r++)
            {
                for (var c = 0; c < BOARD_SIZE; c++)
                {
                    if (buttons[r, c] == btn)
                    {
                        buttonLocation = new Location(r, c);
                        break;
                    }
                }
            }

            // If an origin wasn't clicked, the selected button is the origin
            if (!emptySquare && pieceOfCurrentPlayer && !game.IsOriginClicked())
            {
                btn.BackColor = Color.Cyan;
                chooseDestination.Visible = true;
                cancel.Visible = true;
                game.SetOriginClicked(btn, true);
                game.SetOriginLocation(buttonLocation);
            }
            // Otherwise, selected button is the destination, check if it's a legal move. If it is, allow "move"
            else if (emptySquare && !game.IsDestinationClicked())
            {
                if (board.IsLegal(game.GetOriginLocation(), buttonLocation, game.GetCurrentPlayer()))
                {
                    btn.BackColor = Color.Magenta;
                    chooseDestination.Visible = false;
                    cancel.Visible = true;
                    game.SetDestinationClicked(btn, true);
                    game.SetDestinationLocation(buttonLocation); 
                    move.Visible = true; 
                }
                else
                {
                    MessageBox.Show("That move is not legal");
                }
            }
        }
        private void MoveClick(object sender, EventArgs e)
        {
            Piece color = game.GetCurrentPlayer().Equals(Player.MIN) ? game.GetHumanColor() : game.GetComputerColor();

            if (MoveChecker(game.GetCurrentPlayer(), color, game.GetOriginLocation(), game.GetDestinationLocation()))
            {
                EndOfTurn();
                if (board.GameOver())
                {
                    EndGame();
                }
            } 
            else
            {
                MessageBox.Show("Something went wrong :(");
            }
            
        }
        private bool MoveChecker(Player player, Piece color, Location origin, Location destination)
        {
            buttons[destination.row, destination.col].Tag = buttons[origin.row, origin.col].Tag;
            buttons[destination.row, destination.col].BackgroundImage = color == Piece.GRAY ? Properties.Resources.checkerWhite : Properties.Resources.checkerGray;
            buttons[origin.row, origin.col].Tag = emptySquare;
            buttons[origin.row, origin.col].BackgroundImage = Properties.Resources.checkerNone;


            if (origin.row - destination.row == Math.Abs(2) && origin.col - destination.col == Math.Abs(2))
            { // If jump
                int rowMiddle = origin.row < destination.row ? origin.row + 1 : origin.row - 1;
                int colMiddle = origin.col < destination.col ? origin.col + 1 : origin.col - 1;

                buttons[rowMiddle, colMiddle].Tag = emptySquare;
                buttons[rowMiddle, colMiddle].BackgroundImage = Properties.Resources.checkerNone; 
            }

            board.MakeMove(origin, destination, color);
            game.GetOriginButton().BackColor = Color.Red;
            game.GetDestinationButton().BackColor = Color.Red;

            //    while (board.PieceHasAvailableCapture(destination, player) != 0)
            //    {
            //        if (board.PieceHasAvailableCapture(destination, player) == 1)
            //        {
            //            // Make the available move
            //        }
            //        else if (board.PieceHasAvailableCapture(destination, player) == 2)
            //        {
            //            MessageBoxManager.Yes = "Right";
            //            MessageBoxManager.No = "Left";
            //            DialogResult result = MessageBox.Show("Select RIGHT or LEFT Capture", "Another Capture", MessageBoxButtons.YesNo);

            //            if (result == DialogResult.Yes)
            //            {
            //                // Make right jump
            //            }
            //            else
            //            {
            //                // Make left jump
            //            }
            //        }
            //        if (board.PieceHasAvailableCapture(destination, player) == 0)
            //        {
            //            return true;
            //        }
            //    }
            return true;
            //}
        }
        private void EndOfTurn()
        {
            game.NextPlayersTurn();
            UpdateTurn(game.GetCurrentPlayer());
            ResetRound();
            if (game.GetCurrentPlayer() == Player.MAX)
            {
                ComputerMove();
            }
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            ResetRound(); 
        }
        private void ResetRound()
        {
            if (game.IsOriginClicked())
            {
                game.GetOriginButton().BackColor = Color.Red;
                game.SetOriginClicked(null, false);
                game.SetOriginLocation(null);
            }
            if (game.IsDestinationClicked())
            {
                game.GetDestinationButton().BackColor = Color.Red;
                game.SetDestinationClicked(null, false);
                game.SetDestinationLocation(null);
            }
            cancel.Visible = false;
            move.Visible = false;
            chooseDestination.Visible = false;
        }
        private void UpdateTurn(Player player)
        {
            currentTurn.Text = player == Player.MIN ? "Your turn!" : "Computer's Turn!"; 
        }
        private void EndGame()
        {
            currentTurn.Text = "Game Over!"; 
        }
    }
}
