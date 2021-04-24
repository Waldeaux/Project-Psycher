using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVelocity : MonoBehaviour
{
    void Start()
    {
        GetComponent<Rigidbody>().velocity = -Vector3.forward * 20;
    }

}
