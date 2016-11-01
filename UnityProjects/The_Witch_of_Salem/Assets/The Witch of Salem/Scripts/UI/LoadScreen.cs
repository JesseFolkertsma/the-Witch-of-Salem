using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadScreen : MonoBehaviour {

    public Image loadbar;
    public GameObject text;
    AsyncOperation aSync;

    void Start()
    {
        loadbar.fillAmount = 0;
        text.SetActive(false);
        aSync = SceneManager.LoadSceneAsync("TestLevel");
        //aSync = SceneManager.LoadSceneAsync(GameManager.instance.currentLevel);
    }

    void Update()
    {
        loadbar.fillAmount += Time.deltaTime * .5f;
        if (aSync.progress >= 0.9f)
        {
            aSync.allowSceneActivation = false;

            if (loadbar.fillAmount == 1)
            {
                text.SetActive(true);
                if (Input.GetButtonDown("Jump"))
                {
                    aSync.allowSceneActivation = true;
                }
            }
        }
    }
}
