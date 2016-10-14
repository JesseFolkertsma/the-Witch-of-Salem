using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public SaveLoadSystem slSystem;
    public EnemyManager eManager;
    public PopupMessages popup;

    public LevelManager lm;
    public ConversationManager cm;

    public string playerName;

    void Awake()
    {
        //Makes sure there is only one GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        lm = new LevelManager();
        cm = GetComponent<ConversationManager>();

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
