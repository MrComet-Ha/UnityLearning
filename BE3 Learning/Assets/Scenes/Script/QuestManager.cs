using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    Dictionary<int, QuestData> questList;
    public GameObject[] questObject;
    public int questId;
    public int questActIndex;

    void Awake() {
        questList = new Dictionary<int, QuestData>();
        GenerateData();    
    }
    
    void GenerateData(){
        questList.Add(10, new QuestData("초면인데", new int[]{1000, 100, 300}));
        questList.Add(20, new QuestData("안아줘요", new int[]{2000, 1000}));
        questList.Add(30, new QuestData("퀘스트 종료", new int[]{0}));
    }

    public int GetQuestDiagIndex(int id){
        return questId + questActIndex;
    }
    public string CheckQuest(int id){
        if(id == questList[questId].npcId[questActIndex])
            questActIndex ++;
        ControlObject();
        if(questActIndex == questList[questId].npcId.Length){
            NextQuest();
        }

        return questList[questId].questName;
    }
    public string CheckQuest(){
        return questList[questId].questName;
    }
    void NextQuest(){
        questId += 10;
        questActIndex = 0;
    }
    void ControlObject(){
        switch(questId){
            case 10 :
                if(questActIndex == 2){
                    questObject[0].SetActive(false);
                    questObject[1].SetActive(true);
                }
                if(questActIndex == 3){
                    questObject[1].SetActive(false);
                }
                break;
        }
    }
}
