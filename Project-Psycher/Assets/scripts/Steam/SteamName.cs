using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamName : MonoBehaviour
{

    void Start()
    {
        Text text = GetComponent<Text>();
        text.text = SteamFriends.GetPersonaName();
    }

}
