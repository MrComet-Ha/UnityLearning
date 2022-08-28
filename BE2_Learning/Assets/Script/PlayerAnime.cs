using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    SpriteRenderer spr;
    Animator anim;
    public bool isRun;
    public bool isJump;
    public bool isFall;
    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        anim.SetBool("isRun", isRun);
        anim.SetBool("isJump",isJump);
        anim.SetBool("isFall",isFall);
        if (Input.GetButtonDown("Horizontal"))
            spr.flipX = (Input.GetAxisRaw("Horizontal") == -1);
    }
}
