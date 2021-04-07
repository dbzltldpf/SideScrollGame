using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public PlayerController player;
    public GameManager gameManager;
    public Cooldown[] cooldown;
    public GameController gameController;
    public GameObject skillAnimator;
    public GameObject[] objs; //objs : [0]-힐,[1]-기본공격스킬,[2]-마나회복,[3]-활스킬
    public Button[] objButton;

    public float skillTime;
    public bool casting;
    private int HPS;
    private int[] MPS;

    public GameObject[] coolTimePanel;

    private string HpsSoundName;
    private string[] SkillSound;


    public void Awake()
    {
        HPS = 1;
        MPS = new int[4];
        SkillSound = new string[3];

        cooldown = new Cooldown[objs.Length];
        objButton = new Button[objs.Length];

        for (int i = 0; i < objs.Length; i++)
        {
            objButton[i] = objs[i].GetComponent<Button>();
            cooldown[i] = objs[i].GetComponent<Cooldown>();
        }

    }
    public void Start()
    {
        player.attacking = false;
        objs[3].SetActive(false);
 
        SkillSound[0] = "텔레포트.";
        SkillSound[1] = "공기를 정화 합니다.";
        SkillSound[2] = "바람 화살";
        HpsSoundName = $"{HPS}만큼 치유됨";

        MPS[0] = 2;
        MPS[1] = 1;
        MPS[2] = 1;
        MPS[3] = 1;
        casting = false;

    }

    public void FixedUpdate()
    {
       if (casting)
       {
           skillTime += Time.deltaTime;
       }
       if (skillTime > 0.5f)
       {
           player.state = State.Idle;
           skillAnimator.SetActive(false);
           casting = false;
           skillTime = 0;
       }

       WeaponTypeCheck();

    }
    //무기의 타입별로 스킬만들어줌
    public void WeaponTypeCheck()
    {
        switch (player.equipment.weaponType)
        {
            case "unarmed":
                objs[3].SetActive(false);
                break;
            case "sword":
                objs[3].SetActive(false);
                break;
            case "bow":
                objs[3].SetActive(true);
                break;
            default:
                break;
        }

        for (int i = 0; i < objs.Length; i++)
        {
            if (player.Hp == player.jsonManager.playerState.character[0].Hp)
            {
                objButton[0].interactable = false;
            }

            if (player.attacking || player.Mp < MPS[i])
            {
                objButton[i].interactable = false;

            }
            else
            {
                objButton[i].interactable = true;

                if (casting)
                {
                    objButton[0].interactable = false;
                }
            }
        }
    }
    public void SetNum(int num)
    {
        WeaponTypeCheck();

        switch (num)
        {
            case 0:
                cooldown[0].enabled = true;
                Heal(num);
                break;
            case 1:
                cooldown[1].enabled = true;
                Skill01(num);
                break;
            case 2:
                cooldown[2].enabled = true;
                MpRecovery(num);
                break;
            case 3:
                cooldown[3].enabled = true;
                Arrow(num);
                break;
            default:
                break;
        }
    }

    public void Heal(int _num)
    {
        //coolTime == false
        if (!cooldown[_num].GetFlag())
        {
            player.state = State.Action;

            player.Hp += HPS;
            player.Mp -= MPS[0];
            player.PlayerStateSet();
            gameManager.Notify(HpsSoundName);
            player.animator.SetTrigger("Skill");
            coolTimePanel[_num].SetActive(true);
            casting = true;
           
        }
        
    }
    public void Skill01(int _num)
    {
        if (!cooldown[_num].GetFlag())
        {
            player.state = State.Action;
            player.Mp -= MPS[1];
            player.PlayerStateSet();
            gameManager.Notify(SkillSound[0]);
            coolTimePanel[_num].SetActive(true);
            //player.
            casting = true;

            //기존의 공격 스킬 이었던 아이
            //player.state = State.Action;
            //gameController.damageType = "skill01";
            //
            //player.Mp -= MPS[1];
            //player.PlayerStateSet();
            //gameManager.Notify(SkillSound[0]);
            //coolTimePanel[_num].SetActive(true);
            //player.animator.SetTrigger("Skill");
            //skillAnimator.SetActive(true);
            //casting = true;
        }
    }
    public void MpRecovery(int _num)
    {
        if (!cooldown[_num].GetFlag())
        {
            player.Mp -= MPS[2];
            player.state = State.Action;
            player.O2++;
            gameController.damageType = "mpRecovery";
            gameManager.Notify(SkillSound[1]);
            player.PlayerStateSet();


            
            player.animator.SetTrigger("Skill");
            coolTimePanel[_num].SetActive(true);
            casting = true;
        }
    }
    // 활 사용 할때 만 활성화
    public void Arrow(int _num)
    {
        if (!cooldown[_num].GetFlag())
        {
            player.state = State.Action;
            gameController.damageType = "arrow";

            player.Mp -= MPS[2];
            player.PlayerStateSet();
            gameManager.Notify(SkillSound[2]);
            coolTimePanel[_num].SetActive(true);
            gameController.SkillBullet();
            FindObjectOfType<Bullet>().transform.GetChild(1).gameObject.SetActive(true);
            casting = true;
        }
    }
}
