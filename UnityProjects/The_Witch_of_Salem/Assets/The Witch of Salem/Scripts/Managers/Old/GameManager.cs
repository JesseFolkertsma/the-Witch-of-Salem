using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public PlayerStats playerS;

    public bool loadGameOnStart;

    public SaveLoadSystem slSystem;
    public PopupMessages popup;

    public LevelManager lm;
    public ConversationManager cm;
    public PlayerUIManager pUI;

    public string playerName;

    public int currentLevel;
    public int currentCheckpoint = 0;

    public Vector3 loadPos;

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

        playerS = new PlayerStats();

        DontDestroyOnLoad(gameObject);

        lm = new LevelManager();
        cm = GetComponent<ConversationManager>();
        pUI = GetComponent<PlayerUIManager>();


        slSystem = new SaveLoadSystem();
        //slSystem.gm = this;
        popup = GetComponent<PopupMessages>();

        currentLevel = 5;
    }

    public void InitPlayer()
    {
        pUI.UIStart();
        cm.Init();
        popup.Init();
        if (loadGameOnStart)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateMachine>().ps = playerS;
            GameObject.FindGameObjectWithTag("Player").transform.position = loadPos;
            //slSystem.SetupLevel(slSystem.sFile.currentlevel, slSystem.sFile.currentCheckpoint);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Save"))
        {
            //slSystem.SaveGame(playerName);
            popup.DisplayPopup("Quicksaving", 2);
        }
        if (Input.GetButtonDown("Load"))
        {
            //slSystem.LoadGame(playerName);
            popup.DisplayPopup("Quickloading", 2);
        }

        pUI.UIUpdate();
    }
}
