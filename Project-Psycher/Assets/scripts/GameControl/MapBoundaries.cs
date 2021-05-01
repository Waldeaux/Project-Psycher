using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoundaries : MonoBehaviour
{
    public GameControl gameControl;

    public void RemovePlayer(GameObject player)
    {
        gameControl.RemovePlayer(player);
    }
}
