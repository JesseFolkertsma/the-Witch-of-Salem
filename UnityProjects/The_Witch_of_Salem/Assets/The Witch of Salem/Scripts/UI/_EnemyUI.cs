using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class _EnemyUI : MonoBehaviour {

    public _EnemyUIData data;

    public int livesI;
    public Image[] lives;
    public Image head;

    bool dataSetup;

    void Awake()
    {
        lives = transform.GetChild(0).GetComponentsInChildren<Image>();
        head = transform.GetChild(1).GetComponent<Image>();
    }

    void Update()
    {
        if(data != null)
        {
            if (!dataSetup)
            {
                print("Whut");
                for (int i = 0; i < lives.Length; i++)
                {
                    if (i > data.lives)
                    {
                        lives[i].gameObject.SetActive(false);
                    }
                }
                head.sprite = data.character;
                livesI = data.lives;
                dataSetup = true;
            }
            if(data.lives != livesI)
            {
                for (int i = 0; i < lives.Length; i++)
                {
                    if (i > data.lives)
                    {
                        lives[i].gameObject.SetActive(false);
                    }
                }
                livesI = data.lives;
            }
            if(livesI < 1)
            {
                Destroy(gameObject);
            }
        }
    }
}
