using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperLobby : NetworkBehaviour
{
    public GameObject entryObject;
    public Matchmaking matchmaking;
    List<PlayerLobbyEntry> entries;
    private void Start()
    {
        entries = new List<PlayerLobbyEntry>();
    }

    public void AddUser(int connId)
    {
        GameObject entryInst = GameObject.Instantiate(entryObject);
        NetworkServer.Spawn(entryInst);
        PlayerLobbyEntry entryScript = entryInst.GetComponent<PlayerLobbyEntry>();
        entries.Add(entryScript);
        entryScript.connId = connId;
        string newPlayerName = "";
        if (matchmaking.currentLobby.IsValid())
        {
            //Get new player's name
            int numPlayers = SteamMatchmaking.GetNumLobbyMembers(matchmaking.currentLobby);
            CSteamID newPlayer = SteamMatchmaking.GetLobbyMemberByIndex(matchmaking.currentLobby, numPlayers - 1);
            newPlayerName = SteamFriends.GetFriendPersonaName(newPlayer);
        }
        else
        {
            newPlayerName = "Player " + (entries.Count);
        }
        entryScript.UpdateText(gameObject.name, entries.Count, newPlayerName);
    }


    public void RemoveUser(int connId)
    {
        int index = 0;
        while(index < entries.Count)
        {
            PlayerLobbyEntry entry = entries[index];
            if(entry.connId == connId)
            {
                entries.RemoveAt(index);
                Destroy(entry.gameObject);
            }
            else
            {
                index++;
            }
        }
        ReconfigureLobby();
    }

    void ReconfigureLobby()
    {
        int index = 0;
        foreach(PlayerLobbyEntry entry in entries)
        {
            string playerName = "Player " + (index + 1);
            entry.UpdateText(gameObject.name, index, playerName);
            index++;
        }
    }

}
