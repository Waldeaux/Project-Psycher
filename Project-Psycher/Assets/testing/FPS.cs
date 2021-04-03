using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public Text text;

    void Update()
    {
        text.text = (1.0f / Time.deltaTime).ToString();
    }
}
