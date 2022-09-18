using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovemenet : MonoBehaviour
{
    public float speed;
    public bool isStkLeft;
    public bool isStkRight;
    public bool isStkTop;
    public bool isStkBottom;

    public Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "border"){
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
        if(other.gameObject.tag == "border"){
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
