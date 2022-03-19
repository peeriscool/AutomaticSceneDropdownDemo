using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blendshapecontrol : MonoBehaviour
{
    public float animationSpeed = 0.2f;
    public int blendShapeCount = 1; //Didn't find other way to get the count of the blend shapes. I am going to look again tomorrow
    public SkinnedMeshRenderer meshRenderer;

    int curFrame;
    int index = 1;
    float frameLength;

    void Update()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            
            meshRenderer.SetBlendShapeWeight(curFrame, index+=(int)animationSpeed);

            //meshRenderer.SetBlendShapeWeight((int)(Time.time * 100) % index, index++);
            
        }
        //if (frameLength >= animationSpeed)
        //{
        //    UpdateAnimationFrame();
        //    frameLength = 0;
        //}
        //else
        //{
        //    frameLength += Time.deltaTime;
        //}
    }

    void UpdateAnimationFrame()
    {
        meshRenderer.SetBlendShapeWeight(curFrame, 0);
        curFrame++;
        if (curFrame > blendShapeCount) Loop();
        meshRenderer.SetBlendShapeWeight(curFrame, 100);
    }
    void Loop()
    {
        meshRenderer.SetBlendShapeWeight(curFrame - 1, 0);
        curFrame = 0;
    }
}
