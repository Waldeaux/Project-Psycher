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

    public bool relativeVelocity;

    public bool easeRecalibrate;

    KeyCode recalibrateKey = KeyCode.H;
    KeyCode jumpKey = KeyCode.Space;

    private enum PlayerState { 
        normal,
        recalibrate
    }

    private PlayerState currentState = PlayerState.normal;

    private Quaternion targetCalibrationRotation;

    private Vector3 jumpCastExtents = new Vector3(.5f, .1f, .5f);


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

    public void ScrapperUpdate()
    {

        switch (currentState)
        {
            case (PlayerState.recalibrate):
                MoveCamera();
                OneTimeRecalibrate();
                break;
            default:
                NormalMovement();
                break;
        }
    }
    public void ScrapperFixedUpdate()
    {
        if (Input.GetKey(jumpKey))
        {
            Jump();
        }
    }

    private void NormalMovement()
    {
        Vector3 planarMovement = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
        Vector3 upMovement = Vector3.Project(rb.velocity, transform.up) - transform.up * 9.8f * Time.deltaTime;
        rb.velocity = upMovement + planarMovement;

        rb.rotation *= Quaternion.AngleAxis(rotationSpeed * Input.GetAxis("Mouse X"), transform.InverseTransformDirection(transform.up));

        cam.transform.rotation *= Quaternion.AngleAxis(rotationSpeed * -Input.GetAxis("Mouse Y"), cam.transform.InverseTransformDirection(cam.transform.right));
        Vector3 cross = Vector3.Cross(cam.transform.forward, rb.transform.forward);
        float dot = Vector3.Dot(rb.transform.right, cross);
        if (dot != 0)
        {
            bool aboveParallel = dot >= 0;
            if (aboveParallel)
            {
                if (cam.transform.localRotation.eulerAngles.x > 0)
                {
                    cam.transform.localRotation = Quaternion.Euler(Mathf.Max(cam.transform.localRotation.eulerAngles.x, 360 - upperLimit), 0, 0);
                }
            }
            else
            {
                cam.transform.localRotation = Quaternion.Euler(Mathf.Min(cam.transform.localRotation.eulerAngles.x, lowerLimit), 0, 0);
            }
        }
        if (Input.GetKeyDown(recalibrateKey))
        {
            SwitchToRecalibrate();
        }
    }

    private void Jump()
    {
        print("jump");
        if(Physics.BoxCast(transform.position, jumpCastExtents, -transform.up, transform.rotation, .9f))
        {
            print("jump successful");
            Vector3 currentUp = Vector3.Project(rb.velocity, transform.up);
            rb.AddForce(transform.up * (10 - currentUp.magnitude), ForceMode.VelocityChange);
        }
    }
    private void SwitchToNormal()
    {
        rb.velocity = storedVelocity;
        currentState = PlayerState.normal;
        camPosition = Vector3.zero;
        camRotation = Quaternion.Euler(Vector3.zero);
        NormalMovement();
    }

    private void SwitchToRecalibrate()
    {
        storedVelocity = rb.velocity;
        rb.velocity = Vector3.zero;
        camPosition = new Vector3(0, 10, 0);
        camRotation = Quaternion.Euler(90, 0, 0);
        targetCalibrationRotation = transform.rotation;

        currentState = PlayerState.recalibrate;
    }
    private void OneTimeRecalibrate()
    {
        if (!Input.GetKey(recalibrateKey))
        {
            SwitchToNormal();
            return;
        }
        if (easeRecalibrate)
        {
            EaseRecalibrationRotation();
        }
        else
        {
            ConstantRecalibrationRotation();
        }
        if(Quaternion.Angle(transform.rotation, targetCalibrationRotation) <= 1)
        {
            float verticalInput = Input.GetAxis("Vertical");
            float horizontalInput = Input.GetAxis("Horizontal");
            int forwardRotation = (int)(Mathf.Abs(horizontalInput) / horizontalInput) * 90;
            int rightRotation = -(int)(Mathf.Abs(verticalInput) / verticalInput) * 90;
            targetCalibrationRotation *= Quaternion.AngleAxis(forwardRotation, transform.InverseTransformDirection(transform.forward));
            targetCalibrationRotation *= Quaternion.AngleAxis(rightRotation, transform.InverseTransformDirection(transform.right));
        }
    }


    private void ConstantRecalibrationRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCalibrationRotation, Mathf.Min(1, (Time.deltaTime * recalibrateSpeed) / Quaternion.Angle(transform.rotation, targetCalibrationRotation)));
    }

    private void EaseRecalibrationRotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCalibrationRotation, Mathf.Min(Time.deltaTime * 10, 1));
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
