using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointRotation : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.O))
        {
            rb.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}
