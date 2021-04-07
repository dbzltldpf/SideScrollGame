using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour,NewInput.IPlayerControlsActions
{
    public State state;

    public NewInput newInput;

    public Inventory inventory;
    public Equipment equipment;
    public Animator animator;
    public GameObject player;
    public EnemyController enemy;
    public GameManager manager;
    public SkillManager skillManager;
    public GameObject npc;
    public GameObject attackNseach;
    public GameObject store;
    public GameObject invenUi;
    public Scrollbar scrollbar;

    public Text[] playerStat;
    public GameObject[] playerHpBar;
    public GameObject[] playerMpBar;
    public GameObject[] playerO2Bar;
    public JsonManager jsonManager;

    //사운드
    private string walkSound_1;
    private string walkSound_2;
    private string walkSound_3;
    private string walkSound_4;

    private AudioManager theAudio;
    public GameController gameController;
    public float speed;

    public float attackTime;
    public bool attacking;

    public float combatStateTime;
    public bool combatState;
    
    public string currentMapName;
    public int Hp;
    public int Mp;
    public int O2;
    public float maxHp;
    public float maxMp;
    public float maxO2;
    public Text[] coinText;

    public Vector2 bulletDir;
    Vector2 dir;
    Vector3 dirVec;
    public GameObject scanObject;
    public Transform overlapBoxPos;

    public bool bFlag;
    public float oxygenNotExistTime = 0;
    public float oxygenExistTime = 0;
    public float healingTime = 0;
    public float currentTime = 0;

    void Awake()
    {
        newInput = new NewInput();
        newInput.PlayerControls.SetCallbacks(this);

        walkSound_1 = "footsteps 1";
        walkSound_2 = "footsteps 2";
        walkSound_3 = "footsteps 3";
        walkSound_4 = "footsteps 4";
    }
    void Start()
    {
        bFlag = false;
        state = State.Idle;
        attacking = false;
        animator = GetComponent<Animator>();
        theAudio = FindObjectOfType<AudioManager>();
        Hp = jsonManager.playerState.character[0].Hp;
        Mp = jsonManager.playerState.character[0].Mp;
        O2 = 0;
        maxHp = Hp;
        maxMp = Mp;
        maxO2 = jsonManager.playerState.character[0].O2;
        coinText[0].text = Convert.ToString(jsonManager.playerState.character[0].Money);
        coinText[1].text = Convert.ToString(jsonManager.playerState.character[0].Energy);
        npc.SetActive(false);
        PlayerStateSet();
        equipment.weaponType = "unarmed";
        combatState = false;

    }
    void OnEnable() => newInput.PlayerControls.Enable();
    void OnDisable() => newInput.PlayerControls.Disable();

    private void Update()
    {
        //조이스틱 움직일 때 제어값 변경
        if (dir.x != 0 && dir.y != 0)
        {
            //프레임 연산
            currentTime += Time.deltaTime;
            bFlag = true;
        }
 

        PlaySound();

        //조사 센서 방향
        if (dir.x < 0)
        {
            dirVec = Vector3.left;
        }
        else if (dir.x > 0)
        {
            dirVec = Vector3.right;
        }

        //npc.setActive
        if (currentMapName == "village")
        {
            npc.SetActive(true);
           
        }
        else
        {
            npc.SetActive(false);

        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(overlapBoxPos.position, new Vector2(0.6f, 0.6f));
    }

    private void FixedUpdate()
    {
        //조사할때 필요한 센서 (박스)
        Collider2D Hit = Physics2D.OverlapBox(overlapBoxPos.position, new Vector2(0.6f,0.6f),0, LayerMask.GetMask("Object"));

        if (Hit != null && !combatState)
        {
            state = State.Check;
            scanObject = Hit.gameObject;
            
        }
        else
        {
            scanObject = null;
        }

        //조사할때 필요한 센서 (레이져)
        //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(0, 1, 0));
        //RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 0.7f, LayerMask.GetMask("Object"));
        //
        //if (rayHit.collider != null)
        //{
        //    state = State.Check;
        //    scanObject = rayHit.collider.gameObject;
        //}
        //else
        //{
        //    scanObject = null;
        //}

        // 공격에 필요한 코드 
        // 전투상태에 대해 생각해보기
        if (state == State.Attack)
        {
            state = State.Combat;
        }
        else if (state != State.Action)
        {
            transform.Translate(dir * Time.deltaTime * speed);
        }
        if (state == State.Combat)
        {
            combatState = true;
            combatStateTime += Time.deltaTime;
            attackTime += Time.deltaTime;
        }

        if (combatStateTime > 3f || state == State.Move)
        {
            combatState = false;
            combatStateTime = 0;
            state = State.Idle;

        }
        if (attackTime > 0.5f) 
        {
            attacking = false;
            attackTime = 0;
        }

        SafetyZone();
        Oxygen();
    }
    private bool SafetyZone()
    {
        if (currentMapName == "village")
        {
            O2 = (int)maxO2;
            PlayerStateSet();
            oxygenExistTime = 0;
            oxygenNotExistTime = 0;
            
            if (Hp < maxHp)
            {
                healingTime++;

                if (healingTime > 200f)
                {
                    Hp += 1;
                    healingTime = 0;
                }
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Oxygen()
    {
        if (SafetyZone() == false)
        {
            if (O2 < 1)
            {
                oxygenNotExistTime++;

                if (oxygenNotExistTime > 50f)
                {
                    Hp -= 1;
                    PlayerStateSet();
                    oxygenNotExistTime = 0;
                }

            }
            else
            {
                oxygenExistTime++;
                if (oxygenExistTime > 200f)
                {
                    O2 -= 1;
                    PlayerStateSet();
                    oxygenExistTime = 0;
                }
            }
        }
     
    }
    //스텟이 변경됐을때 알려줌
    public void PlayerStateSet()
    {
        playerStat[0].text = jsonManager.playerState.character[0].Name;
        playerStat[1].text = $"{Hp} / {jsonManager.playerState.character[0].Hp} ";
        playerStat[2].text = $"{Hp} / {jsonManager.playerState.character[0].Hp} ";
        playerStat[3].text = $"{Mp} / {jsonManager.playerState.character[0].Mp} ";
        playerStat[4].text = $"{Mp} / {jsonManager.playerState.character[0].Mp} ";
        playerStat[5].text = $"{O2} / {jsonManager.playerState.character[0].O2} ";
        playerStat[6].text = $"{O2} / {jsonManager.playerState.character[0].O2} ";
        playerStat[7].text = jsonManager.playerState.character[0].Str.ToString();
        playerStat[8].text = jsonManager.playerState.character[0].Int.ToString();
        playerStat[9].text = jsonManager.playerState.character[0].Dex.ToString();
        playerStat[10].text = jsonManager.playerState.character[0].Con.ToString();

        PlayerHpBar();
        PlayerMpBar();
        Player02Bar();
    }
    public void PlayerCoinState()
    {
        coinText[0].text = Convert.ToString(jsonManager.playerState.character[0].Money);
        coinText[1].text = Convert.ToString(jsonManager.playerState.character[0].Energy);
    }
    private void PlayerHpBar()
    {
        for (int i = 0; i < playerHpBar.Length; i++)
        {
            playerHpBar[i].GetComponent<Image>().fillAmount = Mathf.Clamp01(Hp / maxHp);
        }
    }
    private void PlayerMpBar()
    {
        for (int i = 0; i < playerMpBar.Length; i++)
        {
            playerMpBar[i].GetComponent<Image>().fillAmount = Mathf.Clamp01(Mp / maxMp);
        }
    }
    private void Player02Bar()
    {
        for (int i = 0; i < playerO2Bar.Length; i++)
        {
            playerO2Bar[i].GetComponent<Image>().fillAmount = Mathf.Clamp01(O2 / maxO2);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!manager.isAction && !attacking && !skillManager.casting && state != State.Check)
        {
            if (context.started)
            {
                Debug.Log("공격");
                state = State.Attack;
                attacking = true;
                animator.SetTrigger("Attack");


                if (equipment.longDistanceWeapon)
                {
                    gameController.CreateBullet();
                }

            }
        }
    }
    public void OnLook(InputAction.CallbackContext context)
    {
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        // !조사 && !공격 && !스킬캐스팅
        if (!manager.isAction && !attacking && state != State.Action)
        {
            state = State.Move;
            dir = context.ReadValue<Vector2>();

            //방향
            if (dir.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                bulletDir = new Vector2(1f, 1f);
            }
            else if (dir.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                bulletDir = new Vector2(-1f, 1f);
            }
            animator.SetFloat("Speed", dir.magnitude);

            if (context.canceled)
            {
                state = State.Idle;
            }
        }
      
    }
    public void OnSearch(InputAction.CallbackContext context)
    {
        if (state == State.Check)
        {
            //Debug.DrawRay(transform.position, dirVec * 0.7f, new Color(0, 1, 0));
            //RaycastHit2D rayHit = Physics2D.Raycast(transform.position, dirVec, 0.7f, LayerMask.GetMask("Object"));
            //
            //if (rayHit.collider != null)
            //{
            //    state = State.Check;
            //    scanObject = rayHit.collider.gameObject;
            //}
            //else
            //{
            //    scanObject = null;
            //}
            

            state = State.Search;

            if (scanObject.tag != "Item")
            {
                manager.Action(scanObject);
            }
            if (scanObject.tag == "Item")
            {
                if (context.started)
                {
                    //아이템 겹쳐져 있을때 하나씩만 먹어지게 해야함
                    //구상 하는거 잊지않기!!!!
                    Debug.Log(scanObject.GetComponent<SpriteRenderer>().sprite.name);
                    scanObject.name = scanObject.GetComponent<SpriteRenderer>().sprite.name;
                    int itemId = Convert.ToInt32(scanObject.name);


                    invenUi.SetActive(true);

                    bool success = inventory.CreateItemToInventory(itemId);

                    if (success)
                    {
                        scanObject.SetActive(false);
                    }
                    else
                    {
                        Debug.Log("슬롯이 가득 참");
                    }

                    invenUi.SetActive(false);
                }
               
            }
        }

    }
    public void GetMoney()
    {
        //silver 1000 == energy 1의 가치(정해야 함)

        jsonManager.playerState.character[0].Money+= 10;
        PlayerCoinState();
    }
    public void PlaySound()
    {
        //걷는 소리
        int temp = UnityEngine.Random.Range(1, 4);
        if (bFlag && (currentTime >= 0.3f))    // 0.5f는 0.5초당 1회 라고 보시면됩니다. (60프레임 1초)
        {
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }
            theAudio.SetVolumn(walkSound_1, 0.1f);
            theAudio.SetVolumn(walkSound_2, 0.1f);
            theAudio.SetVolumn(walkSound_3, 0.1f);
            theAudio.SetVolumn(walkSound_4, 0.1f);

            // 사운드 출력시 현제 프레임값 제거 (update는 정확히 60프레임을 가르키지 않기 때문에 자기자신을 뺍니다.)
            currentTime -= currentTime;
            // 사운드 출력후 제어값 변경
            bFlag = false;
        }

    }

}
