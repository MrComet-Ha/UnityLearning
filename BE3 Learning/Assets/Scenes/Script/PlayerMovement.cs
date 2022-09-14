using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    bool isHMove;
    PlayerMaster master;
    Animator anim;
    void Start()
    {
        master = GetComponent<PlayerMaster>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(master.vUp ||master.hDown){
            isHMove = true;
        }
        else if(master.hUp || master.vDown){
            isHMove = false;
        }
        if(anim.GetInteger("hAxisRaw") != (int)master.h){
            anim.SetBool("isChanged",true);
            anim.SetInteger("hAxisRaw",(int)master.h);
        }
        else if(anim.GetInteger("vAxisRaw") != (int)master.v){
            anim.SetBool("isChanged",true);
            anim.SetInteger("vAxisRaw",(int)master.v);
        }  
        else
            anim.SetBool("isChanged",false);
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 moveVec = isHMove ? new UnityEngine.Vector2(master.h,0) : new UnityEngine.Vector2(0,master.v);
        master.rigid.velocity = moveVec * speed * Time.deltaTime;
    }
}
