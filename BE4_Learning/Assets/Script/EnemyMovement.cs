using System.Dynamic;
using System;
using System.IO;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //Reference
    public ObjectManager obj;
    public GameManager gm;
    public GameObject player;
    SpriteRenderer sprRen;
    Animator anim;

    //Value
    public string enemyName;
    public float speed;
    public int hp;
    public Sprite[] spr;
    public int score;
    bool isTargeted;
    

    //Bullet
    public int shotDelay;
    public float shotSpeed;
    public float maxShotDelay;
    public float curShotDelay;

    //Item
    public String[] items;

    //Boss
    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    void Awake(){
        items = new String[]{
            "Coin",
            "Power",
            "Bomb",
            "Rapid",
            "Shot",
            "Heal",
        };
        sprRen = GetComponent<SpriteRenderer>();
        if(enemyName == "B")
            anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        //Enemy Data
        switch(enemyName){
            case "S" : 
                hp = 3;
                break;
            case "M" : 
                hp = 8;
                break;
            case "L" : 
                hp = 16;
                break;
            case "B" : 
                hp = 200;
                Invoke("Stop", 1.5f);
                break;
        }
    }
    void Update(){
        if(enemyName == "B")
            return;
        maxShotDelay = shotDelay * Time.deltaTime;
        Reload();
        Fire();
    }

    //Attack
    void Fire(){
        if(curShotDelay < maxShotDelay)
            return;
            
        switch(enemyName){
            case "S" : 
                GameObject bullet = obj.CreateObj("EnemyBulletA");
                bullet.transform.position = transform.position;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                UnityEngine.Vector3 dirVec = player.transform.position - transform.position;
                rigid.AddForce(dirVec.normalized*shotSpeed, ForceMode2D.Impulse);
                break;
            case "M" :
                if(isTargeted)
                    return;
                Rigidbody2D rigidM = GetComponent<Rigidbody2D>();
                UnityEngine.Vector3 dirVecM = (player.transform.position - transform.position).normalized;
                rigidM.velocity = new UnityEngine.Vector3(dirVecM.x,dirVecM.y,0)*speed;
                float angle = Mathf.Atan2(dirVecM.y,dirVecM.x) * Mathf.Rad2Deg;
                transform.rotation = UnityEngine.Quaternion.AngleAxis(angle+90,UnityEngine.Vector3.forward);
                isTargeted = true;
                break;
            case "L" : 
                GameObject bulletL = obj.CreateObj("EnemyBulletB"); 
                bulletL.transform.position=transform.position + UnityEngine.Vector3.left * 0.3f;
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                UnityEngine.Vector3 dirVecL = player.transform.position - (transform.position + UnityEngine.Vector3.left * 0.3f);
                rigidL.AddForce(dirVecL.normalized*shotSpeed, ForceMode2D.Impulse);
                GameObject bulletR = obj.CreateObj("EnemyBulletB"); 
                bulletR.transform.position = transform.position + UnityEngine.Vector3.right * 0.3f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                UnityEngine.Vector3 dirVecR = player.transform.position - (transform.position + UnityEngine.Vector3.right * 0.3f);
                rigidR.AddForce(dirVecR.normalized*shotSpeed, ForceMode2D.Impulse);
                break;
        }
       
        curShotDelay = 0;
    }
    void Reload(){
        curShotDelay += Time.deltaTime;
    }
    //Hit, Dead
    public void OnHit(int dmg){
        if(hp<=0)
            return;
        hp -= dmg;
        if(enemyName == "B"){
            anim.SetTrigger("onHit");
        }
        else{
            sprRen.sprite = spr[1];
            Invoke("ReturnSprite", 0.1f);
        }

        //Dead Logic
        if(hp<=0){
            PlayerMovement pl = player.GetComponent<PlayerMovement>();
            pl.AddScore(score);
            //Item drop 
            int ran = enemyName == "B" ? 23 : UnityEngine.Random.Range(0,23);
            int index = -1;
            if(ran < 12){
                index = -1;
            }
            else if(ran < 15){
                //coin
                index = 0;
            }
            else if(ran < 17){
                //power
                index = 1;
            }
            else if(ran < 19){
                //bomb
                index = 2;
            }
            else if(ran < 21){
                //rapid
                index = 3;
            }
            else if(ran < 23){
                //shot
                index = 4;
            }
            else if(ran == 23){
                //heal
                index = 5;
            }
            if(index!=-1){
                GameObject item = obj.CreateObj(items[index]);
                Rigidbody2D rigid = item.GetComponent<Rigidbody2D>();
                rigid.velocity = UnityEngine.Vector2.down * 3f;
                item.transform.position = transform.position;
            }

            isTargeted = false;
            transform.rotation = UnityEngine.Quaternion.identity;
            gm.CallExplosion(transform.position,enemyName);
            gameObject.SetActive(false);
            //Boss
            if(enemyName == "B"){
                CancelInvoke("BossAttack");
                CancelInvoke("BossPattern1");
                CancelInvoke("BossPattern2");
                CancelInvoke("BossPattern3");
                CancelInvoke("BossPattern4");
                GameObject[] bulletsA = obj.GetPool("EnemyBulletA");
                GameObject[] bulletsB = obj.GetPool("EnemyBulletB");
                GameObject[] bossBulletsA = obj.GetPool("BossBulletA");
                GameObject[] bossBulletsB = obj.GetPool("BossBulletB");
                for(int i = 0; i < bulletsA.Length; i++){
                    if(bulletsA[i].activeSelf)
                        bulletsA[i].SetActive(false);
                }
                for(int i = 0; i < bulletsB.Length; i++){
                    if(bulletsB[i].activeSelf)
                        bulletsB[i].SetActive(false);
                }
                for(int i = 0; i < bossBulletsA.Length; i++){
                    if(bossBulletsA[i].activeSelf)
                        bossBulletsA[i].SetActive(false);
                }
                for(int i = 0; i < bossBulletsB.Length; i++){
                    if(bossBulletsB[i].activeSelf)
                        bossBulletsB[i].SetActive(false);
                }
                patternIndex = -1;
                gm.Invoke("StageEnd",4f);
            }
        }
    }
    void ReturnSprite(){
        sprRen.sprite = spr[0];
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet" && enemyName != "B"){
            gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Bullet"){
            BulletCycle bullet = other.GetComponent<BulletCycle>();
            OnHit(bullet.dmg);
            other.gameObject.SetActive(false);
        }
    }

    //Boss
    void Stop(){
        if(!gameObject.activeSelf)
            return;
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = UnityEngine.Vector3.zero;
        Invoke("BossAttack",1f);
    }
    
    void BossAttack(){
        patternIndex = patternIndex == 3 ? 0 : patternIndex+1;
        curPatternCount = 0;
        switch(patternIndex){
            case 0 :
                BossPattern1();
                break;
            case 1 :
                BossPattern2();
                break;
            case 2 :
                BossPattern3();
                break;
            case 3 :
                BossPattern4();
                break;
        }
    }
    void BossPattern1(){
        shotSpeed = 9;
        GameObject bulletL = obj.CreateObj("BossBulletA"); 
        bulletL.transform.position=transform.position + UnityEngine.Vector3.left * (0.3f);
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        rigidL.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        GameObject bulletR = obj.CreateObj("BossBulletA"); 
        bulletR.transform.position = transform.position + UnityEngine.Vector3.right * (0.3f);
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        rigidR.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        if(curPatternCount>=1){
            GameObject bulletLL = obj.CreateObj("BossBulletA"); 
            bulletLL.transform.position=transform.position + UnityEngine.Vector3.left * (0.5f);
            Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
            rigidLL.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
            GameObject bulletRR = obj.CreateObj("BossBulletA"); 
            bulletRR.transform.position = transform.position + UnityEngine.Vector3.right * (0.5f);
            Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
            rigidRR.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        }
        if(curPatternCount>=2){
            GameObject bulletLL = obj.CreateObj("BossBulletA"); 
            bulletLL.transform.position=transform.position + UnityEngine.Vector3.left * (0.7f);
            Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
            rigidLL.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
            GameObject bulletRR = obj.CreateObj("BossBulletA"); 
            bulletRR.transform.position = transform.position + UnityEngine.Vector3.right * (0.7f);
            Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
            rigidRR.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        }
        if(curPatternCount>=3){
            GameObject bulletLL = obj.CreateObj("BossBulletA"); 
            bulletLL.transform.position=transform.position + UnityEngine.Vector3.left * (0.9f);
            Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
            rigidLL.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
            GameObject bulletRR = obj.CreateObj("BossBulletA"); 
            bulletRR.transform.position = transform.position + UnityEngine.Vector3.right * (0.9f);
            Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
            rigidRR.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        }
        if(curPatternCount>=4){
            GameObject bulletLL = obj.CreateObj("BossBulletA"); 
            bulletLL.transform.position=transform.position + UnityEngine.Vector3.left * (1.1f);
            Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
            rigidLL.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
            GameObject bulletRR = obj.CreateObj("BossBulletA"); 
            bulletRR.transform.position = transform.position + UnityEngine.Vector3.right * (1.1f);
            Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
            rigidRR.AddForce(UnityEngine.Vector2.down*shotSpeed, ForceMode2D.Impulse);
        }
        curPatternCount++;
        if(curPatternCount < maxPatternCount[0])
            Invoke("BossPattern1",1f);
        else
            Invoke("BossAttack",2f);
    }
    void BossPattern2(){
        shotSpeed = 8;
        for(int i=0;i<5;i++){
            GameObject bullet = obj.CreateObj("EnemyBulletA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            UnityEngine.Vector2 dirVec = player.transform.position - transform.position;
            UnityEngine.Vector2 ranVec = new UnityEngine.Vector2(UnityEngine.Random.Range(-0.5f,0.5f),UnityEngine.Random.Range(0f,2f));
            dirVec+=ranVec;
            rigid.AddForce(dirVec.normalized*shotSpeed, ForceMode2D.Impulse);
        }
        curPatternCount++;
        if(curPatternCount < maxPatternCount[1])
            Invoke("BossPattern2",0.5f);
        else
            Invoke("BossAttack",2f);
    }
    void BossPattern3(){
        shotSpeed = 8;
        GameObject bullet = obj.CreateObj("EnemyBulletB"); 
        bullet.transform.position = transform.position + UnityEngine.Vector3.left * (0.3f);
        bullet.transform.rotation = UnityEngine.Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        UnityEngine.Vector2 dirVec = new UnityEngine.Vector2(Mathf.Cos(Mathf.PI * 8 * curPatternCount/maxPatternCount[2]),-1);
        rigid.AddForce(dirVec*shotSpeed, ForceMode2D.Impulse);
        curPatternCount++;
        if(curPatternCount < maxPatternCount[2])
            Invoke("BossPattern3",0.1f);
        else
            Invoke("BossAttack",2f);
    }
    void BossPattern4(){
        shotSpeed = 5;
        int roundNumA = 45;
        int roundNumB = 35;
        int roundNum = curPatternCount%2==0 ? roundNumA : roundNumB;
        for(int i = 0; i<roundNumA;i++){
            GameObject bullet = obj.CreateObj("BossBulletB");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = UnityEngine.Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            UnityEngine.Vector2 dirVec = 
            new UnityEngine.Vector2(Mathf.Cos(Mathf.PI * 2 * i/roundNum),
                Mathf.Sin(Mathf.PI * 2 * i/roundNum));
            UnityEngine.Vector3 rocVec = UnityEngine.Vector3.forward * 360 * i / roundNum + UnityEngine.Vector3.forward*90;
            bullet.transform.Rotate(rocVec);
            rigid.AddForce(dirVec*shotSpeed, ForceMode2D.Impulse);
        }
        curPatternCount++;
        if(curPatternCount < maxPatternCount[3])
            Invoke("BossPattern4",0.6f);
        else
            Invoke("BossAttack",2f);
    }
}
