using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemenet : MonoBehaviour
{
    //Movement
    public float speed;
    public bool isStkLeft;
    public bool isStkRight;
    public bool isStkTop;
    public bool isStkBottom;

    //Bullet
    public float shotSpeed;
    public int power;
    public int shotDelay;
    public float maxShotDelay;
    public float curShotDelay;
    public GameObject PlayerBulletA;
    public GameObject PlayerBulletB;

    //Reference
    public Animator anim;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        maxShotDelay = shotDelay * Time.deltaTime;
        Move();
        Reload();
        Fire();
    }
    //Movement
    void Move(){
        float h = Input.GetAxisRaw("Horizontal");
        if((isStkLeft && h == -1 )||(isStkRight && h == 1))
            h = 0;
        
        float v = Input.GetAxisRaw("Vertical");
        if((isStkTop && v == 1)||(isStkBottom && v == -1))
            v = 0;
        
        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h,v,0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal")){
            anim.SetInteger("Input",(int)h);
        }
    }
    //Shot
    void Fire(){
        if(!Input.GetKey("space"))
            return;
        if(curShotDelay < maxShotDelay)
            return;
        
        switch(power){
            case 1 : 
                GameObject bullet = Instantiate(PlayerBulletA, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
            case 2 :
                    GameObject bulletL = Instantiate(PlayerBulletA, transform.position + Vector3.left * 0.1f, transform.rotation);
                    GameObject bulletR = Instantiate(PlayerBulletA, transform.position + Vector3.right * 0.1f, transform.rotation);
                    Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                    Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                    rigidL.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                    rigidR.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
            case 3 :
                GameObject bulletLL = Instantiate(PlayerBulletA, transform.position + Vector3.left * 0.2f, transform.rotation);
                GameObject bulletCC = Instantiate(PlayerBulletB, transform.position, transform.rotation);
                GameObject bulletRR = Instantiate(PlayerBulletA, transform.position + Vector3.right * 0.2f, transform.rotation);
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                rigidLL.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                rigidRR.AddForce(Vector2.up*shotSpeed, ForceMode2D.Impulse);
                break;
        }
        curShotDelay = 0;
    }
    void Reload(){
        curShotDelay += Time.deltaTime;
    }

    //Collision
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Border"){
            switch(other.gameObject.name){
                case "Left" : 
                    isStkLeft = true;
                    break;
                case "Right" : 
                    isStkRight = true;
                    break;
                case "Top" : 
                    isStkTop = true;
                    break;
                case "Bottom" : 
                    isStkBottom = true;
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Border"){
            switch(other.gameObject.name){
                case "Left" : 
                    isStkLeft = false;
                    break;
                case "Right" : 
                    isStkRight = false;
                    break;
                case "Top" : 
                    isStkTop = false;
                    break;
                case "Bottom" : 
                    isStkBottom = false;
                    break;
            }
        }
    }
}
