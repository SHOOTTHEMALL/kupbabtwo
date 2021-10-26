using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;


public class AxeScript : Item
{
    public GameObject dropPrefab; // ����߸��Ŷ� �����ִ°Ŷ� �ٸ���� Drop�� ���, �������ƴ�

    const string saveFileName = "ItemSave.sav";
    private bool getAxe = true;
    private WaitForSeconds ws;
    private float delay = 0.5f;

    private void Start()
    {
        ws = new WaitForSeconds(delay);
    }
    private string getFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public override bool Save(bool getItem)
    {
        JObject axe = new JObject();
        axe.Add("getAxe", getAxe); //�ҷ��� ������Ʈ

        StreamWriter sw = new StreamWriter(getFilePath(saveFileName));
        sw.WriteLine(axe.ToString());//�ܼ��� �ƴ� �� �ҷ����°�
        sw.Close();
        base.Save(getItem);

        StreamReader sr = new StreamReader(getFilePath(saveFileName));//������ ���

        string jsonString = sr.ReadToEnd();
        sr.Close();
        Debug.Log(jsonString);
        JObject jO = JObject.Parse(jsonString);

        getAxe = jO["getAxe"].Value<bool>(); //������ �簡��
        return getItem;
    }
    public override void Drop(Vector2 pos)
    {
        //Instantiate(dropPrefab, pos, Quaternion.identity);
        //Destroy(gameObject);

        Save(!getAxe);

        base.Drop(pos);
        getAxe = false;
    }

    public override void Use()
    {
        if(GameManager.Instance.player.isBroken == true)
        {
            GameManager.Instance.player.digCount++;
            StartCoroutine(hit());
        }

        if(GameManager.Instance.player.getAxetwo)
        {
            Destroy(GameManager.Instance.player.blue);
        }

        Debug.Log("���� �����");
        base.Use();
    }

    public override void Get()
    {
        Debug.Log("���� ȹ��");
        base.Get();
        Save(getAxe);
    }

    private IEnumerator hit()
    {
            GameManager.Instance.player.tree.GetComponent<SpriteRenderer>().color = Color.red;
        yield return ws;
                GameManager.Instance.player.tree.GetComponent<SpriteRenderer>().color = Color.white;
                
            
        
    }
}
