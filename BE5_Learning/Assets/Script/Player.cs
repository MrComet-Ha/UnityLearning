using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public int jumpPow;
    public int jumpCount;
    public int dodgeCool;
    
    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isJump;
    int jumpCnt;
    bool isDodge;
    
    UnityEngine.Vector3 moveVec;
    UnityEngine.Vector3 dodgeVec;
    Animator anim;
    Rigidbody rigid;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
        ResetJump();
    }

    
    void Update()
    {
        GetInput();
        Move();
        Jump();
        Dodge();
        Turn();
    }

    void GetInput(){
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.LeftShift);
        jDown = Input.GetKeyDown(KeyCode.Space);
    }

    void Move(){
        moveVec = new UnityEngine.Vector3(hAxis,0,vAxis).normalized;
        if(isDodge)
            moveVec = dodgeVec;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) *(isDodge ? 2f : 1f) * Time.deltaTime;
        anim.SetBool("isRun",moveVec != UnityEngine.Vector3.zero);
        anim.SetBool("isWalk",wDown);
    }

    void Turn(){
        transform.LookAt(transform.position + moveVec);
    }

    void Jump(){
        if(jDown && moveVec == UnityEngine.Vector3.zero && jumpCnt > 0 && !isDodge){
            rigid.AddForce(Vector3.up * jumpPow,ForceMode.Impulse);
            anim.SetBool("isJump",true);
            anim.SetTrigger("doJump");
            jumpCnt-=1;
        }
    }

    void ResetJump(){
        anim.SetBool("isJump",false);
        jumpCnt = jumpCount;
    }

    void Dodge(){
        if(jDown && moveVec != UnityEngine.Vector3.zero && jumpCnt > 0 && !isDodge){
            dodgeVec = moveVec;
            anim.SetTrigger("doDodge");
            isDodge = true;
            jumpCnt = 0;
            Invoke("EndDodge",dodgeCool * Time.deltaTime);
        }
    }

    void EndDodge(){
        ResetJump();
        isDodge = false;
    }
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor"){
            ResetJump();
        }
    }
}
