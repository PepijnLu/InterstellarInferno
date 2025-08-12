using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static Dictionary<string, KeyCode> keycodes = new Dictionary<string, KeyCode>();
    public static float musicVolume = 50, sfxVolume = 50;
    public static bool notFirstTimeMenu;
    public static int planetsConquered, score, highScore;
}
