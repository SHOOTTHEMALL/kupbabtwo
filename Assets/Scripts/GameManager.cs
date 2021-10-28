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

    public Image fillImage;
    public RectTransform check;
    private RectTransform reload;
    public bool checkStart = false;
    public bool checkComplete = false;

    GameObject treeObj;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("adasd");
        }
        instance = this;

        reload = fillImage.rectTransform.gameObject.GetComponent<RectTransform>();
    }

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
        if (fillImage.transform.position.x < 1f || !checkComplete)
        {
            fillImage.fillAmount += Time.deltaTime / 3;
        }
    }



    public void CircleFunctionStart(GameObject obj)
    {
        checkStart = true;
        treeObj = obj;
        Vector2 aPosition = check.anchoredPosition;
        aPosition.x = Random.Range(1, 10);
        check.anchoredPosition = aPosition;
        Debug.Log("angle : " + check.transform.position.x);
    }

    public bool CheckSuccess()
    {
        float currentPoint = reload.rect.width * fillImage.fillAmount;
        Vector2 aPosition = check.anchoredPosition;

        return currentPoint >= aPosition.x && currentPoint <= aPosition.x + check.rect.width;
    }
}
