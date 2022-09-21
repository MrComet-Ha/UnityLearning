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
    public GameObject player;
    SpriteRenderer sprRen;

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

    void Awake(){
        items = new String[]{
            "Power",
            "Bomb",
            "Rapid",
            "Shot",
            "Heal",
            "Coin",
            "Sub"
        };
        sprRen = GetComponent<SpriteRenderer>();    
    }

    void OnEnable()
    {
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
        }
    }
    void Update(){
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
        sprRen.sprite = spr[1];
        Invoke("ReturnSprite", 0.1f);
        if(hp<=0){
            PlayerMovement pl = player.GetComponent<PlayerMovement>();
            pl.AddScore(score);

            int ran = UnityEngine.Random.Range(0,23);
            int index = -1;
            if(ran < 12){
                index = -1;
            }
            else if(ran < 14){
                //coin
                index = 0;
            }
            else if(ran < 16){
                //power
                index = 1;
            }
            else if(ran < 18){
                //bomb
                index = 2;
            }
            else if(ran < 20){
                //rapid
                index = 3;
            }
            else if(ran < 22){
                //shot
                index = 4;
            }
            else if(ran < 23){
                //heal
                index = 5;
            }
            else if(ran == 23){
                //sub
                index = 6;
            }
            if(index!=-1){
                GameObject item = obj.CreateObj(items[index]);
                Rigidbody2D rigid = item.GetComponent<Rigidbody2D>();
                rigid.velocity = UnityEngine.Vector2.down * 3f;
                item.transform.position = transform.position;
            }
            isTargeted = false;
            transform.rotation = UnityEngine.Quaternion.identity;
            gameObject.SetActive(false);
        }
    }
    void ReturnSprite(){
        sprRen.sprite = spr[0];
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet"){
            gameObject.SetActive(false);
        }
        else if(other.gameObject.tag == "Bullet"){
            BulletCycle bullet = other.GetComponent<BulletCycle>();
            OnHit(bullet.dmg);
            other.gameObject.SetActive(false);
        }
    }
}
