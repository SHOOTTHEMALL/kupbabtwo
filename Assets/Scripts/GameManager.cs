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

    public List<Item> itemList = new List<Item>(); // �ݷ��� �Ϸ�

    public Image fillImage;
    public Image check;
    public bool checkStart = false;
    public bool checkComplete = false;
    private float checkAmount = 0.1f; // 1=100% 0.1=10% ����ũ

    private float fillImageWidth; //�� ������ �������� �簢���̴�.

    GameObject treeObj;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("adasd");
        }
        instance = this;
    }
    private void Start()
    {
        fillImageWidth = fillImage.rectTransform.rect.width; //���̹����� ������ �����´�. ���̸�
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
        //if (fillImage.transform.localPosition.x < 1f || checkStart && !checkComplete)
        //{
        //    fillImage.fillAmount += Time.deltaTime / 2; //�ذ��ؾ��ϴ°�
        //}

        if (checkStart)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                checkComplete = true;
                checkStart = false;
                //// �����ȿ�������
                //Debug.Log(check.transform.localPosition.x);
                //Debug.Log(fillImage.rectTransform.rect.width);


                float fillPosition = fillImageWidth * fillImage.fillAmount; // �簢�� ���̿��ٰ� ���̹����� �� üũ�� ���װ� ���Ѵ�.
                if (fillPosition >= check.transform.localPosition.x - check.rectTransform.rect.width //�������� üũ�̹������� ���̺��� ũ�� üũ�̹������� �۰ų� ����.
                    && fillPosition <= check.transform.localPosition.x)
                {
                    // ���� ó��
                    Debug.Log("Success");
                }
                else
                {
                    Debug.Log("Fail");
                }
                
            }

            if (fillImage.fillAmount  < 1f && checkStart)
            {
                fillImage.fillAmount += Time.deltaTime / 2;
            }
        }
    }



    public void CircleFunctionStart(GameObject obj)
    {
        check.gameObject.SetActive(true);
        checkStart = true;
        treeObj = obj;
        check.fillAmount = checkAmount;
        
        float x = Random.Range(200, fillImageWidth);
        Debug.Log(x);
        
        check.transform.localPosition = new Vector3(x,0,0);
        //Debug.Log("position : " + check.transform.position.x);
    }
}
