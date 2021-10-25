using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typeEffect : MonoBehaviour
{
    string targetMsg;
    public int charS;
    Text msgText;
    int index;
    float interval;
    public bool isAni;

    private void Awake()
    {
        msgText = GetComponent<Text>();
    }
    // Start is called before the first frame update
    public void setMsg(string msg)
    {
        if (isAni)
        {
            CancelInvoke();
            msgText.text = targetMsg;
            EffectEnd();
        }
        else
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    // Update is called once per frame
    void EffectStart()
    {
        msgText.text = "";
        index = 0;

        interval = 1.0f / charS; // 조금씩나오게
        Debug.Log(interval);
        isAni = true;
        Invoke("EffectIng", interval);
    }

    void EffectIng()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index++;

        Invoke("EffectIng", interval);
    }

    void EffectEnd()
    {
        isAni = false;
    }
}
