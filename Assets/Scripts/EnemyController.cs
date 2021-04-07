using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool MoveRight;
    public Animator animator;
    public float stayTime;
    public float attackTime;
    public bool underAttack;
    public bool backAttack;
    public float invincibleTime;

    public int Hp;
    public GameObject enemyHpBar;
    public float maxHp;

    public int GetExp;
    private float attackDistance;

    private DamageManager damageManager;

    //길찾기 구현하기
    public Vector3 destinationPosition; // 목적지
    private PathFinding pathFinding;
    private Tilemaping tilemaping;
    Vector3 vPosCorrection; //걷는걸 타일에 맞춰 걷게하기 위한 변수
    public Vector3 beforeTarget; //타겟이 이동하면 다시 findpath를 돌리기위한 변수 
    float currentTime;
    float oldTime;

    public GameObject enemy;
    [SerializeField] private PlayerController player;
    public GameController gameController;
    [SerializeField] private WeaponBoard weaponBoard;

    private void Awake()
    {
        damageManager = FindObjectOfType<DamageManager>();
        pathFinding = FindObjectOfType<PathFinding>();
        tilemaping = FindObjectOfType<Tilemaping>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        gameController = FindObjectOfType<GameController>();
        weaponBoard = GameObject.Find("UICanvus").transform.GetChild(0).GetComponentInChildren<WeaponBoard>();
    }
    private void Start()
    {
        vPosCorrection = new Vector3(0.5f, 0.5f, 0f);
        currentTime = 0f;
        oldTime = 0.5f;
        maxHp = Hp;
    }
    void OnEnable()
    {
        transform.position = transform.parent.position;
        destinationPosition = transform.position;
        attackTime = 0f;
        stayTime = 0f;
        Hp = 3;
        GetExp = 1;
        MoveRight = true;
    }

    public void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        enemyHpBar.GetComponent<Image>().fillAmount = Mathf.Clamp01(Hp / maxHp);

        // target이 있을때
        if (target != null)
        {
            // 목적지에 타겟 포지션 넣어줌
            destinationPosition = target.position;

            // player와의 거리
            attackDistance = Vector2.Distance(transform.position, target.position);

            if (attackDistance > 1f)
            {
                if (beforeTarget != Vector3Int.FloorToInt(destinationPosition) && currentTime >= oldTime)
                {
                    pathFinding.FindPath(transform.position, destinationPosition, FinishedProcessingPath);
                    beforeTarget = Vector3Int.FloorToInt(destinationPosition);
                    currentTime -= oldTime;
                }

                animator.SetFloat("Speed", 1f);
            }
            else if (attackDistance <= 1f)
            {
                if (attackTime > 2f)
                {
                    animator.SetFloat("Speed", 0f);
                    animator.SetTrigger("Attack");
                    player.Hp--;
                    player.PlayerStateSet();
                    attackTime = 0f;
                }
            }
            attackTime += Time.deltaTime;
            FlipFacing();
        }

        //target이 없을때
        else
        {
            //Debug.Log(Vector2.Distance(transform.position, destinationPosition));
            // enemy의 위치와 목적지의 거리가 0.1보다 작으면 //목적지에 도착했을 때
            if (Vector2.Distance(transform.position, destinationPosition) < 2.1f)
            {
                stayTime += Time.deltaTime;
                animator.SetFloat("Speed", 0f);

                if (stayTime > 2f)
                {
                    //랜덤으로 목적지 위치 생성
                    SetRedirection();
                    stayTime = 0;
                }
            }
            else
            {
                pathFinding.FindPath(transform.position, destinationPosition, FinishedProcessingPath);
                animator.SetFloat("Speed", 1f);
            }
        }

        if (underAttack)
        {
            invincibleTime += Time.deltaTime;
            enemy.GetComponent<BoxCollider2D>().isTrigger = true;

        }
        if (invincibleTime > 1f)
        {
            underAttack = false;
            enemy.GetComponent<BoxCollider2D>().isTrigger = false;
            invincibleTime = 0;
        }

    }

    private void FinishedProcessingPath(Stack<Node> path, bool Successed)
    {
        StopCoroutine("FollowPath");

        if (Successed)
        {
            StartCoroutine("FollowPath", path);
        }
    }

    IEnumerator FollowPath(Stack<Node> path)
    {
        Node nextNode = path.Pop();
        nextNode = path.Pop();

        Vector3 nextPosition = nextNode.Position + vPosCorrection;

        while (path.Count > 0)
        {
            if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
            {
                nextNode = path.Pop();
                nextPosition = nextNode.Position + vPosCorrection;
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Time.deltaTime * speed);

            yield return new WaitForSeconds(0.02f);
        }

        yield return null;
    }

    private void SetRedirection()
    {
        bool bFlag = false;
        while (!bFlag)
        {
            destinationPosition = new Vector3(Random.Range(-14f, 14f), Random.Range(8f, 16f), 0f);
        
            //Random.Range(-14f, 14f), Random.Range(8f, 16f) : dungeon
            //Random.Range(-14f, 14f), Random.Range(-19f, -22f) : village
        
            foreach (var pos in tilemaping.nodes)
            {
                if (pos.Key == Vector3Int.FloorToInt(destinationPosition))
                {
                    if (pos.Value.Walkable == true)
                    {
                        bFlag = true;
                        //Debug.Log($"목적지 : {destinationPosition},{pos.Value.Walkable}");
                    }
                }
            }
        }

        FlipFacing();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                collision.isTrigger = true;
                break;
            case "Skill":
                if (!underAttack)
                {
                    switch (gameController.damageType)
                    {
                        case "skill01":
                            OnDamage(3);
                            Debug.Log("skill01");
                            break;
                    }
                    underAttack = true;
                }
                break;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                collision.isTrigger = false;
                break;
        }
    }

    public void OnDamage(int _damage)
    {

        //오류(수정 완료) : 죽을때는 데미지 안나왔다가 다시 적이 재생성 될때 데미지뜸
        //오류(수정 완료) : HpBar & Damage enemyScale이 바뀔 때 같이 바뀜
        //오류(수정 완료) : 죽으면서 아이템 떨어질 때 센서에 닿이면 에러 // combat상태를 만들었는데 저 조정해야함
        underAttack = true;

        damageManager.Create(transform.position, _damage);
        Hp -= _damage;

        animator.SetTrigger("UnderAttack");

        if (Hp <= 0)
        {
            enemy.transform.parent.gameObject.SetActive(false);
            gameController.CreateCoin(Vector3Int.FloorToInt(gameObject.transform.position));
            gameController.CreateItem(Vector3Int.FloorToInt(gameObject.transform.position));
            weaponBoard.ExpUp();
        }

        if (!backAttack)
        {
            if (MoveRight)
            {
                enemy.transform.position = new Vector2(transform.position.x - 1, transform.position.y);

            }
            else
            {
                enemy.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }
        else
        {
            if (MoveRight)
            {
                enemy.transform.position = new Vector2(transform.position.x + 1, transform.position.y);

            }
            else
            {
                enemy.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
        }

    }
    // 보는 방향 바꿔주기
    void FlipFacing()
    {
        if (destinationPosition.x < transform.position.x)
        {
            MoveRight = false;
            transform.localScale = new Vector2(-1, 1);
            

        }
        else
        {
            MoveRight = true;
            transform.localScale = new Vector2(1, 1);

        }
    }
}
