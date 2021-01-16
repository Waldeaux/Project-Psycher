using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLobbyEntry : MonoBehaviour
{
    public int connId;
    public Text text;
    public NetworkUI nUI;

    public void UpdateText(string parentName, int count, string name)
    {
        if (nUI)
        {
            nUI.parentName = parentName;
            nUI.count = count;
            nUI.text = name;
        }
    }
}
