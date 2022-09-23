using System.Diagnostics;
using System;
using System.IO;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //UI
    public Image[] lifeImage;
    public Image[] bombImage;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverSet;
    public TextMeshProUGUI scoreEndGame;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fade;


    //Enemy
    public ObjectManager obj;
    public String[] enemyObjs;
    public Transform[] spawnPoints;
    public List<Spawn> spawnList;
    public float nextSpawnDelay;
    public float curSpawnDelay;
    public int spawnIndex;
    public bool spawnEnd;

    //Player
    public GameObject player;
    public PlayerMovement pl;
    public Transform defaultTrans;

    //Data
    public int stage;
    public int score;



    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new String[]{"EnemyS", "EnemyM", "EnemyL","EnemyB"};
        pl = player.GetComponent<PlayerMovement>();
        StageStart();
    }

    //StageManagement
    void StageStart(){
        //StageStartUI
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<TextMeshProUGUI>().text = "STAGE " + stage + "\nSTART!";
        ReadSpawnFile();
        //Fade In
        fade.SetTrigger("In");
    }
    public void StageEnd(){
        //StageEndUI
        clearAnim.SetTrigger("On");
        clearAnim.GetComponent<TextMeshProUGUI>().text = "STAGE " + stage + "\nCLEAR!";
        //Fade Out
        fade.SetTrigger("Out");
        //Player Reposition
        player.transform.position = defaultTrans.position;
        //StageUp
        stage++;
        if(stage>2){
            Invoke("GameOver",3f);
        }
        Invoke("StageStart",3f);
    }
    void Update()
    {
        curSpawnDelay += Time.deltaTime;
        if(curSpawnDelay > nextSpawnDelay && !spawnEnd){
            SpawnEnemy();
            curSpawnDelay = 0;
        }
        scoreText.text = score.ToString();
    }

    //Life
    public void PlayerRespawn(){
        UpdateLifeIcon();
        Invoke("PlayerRespawnExq",2);
    }
    public void PlayerRespawnExq(){
        PlayerMovement pl = player.GetComponent<PlayerMovement>();
        pl.isHit = false;
        player.transform.position = defaultTrans.position;
        pl.SetInvincible();
        player.SetActive(true);
    }
    public void UpdateLifeIcon(){
        PlayerMovement pl = player.GetComponent<PlayerMovement>();
        for(int i=0; i<pl.maxLife; i++){
            lifeImage[i].color = new Color(1,1,1,0);
        }
        for(int i=0; i<pl.life; i++){
            lifeImage[i].color = new Color(1,1,1,1);
        }
    }
    public void UpdateBombIcon(){
        PlayerMovement pl = player.GetComponent<PlayerMovement>();
        for(int i=0; i<pl.maxBomb; i++){
            bombImage[i].color = new Color(1,1,1,0);
        }
        for(int i=0; i<pl.bomb; i++){
            bombImage[i].color = new Color(1,1,1,1);
        }
    }
    //Enemy
    void ReadSpawnFile(){
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;
        TextAsset textFile = Resources.Load("stage_" + stage) as TextAsset;
        StringReader strRead = new StringReader(textFile.text);
        

        while(strRead != null){
            string line = strRead.ReadLine();
            if (line == null){
                break;
            }
            Spawn spawnData = new Spawn();
            spawnData.delay =float.Parse(line.Split(',')[0]);
            spawnData.type =line.Split(',')[1];
            spawnData.point =int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }
        strRead.Close(); 
        nextSpawnDelay= spawnList[0].delay;
    }
    void SpawnEnemy(){
        int enemyIndex = 0;
        switch(spawnList[spawnIndex].type){
            case "S" :
                enemyIndex = 0;
                break;
            case "M" :
                enemyIndex = 1;
                break;
            case "L" :
                enemyIndex = 2;
                break;
            case "B" :
                enemyIndex = 3;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = obj.CreateObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyMovement enemyLogic = enemy.GetComponent<EnemyMovement>();
        enemyLogic.obj = obj;
        enemyLogic.player = player;
        enemyLogic.gm = this;
        if(enemyPoint == 6 || enemyPoint == 8){
            rigid.velocity = new Vector2(1, enemyLogic.speed*(-1));
        }
        else if(enemyPoint == 7 || enemyPoint == 9){
            rigid.velocity = new Vector2(-1, enemyLogic.speed * (-1));
        }
        else{
            rigid.velocity = new Vector2(0, enemyLogic.speed * -1);
        }
        if(enemyIndex == 1){
            UnityEngine.Vector3 dirVecM = (player.transform.position - transform.position).normalized;
                rigid.velocity = new UnityEngine.Vector3(dirVecM.x,dirVecM.y,0)*enemyLogic.speed;
                float angle = Mathf.Atan2(dirVecM.y,dirVecM.x) * Mathf.Rad2Deg;
                transform.rotation = UnityEngine.Quaternion.AngleAxis(angle+90,UnityEngine.Vector3.forward);
        }

        //Respawn Index paging
        spawnIndex++;
        if(spawnIndex == spawnList.Count){
            spawnEnd = true;
            return;
        }
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }

    //Effect
    public void CallExplosion(Vector3 pos, string type){
        GameObject explosion = obj.CreateObj("Explosion");
        Explosion expLogic = explosion.GetComponent<Explosion>();
        explosion.transform.position = pos;
        expLogic.StartExplosion(type);
    }


    //GameOver
    public void GameOver(){
        scoreEndGame.text = "Score : " + score;
        gameOverSet.SetActive(true);
    }

    //BtnAction
    public void GameRestart(){
        SceneManager.LoadScene(0);
    }
}
