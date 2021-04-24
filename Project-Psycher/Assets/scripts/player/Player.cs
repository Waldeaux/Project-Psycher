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
    PlayerCamera playerCamera;

    [SyncVar(hook = "NetworkEnabled")]
    public bool networkEnabled;

    private enum State { player, ragdoll}

    private State currentState = State.player;

    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
        cam = GetComponentInChildren<Camera>();
        playerMovement = GetComponent<PlayerMovement>();
        playerRagdoll = GetComponent<PlayerRagdoll>();
        playerCamera = GetComponent<PlayerCamera>();
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
        currentState = State.ragdoll;
        playerRagdoll.StartRagdoll();
        playerCamera.SwitchToFollow();
    }

    public void EndRagdoll()
    {
        currentState = State.player;
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
}
