using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    
    float hAxis;
    float vAxis;
    bool wDown;
    
    UnityEngine.Vector3 moveVec;
    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetKey(KeyCode.LeftShift);

        moveVec = new UnityEngine.Vector3(hAxis,0,vAxis).normalized;
        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        anim.SetBool("isRun",moveVec != UnityEngine.Vector3.zero);
        anim.SetBool("isWalk",wDown);

        transform.LookAt(transform.position + moveVec);
    }
}
