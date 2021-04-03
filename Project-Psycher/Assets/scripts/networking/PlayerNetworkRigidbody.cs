using Mirror;
using UnityEngine;

public class PlayerNetworkRigidbody : NetworkBehaviour
{
    [SyncVar]
    public bool networkEnabled;

    [SyncVar(hook = "NetworkKinematic")]
    public bool networkKinematic;

    [SyncVar]
    public Vector3 position;

    [SyncVar]
    public Vector3 velocity;

    [SyncVar]
    public Quaternion rotation;

    [SyncVar]
    public Vector3 angularVelocity;

    [SyncVar(hook = "LogNetworkRb")]
    public NetworkRigidbodyStruct networkRb;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        networkKinematic = rb.isKinematic;
        networkRb = new NetworkRigidbodyStruct(
            rb.position,
            rb.rotation,
            rb.velocity,
            rb.angularVelocity);
    }

    private void FixedUpdate()
    {
        if (networkKinematic)
        {
            return;
        }
        if (isLocalPlayer)
        {
            CmdUpdateNetworkRigidbody(rb.position, rb.rotation, rb.velocity, rb.angularVelocity);
        }
        else
        {
            float scale = .9f;
            rb.position = networkRb.position;
            rb.rotation = networkRb.rotation;
            rb.velocity = networkRb.velocity;
            rb.angularVelocity = networkRb.angularVelocity;
        }
    }

    private void LogNetworkRb(NetworkRigidbodyStruct oldValue, NetworkRigidbodyStruct newValue)
    {
    }
    private void NetworkKinematic(bool oldValue, bool newValue)
    {
        if (rb)
        {
            rb.isKinematic = newValue;
        }
    }

    [Command]
    private void CmdUpdateNetworkRigidbody(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
    {
        networkRb = new NetworkRigidbodyStruct(
            position,
            rotation,
            velocity,
            angularVelocity);
    }

    public struct NetworkRigidbodyStruct
    {
        public NetworkRigidbodyStruct(
            Vector3 position,
            Quaternion rotation,
            Vector3 velocity,
            Vector3 angularVelocity
            )
        {
            this.position = position;
            this.rotation = rotation;
            this.velocity = velocity;
            this.angularVelocity = angularVelocity;
        }
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }
}
