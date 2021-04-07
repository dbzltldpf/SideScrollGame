using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TalkManager : MonoBehaviour
{
    public Dictionary<int, string[]> talkData;


    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        //talk data
        //npc :1000, npc1 :2000
        talkData.Add(1000, new string[] {"안녕?",
                                         "이 곳에 처음 왔구나?",
                                         "한번 둘러보도록 해." });

        talkData.Add(2000, new string[] { "여어.",
                                          "이 마을은 정말 아름답지?",
                                          "사실 이 마을에는 무언가의 비밀이 숨겨져있어."});
       
        talkData.Add(100, new string[] { "평범한 나무상자다." });

        //quest talk
        talkData.Add(10 + 1000, new string[] {"어서 와.",
                                              "이 마을에 놀라운 전설이 있다는데",
                                              "첫번째 왼쪽 집에 루도가 알려줄꺼야." });
        talkData.Add(11 + 2000, new string[] {"여어.",
                                              "이 마을에 전설을 들으러 온거야?",
                                              "그럼 일 좀 하나 해주면 좋을텐데...",
                                              "내 집 근처에 떨어진 동전 좀 주워줬으면 해." });
        talkData.Add(20 + 1000, new string[] {"루도의 동전?.",
                                              "돈을 흘리고 다니면 못쓰지!",
                                              "나중에 루도에게 한마디 해야겠어."});
        talkData.Add(20 + 2000, new string[] { "찾으면 꼭 좀 가져다 줘." });
        talkData.Add(20 + 5000, new string[] { "근처에서 동전을 찾았다." });

        talkData.Add(21 + 2000, new string[] { "엇, 찾아줘서 고마워!" });

    }
    public string GetTalk(int id, int talkIndex)
    {
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                //퀘스트 맨처음 대사마저 없을때.
                //기본 대사를 자기고 온다.
                return GetTalk(id - id % 100, talkIndex); //반환 값이 있는 재귀함수는 return까지 꼭 써줘야 함.
            }
            else
            {
                //해당 퀘스트 진행 순서 대사가 없을때.
                //퀘스트 맨 처음 대사를 가지고 온다.
                return GetTalk(id - id % 10, talkIndex);
            }
        }
        if (talkIndex == talkData[id].Length)
        {
            return null;
        }
        else
        {
            return talkData[id][talkIndex];

        }

    }

}
