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
        GameManager.Instance.player.StopFire();
        GameManager.Instance.player.isFired = true;
        Debug.Log("³ª¹µ°¡Áö »ç¿ëÁß");
        base.Use();
    }

    public override void Get()
    {
        Debug.Log("³ª¹µ°¡Áö È¹µæ");
        base.Get();
        Save(getBranch);
    }
}
