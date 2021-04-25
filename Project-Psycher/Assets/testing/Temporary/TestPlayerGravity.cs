using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerGravity : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 gravity;
    public bool debug;

    private enum GravityMode
    {
        standard,
        jumping
    }

    private GravityMode currentMode;
    public GameObject target;
    public GameObject indicator;
    private Vector3 targetUp;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = -transform.up;
        indicator.transform.SetParent(null);
    }

    void Update()
    {
        switch (currentMode)
        {
            case (GravityMode.standard):
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    indicator.SetActive(true);
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    indicator.SetActive(false);
                }
                break;
            case (GravityMode.jumping):
                gravity = (target.transform.position - transform.position).normalized;
                Correction();
                break;
        }
        rb.AddForce(gravity * Time.deltaTime * 9.8f, ForceMode.VelocityChange);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(transform.up * 10, ForceMode.VelocityChange);
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, target.transform.position - transform.position, out hitInfo);
            targetUp = hitInfo.normal;
            SwitchToJumping();
        }
    }

    void SwitchToStandard()
    {
        gravity = -transform.up;
        currentMode = GravityMode.standard;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void UpdateIndicator()
    {
    }
    void SwitchToJumping()
    {
        currentMode = GravityMode.jumping;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentMode == GravityMode.jumping)
        {
            SwitchToStandard();
        }
    }

    private void Correction()
    {
        Vector3 rotationAxis = Vector3.Cross(targetUp, transform.up);
        rotationAxis = -rotationAxis;
        float angle = Vector3.Angle(targetUp, transform.up);
        transform.Rotate(rotationAxis.normalized * Mathf.Min(angle, Time.deltaTime * 90));
    }
}
