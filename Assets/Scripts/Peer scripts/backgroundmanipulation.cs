using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


    public abstract class backgroundmanipulation
    {
    protected Camera cam;
    protected Light lightsource;
    protected Color color1 = Color.green;
    protected Color color2 = Color.yellow;
    protected Color color3 = Color.white;

     public abstract void Start();
     public abstract void Update();

    public class LerpColor : backgroundmanipulation
    {
        float duration = 6.0F;
        public LerpColor(Color _color1, Color _color2, Color _color3, Camera _cam)
        {
            color1 = _color1; color2 = _color2; color3 = _color3;
            cam = _cam;
        }

        public override void Start()
        {
            Debug.Log(cam.name);
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
        public override void Update()
        {
            Debug.Log("1");
            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //  Color one = color1;
            // Color three = color3;
           // color1 = color2;
           //     color3 = color2;
          //  }
            float t = Mathf.PingPong(Time.time, duration) / duration;   
           cam.backgroundColor = Lerp3(color1, color2, color3, t); //lerp between 3 background colors           
        }
        Color Lerp3(Color a, Color b, Color c, float t)
        {
            if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
                return Color.Lerp(a, b, t / 0.5f);
            else // 0.5 to 1.0 goes to b -> c
                return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
        }
    }

    [System.Serializable]
    public class MouseControl : backgroundmanipulation
    {
        public MouseControl(Color _color1, Color _color2, Color _color3, Camera _cam)
        {
            color1 = _color1; color2 = _color2; color3 = _color3;
            cam = _cam;
        }
        //public Color color1 = Color.green;
        //public Color color2 = Color.yellow;
        //public Color color3 = Color.white;
        bool mousemanipulation = true;

        public override void Start()
        {
            Debug.Log(cam.name);
            //cam = GetComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
        }
        public override void Update()
        {
            cam.backgroundColor = color1;
           Debug.Log("2");
            //if (Input.GetKeyDown(KeyCode.Y))
            //{
          //  mousemanipulation = true;
              //  color1 = new Color();
            //    color1.r = 0;


           //     color1.a = 1;
          //  }
            //if (mousemanipulation)
            //{
                float variablex = Mathf.InverseLerp(0, 255, Input.mousePosition.x) / 2;
                float variabley = Mathf.InverseLerp(0, 255, Input.mousePosition.y) / 2;
               // color1.r = variablex;
                color1.b = variablex;
                color1.g = variabley;

           // }
        }

        void init()
        {
           
        }
    }


/*         backgroundmanipulation Mousecontrol;



void Start()
{
    if (InDevelopment)
    {
        //   Mousecontrol = new backgroundmanipulation();
        backgroundmanipulation lerp = backgroundmanipulation.LerpColor;
    }
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
}
*/
}