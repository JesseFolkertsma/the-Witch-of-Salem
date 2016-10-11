using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SaveLoadSystem slSystem;

	void Start()
    {
        slSystem = new SaveLoadSystem();
    }

    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            slSystem.SaveGame();
        }
        if (Input.GetButtonDown("Load"))
        {
            slSystem.LoadGame();
        }
    }
}
