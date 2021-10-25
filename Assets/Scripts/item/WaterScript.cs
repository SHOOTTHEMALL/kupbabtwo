using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.IO;

public class WaterScript : Item
{
    const string saveFileName = "ItemSave.sav";
    private bool getWater = true;

    private string getFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    public override bool Save(bool getItem)
    {
        JObject axe = new JObject();
        axe.Add("getWater", getWater);

        StreamWriter sw = new StreamWriter(getFilePath(saveFileName));
        sw.WriteLine(axe.ToString());
        sw.Close();
        base.Save(getItem);

        StreamReader sr = new StreamReader(getFilePath(saveFileName));

        string jsonString = sr.ReadToEnd();
        sr.Close();
        Debug.Log(jsonString);
        JObject jO = JObject.Parse(jsonString);

        getWater = jO["getWater"].Value<bool>();
        return getItem;
    }

    public override void Drop(Vector2 pos)
    {
        //Instantiate(dropPrefab, pos, Quaternion.identity);
        //Destroy(gameObject);
        Save(!getWater);
        base.Drop(pos);
    }

    public override void Use()
    {
        Debug.Log("¹° »ç¿ëÁß");
        base.Use();
    }

    public override void Get()
    {
        Debug.Log("¹° È¹µæ");
        base.Get();
        Save(getWater);
    }
}
