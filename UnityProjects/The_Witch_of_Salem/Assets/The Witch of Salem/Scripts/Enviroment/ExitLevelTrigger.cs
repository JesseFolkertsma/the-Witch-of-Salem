using UnityEngine;
using System.Collections;

public class ExitLevelTrigger : MonoBehaviour {

    public int nextLevelBuildIndex;

    void ChangeLevel()
    {
        _GameManager.instance.SaveGame();
        _GameManager.instance.InstantiateLoad();

        Invoke("ChangeLevelFollowup", 3.5f);
    }

    void ChangeLevelFollowup()
    {
        _GameManager.instance.LoadOtherLevelWithSave(nextLevelBuildIndex);
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            ChangeLevel();
        }
    }
}
