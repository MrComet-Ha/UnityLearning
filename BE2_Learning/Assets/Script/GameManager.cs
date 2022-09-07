using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public int totalscore;
    public int stagescore;
    public int stageindex;
    public GameObject[] stage;
    public PlayerMovement player;
    public int health;
    public TextMeshProUGUI Stageinfo;
    public TextMeshProUGUI Score;
    public Image[] Life;
    public GameObject RestartBtn;

    void Start()
    {
        health = 3;
    }

    void Update(){
        Score.text = (totalscore + stagescore).ToString();
        Stageinfo.text = "STAGE " + (stageindex+1).ToString();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.name == "Player"){
            HPDamage(3);
        }
    }
    public void SendScore(int input){
        stagescore += input;
    }
    public void SoundManagement(string name){
        GameObject other = transform.Find(name).gameObject;
        AudioSource snd = other.GetComponent<AudioSource>();
        snd.Play();
    }

    public void HPDamage(int damage){
        health -= damage;
        if(damage == 1){
            Life[health].color = new Color(1,0,0,0.4f);
        }
        if(health <= 0){
            StageManagement(true);
            for(int i =0; i<3; i++){
                Life[i].color = new Color(1,1,1,1);
            }
            health = 3;
        }
    }
    public void StageManagement(bool isDead){
        stage[stageindex].SetActive(false);
        if(!isDead){
            stageindex ++;
            totalscore += stagescore;
        }
        stagescore = 0;      
        if(stageindex <= stage.Length -1){
            GameObject loadedstage = stage[stageindex];
            int instage = loadedstage.transform.childCount;
            for(int i = 0; i<instage; i ++){
            loadedstage.transform.GetChild(i).gameObject.SetActive(false);
            loadedstage.transform.GetChild(i).gameObject.SetActive(true);
            }
            loadedstage.SetActive(true);
            player.Reposition();
        }
        else{
            if(!isDead){
                player.gameObject.SetActive(false);
                Time.timeScale = 0;
                RestartBtn.SetActive(true);
            }
        }
    }
    public void Restart(){
        stageindex = 0;
        totalscore = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
