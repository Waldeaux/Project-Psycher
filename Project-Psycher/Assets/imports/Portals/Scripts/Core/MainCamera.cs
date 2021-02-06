using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

    protected List<Portal> portals;
    void Awake () {
        portals = new List<Portal>(FindObjectsOfType<Portal>());
    }

    void OnPreCull () {
        PreCullLogic();
    }

    public virtual void PreCullLogic()
    {
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].PrePortalRender();
        }
        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].Render();
        }

        for (int i = 0; i < portals.Count; i++)
        {
            portals[i].PostPortalRender();
        }
    }

}