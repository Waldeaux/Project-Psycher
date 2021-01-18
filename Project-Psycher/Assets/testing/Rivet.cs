using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rivet : MonoBehaviour
{
    public GameObject target;
    public Rigidbody rb;
    void Update()
    {
        Vector3 gravityVector = target.transform.position - transform.position;
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }
}
