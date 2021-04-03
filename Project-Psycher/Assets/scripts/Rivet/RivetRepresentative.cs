using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetRepresentative : MonoBehaviour
{
    public GameObject indicatorPrefab;
    public GameObject indicatorInstance;

    public void CreateInstance(GameObject paired)
    {
        indicatorInstance = GameObject.Instantiate(indicatorPrefab);
        RivetIndicator indicator = indicatorInstance.GetComponent<RivetIndicator>();
        indicator.rivet1 = this.gameObject;
        indicator.rivet2 = paired;
    }
    public void OnDestroy()
    {
        if (indicatorInstance)
        {
            Destroy(indicatorInstance);
        }
    }
}
