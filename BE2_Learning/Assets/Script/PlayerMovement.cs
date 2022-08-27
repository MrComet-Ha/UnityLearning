using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed;
    Rigidbody2D rigid;
    PlayerAnime spr;
    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();
        spr=GetComponent<PlayerAnime>();
    }

    void Update()
    {
        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new UnityEngine.Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }

        if(Mathf.Abs(rigid.velocity.normalized.x) < 0.3)
            spr.isWalk = false;
        else
            spr.isWalk = true;
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal") * 25;

        rigid.AddForce(UnityEngine.Vector2.right * h * Time.deltaTime, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * Time.deltaTime, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1f) * Time.deltaTime){
            rigid.velocity = new UnityEngine.Vector2(maxSpeed * (-1f) * Time.deltaTime , rigid.velocity.y);
        }
    }
}
