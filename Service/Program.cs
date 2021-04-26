using GomokuLibrary;
using System;
using System.ServiceModel;
/*
 * Project: Project 2, Gomoku Game
 * Purpose: Using a multiplayer style game, demonstrate understanding using WCF 
 * Coders: An Le and Sonia Friesen
 * Date: Due April 6th, 2021
 */
namespace GomokuService
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;
            try
            {
                // Register the service address
                // servHost = new ServiceHost(typeof(Game), new Uri("net.tcp://localhost:13200/GomokuLibaray/")); 
                // Register the service contract and binding
                //servHost.AddServiceEndpoint(typeof(IGame), new NetTcpBinding(), "GameService");

                //other servHost are now implemented in app.config
                servHost = new ServiceHost(typeof(Game));

                // Run the service
                servHost.Open();
                Console.WriteLine("Service started. Please any key to quit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadKey();
                if (servHost != null)
                    servHost.Close();
            }
        }
    }
}