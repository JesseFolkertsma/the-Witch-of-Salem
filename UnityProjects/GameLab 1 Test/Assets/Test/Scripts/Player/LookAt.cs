using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

    PlayerManager pm;

    public Transform backBone;

    public bool look;

    void Start()
    {
        pm = GetComponent<PlayerManager>();
        backBone = GameObject.Find("Spine1").transform;
    }

    void LateUpdate()
    {
        if (look)
        {
            LookAtMouse();
        }
    }

    public void LookAtMouse()
    {
        pm.playerMiddle.LookAt(pm.lp.transform.position);
        //backBone.rotation = Quaternion.Lerp(backBone.rotation, pm.playerMiddle.rotation, .1f);
        backBone.rotation = pm.playerMiddle.rotation;
    }

    public bool IsRight()
    {
        bool isRight;
        if (pm.lp.transform.position.x < transform.position.x)
        {
            isRight = false;
        }
        else
        {
            isRight = true;
        }
        return isRight;
    }
}
