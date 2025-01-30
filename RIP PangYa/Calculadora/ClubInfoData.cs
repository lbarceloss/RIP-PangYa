﻿using System.Collections.Generic;

namespace Calculadora
{

    public enum ClubInfoEnum
    {
        _1W,
        _2W,
        _3W,
        _2I,
        _3I,
        _4I,
        _5I,
        _6I,
        _7I,
        _8I,
        _9I,
        PW,
        SW,
        PT1,
        PT2
    }

    public static class ClubInfoData
    {

        public static Dictionary<string, ClubInfo> CLUB_INFO = new Dictionary<string, ClubInfo>()
        {

            {"_1W", new ClubInfo(ClubType.WOOD, 0.55, 1.61, 236.0, 10.0, 230.0)},
            {"_2W", new ClubInfo(ClubType.WOOD, 0.50, 1.41, 204.0, 13.0, 210.0)},
            {"_3W", new ClubInfo(ClubType.WOOD, 0.45, 1.26, 176.0, 16.0, 190.0)},
            {"_2I", new ClubInfo(ClubType.IRON, 0.45, 1.07, 161.0, 20.0, 180.0)},
            {"_3I", new ClubInfo(ClubType.IRON, 0.45, 0.95, 149.0, 24.0, 170.0)},
            {"_4I", new ClubInfo(ClubType.IRON, 0.45, 0.83, 139.0, 28.0, 160.0)},
            {"_5I", new ClubInfo(ClubType.IRON, 0.45, 0.73, 131.0, 32.0, 150.0)},
            {"_6I", new ClubInfo(ClubType.IRON, 0.41, 0.67, 124.0, 36.0, 140.0)},
            {"_7I", new ClubInfo(ClubType.IRON, 0.36, 0.61, 118.0, 40.0, 130.0)},
            {"_8I", new ClubInfo(ClubType.IRON, 0.30, 0.57, 114.0, 44.0, 120.0)},
            {"_9I", new ClubInfo(ClubType.IRON, 0.25, 0.53, 110.0, 48.0, 110.0)},
            {"PW", new ClubInfo(ClubType.PW, 0.18, 0.49, 107.0, 52.0, 100.0)},
            {"SW", new ClubInfo(ClubType.PW, 0.17, 0.42, 93.0, 56.0, 80.0)},
            {"PT1", new ClubInfo(ClubType.PT, 0.00, 0.00, 30.0, 0.00, 20.0)},
            {"PT2", new ClubInfo(ClubType.PT, 0.00, 0.00, 21.0, 0.00, 10.0)}

        };
    }
}
