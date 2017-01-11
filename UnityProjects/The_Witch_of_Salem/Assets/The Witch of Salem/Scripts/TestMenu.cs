using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TestMenu : MonoBehaviour {

    void Start()
    {
        print(Application.persistentDataPath);
    }

    public void NewGameButton()
    {
        //SceneManager.LoadScene(0);
        //GameManager.instance.slSystem.NewGame();
    }

    public void LoadGameButton()
    {
        //SceneManager.LoadScene(0);
        //GameManager.instance.slSystem.LoadScene("Jonas");
    }
}
