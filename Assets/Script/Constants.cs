using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants{
    //The x,y bounds of scenes
    public static float maxX = 16;
    public static float maxY = 10;

    public static float dustColiderRatio = 21.0f;

    public static int maxNumOfLevel = 20;

    public static float deathHealthVal = 10;
    public static float screenRatio = 17.77f;
    //public static float energy2dis = 0.2f; //100 energy is 25 units

    public static float playerGlowSizeMax = 1.5f;
    public int deviceType = 0;

    public static string appId = "";


    public static string curLevelKey = "unlockedLevelID";
    public static string maxConstJumpKey = "maxConstJumpKey";
    public static Hashtable awardProbability = new Hashtable() { {"sagi", 200}, {"pisces", 300}, {"aqua", 500}, {"libra", 50} };
}
