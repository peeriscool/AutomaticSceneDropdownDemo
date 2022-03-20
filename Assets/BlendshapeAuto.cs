using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendshapeAuto : MonoBehaviour
{ 
    public int blendShapeCount = 1; //Didn't find other way to get the count of the blend shapes. I am going to look again tomorrow
    public SkinnedMeshRenderer meshRenderer;
    public bool stackshapes;
    public float settime;
    int curFrame = 0;
    int lastFrame = -1;
    float frameLength;
    float animationSpeed = 1f;
    float[] blendshapeweights;
    

    private void Awake()
    {
        Time.timeScale = settime;
        blendshapeweights = new float[blendShapeCount];
        getvalues(meshRenderer, blendShapeCount);
    }
    void getvalues(SkinnedMeshRenderer owner, int count)
    {
        for (int i = 0; i < count; i++)
        {
            blendshapeweights[i] = owner.GetBlendShapeWeight(i);
        }
    }
    void Update()
    {
        getvalues(meshRenderer, blendShapeCount);
       if(curFrame != lastFrame)
        {
            for (int i = 0; i < blendshapeweights.Length; i++)
            {
                //loop through values until all shapes have been used  
                if (blendshapeweights[i] >= 100)
                {
                    //move on to the next blendshape
                   
                    lastFrame = curFrame;
                   
                      blendshapeweights[i] = 0;
                    // animationSpeed = animationSpeed;
                    //  blendshapeweights.SetValue(0,i);
                    //meshRenderer.SetBlendShapeWeight(i, 0);

                }
                //  meshRenderer.SetBlendShapeWeight(i, blendshapeassign());
            }
        }
        else
        {
            curFrame++;
        }
       if(curFrame >= blendShapeCount)
        {
            curFrame = 0;
        }
            meshRenderer.SetBlendShapeWeight(curFrame, Time.time % (int)animationSpeed *101 );
        if(stackshapes)
        meshRenderer.SetBlendShapeWeight(lastFrame, Time.time % (int)animationSpeed * 101);
    }
    //void UpdateAnimationFrame()
    //{
    //    meshRenderer.SetBlendShapeWeight(curFrame, 0);
    //    curFrame++;
    //    if (curFrame > blendShapeCount) Loop();
    //    meshRenderer.SetBlendShapeWeight(curFrame, 100);
    //}
    //void Loop()
    //{
    //    meshRenderer.SetBlendShapeWeight(curFrame - 1, 0);
    //    curFrame = 0;
    //}
}
