using UnityEngine;
using System.Collections;

public class PlayerIK : MonoBehaviour {

    Transform mouse;
    Animator anim;
    public bool useIK;
    public bool useBow;
    bool lookAt;
    float str;

    Vector3 bowpos1, bowpos2;

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

    void OnAnimatorIK()
    {
        if (useIK == true)
        {
            if (lookAt == true)
            {
                anim.SetLookAtWeight(1f, str, 1, 0, 0.5f);
                anim.SetLookAtPosition(mouse.position);
                lookAt = true;
            }
            useIK = false;
        }
    }
}
