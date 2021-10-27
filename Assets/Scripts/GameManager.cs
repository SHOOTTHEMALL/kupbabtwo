using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("adasd");
        }
        instance = this;
    }

    public sayManager sayManager;
    public Animator sayPanel;
    public typeEffect say;
    public GameObject sObj;
    public bool show;
    public int sayIndex;
    public PlayerEquip playerEquip;
    public Player player;

    // public Player player; 

    public List<Item> itemList = new List<Item>(); // 콜렉션 완료

    public Image emptyImage;
    public Image fillImage;
    public Image check;
    public bool checkStart = false;
    public bool checkComplete = false;
    private float checkAmount = 0.12f; // 1=100% 0.1=10% 범위크기

    public void Action(GameObject scanObj)
    {
        sObj = scanObj;
        Debug.Log(sObj.name);
        objData objData = sObj.GetComponent<objData>();
        Say(objData.id, objData.npc);
        sayPanel.SetBool("isShow", show);
    }

    void Say(int id, bool npc)
    {
        string sayData = "";
        if (say.isAni)
        {
            say.setMsg("");
            return;
        }
        else
        {
            sayData = sayManager.GetSay(id, sayIndex);
        }



        if (sayData == null)
        {
            show = false;
            sayIndex = 0;
            return;
        }

        if (npc)
        {
            say.setMsg(sayData.Split(':')[0]);
        }
        else
        {
            say.setMsg(sayData);
        }

        show = true;
        sayIndex++;
    }

    private void Update()
    {
        if (checkStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkComplete = true;
                checkStart = false;
                // 범위안에들어오면

                if (fillImage.fillAmount >= (360 - check.transform.rotation.eulerAngles.z) / 360 &&
                   fillImage.fillAmount <= (360 - check.transform.rotation.eulerAngles.z) / 360 + checkAmount)
                {
                    // 성공 처리
                    Debug.Log("Success");
                    
                    instance.player.panel.SetActive(false);
                }
                else
                {
                    // 실패 처리
                    Debug.Log("fail");
                    instance.player.panel.SetActive(false);
                }
            }

            if (fillImage.fillAmount < 1f || !checkComplete)
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
