using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public SaveLoadSystem slSystem;
    public EnemyManager eManager;
    public PopupMessages popup;

	void Start()
    {
        slSystem = new SaveLoadSystem();
        popup = GameObject.Find("UI Manager").GetComponent<PopupMessages>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            slSystem.SaveGame();
            popup.DisplayPopup("Quicksaving", 2);
        }
        if (Input.GetButtonDown("Load"))
        {
            slSystem.LoadGame();
            popup.DisplayPopup("Quickloading", 2);
        }
    }

    public void LoadLevel()
    {

    }
}
