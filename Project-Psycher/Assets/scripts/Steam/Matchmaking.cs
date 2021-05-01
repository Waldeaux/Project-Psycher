using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;
using System.Net;
using System.Linq;

public class Matchmaking : MonoBehaviour
{
    ScrapperNetworkManager networkManager;
    public CSteamID currentLobby;
    Callback<GameLobbyJoinRequested_t> lobbyJoinCallback;
    Callback<GameOverlayActivated_t> overlayCallback;
    Callback<LobbyCreated_t> lobbyCreatedCallback;
    Callback<LobbyEnter_t> lobbyEnteredCallback;

    public GameObject startLobbyButton;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<ScrapperNetworkManager>();
        if (SteamManager.Initialized)
        {
            lobbyJoinCallback = Callback<GameLobbyJoinRequested_t>.Create(JoinLobby);
            lobbyCreatedCallback = Callback<LobbyCreated_t>.Create(SetLobbyData);
            lobbyEnteredCallback = Callback<LobbyEnter_t>.Create(EnteredLobby);
        }
        else
        {
            startLobbyButton.SetActive(false);
        }
        Debug.Log(GetLocalIPv4());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void JoinLobby(GameLobbyJoinRequested_t joinRequest)
    {
        SteamMatchmaking.JoinLobby(joinRequest.m_steamIDLobby);
    }

    void EnteredLobby(LobbyEnter_t enterCallback)
    {
        var ip = SteamMatchmaking.GetLobbyData((CSteamID)enterCallback.m_ulSteamIDLobby, "connection_ip");
        if(ip == GetLocalIPv4())
        {
            return;
        }
        var uri = new UriBuilder(ip);
        if (networkManager)
        {
            networkManager.StartClient(uri.Uri);
        }
    }

    public void CreateLobby()
    {
        if (SteamManager.Initialized)
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
        }
        else
        {
            networkManager.StartHost();
        }
    }

    public void SetLobbyData(LobbyCreated_t createdCallback)
    {
        currentLobby = (CSteamID)createdCallback.m_ulSteamIDLobby;
        SteamMatchmaking.SetLobbyData((CSteamID) createdCallback.m_ulSteamIDLobby, "connection_ip", GetLocalIPv4());
        networkManager.StartHost();
    }
    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }
}
