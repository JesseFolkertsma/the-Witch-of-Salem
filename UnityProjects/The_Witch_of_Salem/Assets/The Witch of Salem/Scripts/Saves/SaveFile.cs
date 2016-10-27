using UnityEngine;
using System.Collections;

public class SaveFile  {

    public Vector3 playerPos;
    public int lives;
    public int apples;
    public int arrows;

    public int currentlevel;
    public int currentCheckpoint;

    //public Tutorial tutorialInfo;
    //public Farmlands farmlandsInfo;
    //public Forest forestInfo;
    //public Caves cavesInfo;
    //public WitchTower witchTowerInfo;
    //public TestLevel testInfo;


    public SaveFile (Vector3 pos, PlayerStats playerS, int ccp)
    {
        playerPos = pos;
        lives = playerS.lives;
        apples = playerS.apples;
        arrows = playerS.arrows;
        currentlevel = 5;
        currentCheckpoint = ccp;
        //testInfo = lvl;
    }

    public SaveFile() { }

}
