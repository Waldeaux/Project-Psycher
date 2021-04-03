using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    RivetManagement rivetManagement;
    Camera cam;
    KeyCode shootRivet = KeyCode.E;
    KeyCode clearRivets = KeyCode.R;
    PlayerMovement playerMovement;

    [SyncVar(hook = "NetworkEnabled")]
    public bool networkEnabled;

    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
        cam = GetComponentInChildren<Camera>();
        playerMovement = GetComponent<PlayerMovement>();
        if (!isLocalPlayer)
        {
            ClearNonPlayerComponents();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkEnabled|| !hasAuthority)
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
    }

    void ShootRivet()
    {
        if (rivetManagement)
        {
            rivetManagement.ShootRivet(cam.transform.forward, cam.transform.position);
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
