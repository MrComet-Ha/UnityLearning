using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int movement;
    int moveCool;
    Rigidbody2D rigid;
    SpriteRenderer spr;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        nextMove();
    }

    // Update is called once per frame
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
        switch(movement){
            case -1 : anim.SetBool("isJump", true);
            break;
            case 0 : anim.SetBool("isJump", false);
            break;
            case 1 : anim.SetBool("isJump", true);
            break;
        }
        if(movement == 1){
            spr.flipX = true;
        }
        else
            spr.flipX = false;
        movement = Random.Range(-1,2);
        Invoke("nextMove",1.5f);
    }
}
    