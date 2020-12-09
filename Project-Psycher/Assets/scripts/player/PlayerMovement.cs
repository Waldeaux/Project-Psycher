using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float rotationSpeed;
    public float recalibrateSpeed;
    public Camera cam;
    public float upperLimit;
    public float lowerLimit;

    private Vector3 camPosition;
    private Quaternion camRotation;

    KeyCode recalibrateKey = KeyCode.H;

    private enum PlayerState { 
        normal,
        recalibrate
    }

    private PlayerState currentState = PlayerState.normal;


    private Vector3 storedVelocity;

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
        if(recalibrateSpeed == 0)
        {
            recalibrateSpeed = 1;
        }
    }

    private void Update()
    {
        MoveCamera();
        switch (currentState)
        {
            case (PlayerState.recalibrate):
                Recalibrate();
                break;
            default:
                NormalMovement();
                break;
        }
    }

    private void NormalMovement()
    {
        Vector3 planarMovement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        Vector3 upMovement = Vector3.Project(rb.velocity, transform.up) - transform.up * 9.8f * Time.deltaTime;
        rb.velocity = upMovement + planarMovement;

        rb.rotation *= Quaternion.AngleAxis(rotationSpeed * Input.GetAxis("Mouse X"), cam.transform.InverseTransformDirection(transform.up));

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
        if (Input.GetKeyDown(recalibrateKey))
        {
            storedVelocity = rb.velocity;
            rb.velocity = Vector3.zero;
            currentState = PlayerState.recalibrate;
            camPosition = new Vector3(0,10, 0);
            camRotation = Quaternion.Euler(90, 0, 0);
        }
    }

    private void Recalibrate()
    {

        if (!Input.GetKey(recalibrateKey))
        {
            rb.velocity = storedVelocity;
            currentState = PlayerState.normal;
            camPosition = Vector3.zero;
            camRotation = Quaternion.Euler(Vector3.zero);
            NormalMovement();
            return;
        }
        Quaternion rotation = Quaternion.AngleAxis(recalibrateSpeed * Input.GetAxis("Vertical") * Time.deltaTime, transform.InverseTransformDirection(transform.right));
            rotation *= Quaternion.AngleAxis(-recalibrateSpeed * Time.deltaTime * Input.GetAxis("Horizontal"), transform.InverseTransformDirection(transform.forward));
        transform.rotation *= rotation;
    }

    private void MoveCamera()
    {
        Vector3 differenceVector = (camPosition - cam.transform.localPosition);
        cam.transform.localPosition += differenceVector.normalized * Mathf.Min(differenceVector.magnitude, 25*Time.deltaTime);

        float xDifference = camRotation.eulerAngles.x - cam.transform.localRotation.eulerAngles.x;
        float newX = cam.transform.localRotation.eulerAngles.x + Mathf.Min(xDifference, camRotation.eulerAngles.x * 25 * Time.deltaTime);
        cam.transform.localRotation = Quaternion.Euler(newX, 0, 0);
    }

    
}
