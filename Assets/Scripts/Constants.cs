using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public struct VALUES
    {
        public const float CORNER_BET_PAY = 8;
        public const float STREET_BET_PAY = 11;
        public const float SPLIT_BET_PAY = 17;
        public const float STRAIGHT_BET_PAY = 35;
        public const float RED_BLACK_BET_PAY = 1;
        public const float ODD_EVEN_BET_PAY = 1;
        public const float HIGH_LOW_BET_PAY = 1;
        public const float DOZEN_BET_PAY = 2;
        public const float COLUMN_BET_PAY = 2;
    }

    public struct EVENTS
    {
        public const string BET_PLACED = "BET_PLACED";
        public const string BET_PLAYED = "BET_PLAYED";
        public const string BET_REMOVED = "BET_REMOVED";
        public const string PLAYER_CHIP_PLACED = "PLAYER_CHIP_PLACED";
    }

    public struct SOUNDS
    {
        public const string ROLLING_BALL = "rolling_ball";
    }
}
