using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    AXE,
    WATER,
    PAPER
}

public class Slot : MonoBehaviour
{
    public bool isEmpty = true;
    public ItemType comportType; 

    Image sr;

    private void Awake()
    {
        sr = GetComponent<Image>();
    }


    public void SetItem(Item item)
    {
        isEmpty = false;
        sr.sprite = item.GetSprite();
    }

    public void RemoveItem()
    {
        isEmpty = true;
        sr.sprite = null;
    }

    public void Select()
    {

    }

    public void SelectEnd()
    {

    }
}
