using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float rotationSpeed;
    public Camera cam;
    public float upperLimit;
    public float lowerLimit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(speed == 0)
        {
            speed = 1;
        }
        if(rotationSpeed == 0)
        {
            rotationSpeed = 1;
        }
    }

    private void Update()
    {
        Vector3 planarMovement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        Vector3 upMovement = Vector3.Project(rb.velocity, transform.up);
        rb.velocity = upMovement + planarMovement;

        rb.rotation *= Quaternion.AngleAxis(rotationSpeed * Input.GetAxis("Mouse X"), transform.up);

        cam.transform.rotation *= Quaternion.AngleAxis(rotationSpeed * -Input.GetAxis("Mouse Y"), cam.transform.InverseTransformDirection(cam.transform.right));
        Vector3 cross = Vector3.Cross(cam.transform.forward, rb.transform.forward);
        float dot = Vector3.Dot(rb.transform.right, cross);
        if (dot != 0)
        {
            bool aboveParallel = dot >= 0;
            if (aboveParallel)
            {
                cam.transform.localRotation = Quaternion.Euler(Mathf.Max(cam.transform.localRotation.eulerAngles.x, 360 - upperLimit), 0, 0);
            }
            else
            {
                cam.transform.localRotation = Quaternion.Euler(Mathf.Min(cam.transform.localRotation.eulerAngles.x, lowerLimit), 0, 0);
            }
        }
    }

    
}
