using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

/*
 * Project: Project 2, Gomoku Game
 * Purpose: Using a multiplayer style game, demonstrate understanding using WCF 
 * Coders: An Le and Sonia Friesen
 * Date: Due April 10th, 2021
 */
namespace GomokuLibrary
{
    //client callback contract 
    [ServiceContract]
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateGameUI(CallbackInfo info);
    }
    //define a Service contract for Interface
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IGame
    {
        [OperationContract]
        Symbol Play(bool p1InitalMove, int gridPosition);
        [OperationContract]
        string GetSymbol(int gridPosition);
        [OperationContract]
        void SeeWinner();
        [OperationContract(IsOneWay = true)]
        void StartNewGame();
        [OperationContract(IsOneWay = true)]
        void Repopulate();
        bool GameOver { [OperationContract]get; }
        bool P1Turn { [OperationContract] get; [OperationContract]set; }
        int P1Score { [OperationContract] get; }
        int P2Score { [OperationContract] get; }
        [OperationContract(IsOneWay = true)]
        void RegisterForCallbacks();
        [OperationContract(IsOneWay = true)]
        void UnregisterFromCallbacks();
    }
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private List<Symbol> gameGrid = null;
        private bool gameOver;
        private bool p1Turn;
        private int p1Score = 0, p2Score = 0;
        private string result = "";
        private static uint objCount = 0;
        private uint objNum;
        private List<int> winningCells = null;
        private Symbol selectedMark = null;
        private HashSet<ICallback> callbacks = new HashSet<ICallback>();

        /*-----constructor-----*/
        public Game()
        {
            objNum = ++objCount;
            Console.WriteLine($"Creating GamePlay Object #{objNum}");
            gameGrid = new List<Symbol>();
            Repopulate();
        }
        /* Interface inheritance */
        public bool GameOver
        {
            get
            {
                return gameOver;
            }
        }
        public bool P1Turn
        {
            get { return p1Turn; }
            set
            {
                p1Turn = value;
            }
        }
        public string GetSymbol(int gridPosition)
        {
            return gameGrid[gridPosition].ToString();
        }
        public int P1Score
        {
            get { return p1Score; }
        }

        public int P2Score
        {
            get { return p2Score; }
        }
        /*
         * Method: Play()
         * Purpose: Allows a player to place X or O on the game board, making sure that cell is "empty"
         * Parameters:bool, int
         * Returns:bool
         */
        public Symbol Play(bool p1InitalMove, int gridPosition)
        {
            if (gameGrid[gridPosition].Mark == Symbol.ValueMark.Blank)
            {
                if (p1InitalMove)
                {
                    gameGrid[gridPosition] = new Symbol(Symbol.ValueMark.X, gridPosition);
                    p1Turn = false;
                }
                else
                {
                    gameGrid[gridPosition] = new Symbol(Symbol.ValueMark.O, gridPosition);
                    p1Turn = true;
                }
                Console.WriteLine($"GamePlay Object #{objNum} Playing with {gameGrid[gridPosition]}.");
            }
            else
            {
                // If the selected cell has been used, then return null.
                return null;
            }
            selectedMark = gameGrid[gridPosition];

            return gameGrid[gridPosition];

        }
        /*
         * Method: SeeWinner()
         * Purpose: Checks to see if there is a winner
         * Parameters:none
         * Returns:List<int> (indicates the wining area (row,col, diagonal).
         */
        public void SeeWinner()
        {
            List<int> winnings = new List<int>(); //indicater to if someone won and who or tie

            //we have a 5by5 board grid 
            //[00][01][02][03][04]
            //[05][06][07][08][09]          This is what our board looks 
            //[10][11][12][13][14]          Each player takes turns in playing thier move
            //[15][16][17][18][19]
            //[20][21][22][23][24]

            /*----------------------------------*/
            /*             Row Check            */
            /*----------------------------------*/

            /*--1 row on grid [0][1][2][3][4]--*/
            if (gameGrid[0].Mark != Symbol.ValueMark.Blank && (gameGrid[1].Mark == gameGrid[0].Mark) && (gameGrid[2].Mark == gameGrid[0].Mark) && (gameGrid[3].Mark == gameGrid[0].Mark) && (gameGrid[4].Mark == gameGrid[0].Mark))
            {
                // [0][1][2][3][4] wining row
                winnings.Add(0);
                winnings.Add(1);
                winnings.Add(2);
                winnings.Add(3);
                winnings.Add(4);
                gameOver = true; //end the game
            }
            /*--2 row on grid [5][6][7][8][9]--*/
            if (gameGrid[5].Mark != Symbol.ValueMark.Blank && (gameGrid[6].Mark == gameGrid[5].Mark) && (gameGrid[7].Mark == gameGrid[5].Mark) && (gameGrid[8].Mark == gameGrid[5].Mark) && (gameGrid[9].Mark == gameGrid[5].Mark))
            {
                // [5][6][7][8][9] wining row
                winnings.Add(5);
                winnings.Add(6);
                winnings.Add(7);
                winnings.Add(8);
                winnings.Add(9);
                gameOver = true; //end the game
            }
            /*--3 row on grid [10][11][12][13][14]--*/
            if (gameGrid[10].Mark != Symbol.ValueMark.Blank && (gameGrid[11].Mark == gameGrid[10].Mark) && (gameGrid[12].Mark == gameGrid[10].Mark) && (gameGrid[13].Mark == gameGrid[10].Mark) && (gameGrid[14].Mark == gameGrid[10].Mark))
            {
                // [10][11][12][13][14] wining row
                winnings.Add(10);
                winnings.Add(11);
                winnings.Add(12);
                winnings.Add(13);
                winnings.Add(14);
                gameOver = true; //end the game
            }
            /*--4 row on grid [15][16][17][18][19]--*/
            if (gameGrid[15].Mark != Symbol.ValueMark.Blank && (gameGrid[16].Mark == gameGrid[15].Mark) && (gameGrid[17].Mark == gameGrid[15].Mark) && (gameGrid[18].Mark == gameGrid[15].Mark) && (gameGrid[19].Mark == gameGrid[15].Mark))
            {
                // [15][16][17][18][19] wining row
                winnings.Add(15);
                winnings.Add(16);
                winnings.Add(17);
                winnings.Add(18);
                winnings.Add(19);
                gameOver = true; //end the game
            }
            /*--5 row on grid [20][21][22][23][24]--*/
            if (gameGrid[20].Mark != Symbol.ValueMark.Blank && (gameGrid[21].Mark == gameGrid[20].Mark) && (gameGrid[22].Mark == gameGrid[20].Mark) && (gameGrid[23].Mark == gameGrid[20].Mark) && (gameGrid[24].Mark == gameGrid[20].Mark))
            {
                // [20][21][22][23][24] wining row
                winnings.Add(20);
                winnings.Add(21);
                winnings.Add(22);
                winnings.Add(23);
                winnings.Add(24);
                gameOver = true; //end the game
            }
            /*----------------------------------*/
            /*           Column Check           */
            /*----------------------------------*/

            /*--1 column on grid [0][5][10][15][20]--*/
            if (gameGrid[0].Mark != Symbol.ValueMark.Blank && (gameGrid[5].Mark == gameGrid[0].Mark) && (gameGrid[10].Mark == gameGrid[0].Mark) && (gameGrid[15].Mark == gameGrid[0].Mark) && (gameGrid[20].Mark == gameGrid[0].Mark))
            {
                // [0]
                // [5]
                // [10]
                // [15]
                // [20]
                // wining col
                winnings.Add(0);
                winnings.Add(5);
                winnings.Add(10);
                winnings.Add(15);
                winnings.Add(20);
                gameOver = true; //end the game
            }
            /*--2 column on grid [1][6][11][16][21]--*/
            if (gameGrid[1].Mark != Symbol.ValueMark.Blank && (gameGrid[6].Mark == gameGrid[1].Mark) && (gameGrid[11].Mark == gameGrid[1].Mark) && (gameGrid[16].Mark == gameGrid[1].Mark) && (gameGrid[21].Mark == gameGrid[1].Mark))
            {
                // [1]
                // [6]
                // [11]
                // [16]
                // [21]
                // wining col
                winnings.Add(1);
                winnings.Add(6);
                winnings.Add(11);
                winnings.Add(16);
                winnings.Add(21);
                gameOver = true; //end the game
            }
            /*--3 column on grid [2][7][12][17][22]--*/
            if (gameGrid[2].Mark != Symbol.ValueMark.Blank && (gameGrid[7].Mark == gameGrid[2].Mark) && (gameGrid[12].Mark == gameGrid[2].Mark) && (gameGrid[17].Mark == gameGrid[2].Mark) && (gameGrid[22].Mark == gameGrid[2].Mark))
            {
                // [2]
                // [7]
                // [12]
                // [17]
                // [22]
                // wining col
                winnings.Add(2);
                winnings.Add(7);
                winnings.Add(12);
                winnings.Add(17);
                winnings.Add(22);
                gameOver = true; //end the game
            }
            /*--4 column on grid [3][8][13][18][23]--*/
            if (gameGrid[3].Mark != Symbol.ValueMark.Blank && (gameGrid[8].Mark == gameGrid[3].Mark) && (gameGrid[13].Mark == gameGrid[3].Mark) && (gameGrid[18].Mark == gameGrid[3].Mark) && (gameGrid[23].Mark == gameGrid[3].Mark))
            {
                // [3]
                // [8]
                // [13]
                // [18]
                // [23]
                // wining col
                winnings.Add(3);
                winnings.Add(8);
                winnings.Add(13);
                winnings.Add(18);
                winnings.Add(23);
                gameOver = true; //end the game
            }
            /*--5 column on grid [4][9][14][19][24]--*/
            if (gameGrid[4].Mark != Symbol.ValueMark.Blank && (gameGrid[9].Mark == gameGrid[4].Mark) && (gameGrid[14].Mark == gameGrid[4].Mark) && (gameGrid[19].Mark == gameGrid[4].Mark) && (gameGrid[24].Mark == gameGrid[4].Mark))
            {
                // [4]
                // [9]
                // [14]
                // [19]
                // [24]
                // wining col
                winnings.Add(4);
                winnings.Add(9);
                winnings.Add(14);
                winnings.Add(19);
                winnings.Add(24);
                gameOver = true; //end the game
            }
            /*----------------------------------*/
            /*          Diagonal Check          */
            /*----------------------------------*/

            /*--1 left top to bottom right [0][6][12][18][24]*/
            if (gameGrid[0].Mark != Symbol.ValueMark.Blank && (gameGrid[6].Mark == gameGrid[0].Mark) && (gameGrid[12].Mark == gameGrid[0].Mark) && (gameGrid[18].Mark == gameGrid[0].Mark) && (gameGrid[24].Mark == gameGrid[0].Mark))
            {
                // [0]
                //   [6]
                //     [12]
                //       [18]
                //          [24]               
                // winning diagonal
                winnings.Add(0);
                winnings.Add(6);
                winnings.Add(12);
                winnings.Add(18);
                winnings.Add(24);
                gameOver = true; //end the game
            }
            /*--1 right top to left right [4][8][12][16][20]*/
            if (gameGrid[4].Mark != Symbol.ValueMark.Blank && (gameGrid[8].Mark == gameGrid[4].Mark) && (gameGrid[12].Mark == gameGrid[4].Mark) && (gameGrid[16].Mark == gameGrid[4].Mark) && (gameGrid[20].Mark == gameGrid[4].Mark))
            {
                //          [4]
                //        [8]
                //     [12]
                //   [16]
                // [20]               
                // winning diagonal
                winnings.Add(4);
                winnings.Add(8);
                winnings.Add(12);
                winnings.Add(16);
                winnings.Add(20);
                gameOver = true; //end the game
            }
            if (!gameGrid.Any(mark => mark.Mark == Symbol.ValueMark.Blank)) //no more blank cell
            {
                result = "Tie!";
                gameOver = true;
            }
            //if we have a winner if not it should return null, meaning its a tie
            if (winnings.Count != 0)
            {
                Console.WriteLine($"GamePlay Object #{objNum} Won.");
                GetScore();
            }
            winningCells = winnings;
            updateClients(gameOver);
        }
        /*
       * Method: GetScore()
       * Purpose: Determines who won the game or tie
       * Parameters:none
       * Returns:none
       */
        public void GetScore()
        {
            if (gameOver && p1Turn)
            {
                p2Score += 1;
                result = "Player B Won";
            }
            else if (gameOver && !p1Turn)
            {
                p1Score += 1;
                result = "Player A Won";
            }
        }
        /*
        * Method: StartNewGame()
        * Purpose: crestes a game or "new" game
        * Parameters:none
        * Returns:none
        */
        public void StartNewGame()
        {
            Console.WriteLine($"GamePlay Object #{objNum} left.");
            Repopulate();
            p1Score = 0;
            p2Score = 0;
        }
        /*---------Helper Methods---------*/

        /*
        * Method: Repopulate
        * Purpose: resets and populates values
        * Parameters:none
        * Returns:none
        */
        public void Repopulate()
        {
            //clear all cells in current game
            gameGrid.Clear();
            //create a list of blank cells (25 cells)
            for (int i = 0; i < 25; ++i)
            {
                gameGrid.Add(new Symbol(Symbol.ValueMark.Blank, i));
            }


            // Resets member variables
            p1Turn = true;
            result = "";
            gameOver = false;
            selectedMark = null;

            updateClients(gameOver);
        }


        /*
         * Method: updateClients
         * Purpose: update the client if there are changes
         * Parameters:boolean
         * Returns:none
         */
        public void updateClients(bool gameStatus)
        {

            CallbackInfo info = new CallbackInfo(gameStatus, p1Turn, p1Score, p2Score, result, selectedMark, winningCells);

            foreach (ICallback cb in callbacks)
                if (cb != null)
                    cb.UpdateGameUI(info);
        }
        /*------Register and unregister for ICallBack*/

        /* Using Shoe Object as a reference from in-class examples */
        public void RegisterForCallbacks()
        {
            //who is calling this method
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            //if we don't already have this client, add the proxy to the callbacks hashset
            if (!callbacks.Contains(cb))
                callbacks.Add(cb);
        }
        /* Using Shoe Object as a reference from in-class examples */
        public void UnregisterFromCallbacks()
        {
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            //remove so we don't have hanging references (ex Shoe object in class)
            if (callbacks.Contains(cb))
                callbacks.Remove(cb);
        }
    }
}