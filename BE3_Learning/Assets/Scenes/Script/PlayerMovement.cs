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

    public void ButtonDown(string key){
        switch(key){
            case "U" :
                master.up_value = 1;
                master.up_down = true;
                break;
            case "D" :
                master.down_value = -1;
                master.down_down = true;
                break;
            case "L" :
                master.left_value = -1;
                master.left_down = true;
                break;
            case "R" :
                master.right_value = 1;
                master.right_down = true;
                break;
            case "A" : 
                if(master.pinter.scanObj != null){
                master.gm.Action(master.pinter.scanObj);
                }
                break;
            case "C" :
                master.gm.SubMenuActive();
                break;
        }
    }
    public void ButtonUp(string key){
        switch(key){
            case "U" :
                master.up_value = 0;
                master.up_up = true;
                break;
            case "D" :
                master.down_value = 0;
                master.down_up = true;
                break;
            case "L" :
                master.left_value = 0;
                master.left_up = true;
                break;
            case "R" :
                master.right_value = 0;
                master.right_up = true;
                break;
        }
    }
}
