using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectMissile : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(new Vector3(-Time.deltaTime * 9.8f, 0), ForceMode.Acceleration);
    }
}
