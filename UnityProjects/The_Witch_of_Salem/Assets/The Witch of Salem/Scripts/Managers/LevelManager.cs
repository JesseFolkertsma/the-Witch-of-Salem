﻿using UnityEngine;
using System.Collections;

public class LevelManager {

    public Level currentLevel;

    public Tutorial tutorialInfo;
    public Farmlands farmlandsInfo;
    public Forest forestInfo;
    public Caves cavesInfo;
    public WitchTower witchTowerInfo;
    public TestLevel testInfo;

    GameManager gameManager;

    public LevelManager() {
        LoadLevel(5);
    }

    public void LoadLevel(int level)
    {
        switch (level)
        {
            case 0:
                currentLevel = tutorialInfo;
                break;
            case 1:
                currentLevel = farmlandsInfo;
                break;
            case 2:
                currentLevel = forestInfo;
                break;
            case 3:
                currentLevel = cavesInfo;
                break;
            case 4:
                currentLevel = witchTowerInfo;
                break;
            case 5:
                currentLevel = witchTowerInfo;
                break;
        }
    }

}
