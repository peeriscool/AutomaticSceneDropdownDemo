using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class ImageOrderCanvas : MonoBehaviour
{

    /// <summary>
    /// ToDo: Make a cap at the max of the prins list and 0 for ImageIndex
    /// 
    /// </summary>
    private GameObject DisplayHost;
    List<Image> Prins;
    Image displayed;
    AspectRatioFitter aspect;
    Canvas owner;
    int ImageIndex = -1;

    void Start()
    {
        Prins = new List<Image>();
        owner = GetComponent<Canvas>();
        GameObject item = owner.transform.GetChild(0).gameObject;
        foreach (Transform obj in item.transform)
        {
            Prins.Add(obj.gameObject.GetComponentInChildren<Image>());
        }
        Debug.Log("Total images= "+Prins.Count);
        foreach (Image img in Prins)
        {
            Debug.Log(img.name);
        }
        InitPlayhost();
    }
    void Update()
    {
        if(Input.anyKey)
        {
            Keypressed();
        }
    }
    void InitPlayhost()
    {
        DisplayHost = new GameObject();
        displayed = DisplayHost.AddComponent<Image>();
        aspect =  DisplayHost.AddComponent<AspectRatioFitter>();
        aspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        DisplayHost.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
        DisplayHost.transform.localScale  = new Vector3(1f,0.5f);//*= scaleOfImage; //sets the base scale for all the images
        DisplayHost.transform.parent = owner.gameObject.transform;
    }
    void Keypressed()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            ImageIndex--;
            Debug.Log(ImageIndex);
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            ImageIndex++;
            Debug.Log(ImageIndex);
        }
        AssignIndexToImage();
    }
    void AssignIndexToImage()
    {
        displayed.sprite = Prins[ImageIndex].sprite;
        aspect.aspectMode = AspectRatioFitter.AspectMode.EnvelopeParent;
    }
}