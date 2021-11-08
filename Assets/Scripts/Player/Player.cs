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
    private Vector2 twoVer = new Vector2(190,-2.37f);
    private Vector2 tigerPoint = new Vector2(249, -2.37f);
    private Vector2 playerPosition;
    [SerializeField]private float time = 0;
    

    public bool isDownArrow = false;
    public bool getWater = false;
    public bool getAxetwo = false;
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
    public bool isTiger = false;
    public bool isTurnedLastUpdate = false; // Turn�Ǹ� true �ǰ� FixedUpdate���� false
    public int digCount = 0;
    public int tigetCount = 0;
    public bool isYellow = false;
    public GameObject panel;
    public GameObject Tiger;
    public GameObject Tiger1;

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

        playerPosition = gameObject.transform.position;

        Invoke("startChase", 3);
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

        //Debug.Log(Obj);
        if (Input.GetButtonDown("Jump") && Obj != null)
        {
            //Debug.Log(GameManager.Instance);   
            GameManager.Instance.Action(Obj); // ��ü�� �����ϸ� ��簡 ������ ���ǹ�
            //Debug.Log("�־ȵ�");
        }
        

        

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SlowMove(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SlowMove(false);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))  // �̷κκ��� �Ʒ�Ű ������ �ٸ� ������ �̵��ϴ°�
        {
            if(isDownArrow)
            {
                Debug.Log("�̵����̵�");
                playerPosition = twoVer;
                transform.position = playerPosition;
            }
        }

        if(digCount > 2) //3�� �̻� �ĸ����� ������ �μ�����
        {
            tree.SetActive(false);
            state = PlayerState.Unstable;
            branch.SetActive(true);
        }

        if(tigetCount > 20) //ȣ���� 21�� �̻� ������ ȣ���̰� Ʀ
        {
            Tiger1.transform.Translate(Vector3.right * 0.2f);
        }

        if(!isSlow && getWater) //���� ��� �ִ°�?
        {
            getWater = false;
        }

        time += Time.deltaTime;

        //if(time>UnityEngine.Random.Range(3,10))
        //{
        //    panel.SetActive(true);
        //    GameManager.Instance.BossCheck();
        //    time = 0;
        //} ȣ���̰� �ڿ��� �i�ƿ��� ������ ��ư ��ųüũ �ߴ� ���� ���� ���� ������ ��� üũ

    }

    public void startChase() //ȣ���̰� �i�ƿ� ����
    {
        Tiger.SetActive(true);
        playerPosition = tigerPoint;
        transform.position = playerPosition;
    }

    public void TurnEvent() 
    {
        //Debug.Log("���ҽ��ϴ�");
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

    public void StopFire() //�� ��
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

    public void HandFucntion() //���� õ �νñ�
    {
        if(getWater && isSlow)
        {
            Destroy(red);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fire")) //������ ���� ���´� ���� ��
        {
            Debug.Log("���Ͷ�");
            if (Fire != null && !isFired)
            {
                Fire.gameObject.SetActive(true);
                Invoke("StopFire", 3);
            }
        }

        if(collision.gameObject.CompareTag("tree")) //������ ������� �ֵθ��� ���ū�� Ʈ�簡 �ȴ�
        {
            isBroken = true;
            
            Debug.Log("dks");
        }

        if (collision.gameObject.CompareTag("tiger"))//ȣ����
        {
            isTiger = true;

            Debug.Log("dks");
        }

        if (collision.gameObject.CompareTag("sike"))//�ڷ���Ʈ ���� ������
        {
            isDownArrow = true;
        }

        if(collision.gameObject.CompareTag("skillCheck")) //��ųüũ ������ ��´�
        {
            panel.SetActive(true);
            GameManager.Instance.CircleFunctionStart(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tree")) //�������� �������m��? �ƹ��ϵ� ���Ͼ �ص� ����������
        {
            isBroken = false;

            Debug.Log("�ɳ���");
        }

        if (collision.gameObject.CompareTag("tiger")) // ȣ����
        {
            isTiger = false;

            Debug.Log("�ɳ���");
        }

        if (collision.gameObject.CompareTag("blueChon")) //�Ķ�õ
        {
            getAxetwo = false;

            Debug.Log("�ɳ���");
        }

        if (collision.gameObject.CompareTag("yellowChon")) //���õ
        {
            isYellow = false;
        }

        if (collision.gameObject.CompareTag("sike"))//���� ������
        {
            isDownArrow = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("blueChon")) //�Ķ�õ�� ��� ����? ������ �����ȴ�
        {
            getAxetwo = true;
        }

        if (collision.gameObject.CompareTag("water")) //�����̿� ������ �ȴ�
        {
            Debug.Log("���� ����");
            getWater = true;
        }

        if (collision.gameObject.CompareTag("yellowChon"))//���õ
        {
            isYellow = true;
        }
    }
}
