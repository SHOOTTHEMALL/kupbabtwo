using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static int maxItemHave = 5;
    public CanvasGroup inventoryPanel;
    private List<Item> haveItems = new List<Item>();
    public GameObject[] viewItems = new GameObject[maxItemHave];
    

    public Dictionary<Item, GameObject> items = new Dictionary<Item, GameObject>();
    public bool[] isHaveItem = new bool[maxItemHave];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewIntentory()
    {


        foreach(var item in items)
        {
            if(items.ContainsKey(item.Key))
            {
                item.Value.SetActive(true);
            }
        }
    }

    public void ItemGet(Item item)
    {
        haveItems.Add(item);
    }

    public void ItemDrop(Item item)
    {
        if(haveItems.Contains(item))
        {
            haveItems.Remove(item);
        }
        else
        {
            Debug.Log("¶³±¼ ¾ÆÀÌÅÛÀÌ ¾ø½À´Ï´Ù");
        }
    }

    public Item ChangeItem(int itemIndex)
    {
        if(itemIndex >= haveItems.Count)
        {
            return null;
        }
        return haveItems[itemIndex + 1];
    }
}
