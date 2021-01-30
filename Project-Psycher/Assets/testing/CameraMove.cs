using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float speed;
    void Update()
    {
        transform.position += (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * Time.deltaTime * speed;
        Vector3 adjustVector = Vector3.zero;
        if (transform.position.x > 125)
        {
            adjustVector.x = -250;
        }
        else if (transform.position.x < -125)
        {
            adjustVector.x = 250;
        }

        if (transform.position.z > 200)
        {
            adjustVector.z = -400;
        }
        else if (transform.position.z < -200)
        {
            adjustVector.z = 400;
        }

        transform.position += adjustVector;
    }
}
