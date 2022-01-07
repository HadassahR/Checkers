using CheckersGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersProject
{
    class MiniMax
    {
        public static int NrEntries = 0;
        public static double Value(Board board, int depth, Player player)
        {
            Trace.println($"Enter minimax d = {depth} P = {player}", 5);
            Trace.println($"Enter minimax d = {depth} P = {player}", 11, depth);
            Player opponent = player == Player.MAX ? Player.MIN : Player.MAX;
            ++NrEntries;
            double value = 0.0;
            if (depth == 0)
            {
                value = board.HeuristicValue();
            }
            else if (board.GameOver() && board.GetWinner() == opponent)
            {
                value = player == Player.MAX ? Game.MIN_VALUE :
                Game.MAX_VALUE;
            }
            else
            {
                if (player == Player.MAX)
                {
                    double BestValue = Game.MIN_VALUE;
                    foreach (Board nextMove in board.GetAllMoves(player))
                    {
                        double thisVal = Value(nextMove, depth - 1, opponent);
                        if (thisVal > BestValue)
                        {
                            BestValue = thisVal;
                        }
                    }
                    value = BestValue;
                }
                else  // player == Player.MIN
                {
                    double BestValue = 1.0;
                    foreach (Board nextMove in board.GetAllMoves(player))
                    {
                        double thisVal = Value(nextMove, depth - 1, opponent);
                        if (BestValue > thisVal)
                        {
                            BestValue = thisVal;
                        }
                    }
                    value = BestValue;
                }
            }
            Trace.println("Exit miniMax value = " + value + " depth " + depth, 5);
            Trace.println("Exit miniMax value = " + value + " depth " + depth, 11,
depth);
            return value;
        }
    }
}

