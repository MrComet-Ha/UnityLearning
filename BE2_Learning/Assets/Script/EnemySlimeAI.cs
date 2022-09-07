using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlimeAI : MonoBehaviour
{
    public int movement;
    int moveCool;
    Rigidbody2D rigid;
    SpriteRenderer spr;
    Animator anim;
    CapsuleCollider2D col;
    Vector3 startingpoint;
    // Start is called before the first frame update
    void Awake()
    {
        startingpoint = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
    }
    void OnEnable(){
        if(transform.position != startingpoint)
        {
            transform.position = startingpoint;
        }
        spr.color = new Color(1,1,1,1);
        spr.flipX = false;
        spr.flipY = false;
        col.enabled = true;
        rigid.velocity=new Vector2(0,0);
        Invoke("nextMove",3);
    }
    void Update(){
        if(!col.enabled){
            CancelInvoke("nextMove");
            movement = 0;
        }
        switch(movement){
            case -1 : anim.SetBool("isJump", true);
            spr.flipX = false;
            break;
            case 0 : anim.SetBool("isJump", false);
            break;
            case 1 : anim.SetBool("isJump", true);
            spr.flipX = true;
            break;
        }
    }    // Update is called once per frame
    void FixedUpdate()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("AnimSlimeJumped") && movement != 0){
            rigid.AddForce(new UnityEngine.Vector2(2 * movement, 5), ForceMode2D.Impulse);
            movement = 0;
        }
        if(rigid.velocity.y < 0){
            RaycastHit2D onGround = Physics2D.Raycast(rigid.position,UnityEngine.Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(onGround.collider!=null){
                anim.SetBool("isJump", false);
            }
        }
    }
    void nextMove(){
        movement = Random.Range(-1,2);
        Invoke("nextMove",1.5f);
    }
}
    
