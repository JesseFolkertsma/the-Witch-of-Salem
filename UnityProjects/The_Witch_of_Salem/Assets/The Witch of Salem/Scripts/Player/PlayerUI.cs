using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerUI : MonoBehaviour {

    PlayerStateMachine psm;

    public GameObject heartPanel;
    public List<Image> hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Text applesUI;
    public Text arrowsUI;

    public int lives;
    int prevLives;
    public int apples;
    public int arrows;

    public void UIStart()
    {
        Image[] im = heartPanel.GetComponentsInChildren<Image>();
        for(int i = 0; i < im.Length; i++)
        {
            hearts.Add(im[i]);
        }
    }

    public void UIUpdate()
    {
        if (prevLives != psm.ps.lives)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                if (i <= lives)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
            }
        }
        prevLives = psm.ps.lives;
    }
}
