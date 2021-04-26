using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
/*
 * Project: Project 2, Gomoku Game
 * Purpose: Using a multiplayer style game, demonstrate understanding using WCF 
 * Coders: An Le and Sonia Friesen
 * Date: Due April 6th, 2021
 */
namespace GomokuLibrary
{
    [DataContract]
    public class Symbol
    {
        public enum ValueMark { X, O,Y, Blank };
        [DataMember]
        public ValueMark Mark { get; private set; }
        [DataMember]
        public int GridPosition { get; private set; }

        //contructor to which the client will not use
        internal Symbol(ValueMark mark, int gridPosition)
        {
            Mark = mark;
            GridPosition = gridPosition;
        }
        public override string ToString()
        {
            return Mark.ToString();
        }

    }
}

