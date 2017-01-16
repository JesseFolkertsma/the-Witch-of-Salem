using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class _UIManager : MonoBehaviour {

    public static _UIManager instance;

    GameObject mainCanvas;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public Sprite fullSkull;
    public Sprite emptySkull;
    public Sprite golem;
    public Sprite farmer;
    public Sprite raven;

    #region playerdata
    _Player player;
    public int lives;
    public int apples;
    public int arrows;
    public Text applesUI;
    public Text arrowsUI;
    public Text popupText;
    public Image[] livesUI;
    #endregion

    public GameObject enemyPanel;
    public GameObject enemy;
    public List<GameObject> enemies;

    #region Conversation
    [SerializeField]
    float writeRate;
    List<string> wholeConv;
    public bool writing;
    public bool inConv;
    public GameObject screenConv;
    public Text screenText;
    int convInt = 0;
    #endregion

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        mainCanvas = GameObject.Find("MainCanvas");
        livesUI = mainCanvas.transform.FindChild("InGameUI").FindChild("PlayerPanel").FindChild("Lives").GetComponentsInChildren<Image>();
        enemyPanel = mainCanvas.transform.FindChild("InGameUI").FindChild("EnemyPanel").gameObject;
        player = FindObjectOfType<_Player>();
        applesUI = mainCanvas.transform.FindChild("InGameUI").FindChild("PlayerPanel").FindChild("Utilities").FindChild("Apple").GetComponentInChildren<Text>();
        arrowsUI = mainCanvas.transform.FindChild("InGameUI").FindChild("PlayerPanel").FindChild("Utilities").FindChild("Arrow").GetComponentInChildren<Text>();
        popupText = mainCanvas.transform.FindChild("Popup").GetComponentInChildren<Text>();
        DisplayPopup(" ", 1f);
        screenConv.SetActive(false);
    }

    public void UpdateUI()
    {
        if (player != null)
        {
            if (player.lives != lives)
            {
                for (int i = 0; i < livesUI.Length; i++)
                {
                    if (i > player.lives)
                    {
                        livesUI[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        livesUI[i].gameObject.SetActive(true);
                    }
                }
            }
            if (apples != player.apples)
            {
                apples = player.apples;
                applesUI.text = apples.ToString();
            }
            if (arrows != player.arrows)
            {
                arrows = player.arrows;
                arrowsUI.text = arrows.ToString();
            }
        }
    }

    void Update()
    {
        if (inConv)
        {
            if (writing)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    StopAllCoroutines();
                    screenText.text = wholeConv[convInt];
                    writing = false;
                    convInt++;
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (convInt < wholeConv.Count)
                    {
                        screenText.text = "";
                        StartCoroutine(AddLetters(wholeConv[convInt]));
                    }
                    else
                    {
                        screenConv.SetActive(false);
                        player.enabled = true;
                    }
                }
            }
        }
    }

    public void StartConversation(List<string> conv)
    {
        screenConv.SetActive(true);
        wholeConv = conv;
        screenText.text = "";
        player.xInput = 0;
        player.enabled = false;
        inConv = true;
        convInt = 0;
        writing = true;
        print("Starting conv" + conv[convInt]);
        StartCoroutine(AddLetters(conv[convInt]));
    }

    IEnumerator AddLetters(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            yield return new WaitForSeconds(1/writeRate);
            writing = true;
            screenText.text += text[i];
        }
        writing = false;
        convInt++;
    }

    public void DisplayPopup(string text, float time)
    {
        popupText.text = text;
        StartCoroutine(PopupCoroutine(text, time));
    }

    IEnumerator PopupCoroutine(string text, float time)
    {
        popupText.CrossFadeAlpha(1, 1, false);
        yield return new WaitForSeconds(time);
        popupText.CrossFadeAlpha(0, 1, false);
    }

    public void AddEnemy(_EnemyUIData _enemy)
    {
        GameObject g = Instantiate(enemy, enemyPanel.transform) as GameObject;
        g.GetComponent<_EnemyUI>().data = _enemy;
    }
}
