using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
