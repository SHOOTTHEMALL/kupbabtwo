using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSpacePuzzle : MonoBehaviour
{
    public Image emptyImage;
    public Image fillImage;
    public Image check;
    public bool checkStart = false;
    public bool checkComplete = false;
    private float checkAmount = 0.12f; // 1=100% 0.1=10% 범위크기

    void Start()
    {
        CircleFunctionStart();
    }

    private void Update()
    {
        if(checkStart)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                checkComplete = true;
                checkStart = false;
                // 범위안에들어오면

                if (fillImage.fillAmount >= (360 - check.transform.rotation.eulerAngles.z) / 360 &&
                   fillImage.fillAmount <= (360 - check.transform.rotation.eulerAngles.z) / 360 + checkAmount)
                {
                    // 성공 처리
                    Debug.Log("Success");
                }
                else
                {
                    // 실패 처리
                    Debug.Log("fail");
                }
            }

            if(fillImage.fillAmount < 1f || !checkComplete)
            {
                fillImage.fillAmount += Time.deltaTime / 2;
            }
        }
    }

    public void CircleFunctionStart()
    {
        checkStart = true;

        emptyImage.gameObject.SetActive(true);
         fillImage.gameObject.SetActive(true);
        check.fillAmount = checkAmount;
        check.rectTransform.rotation = Quaternion.Euler(new Vector3
            (0, 0, Random.Range(36, 216)));
        Debug.Log("angle : " + check.transform.rotation.eulerAngles.z);
    }
}
