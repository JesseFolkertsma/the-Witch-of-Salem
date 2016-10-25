using UnityEngine;
using System.Collections;

public class IKTest : MonoBehaviour {

    Animator anim;

    public Transform box;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnAnimatorIK()
    {
        anim.SetLookAtWeight(1, .5f, 1, 0, .5f);
        anim.SetLookAtPosition(box.position);
    }
}
