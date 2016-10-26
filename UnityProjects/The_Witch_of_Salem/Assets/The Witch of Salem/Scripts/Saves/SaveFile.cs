using UnityEngine;
using System.Collections;

public class SaveFile  {

    public Vector3 playerPos;
    public PlayerStats ps;

    public int currentlevel;

    public Tutorial tutorialInfo;
    public Farmlands farmlandsInfo;
    public Forest forestInfo;
    public Caves cavesInfo;
    public WitchTower witchTowerInfo;
    public TestLevel testInfo;


    public SaveFile (Vector3 pos, PlayerStats playerS, GameManager GameM)
    {
        playerPos = pos;
        ps = playerS;
    }

    public SaveFile() { }

}
