using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class textfadeout : MonoBehaviour
{
    public bool hide_at_start;
    Text manip;
    float time;

    void Start()
    {
        manip = this.gameObject.GetComponent<Text>();
        if(hide_at_start == true)
        {
            manip.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
                manip.enabled = true;
                Debug.Log("Text Should appear");
                StartCoroutine(FadeTextToFullAlpha(3f, manip));
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            manip.enabled = false;
        }
        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Text Should disappear");
             StartCoroutine(FadeAlphaToFullText(3f, manip));
        }     
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeAlphaToFullText(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime * t));
            yield return null;
        }
    }
}
