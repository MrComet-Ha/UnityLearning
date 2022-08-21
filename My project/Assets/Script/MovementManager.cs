using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        //rigid.velocity : 고정속도를 정해줌
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.AddForce(new Vector3(
            Input.GetAxis("Horizontal") * Time.deltaTime * 3,
            0,
            Input.GetAxis("Vertical") * Time.deltaTime * 3),
            ForceMode.Impulse);
        if (Input.GetButtonDown("Jump")){
            rigid.AddForce(Vector3.up * 300 * Time.deltaTime, ForceMode.Impulse);
        }

    }
    private void OnTriggerStay(Collider other){
        Debug.Log(other.name);
        if(other.name == "Cube"){
            rigid.AddForce(Vector3.up * 50 * Time.deltaTime, ForceMode.Impulse);
        }
    }
}