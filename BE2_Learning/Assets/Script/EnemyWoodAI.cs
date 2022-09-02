using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWoodAI : MonoBehaviour
{
    public int movement;
    public float maxSpeed;
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
        Invoke("nextMove",3);
    }
    void Update(){
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
