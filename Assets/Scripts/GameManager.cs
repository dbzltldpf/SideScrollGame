using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public QuestManager questManager;
    public GameObject talkPanel;
    public Text talkText;
    public Text questText;
    public GameObject uiPanel;
    public Text uiText;


    public GameObject joystickPanel;
    public GameObject scanObject;
    public PlayerController player;
    public bool isAction;
    public float notifyTime;
    public int talkIndex;

    private void Start()
    {
        questText.text = questManager.CheckQuest(questManager.questId);
    }
    public void FixedUpdate()
    {
        if(uiText.text != "")
        {
            uiPanel.SetActive(true);
            notifyTime += Time.deltaTime;
        }
        if (notifyTime >= 2f)
        {
            uiText.text = "";
            notifyTime -= notifyTime;
            uiPanel.SetActive(false);
        }
        
    }
    //npc와 말하기
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.objId, objData.isNpc);

        talkPanel.SetActive(isAction);
        joystickPanel.SetActive(!isAction);
    }
    public void Notify(string notify)
    {
        uiText.text = notify;
    }
    // panel.text == "" setactive false
    // 스킬 누르면 text = notify -> setactive true
    void Talk(int id, bool isNpc)
    {
        //talk data 시작
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        string talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);

        //talk data 끝
        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            //Debug.Log(questManager.CheckQuest(id));
            return; //void 함수에서 return은 강제종료
        }
        if (isNpc)
        {
            talkText.text = talkData;
        }
        else
        {
            talkText.text = talkData;
        }
        isAction = true;
        talkIndex++;


    }
    public void Click()
    {
        Action(player.scanObject);
    }
}
