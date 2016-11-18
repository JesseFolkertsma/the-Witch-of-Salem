using UnityEngine;
using System.Collections;

public class _PlayerIKHandler : MonoBehaviour {
    _PlayerBase pBase;

    void Start()
    {
        pBase = GetComponentInParent<_PlayerBase>();
    }

    void OnAnimatorIK()
    {
    }
}
