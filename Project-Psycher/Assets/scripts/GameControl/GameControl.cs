using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : NetworkBehaviour
{

    public List<GameObject> currentPlayers;
    public Text globalText;
    public GameObject globalTextParent;

    private void Start()
    {
        //currentPlayers = new List<GameObject>();
    }

    public void AddPlayer(GameObject player)
    {
        player.GetComponent<Player>().networkEnabled = true;
        player.GetComponent<PlayerNetworkRigidbody>().networkKinematic = false;
        currentPlayers.Add(player);
    }

    public void RemovePlayer(GameObject player)
    {
        print(player);
        print(isServer);
        if (isServer)
        {
            HandleRemovePlayer(player);
        }
        else
        {
            CmdRemovePlayer(player);
        }
    }

    [Command]
    public void CmdRemovePlayer(GameObject player)
    {
        print(currentPlayers.Contains(player));
        HandleRemovePlayer(player);
        
    }

    public void HandleRemovePlayer(GameObject player)
    {
        currentPlayers.Remove(player);
        if (currentPlayers.Count == 1)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        RpcGlobalTextUpdate(currentPlayers[0].GetComponent<PlayerInfo>().name + " wins!");
        //currentPlayers[0].GetComponent<PlayerCommands>().RpcSwitchToSpectate(currentPlayers[0]);
        RpcGlobalTextParentToggle(true);
    }

    [ClientRpc]
    public void RpcGlobalTextParentToggle(bool active)
    {
        globalTextParent.SetActive(active);
    }
    [ClientRpc]
    public void RpcGlobalTextUpdate(string text)
    {
        globalText.text = text;
    }
    public void DrawGame()
    {

    }

    public GameObject FindNextPlayer(ref int index)
    {
        if(currentPlayers.Count > 0)
        {
            if(currentPlayers.Count <= index)
            {
                index = 0;
            }
            else if(index < 0)
            {
                index = currentPlayers.Count - 1;
            }
            return currentPlayers[index];
        }
        return null;
    }
}
