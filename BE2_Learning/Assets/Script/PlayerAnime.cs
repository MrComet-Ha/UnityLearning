using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    SpriteRenderer spr;
    Animator anim;
    public bool isRun;
    public bool isJump;
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
        if (Input.GetButtonDown("Horizontal"))
            spr.flipX = (Input.GetAxisRaw("Horizontal") == -1);
    }
}
