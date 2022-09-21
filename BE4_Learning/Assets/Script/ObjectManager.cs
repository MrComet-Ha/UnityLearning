using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //Prefabs
    public GameObject EnemySPrefab;
    public GameObject EnemyMPrefab;
    public GameObject EnemyLPrefab;
    public GameObject PowerPrefab;
    public GameObject BombPrefab;
    public GameObject RapidPrefab;
    public GameObject ShotPrefab;
    public GameObject HealPrefab;
    public GameObject CoinPrefab;
    public GameObject SubPrefab;
    public GameObject EnemyBulletAPrefab;
    public GameObject EnemyBulletBPrefab;
    public GameObject PlayerBulletAPrefab;
    public GameObject PlayerBulletBPrefab;

    //Enemies
    GameObject[] enemyS;
    GameObject[] enemyM;
    GameObject[] enemyL;

    //Items
    GameObject[] power;
    GameObject[] bomb;
    GameObject[] rapid;
    GameObject[] shot;
    GameObject[] heal;
    GameObject[] coin;
    GameObject[] sub;

    //Bullets
    GameObject[] enemyBulletA;
    GameObject[] enemyBulletB;
    GameObject[] playerBulletA;
    GameObject[] playerBulletB;

    public GameObject[] targetPool;


    void Awake(){
        enemyS = new GameObject[20];
        enemyM = new GameObject[10];
        enemyL = new GameObject[10];

        power = new GameObject[10];
        bomb = new GameObject[10];
        rapid = new GameObject[10];
        shot = new GameObject[10];
        heal = new GameObject[10];
        coin = new GameObject[10];
        sub = new GameObject[10];

        enemyBulletA = new GameObject[200];
        enemyBulletB = new GameObject[200];
        playerBulletA = new GameObject[100];
        playerBulletB = new GameObject[100];

        Generate();
    }

    void Generate(){
        for(int i=0; i<enemyS.Length;i ++){
            enemyS[i] = Instantiate(EnemySPrefab);
            enemyS[i].SetActive(false);
        }
        for(int i=0; i<enemyM.Length;i ++){
            enemyM[i] = Instantiate(EnemyMPrefab);
            enemyM[i].SetActive(false);
        }
        for(int i=0; i<enemyL.Length;i ++){
            enemyL[i] = Instantiate(EnemyLPrefab);
            enemyL[i].SetActive(false);
        }
        for(int i=0; i<power.Length;i ++){
            power[i] = Instantiate(PowerPrefab);
            power[i].SetActive(false);
        }
        for(int i=0; i<bomb.Length;i ++){
            bomb[i] = Instantiate(BombPrefab);
            bomb[i].SetActive(false);
        }
        for(int i=0; i<rapid.Length;i ++){
            rapid[i] = Instantiate(RapidPrefab);
            rapid[i].SetActive(false);
        }
        for(int i=0; i<shot.Length;i ++){
            shot[i] = Instantiate(ShotPrefab);
            shot[i].SetActive(false);
        }
        for(int i=0; i<heal.Length;i ++){
            heal[i] = Instantiate(HealPrefab);
            heal[i].SetActive(false);
        }
        for(int i=0; i<coin.Length;i ++){
            coin[i] = Instantiate(CoinPrefab);
            coin[i].SetActive(false);
        }
        for(int i=0; i<sub.Length;i ++){
            sub[i] = Instantiate(SubPrefab);
            sub[i].SetActive(false);
        }
        for(int i=0; i<enemyBulletA.Length;i ++){
            enemyBulletA[i] = Instantiate(EnemyBulletAPrefab);
            enemyBulletA[i].SetActive(false);
        }
        for(int i=0; i<enemyBulletB.Length;i ++){
            enemyBulletB[i] = Instantiate(EnemyBulletBPrefab);
            enemyBulletB[i].SetActive(false);
        }
        for(int i=0; i<playerBulletA.Length;i ++){
            playerBulletA[i] = Instantiate(PlayerBulletAPrefab);
            playerBulletA[i].SetActive(false);
        }
        for(int i=0; i<playerBulletB.Length;i ++){
            playerBulletB[i] = Instantiate(PlayerBulletBPrefab);
            playerBulletB[i].SetActive(false);
        }
    }

    public GameObject CreateObj(string type){
        switch(type){
            case "EnemyS": 
                targetPool = enemyS;
                break;
            case "EnemyM": 
                targetPool = enemyM;
                break;
            case "EnemyL": 
                targetPool = enemyL;
                break;
            case "Power": 
                targetPool = power;
                break;
            case "Bomb": 
                targetPool = bomb;
                break;
            case "Rapid": 
                targetPool = rapid;
                break;
            case "Shot": 
                targetPool = shot;
                break;
            case "Heal": 
                targetPool = heal;
                break;
            case "Coin": 
                targetPool = coin;
                break;
            case "Sub": 
                targetPool = sub;
                break;
            case "EnemyBulletA": 
                targetPool = enemyBulletA;
                break;
            case "EnemyBulletB": 
                targetPool = enemyBulletB;
                break;
            case "PlayerBulletA": 
                targetPool = playerBulletA;
                break;
            case "PlayerBulletB": 
                targetPool = playerBulletB;
                break;
        }
        Debug.Log(targetPool);
        for(int i=0;i<targetPool.Length;i++){
            if(!targetPool[i].activeSelf){
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }

    public GameObject[] GetPool(string type){
        switch(type){
            case "EnemyS": 
                targetPool = enemyS;
                break;
            case "EnemyM": 
                targetPool = enemyM;
                break;
            case "EnemyL": 
                targetPool = enemyL;
                break;
            case "Power": 
                targetPool = power;
                break;
            case "Bomb": 
                targetPool = bomb;
                break;
            case "Rapid": 
                targetPool = rapid;
                break;
            case "Shot": 
                targetPool = shot;
                break;
            case "Heal": 
                targetPool = heal;
                break;
            case "Coin": 
                targetPool = coin;
                break;
            case "Sub": 
                targetPool = sub;
                break;
            case "EnemyBulletA": 
                targetPool = enemyBulletA;
                break;
            case "EnemyBulletB": 
                targetPool = enemyBulletB;
                break;
            case "PlayerBulletA": 
                targetPool = playerBulletA;
                break;
            case "PlayerBulletB": 
                targetPool = playerBulletB;
                break;
        }
        return targetPool;
    }
}


