using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Explain : MonoBehaviour
{
    public CanvasGroup cg;

    private float sec = 0f;

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        if(sec > 10)
        {
            cg.alpha -= .005f;
        }
        else if (sec > 5)
        {
            cg.alpha += .005f;
        }
    }
}
