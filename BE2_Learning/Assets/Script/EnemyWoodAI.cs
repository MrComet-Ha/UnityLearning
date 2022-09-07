using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWoodAI : MonoBehaviour
{
    public int movement;
    public float maxSpeed;
    int moveCool;
    Rigidbody2D rigid;
    Vector3 startingpoint;
    SpriteRenderer spr;
    Animator anim;
    CapsuleCollider2D col;
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
            case -1 : anim.SetBool("isRun", true);
            spr.flipX = false;
            break;
            case 0 : anim.SetBool("isRun", false);
            rigid.velocity = new Vector2(0, rigid.velocity.y);
            break;
            case 1 : anim.SetBool("isRun", true);
            spr.flipX = true;
            break;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.AddForce(UnityEngine.Vector2.right * movement * 13 * Time.deltaTime, ForceMode2D.Impulse);
        if(rigid.velocity.x > maxSpeed * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * Time.deltaTime, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1f) * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * (-1f) * Time.deltaTime , rigid.velocity.y);
        }
        Vector2 frontVec = new Vector2(rigid.position.x + movement,rigid.position.y);
        RaycastHit2D PlatChk = Physics2D.Raycast(frontVec, Vector3.down,1,LayerMask.GetMask("Platform"));
        if(PlatChk.collider == null){
            movement *= -1;
            CancelInvoke();
            Invoke("nextMove", 2.3f);
        }
    }

    void nextMove(){
        movement = Random.Range(-1,1);
        Invoke("nextMove",2.3f);
    }
}
