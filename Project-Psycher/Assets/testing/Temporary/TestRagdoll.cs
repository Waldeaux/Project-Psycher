using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRagdoll : MonoBehaviour
{
    private enum State { player, ragdoll}
    private State currentState = State.player;

    private float ragdollTimer = 0;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SwitchToPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (State.player):
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SwitchToRagdoll();
                }
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, transform.up);
                rotationAxis = -rotationAxis;
                transform.Rotate(rotationAxis.normalized*Mathf.Min(Vector3.Angle(Vector3.up, transform.up),Time.deltaTime*90));
                break;
            case (State.ragdoll):

                if (Input.GetKeyDown(KeyCode.R))
                {
                    SwitchToPlayer();
                }
                ragdollTimer += Time.deltaTime;
                if(ragdollTimer > 5)
                {
                    SwitchToPlayer();
                }
                break;
        }
    }

    void SwitchToRagdoll()
    {
        ragdollTimer = 0;
        currentState = State.ragdoll;
        rb.constraints = RigidbodyConstraints.None;
    }

    void SwitchToPlayer()
    {
        ragdollTimer = 0;
        currentState = State.player;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print(collision.relativeVelocity);
        print(collision.rigidbody);
        if (collision.rigidbody)
        {
            if ((collision.rigidbody.mass * collision.relativeVelocity).magnitude >= 100)
            {
                SwitchToRagdoll();
            }
        }
    }
}
