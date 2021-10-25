using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerItem : MonoBehaviour
{
    private Player player;
    private PlayerEquip playerEquip; // ��
    private Inventory inven;
    public bool isItem = false; // �������������ִ°�
    // �̰ɷ� �����ϴ��� �ƴϸ� GameManager instance�� ������, player�� turnAction += void�Լ�(�͸�) �������
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
                Debug.Log("������ ����!");
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
        if (playerEquip.currentItem != null) // ���
        {
            inven.ItemDrop(playerEquip.currentItem);
            Inventory2.DeleteItem(playerEquip.currentItem);
            playerEquip.DropItem();
        }

        if (obj == null) return; // ��ü������ ����

        Item item = obj.GetComponent<Item>(); // ȹ��
        if (item != null)
        {
            Debug.Log("Get Item");
            inven.ItemGet(item);
            playerEquip.EquipItem(item);
        }
    }
}
