using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed;
    Rigidbody2D rigid;
    UnityEngine.Vector3 startpoint;
    PlayerAnime spr;
    PlayerSendData send;
    public float h;
    public float v;
    
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        spr=GetComponent<PlayerAnime>();
        send = GetComponent<PlayerSendData>();
        startpoint = transform.position;
    }
    void OnEnable()
    {
        Reposition();
    }
    void Update()
    {
        if(Input.GetButtonDown("Jump") && spr.isJump == false){
            spr.isJump = true;
            rigid.AddForce(UnityEngine.Vector2.up * v, ForceMode2D.Impulse);
            send.gm.SoundManagement("sndJUMP");
        }
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new UnityEngine.Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }
        
        if(Mathf.Abs(rigid.velocity.normalized.x) < 0.3)
            spr.isRun = false;
        else
            spr.isRun = true;
    }
    void FixedUpdate()
    {
        float hspeed = h;
        hspeed *= Input.GetAxisRaw("Horizontal");
        rigid.AddForce(UnityEngine.Vector2.right * hspeed * rigid.gravityScale * Time.deltaTime, ForceMode2D.Impulse);
        if(rigid.velocity.x > maxSpeed * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * Time.deltaTime, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1f) * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * (-1f) * Time.deltaTime , rigid.velocity.y);
        }
        if(rigid.velocity.y < 0){
            RaycastHit2D onGround = Physics2D.Raycast(rigid.position,UnityEngine.Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(onGround.collider != null){
                if(onGround.distance < 0.5){
                    spr.isJump = false;
                }
            }
        }
    }
    public void Reposition(){
        rigid.velocity = UnityEngine.Vector2.zero;
        transform.position = startpoint;
    }
}
