using UnityEngine;
using System.Collections;

public class LightStone : MonoBehaviour {

    Material m;
    public bool active;
    public Color color = Color.red;

    public LightStone[] attachedLightstones;

    void Start()
    {
        m = GetComponentInChildren<Renderer>().material;
        Activate(false);
    }

    public void Activate(bool activateNeigbors)
    {
        float e = 0;
        if (active)
        {
            e = 1;
            active = false;
        }
        else
        {
            active = true;
        }

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
