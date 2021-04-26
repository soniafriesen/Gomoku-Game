using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization; // WCF data contract types
/*
 * Project: Project 2,Gomoku Game
 * Purpose: Using a multiplayer style game, demonstrate understanding using WCF 
 * Coders: An Le and Sonia Friesen
 * Date: Due April 6th, 2021
 */
namespace GomokuLibrary
{
    [DataContract]
    public class CallbackInfo
    {
        [DataMember]
        public bool GameOver { get; set; }
        [DataMember]
        public bool P1Turn { get; set; }
        [DataMember]
        public int P1Score { get; set; }
        [DataMember]
        public int P2Score { get; set; }
        [DataMember]
        public string Results { get; set; }
        [DataMember]
        public Symbol SelectedMark { get; private set; }
        [DataMember]
        public List<int> WinningCells { get; private set; }
        public CallbackInfo(bool gameEnd, bool p1turn, int p1score, int p2score, string result, Symbol selectedMark, List<int> winningCells)
        {
            GameOver = gameEnd;
            P1Turn = p1turn;
            P1Score = p1score;
            P2Score = p2score;
            Results = result;
            SelectedMark = selectedMark;
            WinningCells = winningCells;
        }
    }
}