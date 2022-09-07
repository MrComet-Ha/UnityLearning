using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spr;
    CapsuleCollider2D col;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
    }

    public void onDamaged()
    {
        spr.color = new Color(1,1,1,0.4f);
        spr.flipY = true;
        col.enabled = false;
        rigid.velocity=new Vector2(0,0);
        rigid.AddForce(UnityEngine.Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("Deactive", 1.5f);
    }

    void Deactive(){
        gameObject.SetActive(false);
    }
}
