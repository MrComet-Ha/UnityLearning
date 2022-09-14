using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI diagText;
    public GameObject diagPanel;
    public Image portImg;
    public GameObject scanObject;
    public DialogManager diagManager;
    public QuestManager questManager;
    public bool isAction;
    public int diagIndex;

    void Start()
    {
        Debug.Log(questManager.CheckQuest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action(GameObject Obj){    
        scanObject = Obj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNPC);
        diagPanel.SetActive(isAction);
    }
    void Talk(int id, bool isNPC){
        //Set Diag Data
        int questDiagIndex = questManager.GetQuestDiagIndex(id);
        string talkData = diagManager.GetDiag(id + questDiagIndex,diagIndex);
        //End Diag
        if (talkData == null){
            isAction = false;
            diagIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }
        //Continue Diag
        else{
            if(isNPC){
                diagText.text = talkData.Split(':')[0];
                //Show Portrait
                portImg.sprite = diagManager.GetPort(id, int.Parse(talkData.Split(':')[1]));
                portImg.color = new Color(1,1,1,1);
            }
            else{
                //Hide Portrait
                diagText.text = talkData;
                portImg.color = new Color(1,1,1,0);
            }
            isAction = true;
            diagIndex++;
        }    
    }
}
