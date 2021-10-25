using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public abstract class Item : MonoBehaviour, IItem
{
    public ItemType type;

    private Rigidbody2D rigid;
    private bool isGroundTouch = false;
    private SpriteRenderer sr;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
        sr = GetComponent<SpriteRenderer>();
    }

    public Sprite GetSprite()
    {
        return sr.sprite;
    }

    public virtual void Drop(Vector2 pos)
    {
        isGroundTouch = false;
        rigid.bodyType = RigidbodyType2D.Dynamic;

        gameObject.transform.parent = null;
        gameObject.transform.position = pos;

        rigid.velocity += new Vector2(0,5f); // addforce도나쁘지않음
        rigid.gravityScale = 1f;
    }

    public virtual void Use()
    {

    }

    public virtual void Get()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;
        transform.parent = GameManager.Instance.playerEquip.transform; // 손에 좌표설정
        //transform.position = GameManager.Instance.playerEquip.equipItemTrm.position;
        GetComponent<SpriteRenderer>().flipX = !(GameManager.Instance.player.dir);
        GameManager.Instance.playerEquip.currentItem = this;

        Inventory2.InsertItem(this);
    }

    public virtual bool Save(bool getItem)
    {
        return getItem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("GROUND"))
        {
            rigid.gravityScale = 0;
            if(!isGroundTouch)
            {
                rigid.velocity = Vector2.zero;
                isGroundTouch = true;
            }
        }
    }
}
