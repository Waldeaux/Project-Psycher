using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestGameManager : MonoBehaviour
{
    public ScrapperNetworkManager networkManager;
    public GameObject infoPrefab;
    public void StartGame()
    {
        networkManager.ServerChangeScene("GameMap");
        GameObject info = GameObject.Instantiate(infoPrefab);
        info.GetComponent<GameInfo>().numPlayers = 2;
        DontDestroyOnLoad(info);
    }
}
