using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    RivetManagement rivetManagement;
    Camera cam;
    KeyCode shootRivet = KeyCode.E;
    KeyCode clearRivets = KeyCode.R;
    PlayerMovement playerMovement;
    PlayerRagdoll playerRagdoll;
    PlayerJumping playerJumping;
    Rigidbody rb;
    PlayerCamera playerCamera;

    [SyncVar(hook = "NetworkEnabled")]
    public bool networkEnabled;

    private enum State { player, ragdoll, jumping}

    private State currentState = State.player;

    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
        cam = GetComponentInChildren<Camera>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerCamera = GetComponent<PlayerCamera>();
        playerJumping = GetComponent<PlayerJumping>();
        rb = GetComponent<Rigidbody>();
        if (!isLocalPlayer)
        {
            ClearNonPlayerComponents();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case (State.player):
                if (!networkEnabled || !hasAuthority)
                {
                    return;
                }
                if (playerMovement)
                {
                    playerMovement.ScrapperUpdate();
                }
                if (Input.GetKeyDown(shootRivet))
                {
                    ShootRivet();
                }
                if (Input.GetKeyDown(clearRivets))
                {
                    ClearRivets();
                }
                break;

            case (State.ragdoll):
                RagdollUpdate();
                break;
        }
    }

    public void StartRagdoll()
    {
        print("start ragdoll");
        currentState = State.ragdoll;
        playerRagdoll.StartRagdoll();
        playerCamera.SwitchToFollow();
    }

    public void SwitchToPlayer()
    {
        currentState = State.player;
        playerJumping.SwitchToStandard();
    }

    public void SwitchToJumping()
    {
        currentState = State.jumping;
    }

    public void StartCorrecting()
    {
        playerCamera.SwitchToImmobile();
    }
    void ShootRivet()
    {
        if (rivetManagement)
        {
            rivetManagement.ShootRivet(cam.transform.forward, cam.transform.position);
        }
    }

    void RagdollUpdate()
    {
        if (playerRagdoll)
        {
            playerRagdoll.RagdollUpdate();
        }
    }
    void ClearRivets()
    {
        if (rivetManagement)
        {
            rivetManagement.CmdDestroyAllRivets();
        }
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (playerMovement)
        {
            playerMovement.ScrapperFixedUpdate();
        }
    }

    private void NetworkEnabled(bool oldEnabled, bool newEnabled)
    {
        this.enabled = newEnabled;
    }

    
    [ClientRpc]
    public void RpcClearNonPlayerComponents()
    {
        ClearNonPlayerComponents();
    }

    private void ClearNonPlayerComponents()
    {
        if (isLocalPlayer)
        {
            return;
        }
        Destroy(cam.gameObject);
        Destroy(playerMovement);
        //Destroy(rivetManagement);
        Destroy(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody &&  Vector3.Dot(-transform.up, Vector3.Project(-transform.up, (transform.position - collision.contacts[0].point))) < 0 && (collision.rigidbody.mass * collision.relativeVelocity).magnitude >= 100)
            {
                StartRagdoll();
            }
        else if (currentState == State.jumping)
        {
            SwitchToPlayer();
        }
    }
}
