using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectTester : MonoBehaviour
{
    public GameObject disconnectMissilePrefab;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject spawn = GameObject.Instantiate(disconnectMissilePrefab);
            spawn.transform.position = this.transform.position;
            spawn.GetComponent<Rigidbody>().AddForce(new Vector3(-2000, 0), ForceMode.Acceleration);
        }
    }
}

