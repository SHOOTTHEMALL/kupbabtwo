using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float Speed = 15;//이동에 필요한

    private Rigidbody2D rg;//이동에 필요한
    private float horizontal;//이동에 필요한
    private bool movingMan;//이동에 필요한
    private Vector3 dirVec;//이동에 필요한
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
    public bool isSlow = false; // 느리게 걷고있는가
    public float divideSlowSpeed = 0.5f; // 느리게걸을때 speed * dividespeed로 이속감소
    public bool isFired = false;
    public bool isBroken = false;
    public bool isTiger = false;
    public bool isTurnedLastUpdate = false; // Turn되면 true 되고 FixedUpdate에서 false
    public int digCount = 0;
    public int tigetCount = 0;
    public bool isYellow = false;
    public GameObject panel;
    public GameObject Tiger;
    public GameObject Tiger1;

    public Action<bool> turnAction; // Inventory에서 쓰는 델리게이트
    

    public Vector3 GetDir()
    {
        return dirVec;
    }
    public enum PlayerState
    {
        Normal,//평온한 상태 캔슬가능 속도 15
        Unstable,//불안한 상태 캔슬가능 속도 12
        Fear,//공포 상태 ㅂㄱㄴ 속도8 
        Revenge//복수심 상태 ㅂㄱㄴ 속도 12 행동 직후 속도 5 3초간 이때만 공격력 강 해 진 다 돌격해
    }

    public PlayerState state = PlayerState.Normal;

    private void Awake()
    {
        rg = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ws = new WaitForSeconds(delay);

        turnAction += (x) => { }; // 빈걸로 초기화

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

        bool hDown = GameManager.Instance.show ? false : Input.GetButtonDown("Horizontal"); //버튼 누름
        bool hUp = GameManager.Instance.show ? false : Input.GetButtonUp("Horizontal"); //뗌

        if (hDown || hUp)
            movingMan = true;
        // movingMan = (hDown || hUp) ? true : false;

        int lastDir = dir ? 1 : -1; // 입력받기전 마지막으로 받은 입력
        if(horizontal != lastDir && horizontal != 0) // 과 방금입력이다르면 == 회전
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

        if (movingMan) // PlayerEquip에 연결
            GameManager.Instance.playerEquip.TurnItem(dir);

        //Debug.Log(Obj);
        if (Input.GetButtonDown("Jump") && Obj != null)
        {
            //Debug.Log(GameManager.Instance);   
            GameManager.Instance.Action(Obj); // 물체를 감지하면 대사가 나오는 조건문
            //Debug.Log("왜안돼");
        }
        

        

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            SlowMove(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SlowMove(false);
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))  // 미로부분의 아랫키 누르면 다른 곳으로 이동하는거
        {
            if(isDownArrow)
            {
                Debug.Log("이동해이동");
                playerPosition = twoVer;
                transform.position = playerPosition;
            }
        }

        if(digCount > 2) //3번 이상 쳐맞으면 나무가 부셔진당
        {
            tree.SetActive(false);
            state = PlayerState.Unstable;
            branch.SetActive(true);
        }

        if(tigetCount > 20) //호랑이 21대 이상 맞으면 호랑이가 튐
        {
            Tiger1.transform.Translate(Vector3.right * 0.2f);
        }

        if(!isSlow && getWater) //물을 들고 있는가?
        {
            getWater = false;
        }

        time += Time.deltaTime;

        //if(time>UnityEngine.Random.Range(3,10))
        //{
        //    panel.SetActive(true);
        //    GameManager.Instance.BossCheck();
        //    time = 0;
        //} 호랑이가 뒤에서 쫒아오고 뭐였지 암튼 스킬체크 뜨는 무당 뒤진 때의 엔딩임 상시 체크

    }

    public void startChase() //호랑이가 쫒아옴 어흥
    {
        Tiger.SetActive(true);
        playerPosition = tigerPoint;
        transform.position = playerPosition;
    }

    public void TurnEvent() 
    {
        //Debug.Log("돌았습니다");
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
        

        Vector2 moveDir = new Vector2(horizontal, 0); // 좌우이동 적용
        rg.velocity = moveDir * Speed;
    }

    public void StopFire() //불 꺼
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

    public void HandFucntion() //빨간 천 부시기
    {
        if(getWater && isSlow)
        {
            Destroy(red);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fire")) //닿으면 불이 나온다 아잇 어
        {
            Debug.Log("나와라");
            if (Fire != null && !isFired)
            {
                Fire.gameObject.SetActive(true);
                Invoke("StopFire", 3);
            }
        }

        if(collision.gameObject.CompareTag("tree")) //나무에 닿았을때 휘두르면 브로큰이 트루가 된다
        {
            isBroken = true;
            
            Debug.Log("dks");
        }

        if (collision.gameObject.CompareTag("tiger"))//호랑이
        {
            isTiger = true;

            Debug.Log("dks");
        }

        if (collision.gameObject.CompareTag("sike"))//텔레포트 검은 구덩이
        {
            isDownArrow = true;
        }

        if(collision.gameObject.CompareTag("skillCheck")) //스킬체크 나무에 닿는다
        {
            panel.SetActive(true);
            GameManager.Instance.CircleFunctionStart(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("tree")) //나무에서 떨어지몀ㄴ? 아무일도 안일어남 밑도 마찬가지임
        {
            isBroken = false;

            Debug.Log("냥냥이");
        }

        if (collision.gameObject.CompareTag("tiger")) // 호랑이
        {
            isTiger = false;

            Debug.Log("냥냥이");
        }

        if (collision.gameObject.CompareTag("blueChon")) //파란천
        {
            getAxetwo = false;

            Debug.Log("냥냥이");
        }

        if (collision.gameObject.CompareTag("yellowChon")) //노란천
        {
            isYellow = false;
        }

        if (collision.gameObject.CompareTag("sike"))//검은 구덩이
        {
            isDownArrow = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("blueChon")) //파란천에 닿는 동안? 조건이 성립된다
        {
            getAxetwo = true;
        }

        if (collision.gameObject.CompareTag("water")) //웅덩이에 닿으면 된다
        {
            Debug.Log("물을 받음");
            getWater = true;
        }

        if (collision.gameObject.CompareTag("yellowChon"))//노란천
        {
            isYellow = true;
        }
    }
}
