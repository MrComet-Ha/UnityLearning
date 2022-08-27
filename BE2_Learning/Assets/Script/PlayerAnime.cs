using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnime : MonoBehaviour
{
    SpriteRenderer spr;
    Animator anim;
    public bool isWalk;
    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        anim.SetBool("isRun", isWalk);
        if (Input.GetButtonDown("Horizontal"))
            spr.flipX = (Input.GetAxisRaw("Horizontal") == -1);
    }
}
