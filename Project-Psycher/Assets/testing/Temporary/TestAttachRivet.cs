using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttachRivet : MonoBehaviour
{
    public GameObject rivetPrefab;
    public GameObject target;
    public GameObject target2;

    private void Start()
    {
        GameObject rivet = GameObject.Instantiate(rivetPrefab);
        Vector3 position = target.transform.position + target.transform.up * .5f;
        Vector3 normal = Vector3.up;
        RivetFunctions.CreateRivet(position, normal, target, rivet, false, null);
        GameObject rivet2 = GameObject.Instantiate(rivetPrefab);
        Vector3 position2 = target2.transform.position + target2.transform.up * .25f;
        Vector3 normal2 = Vector3.up;
        RivetFunctions.CreateRivet(position2, normal2, target2, rivet2, true, rivet.GetComponent<Rivet>());
    }
}
