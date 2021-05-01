using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommands : NetworkBehaviour
{
    [ClientRpc]
    public void RpcSwitchToSpectate(GameObject player)
    {
        Player playerScript = GetComponent<Player>();
        if (playerScript)
        {
            playerScript.SwitchToCameraSpectate(player);
        }
    }

    [Command]
    public void CmdRemovePlayer()
    {
        GameControl gameController = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameControl>();
        if (gameController)
        {
            gameController.CmdRemovePlayer(this.gameObject);
        }
    }
}
