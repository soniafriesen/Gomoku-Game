using GomokuLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
/*
 * Project: Project 2. Gomoku Game
 * Purpose: Using a multiplayer style game, demonstrate understanding using WCF 
 * Coders: An Le and Sonia Friesen
 * Date: Due April 6th, 2021
 */
namespace Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public partial class MainWindow : Window, ICallback
    {
        private IGame gomoku = null;        
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //connect to end point
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "GameEndPoint");
                gomoku = channel.CreateChannel();

                // Register for the callbacks
                gomoku.RegisterForCallbacks();                
                //get the board ready
                ResetBoard();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            //create a new game reseting the gridList
            gomoku.Repopulate();
        }

        private void ResetBoard()
        {
            //reest the buttons to have blanks for a new game
            GameBoardContainer.Children.Cast<Button>().ToList().ForEach(grid =>
            {
                grid.Content = "";
                grid.Background = Brushes.Transparent;
                grid.IsEnabled = true;
                grid.Opacity = 1;
            });

            //reset the scores
            TextBoxPlayerAScore.Background = Brushes.LightGoldenrodYellow;
            TextBoxPlayerBScore.Background = Brushes.White;

            // Reset Results
            TextBoxResult.Text = String.Empty;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gomoku.GameOver)
                {
                    return; //games over, just return null/undefined
                }
                var grid = (Button)sender;

                //find the position of where the player places symbol X or O
                var row = Grid.GetRow(grid);
                var column = Grid.GetColumn(grid);

                //get the index
                var index = column + (row * 5); //board id 5 by 5

                //whos played the play
                var p1Turn = gomoku.P1Turn;

                //play and get which did the X or O land on
                Symbol mark = gomoku.Play(p1Turn, index);

                if (mark != null)
                {
                    //assign to position on board
                    grid.Content = mark;
                    //check for a winner
                    gomoku.SeeWinner();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Quitting, so unregister from the client callbacks
                gomoku?.UnregisterFromCallbacks();

                // One of the players leave the game, reset the scores to zero
                gomoku.StartNewGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //callback contract
        private delegate void UpdateClientDelegate(CallbackInfo info);
        public void UpdateGameUI(CallbackInfo info)
        {            
            if (System.Threading.Thread.CurrentThread == this.Dispatcher.Thread)
            {
                //Symbol update on Gameboard
                Symbol mark = info.SelectedMark;
                if (mark != null)
                {                   
                    switch (mark.GridPosition)
                    {
                        case 0:
                            Button0x0.Content = mark;
                            if (info.P1Turn)
                                Button0x0.Foreground = Brushes.Red;
                            else
                                Button0x0.Foreground = Brushes.Black;
                            break;
                        case 1:
                            Button0x1.Content = mark;
                            if (info.P1Turn)
                                Button0x1.Foreground = Brushes.Red;
                            else
                                Button0x1.Foreground = Brushes.Black;
                            break;
                        case 2:
                            Button0x2.Content = mark;
                            if (info.P1Turn)
                                Button0x2.Foreground = Brushes.Red;
                            else
                                Button0x2.Foreground = Brushes.Black;
                            break;
                        case 3:
                            Button0x3.Content = mark;
                            if (info.P1Turn)
                                Button0x3.Foreground = Brushes.Red;
                            else
                                Button0x3.Foreground = Brushes.Black;
                            break;
                        case 4:
                            Button0x4.Content = mark;
                            if (info.P1Turn)
                                Button0x4.Foreground = Brushes.Red;
                            else
                                Button0x4.Foreground = Brushes.Black;
                            break;
                        case 5:
                            Button1x0.Content = mark;
                            if (info.P1Turn)
                                Button1x0.Foreground = Brushes.Red;
                            else
                                Button1x0.Foreground = Brushes.Black;
                            break;
                        case 6:
                            Button1x1.Content = mark;
                            if (info.P1Turn)
                                Button1x1.Foreground = Brushes.Red;
                            else
                                Button1x1.Foreground = Brushes.Black;
                            break;
                        case 7:
                            Button1x2.Content = mark;
                            if (info.P1Turn)
                                Button1x2.Foreground = Brushes.Red;
                            else
                                Button1x2.Foreground = Brushes.Black;
                            break;
                        case 8:
                            Button1x3.Content = mark;
                            if (info.P1Turn)
                                Button1x3.Foreground = Brushes.Red;
                            else
                                Button1x3.Foreground = Brushes.Black;
                            break;
                        case 9:
                            Button1x4.Content = mark;
                            if (info.P1Turn)
                                Button1x4.Foreground = Brushes.Red;
                            else
                                Button1x4.Foreground = Brushes.Black;
                            break;
                        case 10:
                            Button2x0.Content = mark;
                            if (info.P1Turn)
                                Button2x0.Foreground = Brushes.Red;
                            else
                                Button2x0.Foreground = Brushes.Black;
                            break;
                        case 11:
                            Button2x1.Content = mark;
                            if (info.P1Turn)
                                Button2x1.Foreground = Brushes.Red;
                            else
                                Button2x1.Foreground = Brushes.Black;
                            break;
                        case 12:
                            Button2x2.Content = mark;
                            if (info.P1Turn)
                                Button2x2.Foreground = Brushes.Red;
                            else
                                Button2x2.Foreground = Brushes.Black;
                            break;
                        case 13:
                            Button2x3.Content = mark;
                            if (info.P1Turn)
                                Button2x3.Foreground = Brushes.Red;
                            else
                                Button2x3.Foreground = Brushes.Black;
                            break;
                        case 14:
                            Button2x4.Content = mark;
                            if (info.P1Turn)
                                Button2x4.Foreground = Brushes.Red;
                            else
                                Button2x4.Foreground = Brushes.Black;
                            break;
                        case 15:
                            Button3x0.Content = mark;
                            if (info.P1Turn)
                                Button3x0.Foreground = Brushes.Red;
                            else
                                Button3x0.Foreground = Brushes.Black;
                            break;
                        case 16:
                            Button3x1.Content = mark;
                            if (info.P1Turn)
                                Button3x1.Foreground = Brushes.Red;
                            else
                                Button3x1.Foreground = Brushes.Black;
                            break;
                        case 17:
                            Button3x2.Content = mark;
                            if (info.P1Turn)
                                Button3x2.Foreground = Brushes.Red;
                            else
                                Button3x2.Foreground = Brushes.Black;
                            break;
                        case 18:
                            Button3x3.Content = mark;
                            if (info.P1Turn)
                                Button3x3.Foreground = Brushes.Red;
                            else
                                Button3x3.Foreground = Brushes.Black;
                            break;
                        case 19:
                            Button3x4.Content = mark;
                            if (info.P1Turn)
                                Button3x4.Foreground = Brushes.Red;
                            else
                                Button3x4.Foreground = Brushes.Black;
                            break;
                        case 20:
                            Button4x0.Content = mark;
                            if (info.P1Turn)
                                Button4x0.Foreground = Brushes.Red;
                            else
                                Button4x0.Foreground = Brushes.Black;
                            break;
                        case 21:
                            Button4x1.Content = mark;
                            if (info.P1Turn)
                                Button4x1.Foreground = Brushes.Red;
                            else
                                Button4x1.Foreground = Brushes.Black;
                            break;
                        case 22:
                            Button4x2.Content = mark;
                            if (info.P1Turn)
                                Button4x2.Foreground = Brushes.Red;
                            else
                                Button4x2.Foreground = Brushes.Black;
                            break;
                        case 23:
                            Button4x3.Content = mark;
                            if (info.P1Turn)
                                Button4x3.Foreground = Brushes.Red;
                            else
                                Button4x3.Foreground = Brushes.Black;
                            break;
                        case 24:
                            Button4x4.Content = mark;
                            if (info.P1Turn)
                                Button4x4.Foreground = Brushes.Red;
                            else
                                Button4x4.Foreground = Brushes.Black;
                            break;
                        default:
                            break;
                    }
                    if (!info.GameOver)
                    {

                        // Update the turn
                        if (info.P1Turn)
                        {
                            // Change Background color to indicate the next player turn
                            TextBoxResult.Text = "Player A's Turn";
                            TextBoxPlayerAScore.Background = Brushes.LightGoldenrodYellow;
                            TextBoxPlayerBScore.Background = Brushes.White;
                        }
                        else
                        {
                            // Change Background color to indicate the next player turn
                            TextBoxResult.Text = "Player B's Turn";
                            TextBoxPlayerAScore.Background = Brushes.White;
                            TextBoxPlayerBScore.Background = Brushes.LightGoldenrodYellow;
                        }

                    } //game ends
                    else
                    {
                        // Update the result
                        TextBoxResult.Text = info.Results.ToString();

                        // Update the score
                        TextBoxPlayerAScore.Text = info.P1Score.ToString();
                        TextBoxPlayerBScore.Text = info.P2Score.ToString();                       

                        // Show winning cells
                        List<int> marks = info.WinningCells;

                        if (marks.Count != 0) // If the games ends
                        {
                            GameBoardContainer.Children.Cast<Button>().ToList().ForEach(grid =>
                            {
                                grid.IsEnabled = false;
                                grid.Opacity = 0.2;

                            });
                            foreach (var i in marks)
                            {
                                switch (i)
                                {
                                    case 0:
                                        Button0x0.Background = Brushes.Goldenrod;
                                        Button0x0.Opacity = 0.7;
                                        break;
                                    case 1:
                                        Button0x1.Background = Brushes.Goldenrod;
                                        Button0x1.Opacity = 0.7;
                                        break;
                                    case 2:
                                        Button0x2.Background = Brushes.Goldenrod;
                                        Button0x2.Opacity = 0.7;
                                        break;
                                    case 3:
                                        Button0x3.Background = Brushes.Goldenrod;
                                        Button0x3.Opacity = 0.7;
                                        break;
                                    case 4:
                                        Button0x4.Background = Brushes.Goldenrod;
                                        Button0x4.Opacity = 0.7;
                                        break;
                                    case 5:
                                        Button1x0.Background = Brushes.Goldenrod;
                                        Button1x0.Opacity = 0.7;
                                        break;
                                    case 6:
                                        Button1x1.Background = Brushes.Goldenrod;
                                        Button1x1.Opacity = 0.7;
                                        break;
                                    case 7:
                                        Button1x2.Background = Brushes.Goldenrod;
                                        Button1x2.Opacity = 0.7;
                                        break;
                                    case 8:
                                        Button1x3.Background = Brushes.Goldenrod;
                                        Button1x3.Opacity = 0.7;
                                        break;
                                    case 9:
                                        Button1x4.Background = Brushes.Goldenrod;
                                        Button1x4.Opacity = 0.7;
                                        break;
                                    case 10:
                                        Button2x0.Background = Brushes.Goldenrod;
                                        Button2x0.Opacity = 0.7;
                                        break;
                                    case 11:
                                        Button2x1.Background = Brushes.Goldenrod;
                                        Button2x1.Opacity = 0.7;
                                        break;
                                    case 12:
                                        Button2x2.Background = Brushes.Goldenrod;
                                        Button2x2.Opacity = 0.7;
                                        break;
                                    case 13:
                                        Button2x3.Background = Brushes.Goldenrod;
                                        Button2x3.Opacity = 0.7;
                                        break;
                                    case 14:
                                        Button2x4.Background = Brushes.Goldenrod;
                                        Button2x4.Opacity = 0.7;
                                        break;
                                    case 15:
                                        Button3x0.Background = Brushes.Goldenrod;
                                        Button3x0.Opacity = 0.7;
                                        break;
                                    case 16:
                                        Button3x1.Background = Brushes.Goldenrod;
                                        Button3x1.Opacity = 0.7;
                                        break;
                                    case 17:
                                        Button3x2.Background = Brushes.Goldenrod;
                                        Button3x2.Opacity = 0.7;
                                        break;
                                    case 18:
                                        Button3x3.Background = Brushes.Goldenrod;
                                        Button3x3.Opacity = 0.7;
                                        break;
                                    case 19:
                                        Button3x4.Background = Brushes.Goldenrod;
                                        Button3x4.Opacity = 0.7;
                                        break;
                                    case 20:
                                        Button4x0.Background = Brushes.Goldenrod;
                                        Button4x0.Opacity = 0.7;
                                        break;
                                    case 21:
                                        Button4x1.Background = Brushes.Goldenrod;
                                        Button4x1.Opacity = 0.7;
                                        break;
                                    case 22:
                                        Button4x2.Background = Brushes.Goldenrod;
                                        Button4x2.Opacity = 0.7;
                                        break;
                                    case 23:
                                        Button4x3.Background = Brushes.Goldenrod;
                                        Button4x3.Opacity = 0.7;
                                        break;
                                    case 24:
                                        Button4x4.Background = Brushes.Goldenrod;
                                        Button4x4.Opacity = 0.7;
                                        break;
                                    default:
                                        break;
                                }//end switch
                            }//end foreach
                        }//end if
                    }//end if
                }
                //if mark is null
                else
                {
                    // game repopulates, reset the UI.
                    if (!info.GameOver)
                    {
                        // Reset UI
                        ResetBoard();
                    }
                }
            }
            else
            {
                // Not the dispatcher thread that's running this method!
                this.Dispatcher.BeginInvoke(new UpdateClientDelegate(UpdateGameUI), info);
            }
        }
        
    }
}
