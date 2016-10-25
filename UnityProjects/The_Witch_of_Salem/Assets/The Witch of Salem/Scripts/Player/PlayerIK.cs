using UnityEngine;
using System.Collections;

public class PlayerIK : MonoBehaviour {

    Transform mouse;
    Animator anim;
    public bool useIK;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void UseIK(Transform m)
    {
        mouse = m;
        useIK = true;
    }

    void OnAnimatorIK()
    {
        if (useIK == true)
        {
            anim.SetLookAtWeight(1f, 0.3f, 1, 0, 0.5f);
            anim.SetLookAtPosition(mouse.position);
            useIK = false;
        }
    }
}
