using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BckColor : MonoBehaviour
{
    public Color color1 = Color.green;
    public Color color2 = Color.yellow;
    public Color color3 = Color.white;
    float duration = 6.0F;

    public Camera cam;
    public Light lightsource;
    bool lightmanipulation;

    void Start()
    {
        if (!lightsource)
        {
            cam = GetComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
        if (lightsource)
        {
            lightmanipulation = true;
        }
    }

    void Update()
    {
        
        float t = Mathf.PingPong(Time.time, duration) / duration;
        if (!lightmanipulation)
        {

            cam.backgroundColor = Lerp3(color1, color2, color3, t);
        }
        if (lightmanipulation)
        {
            lightsource.color = Lerp3(color1, color2, color3, t);
        }
    }
    Color Lerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return Color.Lerp(a, b, t / 0.5f);
        else // 0.5 to 1.0 goes to b -> c
            return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
    }

}
