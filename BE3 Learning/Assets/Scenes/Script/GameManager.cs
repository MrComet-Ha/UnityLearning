using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isAction;
    public int diagIndex;
    public GameObject scanObject;
    public GameObject player;
    public Animator diagPanel;
    public TextMeshProUGUI diagText;
    public TextEffect diagEffect;
    public Image portImg;
    public Animator portAnim;
    public Sprite portPrev;
    public DialogManager diagManager;
    public TextMeshProUGUI questName;
    public QuestManager questManager;
    public GameObject menu;

    void Start()
    {
        GameLoad();
        questName.text = questManager.CheckQuest();
    }

    // Update is called once per frame
    void Update()
    {
        //menu
        if(Input.GetButtonDown("Cancel")){
            if(menu.activeSelf){
                menu.SetActive(false);
            }
            else{
                menu.SetActive(true);
            } 
        }
    }

    public void Action(GameObject Obj){    
        scanObject = Obj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk(objData.id, objData.isNPC);
        diagPanel.SetBool("isTalk",isAction);
    }
    void Talk(int id, bool isNPC){
        int questDiagIndex = 0;
        string talkData = "";
        if(diagEffect.isAnim)
            diagEffect.isAnim = false;
        //Set Diag Data
        else{
            questDiagIndex = questManager.GetQuestDiagIndex(id);
            talkData = diagManager.GetDiag(id + questDiagIndex,diagIndex);
        //End Diag
            if (talkData == null){
                isAction = false;
                diagIndex = 0;
                questName.text = questManager.CheckQuest(id);
                return;
            }
            //Continue Diag
            else{
                if(isNPC){
                    diagText.text = talkData.Split(':')[0];
                    diagEffect.StartCoroutine("TextAnim",talkData.Split(":")[0].Length);
                    //Show Portrait
                    portImg.sprite = diagManager.GetPort(id, int.Parse(talkData.Split(':')[1]));
                    portImg.color = new Color(1,1,1,1);
                    if(portPrev != portImg.sprite){
                        portAnim.SetTrigger("doEffect");
                        portPrev = portImg.sprite;
                    }    
                }
                else{
                    //Hide Portrait
                    diagText.text = talkData;
                    diagEffect.StartCoroutine("TextAnim",talkData.Length);
                    portImg.color = new Color(1,1,1,0);
                }
            }     
            isAction = true;
            diagIndex++;
        }
    }  
    public void GameSave(){
        PlayerPrefs.SetFloat("PlayerX",player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY",player.transform.position.y);
        PlayerPrefs.SetInt("QuestId",questManager.questId);
        PlayerPrefs.SetInt("QuestActIndex",questManager.questActIndex);
        PlayerPrefs.Save();
    }
    public void GameLoad(){

        if(!PlayerPrefs.HasKey(""))
            return;
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questid = PlayerPrefs.GetInt("QuestId");
        int questactindex = PlayerPrefs.GetInt("QuestActIndex");

        player.transform.position = new UnityEngine.Vector3(x,y,0);
        questManager.questId = questid;
        questManager.questActIndex = questactindex;
        questManager.ControlObject();
    }
    public void GameExit(){
        Application.Quit();
    }
}