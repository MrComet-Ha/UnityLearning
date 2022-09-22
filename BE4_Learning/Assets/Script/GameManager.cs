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
    //Reference
    public ObjectManager obj;
    public String[] enemyObjs;
    public Transform[] spawnPoints;
    public Image[] lifeImage;
    public Image[] bombImage;
    public TextMeshProUGUI scoreText;
    public GameObject player;
    public GameObject gameOverSet;
    public PlayerMovement pl;

    //Data
    public int score;
    public float nextSpawnDelay;
    public float curSpawnDelay;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new String[]{"EnemyS", "EnemyM", "EnemyL"};
        ReadSpawnFile();
        pl = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
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
        player.transform.position = new Vector3(0,-4,0);
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
        TextAsset textFile = Resources.Load("stage0") as TextAsset;
        StringReader strRead = new StringReader(textFile.text);
        

        while(strRead != null){
            string line = strRead.ReadLine();
            if (line == null){
                break;
            }
            Debug.Log(line);
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
        }
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = obj.CreateObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyIndex].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyMovement enemyLogic = enemy.GetComponent<EnemyMovement>();
        enemyLogic.obj = obj;
        enemyLogic.player = player;
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
    //GameOver
    public void GameOver(){
        gameOverSet.SetActive(true);
    }

    //BtnAction
    public void GameRestart(){
        SceneManager.LoadScene(0);
    }
}
