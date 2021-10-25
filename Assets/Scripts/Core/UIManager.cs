using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Inventory playerInventory;
    public static UIManager instance;
    public GameObject[] inventoryItems;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Debug.Log("다수의 UI매니저가 실행중");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Inventory2.IsOpen)
            {
                Inventory2.CloseInventory();
            }
            else
            {
                Inventory2.OpenInventory();
            }
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(Inventory2.IsOpen)
            {
                Inventory2.instance.LeftSelectItem();
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Inventory2.IsOpen)
            {
                Inventory2.instance.RightSelectItem();
            }
        }
    }

    public void OpenInventory(bool[] isItemGet)
    {
        for(int i = 0; i<5; i++)
        {
            inventoryItems[i].SetActive(isItemGet[i]);
        }

        playerInventory.ViewIntentory();
    }
}
