using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject itemAmmoPrefab;
    public GameObject itemHeartPrefab;
    public GameObject itemBronzeCoinPrefab;
    public GameObject itemSilverCoinPrefab;
    public GameObject itemGoldCoinPrefab;
    public GameObject weaponHammerPrefab;
    public GameObject weaponHandGunPrefab;
    public GameObject weaponSubMachineGunPrefab;
    public GameObject weaponGrenadePrefab;
    public GameObject bulletHandGunPrefab;
    public GameObject bulletSubMachineGunPrefab;
    public GameObject bulletCasePrefab;
    public GameObject throwGrenadePrefab;
    public GameObject enemyAPrefab;
    public GameObject enemyBPrefab;
    public GameObject enemyCPrefab;
    public GameObject enemyDPrefab;
    public GameObject enemyMissilePrefab;
    public GameObject bossMissilePrefab;
    public GameObject bossRockPrefab;

    GameObject[] itemAmmo;
    GameObject[] itemHeart;
    GameObject[] itemBronzeCoin;
    GameObject[] itemSilverCoin;
    GameObject[] itemGoldCoin;
    GameObject[] weaponHammer;
    GameObject[] weaponHandGun;
    GameObject[] weaponSubMachineGun;
    GameObject[] weaponGrenade;
    GameObject[] bulletHandGun;
    GameObject[] bulletSubMachineGun;
    GameObject[] bulletCase;
    GameObject[] throwGrenade;
    GameObject[] enemyA;
    GameObject[] enemyB;
    GameObject[] enemyC;
    GameObject[] enemyD;
    GameObject[] bossMissile;
    GameObject[] bossRock;
    GameObject[] enemyMissile;

    GameObject[] targetPool;

    void Awake()
    {
        itemAmmo = new GameObject[20];
        itemHeart = new GameObject[20];
        itemBronzeCoin = new GameObject[50];
        itemSilverCoin = new GameObject[20];
        itemGoldCoin = new GameObject[20];
        weaponHammer = new GameObject[5];
        weaponHandGun = new GameObject[5];
        weaponSubMachineGun = new GameObject[5];
        weaponGrenade = new GameObject[30];
        bulletHandGun = new GameObject[100];
        bulletSubMachineGun = new GameObject[200];
        bulletCase = new GameObject[200];
        throwGrenade = new GameObject[20];
        enemyA = new GameObject[50];
        enemyB = new GameObject[50];
        enemyC = new GameObject[50];
        enemyD = new GameObject[1];
        enemyMissile = new GameObject[100];
        bossMissile = new GameObject[100];
        bossRock = new GameObject[50];

        Generate();
    }

    
    void Generate()
    {
        for(int i=0;i<itemAmmo.Length;i++){
            itemAmmo[i] = Instantiate(itemAmmoPrefab);
            itemAmmo[i].SetActive(false);
        }
        for(int i=0;i<itemHeart.Length;i++){
            itemHeart[i] = Instantiate(itemHeartPrefab);
            itemHeart[i].SetActive(false);
        }
        for(int i=0;i<itemBronzeCoin.Length;i++){
            itemBronzeCoin[i] = Instantiate(itemBronzeCoinPrefab);
            itemBronzeCoin[i].SetActive(false);
        }
        for(int i=0;i<itemSilverCoin.Length;i++){
            itemSilverCoin[i] = Instantiate(itemSilverCoinPrefab);
            itemSilverCoin[i].SetActive(false);
        }
        for(int i=0;i<itemGoldCoin.Length;i++){
            itemGoldCoin[i] = Instantiate(itemGoldCoinPrefab);
            itemGoldCoin[i].SetActive(false);
        }
        for(int i=0;i<weaponHammer.Length;i++){
            weaponHammer[i] = Instantiate(weaponHammerPrefab);
            weaponHammer[i].SetActive(false);
        }
        for(int i=0;i<weaponHandGun.Length;i++){
            weaponHandGun[i] = Instantiate(weaponHandGunPrefab);
            weaponHandGun[i].SetActive(false);
        }
        for(int i=0;i<weaponSubMachineGun.Length;i++){
            weaponSubMachineGun[i] = Instantiate(weaponSubMachineGunPrefab);
            weaponSubMachineGun[i].SetActive(false);
        }
        for(int i=0;i<weaponGrenade.Length;i++){
            weaponGrenade[i] = Instantiate(weaponGrenadePrefab);
            weaponGrenade[i].SetActive(false);
        }
        for(int i=0;i<bulletHandGun.Length;i++){
            bulletHandGun[i] = Instantiate(bulletHandGunPrefab);
            bulletHandGun[i].SetActive(false);
        }
        for(int i=0;i<bulletSubMachineGun.Length;i++){
            bulletSubMachineGun[i] = Instantiate(bulletSubMachineGunPrefab);
            bulletSubMachineGun[i].SetActive(false);
        }
        for(int i=0;i<bulletCase.Length;i++){
            bulletCase[i] = Instantiate(bulletCasePrefab);
            bulletCase[i].SetActive(false);
        }
        for(int i=0;i<throwGrenade.Length;i++){
            throwGrenade[i] = Instantiate(throwGrenadePrefab);
            throwGrenade[i].SetActive(false);
        }
        for(int i=0;i<enemyA.Length;i++){
            enemyA[i] = Instantiate(enemyAPrefab);
            enemyA[i].SetActive(false);
        }
        for(int i=0;i<enemyB.Length;i++){
            enemyB[i] = Instantiate(enemyBPrefab);
            enemyB[i].SetActive(false);
        }
        for(int i=0;i<enemyC.Length;i++){
            enemyC[i] = Instantiate(enemyCPrefab);
            enemyC[i].SetActive(false);
        }
        for(int i=0;i<enemyD.Length;i++){
            enemyD[i] = Instantiate(enemyDPrefab);
            enemyD[i].SetActive(false);
        }
        for(int i=0;i<enemyMissile.Length;i++){
            enemyMissile[i] = Instantiate(enemyMissilePrefab);
            enemyMissile[i].SetActive(false);
        }
        for(int i=0;i<bossMissile.Length;i++){
            bossMissile[i] = Instantiate(bossMissilePrefab);
            bossMissile[i].SetActive(false);
        }
        for(int i=0;i<bossRock.Length;i++){
            bossRock[i] = Instantiate(bossRockPrefab);
            bossRock[i].SetActive(false);
        }
    }

    public GameObject CreateObj(string type){
        switch(type){
            case "Ammo" :
                targetPool = itemAmmo;
                break;
            case "Heart" :
                targetPool = itemHeart;
                break;
            case "BronzeCoin" :
                targetPool = itemBronzeCoin;
                break;
            case "SilverCoin" :
                targetPool = itemSilverCoin;
                break;
            case "GoldCoin" :
                targetPool = itemGoldCoin;
                break;
            case "Hammer" :
                targetPool = weaponHammer;
                break;
            case "HandGun" :
                targetPool = weaponHandGun;
                break;
            case "SubMachineGun" :
                targetPool = weaponSubMachineGun;
                break;
            case "Grenade" :
                targetPool = weaponGrenade;
                break;
            case "HandGunBullet" :
                targetPool = bulletHandGun;
                break;
            case "SubMachineGunBullet" :
                targetPool = bulletSubMachineGun;
                break;
            case "BulletCase" :
                targetPool = bulletCase;
                break;
            case "ThrowGrenade" :
                targetPool = throwGrenade;
                break;
            case "EnemyA" : 
                targetPool = enemyA;
                break;
            case "EnemyB" : 
                targetPool = enemyB;
                break;
            case "EnemyC" : 
                targetPool = enemyC;
                break;
            case "EnemyD" : 
                targetPool = enemyD;
                break;
            case "EnemyMissile" : 
                targetPool = enemyMissile;
                break;
            case "BossMissile" :
                targetPool = bossMissile;
                break;
            case "BossRock" :
                targetPool = bossRock;
                break;
        }
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
            case "Ammo" :
                targetPool = itemAmmo;
                break;
            case "Heart" :
                targetPool = itemHeart;
                break;
            case "BronzeCoin" :
                targetPool = itemBronzeCoin;
                break;
            case "SilverCoin" :
                targetPool = itemSilverCoin;
                break;
            case "GoldCoin" :
                targetPool = itemGoldCoin;
                break;
            case "Hammer" :
                targetPool = weaponHammer;
                break;
            case "HandGun" :
                targetPool = weaponHandGun;
                break;
            case "SubMachineGun" :
                targetPool = weaponSubMachineGun;
                break;
            case "Grenade" :
                targetPool = weaponGrenade;
                break;
            case "HandGunBullet" :
                targetPool = bulletHandGun;
                break;
            case "SubMachineGunBullet" :
                targetPool = bulletSubMachineGun;
                break;
            case "BulletCase" :
                targetPool = bulletCase;
                break;
            case "ThrowGrenade" :
                targetPool = throwGrenade;
                break;
            case "EnemyA" : 
                targetPool = enemyA;
                break;
            case "EnemyB" : 
                targetPool = enemyB;
                break;
            case "EnemyC" : 
                targetPool = enemyC;
                break;
            case "EnemyD" : 
                targetPool = enemyC;
                break;
            case "EnemyMissile" : 
                targetPool = enemyMissile;
                break;
            case "BossMissile" :
                targetPool = bossMissile;
                break;
            case "BossRock" :
                targetPool = bossRock;
                break;
        }
        return targetPool;
    }
}
