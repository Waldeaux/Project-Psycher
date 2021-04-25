using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    
    void Update()
    {
        transform.RotateAround(transform.up, Input.GetAxis("Mouse X") * Time.deltaTime * 90);
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Rotate(transform.forward, -45);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(transform.forward, 45);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Rotate(transform.right, 45);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(transform.right, -45);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = Snap(rotation.x, 90f);
            rotation.z = Snap(rotation.z, 90f);
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    float Snap(float input, float interval)
    {
        float remainder = (input % interval);
        if(remainder < interval / 2)
        {
            return input - remainder;
        }
        return input - remainder + interval;
    }
}
