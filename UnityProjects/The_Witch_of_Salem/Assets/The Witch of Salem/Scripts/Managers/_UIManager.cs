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

    _Player player;
    public int lives;
    public int apples;
    public int arrows;
    public Text applesUI;
    public Text arrowsUI;
    public Text popupText;
    public Image[] livesUI;
    public GameObject enemyPanel;
    public GameObject enemy;
    public List<GameObject> enemies;

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
    }

    void Start()
    {
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
