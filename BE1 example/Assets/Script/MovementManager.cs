using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    Rigidbody rigid;

    public float hspeed;
    public float vspeed;
    public float jumppower;
    int isjumped;
    void Awake()
    {
        isjumped=2;
        rigid=GetComponent<Rigidbody>();
    }

    void Update(){
        
        if(Input.GetButtonDown("Jump") && isjumped > 0){
            rigid.AddForce(new Vector3(0, jumppower, 0), ForceMode.Impulse);
            isjumped--;
        }
    }
    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal") * Time.deltaTime * hspeed;
        float v = Input.GetAxis("Vertical") * Time.deltaTime * vspeed;
        
        rigid.AddForce(new Vector3(h, 0, v),ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.tag == "Floor"){
            isjumped = 2;
        }
    }
}
