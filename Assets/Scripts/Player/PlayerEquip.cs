using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public Item currentItem; // 낀아이템
    public Transform equipItemTrm; // 끼는곳
    public Transform leftTrm;
    public Transform rightTrm;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EquipItem(Item item)
    {
        item.Get();
       
        // 위치 조정할꺼면 
        // item.transform.parent = EquipItemTrm;
    }

    public void DropItem()
    {
        if(currentItem != null)
        {
            currentItem.Drop(transform.position);
            currentItem = null;
        }
    }

    public bool IsPlayerPutItem()
    {
        return (currentItem != null); // 아이템 장착중이면 true
    }

    public void TurnItem(bool dir)
    {
        // Debug.Log(equipItemTrm.position);
        
        //int turnLength = dir ? -4 : 4;
        //equipItemTrm.position = new Vector3(equipItemTrm.position.x + turnLength, equipItemTrm.position.y, equipItemTrm.position.z); // x flip Trm
    }
}
