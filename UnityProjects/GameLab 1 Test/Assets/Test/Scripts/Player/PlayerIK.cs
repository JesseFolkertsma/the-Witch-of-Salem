using UnityEngine;
using System.Collections;

public class PlayerIK : MonoBehaviour
{
    Animator anim;
    public bool useIK = false;
    public Vector3 hPos;
    public static PlayerIK instance;

    void Start()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }

    public void UseIK(Vector3 pos)
    {
        hPos = pos;
        useIK = true;
    }

    void OnAnimatorIK()
    {
        if (useIK)
        {
            //Righthand
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.RightHand, hPos);

            //Lefthand
            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, hPos);
        }
    }
}