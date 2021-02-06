using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalHole : MonoBehaviour
{
    private Rigidbody rb;
    private int frameCountDown;
    public GameObject portalPrefab;
    public PortalCamera cam;
    public GameObject currentPortal;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Destroy(this);
        }
    }
    private void Update()
    {

        //Spawns a portal where the player should be 2 second from now
        if (Input.GetKeyDown(KeyCode.S))
        {
            int frameCount = Mathf.CeilToInt(2f / Time.fixedDeltaTime);
            Vector3 velocity;
            Vector3 newPosition = SimpleSimulate(frameCount, out velocity);
            currentPortal = GameObject.Instantiate(portalPrefab);
            currentPortal.transform.position = newPosition;
            currentPortal.transform.rotation = Quaternion.LookRotation(velocity.normalized);
            GameObject secondPortal = GameObject.Instantiate(portalPrefab);
            secondPortal.transform.position = new Vector3(30, 10, 10);
            secondPortal.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            SelectivePortal firstPortalScript = currentPortal.GetComponentInChildren<SelectivePortal>();
            Portal secondPortalScript = secondPortal.GetComponentInChildren<Portal>();
            if(firstPortalScript && secondPortalScript)
            {
                firstPortalScript.linkedPortal = secondPortalScript;
                secondPortalScript.linkedPortal = firstPortalScript;
                cam.AddPortal(firstPortalScript);
                cam.AddPortal(secondPortalScript);
                firstPortalScript.allowedObject = this.gameObject;
            }
            else
            {
                Destroy(currentPortal);
                Destroy(secondPortal);
            }
        }
    }

    public void OffsetPortal(Vector3 offset)
    {
        if (!currentPortal)
        {
            return;
        }
        currentPortal.transform.position += offset;
    }
    public Vector3 SimpleSimulate(int frames, out Vector3 velocityOutput)
    {
        Vector3 velocity = rb.velocity;
        Vector3 position = rb.position;
        for(int x = 0; x < frames - 1; x++)
        {
            position += velocity * Time.fixedDeltaTime;
            if (rb.useGravity)
            {
                velocity += Physics.gravity*Time.fixedDeltaTime;
            }
        }
        frameCountDown = frames;
        velocityOutput = velocity;
        return position;
    }
}
