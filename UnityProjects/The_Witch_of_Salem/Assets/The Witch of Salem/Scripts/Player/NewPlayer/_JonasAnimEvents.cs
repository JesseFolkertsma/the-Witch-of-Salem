using UnityEngine;
using System.Collections;

public class _JonasAnimEvents : MonoBehaviour {

    _PlayerBase pBase;

    void Start()
    {
        pBase = GetComponentInParent<_PlayerBase>();
    }

    public void StartClimbing()
    {
        pBase.StartClimbEvent();
    }

    public void StopClimbing()
    {
        pBase.StopClimbEvent();
    }
}
