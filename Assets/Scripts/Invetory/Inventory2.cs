using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory2 : MonoBehaviour
{
    public static Inventory2 instance;

    public static bool IsOpen {
        get{
            return instance.isOpen;
        }
    }

    private CanvasGroup cg;
    private bool isOpen = false;

    private List<Slot> slots = new List<Slot>();

    private int selectingIndex;
    private Slot selectingItem = new Slot();
    public GameObject selectEffect;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� �κ��丮 �̱����� �������Դϴ�");
        }
        instance = this;

        cg = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        GetComponentsInChildren<Slot>(slots);
    }

    public static void OpenInventory()
    {
        CanvasGroup cg = instance.cg;
        instance.isOpen = true;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    public static void CloseInventory()
    {
        CanvasGroup cg = instance.cg;
        instance.isOpen = false;
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public static void InsertItem(Item item)
    {
        // item�� Ÿ���� ��ġ�ϸ鼭 ����ִ� ������ ã�ƿͶ� 
        Slot emptySlot = instance.slots.Find(x => x.comportType == item.type && x.isEmpty);

        // �׷� ������ �ִٸ�
        if(emptySlot != null)
        {
            emptySlot.SetItem(item);
        }
    }

    public static void DeleteItem(Item item)
    {
        Slot getSlot = instance.slots.Find(x => x.comportType == item.type && !x.isEmpty);
        // �׷� ������ �ִٸ�
        if (getSlot != null)
        {
            getSlot.RemoveItem();
        }
    }

    public void LeftSelectItem()
    {
        selectingItem = slots[selectingIndex];
        selectEffect.transform.position = selectingItem.gameObject.transform.position;
    }

    public void RightSelectItem()
    {
        selectingItem = slots[selectingIndex];
        selectEffect.transform.position = selectingItem.gameObject.transform.position;
    }

    public int GetItemCount()
    {
        return slots.Count;
    }
}
