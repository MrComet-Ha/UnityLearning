using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public int hp;
    public Sprite[] spr;
    Rigidbody2D rigid;
    SpriteRenderer sprRen;

    // Update is called once per frame
    void Awake(){
        rigid = GetComponent<Rigidbody2D>();
        sprRen = GetComponent<SpriteRenderer>();

        rigid.velocity = Vector2.down * speed;
        
    }
    void OnHit(int dmg){
        hp -= dmg;
        sprRen.sprite = spr[1];
        Invoke("ReturnSprite", 0.1f);
        if(hp<=0){
            Destroy(gameObject);
        }
    }

    void ReturnSprite(){
        sprRen.sprite = spr[0];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "BorderBullet"){
            Destroy(gameObject);
        }
        else if(other.gameObject.tag == "Bullet"){
            BulletCycle bullet = other.GetComponent<BulletCycle>();
            OnHit(bullet.dmg);
            Destroy(other.gameObject);
        }
    }
}
