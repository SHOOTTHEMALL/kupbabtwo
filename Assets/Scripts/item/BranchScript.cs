using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;

public class BranchScript : Item
{
    const string saveFileName = "ItemSave.sav";
    private bool getBranch = true;

    private string getFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public override bool Save(bool getItem)
    {
        JObject axe = new JObject();
        axe.Add("getBranch", getBranch); 

        StreamWriter sw = new StreamWriter(getFilePath(saveFileName));
        sw.WriteLine(axe.ToString());
        sw.Close();
        base.Save(getItem);

        StreamReader sr = new StreamReader(getFilePath(saveFileName));

        string jsonString = sr.ReadToEnd();
        sr.Close();
        Debug.Log(jsonString);
        JObject jO = JObject.Parse(jsonString);

        getBranch = jO["getBranch"].Value<bool>(); 
        return getItem;
    }

    public override void Drop(Vector2 pos)
    {
        //Instantiate(dropPrefab, pos, Quaternion.identity);
        //Destroy(gameObject);
        Save(!getBranch);
        base.Drop(pos);
    }

    public override void Use()
    {
        GameManager.Instance.player.StopFire(); //사용하면 불이 멈춰진다
        GameManager.Instance.player.isFired = true; 

        if (GameManager.Instance.player.isYellow) //노란천을 부신다
        {
            Destroy(GameManager.Instance.player.yellow);
        }

        Debug.Log("나뭇가지 사용중");
        base.Use();
    }

    public override void Get()
    {
        Debug.Log("나뭇가지 획득");
        base.Get();
        Save(getBranch);
    }
}
