using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwitchSceneOnTrigger : MonoBehaviour {

    public int sceneNumber;

	void OnTriggerEnter(Collider col)
    {
        if(col.attachedRigidbody != null)
        {
            if(col.attachedRigidbody.tag == "Player")
            {
                SceneManager.LoadScene(sceneNumber);
            }
        }
    }
}
