using UnityEngine;
using System.Collections;

public class AnimationEventHandler : MonoBehaviour {

    PlayerStateMachine psm;

    void Start()
    {
        psm = GetComponentInParent<PlayerStateMachine>();
    }

    public void CheckForHit()
    {
        psm.pc.CheckForHit();
    }

    public void StopCheckForHit()
    {
        psm.pc.StopCheckForHit();
    }

    public void ActivateWait()
    {
        psm.pc.ActivateWait();
    }

}
