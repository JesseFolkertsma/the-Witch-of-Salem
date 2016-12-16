using UnityEngine;
using System.Collections;

public class LightStone : MonoBehaviour {

    [HideInInspector]
    public bool disable = false;

    Material m;
    public bool active;
    public Color color = Color.red;

    public LightStone[] attachedLightstones;

    LightPuzzle puzzle;

    void Start()
    {
        m = GetComponentInChildren<Renderer>().material;

        if (GetComponentInParent<LightPuzzle>())
        {
            puzzle = GetComponentInParent<LightPuzzle>();
        }
        else
        {
            Debug.LogError("Cant find LightPuzzle base in parent");
        }

        Activate(false);
    }

    public void Activate(bool activateNeigbors)
    {
        if (!disable)
        {
            float e = 0;
            if (!active)
            {
                e = 20;
                active = true;
            }
            else
            {
                active = false;
            }

            puzzle.CheckIfDone();

            Color c = color * e;
            m.SetColor("_EmissionColor", c);

            if (activateNeigbors)
            {
                foreach (LightStone l in attachedLightstones)
                {
                    l.Activate(false);
                }
            }
        }
    }
}
