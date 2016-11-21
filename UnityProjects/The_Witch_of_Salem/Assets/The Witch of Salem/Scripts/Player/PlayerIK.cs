using UnityEngine;
using System.Collections;

public class PlayerIK : MonoBehaviour {

    Transform mouse;
    Animator anim;
    public bool useIK;
    public bool useHandIK;
    bool lookAt;
    float str;

    Vector3 bowpos1, bowpos2;
    Vector3 handPos;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UseIK(Transform m, float bodyStr)
    {
        mouse = m;
        useIK = true;
        lookAt = true;
        str = bodyStr;
    }

    public void UseHandIK(Vector3 pos,bool b)
    {
        handPos = pos;
        useHandIK = b;
    }

    void OnAnimatorIK()
    {
        if (useIK)
        {
            if (lookAt)
            {
                anim.SetLookAtWeight(1f, str, 1, 0, 0.5f);
                anim.SetLookAtPosition(mouse.position);
                lookAt = true;
            }
            useIK = false;
        }
        if (useHandIK)
        {
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            anim.SetIKPosition(AvatarIKGoal.RightHand, handPos + transform.right * .5f + Vector3.down);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, handPos + transform.right * -.5f + Vector3.down);

            //useHandIK = false;
        }
    }
}
