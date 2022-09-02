using System.Security.Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    SpriteRenderer spr;
    Rigidbody2D rigid;
    PlayerMovement plmove;
    PlayerAnime planim;
    // Start is called before the first frame update
    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        plmove = GetComponent<PlayerMovement>();
        planim = GetComponent<PlayerAnime>();
    }
    void Update(){
        
    }
    void onNormal(){
        gameObject.layer = 9;
        spr.color = new Color(1,1,1,1);
    }
    void onDamaged(Vector2 target){
        float refl = 12;
        if(transform.position.x - target.x < 0){
            refl *= -1;
        }
        gameObject.layer = 11;
        spr.color = new Color(1,1,1,0.4f);
        rigid.velocity = new Vector2(0, 0);
        rigid.AddForce(new Vector2(refl * Time.deltaTime, 1)*5, ForceMode2D.Impulse);
        planim.isJump = true;
        Invoke("onNormal", 1);
    }
    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        RaycastHit2D onHead = Physics2D.Raycast(rigid.position,UnityEngine.Vector3.down, 1, LayerMask.GetMask("Enemy"));
        if(other.gameObject.tag == "Monster" && onHead.collider == null){
            onDamaged(other.transform.position);
        }
        else if(other.gameObject.tag == "Monster" && onHead.collider != null){
            rigid.AddForce(UnityEngine.Vector2.up * plmove.v, ForceMode2D.Impulse);
        }
    }

}