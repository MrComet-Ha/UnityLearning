using System;
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
    public float maxSpawnDelay;
    public float curSpawnDelay;
    Transform spawnpoint;
    void Awake()
    {
        enemyObjs = new String[]{"EnemyS", "EnemyM", "EnemyL"};
        pl = player.GetComponent<PlayerMovement>();
        spawnpoint = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        curSpawnDelay += Time.deltaTime;
        if(curSpawnDelay > maxSpawnDelay){
            SpawnEnemy();
            maxSpawnDelay = UnityEngine.Random.Range(0.5f,3f);
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
        player.transform.position = spawnpoint.position;
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
    void SpawnEnemy(){
        int ranEnemy = UnityEngine.Random.Range(0,3);
        int ranPoint = UnityEngine.Random.Range(0,9);
        GameObject enemy = obj.CreateObj(enemyObjs[ranEnemy]);
        enemy.transform.position = spawnPoints[ranPoint].position;
        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        EnemyMovement enemyLogic = enemy.GetComponent<EnemyMovement>();
        enemyLogic.obj = obj;
        enemyLogic.player = player;
        if(ranPoint == 5 || ranPoint == 7){
            rigid.velocity = new Vector2(enemyLogic.speed, 1);
        }
        else if(ranPoint == 6 || ranPoint == 8){
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), 1);
        }
        else{
            rigid.velocity = new Vector2(0, enemyLogic.speed * -1);
        }
        if(ranEnemy == 1){
            UnityEngine.Vector3 dirVecM = (player.transform.position - transform.position).normalized;
                rigid.velocity = new UnityEngine.Vector3(dirVecM.x,dirVecM.y,0)*enemyLogic.speed;
                float angle = Mathf.Atan2(dirVecM.y,dirVecM.x) * Mathf.Rad2Deg;
                transform.rotation = UnityEngine.Quaternion.AngleAxis(angle+90,UnityEngine.Vector3.forward);
        }
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
