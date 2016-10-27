using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public SaveLoadSystem slSystem;
    public PopupMessages popup;

    public LevelManager lm;
    public ConversationManager cm;
    public PlayerUIManager pUI;

    public string playerName;

    public int currentLevel;
    public int currentCheckpoint = 0;

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

        DontDestroyOnLoad(gameObject);

        lm = new LevelManager();
        cm = GetComponent<ConversationManager>();
        pUI = GetComponent<PlayerUIManager>();

        pUI.UIStart();

        slSystem = new SaveLoadSystem();
        popup = GetComponent<PopupMessages>();

        currentLevel = 5;
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

        pUI.UIUpdate();
    }

    public void LoadLevel()
    {

    }
}
