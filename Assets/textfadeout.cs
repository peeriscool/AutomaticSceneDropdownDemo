using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class textfadeout : MonoBehaviour
{
    public bool Invert;
    Text manip;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        manip = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
         if (Invert)
          {
                StartCoroutine(FadeAlphaToFullText(3f, manip));
            }
            else
            {
                StartCoroutine(FadeTextToFullAlpha(3f, manip));        
            }
     }
        //  float alpha = Mathf.Lerp(0,10,Time.deltaTime);
        //   Material alphafade = manip.material;
        // alphafade.color = new Color(alphafade.color.r, alphafade.color.g, alphafade.color.b, Mathf.Lerp(0, 10, Time.deltaTime));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeAlphaToFullText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
