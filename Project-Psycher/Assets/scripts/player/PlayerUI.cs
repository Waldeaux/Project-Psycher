using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Canvas canvas;
    public Text remainingRivets;

    public void UpdateRemainingRivets(int remainingCount)
    {
        remainingRivets.text = remainingCount.ToString();
    }

    private void OnDestroy()
    {
        Destroy(canvas);
    }

    public void ToggleCanvas(bool hidden)
    {
        canvas.gameObject.SetActive(!hidden);
    }
}
