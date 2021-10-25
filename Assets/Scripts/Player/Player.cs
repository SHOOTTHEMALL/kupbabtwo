using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float Speed = 15;//�̵��� �ʿ���

    private Rigidbody2D rg;//�̵��� �ʿ���
    private float horizontal;//�̵��� �ʿ���
    private bool movingMan;//�̵��� �ʿ���
    private Vector3 dirVec;//�̵��� �ʿ���
    private GameObject Obj;
    private SpriteRenderer sr;
    private WaitForSeconds ws;
    private float delay = 3f;
    private bool getWater;

    public GameObject Fire;
    public GameObject tree;
    public GameObject branch;
    public GameObject skyB;
    public GameObject skyD;
    public GameObject blue;
    public GameObject red;
    public GameObject yellow;
    public bool dir = false;
    public bool isSlow = false; // ������ �Ȱ��ִ°�
    public float divideSlowSpeed = 0.5f; // �����԰����� speed * dividespeed�� �̼Ӱ���
    public bool isFired = false;
    public bool isBroken = false;
    public bool isTurnedLastUpdate = false; // Turn�Ǹ� true �ǰ� FixedUpdate���� false
    public int digCount = 0;

    [HideInInspector]
    public LayerMask collisionlayer;




    public Action<bool> turnAction; // Inventory���� ���� ��������Ʈ
    

    public Vector3 GetDir()
    {
        return dirVec;
    }
    public enum PlayerState
    {
        Normal,//����� ���� ĵ������ �ӵ� 15
        Unstable,//�Ҿ��� ���� ĵ������ �ӵ� 12
        Fear,//���� ���� ������ �ӵ�8 
        Revenge//������ ���� ������ �ӵ� 12 �ൿ ���� �ӵ� 5 3�ʰ� �̶��� ���ݷ� �� �� �� �� ������
    }

    public PlayerState state = PlayerState.Normal;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ws = new WaitForSeconds(delay);

        turnAction += (x) => { }; // ��ɷ� �ʱ�ȭ

        StartCoroutine(DoFeeling());
    }

    private void OnEnable()
    {
        state = PlayerState.Normal;
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = GameManager.Instance.show ? 0 : Input.GetAxisRaw("Horizontal");

        bool hDown = GameManager.Instance.show ? false : Input.GetButtonDown("Horizontal"); //��ư ����
        bool hUp = GameManager.Instance.show ? false : Input.GetButtonUp("Horizontal"); //��

        if (hDown || hUp)
            movingMan = true;
        // movingMan = (hDown || hUp) ? true : false;

        int lastDir = dir ? 1 : -1; // �Է¹ޱ��� ���������� ���� �Է�
        if(horizontal != lastDir && horizontal != 0) // �� ����Է��̴ٸ��� == ȸ��
        {
            TurnEvent();
        }

        if (horizontal == -1 && hDown)
        {
            dirVec = Vector3.left;
            dir = false;
        }

        else if (horizontal == 1 && hDown)
        {
            dirVec = Vector3.right;
            dir = true;
        }

        if (movingMan) // PlayerEquip�� ����
            GameManager.Instance.playerEquip.TurnItem(dir);

        Debug.Log(Obj);
        if (Input.GetButtonDown("Jump") && Obj != null)
        {
            Debug.Log(GameManager.Instance);   
            GameManager.Instance.Action(Obj); // ��ü�� �����ϸ� ��簡 ������ ���ǹ�
            Debug.Log("�־ȵ�");
        }


        

        

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SlowMove(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SlowMove(false);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {

        }

        if(digCount > 2)
        {
            tree.SetActive(false);
            state = PlayerState.Unstable;
            branch.SetActive(true);
        }

        if(!isSlow)
        {
            getWater = false;
        }
    }

    public void TurnEvent()
    {
        Debug.Log("���ҽ��ϴ�");
        isTurnedLastUpdate = true;
        sr.flipX = !dir;
        turnAction(dir);
    }

    public void SlowMove(bool isSlow)
    {
        this.isSlow = isSlow;
        Speed = isSlow ? Speed * divideSlowSpeed : Speed / divideSlowSpeed;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rg.position, dirVec * 3, new Color(1, 0, 1));
        RaycastHit2D[] hit = Physics2D.RaycastAll(rg.position, dirVec, 3, LayerMask.GetMask("Objection"));

        for(int i = 0; i < hit.Length; i++)
        {
            if(GameManager.Instance.playerEquip.currentItem == null)
            {
                Obj = hit[i].collider.gameObject;
                break;
            }
            if( !GameObject.ReferenceEquals( hit[i].collider.gameObject, GameManager.Instance.playerEquip.currentItem.gameObject))
            {
                Obj = hit[i].collider.gameObject;
                break;
            }
        }
        

        Vector2 moveDir = new Vector2(horizontal, 0); // �¿��̵� ����
        rg.velocity = moveDir * Speed;
    }

    public void StopFire()
    {
        Fire.gameObject.SetActive(false);
    }

    public IEnumerator DoFeeling()
    {
        while (true)
        {
            yield return ws;
            switch (state)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.Unstable:
                    Speed = 12;
                    skyB.SetActive(false);
                    skyD.SetActive(true);
                    break;
                case PlayerState.Fear:
                    Speed = 8;
                    break;
                case PlayerState.Revenge:
                    Speed = 12;
                    break;
                default:
                    break;
            }
        }

    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {

            if (Fire != null && !isFired)
            {
                Fire.gameObject.SetActive(true);
                Invoke("StopFire", 3);
            }
        }

        if(collision.gameObject.CompareTag("tree"))
        {
            isBroken = true;
            
            Debug.Log("dks");
        }

        if(collision.gameObject.CompareTag("water"))
        {
            getWater = true;
        }

        if(collision.gameObject.CompareTag("blueChon"))
        {
            Destroy(blue);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tree"))
        {
            isBroken = false;

            Debug.Log("�ɳ���");
        }
    }
}
