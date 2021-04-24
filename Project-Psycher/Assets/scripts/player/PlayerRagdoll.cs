using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdoll : MonoBehaviour
{
    private enum State { player, correcting, ragdoll }
    private State currentState = State.player;

    private float ragdollTimer = 0;
    private Rigidbody rb;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void RagdollUpdate()
    {
        switch (currentState)
        {
            case (State.correcting):
                Vector3 rotationAxis = Vector3.Cross(Vector3.up, transform.up);
                rotationAxis = -rotationAxis;
                float angle = Vector3.Angle(Vector3.up, transform.up);
                transform.Rotate(rotationAxis.normalized * Mathf.Min(angle, Time.deltaTime * 90));
                if (angle.Equals(0))
                {
                    EndRagdoll();
                }
                break;
            case (State.ragdoll):
                ragdollTimer += Time.deltaTime;
                if (ragdollTimer > 5)
                {
                    StartCorrecting();
                }
                break;
        }
    }

    public void StartRagdoll()
    {
        print("start ragdoll");
        ragdollTimer = 0;
        currentState = State.ragdoll;
        rb.constraints = RigidbodyConstraints.None;
    }

    public void StartCorrecting()
    {
        ragdollTimer = 0;
        currentState = State.correcting;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        player.StartCorrecting();
    }
    public void EndRagdoll()
    {
        currentState = State.player;
        player.EndRagdoll();
    }
}
