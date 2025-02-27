using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorRotator : MonoBehaviour
{
    Image img;

    public Color color1;
    public Color color2;

    private void Start()
    {
        img = GetComponent<Image>();
    }

    private void Update()
    {
        img.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time, 1));
    }
}