using UnityEngine;
using System.Collections;

public class _PlayerIKHandler : MonoBehaviour {

    _PlayerBase pBase;
    bool useIKHands;
    bool useIKLookat;

    Vector3 holdPosition;
    Vector3 lookatPosition;

    float bodyWeight;

    void Start()
    {
        pBase = GetComponentInParent<_PlayerBase>();
    }

    public void UseIKLookat(Vector3 lookPos, float weight)
    {
        lookatPosition = lookPos;
        bodyWeight = weight;
        useIKLookat = true;
    }

    public void UseIKHands(Vector3 holdPos)
    {
        holdPosition = holdPos;
        useIKHands = true;
    }

    void OnAnimatorIK()
    {
        if (useIKHands)
        {
            pBase.anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            pBase.anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

            pBase.anim.SetIKPosition(AvatarIKGoal.RightHand, holdPosition + Vector3.forward * 0.2f);
            pBase.anim.SetIKPosition(AvatarIKGoal.LeftHand, holdPosition + Vector3.forward * -0.2f);
            useIKHands = false;
        }

        if (useIKLookat)
        {
            pBase.anim.SetLookAtWeight(1, bodyWeight, 1, 1, .5f);
            pBase.anim.SetLookAtPosition(lookatPosition);
            useIKLookat = false;
        }
    }
}
