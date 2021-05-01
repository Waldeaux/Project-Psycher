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
    PlayerUI playerUI;
    Rigidbody rb;
    PlayerCamera playerCamera;
    GameControl gameControl;

    int currentSpectatePlayer = 0;

    [SyncVar(hook = "NetworkEnabled")]
    public bool networkEnabled;

    private enum State { player, ragdoll, jumping, dead}

    private State currentState = State.player;

    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
        cam = GetComponentInChildren<Camera>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerCamera = GetComponent<PlayerCamera>();
        playerJumping = GetComponent<PlayerJumping>();
        playerUI = GetComponent<PlayerUI>();
        rb = GetComponent<Rigidbody>();
        GameObject gameControlObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControlObject)
        {
            gameControl = gameControlObject.GetComponent<GameControl>();
        }
        if (!isLocalPlayer)
        {
            ClearNonPlayerComponents();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkEnabled || !hasAuthority)
        {
            return;
        }
        switch (currentState)
        {
            case (State.player):
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

            case (State.dead):
                if (gameControl)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        currentSpectatePlayer++;
                        playerCamera.SwitchSpectate(gameControl.FindNextPlayer(ref currentSpectatePlayer));
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        currentSpectatePlayer--;
                        playerCamera.SwitchSpectate(gameControl.FindNextPlayer(ref currentSpectatePlayer));
                    }
                }
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

    public void SwitchToDead()
    {
        currentState = State.dead;
        rb.constraints = RigidbodyConstraints.None;
        SwitchToCameraSpectate();
    }

    public void SwitchToCameraSpectate()
    {
        currentSpectatePlayer = 0;
        playerCamera.SwitchToSpectate();
        playerCamera.SwitchSpectate(gameControl.FindNextPlayer(ref currentSpectatePlayer));
    }
    public void SwitchToCameraSpectate(GameObject player)
    {
        currentSpectatePlayer = 0;
        playerCamera.SwitchToSpectate();
        playerCamera.SwitchSpectate(player);
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
        Destroy(playerJumping);
        Destroy(playerCamera);
        Destroy(playerUI);
        Destroy(playerRagdoll);
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MapBoundary"))
        {
            other.GetComponent<MapBoundaries>().RemovePlayer(this.gameObject);
            this.SwitchToDead();
        }
    }
}
