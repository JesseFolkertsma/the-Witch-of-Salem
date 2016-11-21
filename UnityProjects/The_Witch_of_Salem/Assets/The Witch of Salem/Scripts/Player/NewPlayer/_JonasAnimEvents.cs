using UnityEngine;
using System.Collections;

public class _JonasAnimEvents : MonoBehaviour {

    _Player pBase;

    void Start()
    {
        pBase = GetComponentInParent<_Player>();
    }

    public void StartClimbing()
    {
        pBase.StartClimbEvent();
    }

    public void StopClimbing()
    {
        pBase.StopClimbEvent();
    }

    public void CheckForHit()
    {
        pBase.StartAttackEvent();
    }

    public void StopCheckForHit()
    {
        pBase.StopAttackEvent();
    }

    public void ActivateWait()
    {
        pBase.WaitForNextAttackEvent();
    }
}
