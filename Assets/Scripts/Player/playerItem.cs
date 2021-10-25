using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerItem : MonoBehaviour
{
    private Player player;
    private PlayerEquip playerEquip; // 얘
    private Inventory inven;
    public bool isItem = false; // 아이템을끼고있는가
    // 이걸로 접근하던지 아니면 GameManager instance로 오든지, player의 turnAction += void함수(익명) 해줘야함
    GameObject obj = null;
    private LayerMask collisionlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        playerEquip = GetComponent<PlayerEquip>();
        inven = GetComponent<Inventory>();

        GameManager.Instance.player.turnAction += (dir) =>
        {
            playerEquip.equipItemTrm = dir ? playerEquip.rightTrm : playerEquip.leftTrm;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DropEquip();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (playerEquip.currentItem != null)
            {
                playerEquip.currentItem.Use();
            }
            else
            {
                Debug.Log("도구가 없음!");
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.GetDir(), 3, LayerMask.GetMask("Objection"));
        obj = null;
        if (hit.collider != null )
        {
            obj = hit.collider.gameObject;
            if (playerEquip.IsPlayerPutItem())
            {
                if (GameObject.ReferenceEquals(obj, playerEquip.currentItem.gameObject))
                {
                    obj = null;
                }
            }
        }
        //else
        //{
        //    obj = null;
        //}
    }

    public void DropEquip()
    {
        if (playerEquip.currentItem != null) // 드랍
        {
            inven.ItemDrop(playerEquip.currentItem);
            Inventory2.DeleteItem(playerEquip.currentItem);
            playerEquip.DropItem();
        }

        if (obj == null) return; // 물체없으면 종료

        Item item = obj.GetComponent<Item>(); // 획득
        if (item != null)
        {
            Debug.Log("Get Item");
            inven.ItemGet(item);
            playerEquip.EquipItem(item);
        }
    }
}
