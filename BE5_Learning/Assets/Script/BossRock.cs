using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rigid;
    public float angularPower;
    public float scaleValue;
    bool isShot;

    void OnEnable(){
        rigid=GetComponent<Rigidbody>();
        StartCoroutine(CheckEnable());  
    }

    IEnumerator CheckEnable(){
        yield return new WaitForEndOfFrame();
        if(gameObject.activeSelf){
            StartCoroutine(GainPowerTime());
            StartCoroutine(GainPower());
        }
    }
    IEnumerator GainPowerTime(){
        yield return new WaitForSeconds(2.2f);
        isShot = true;
    }

    IEnumerator GainPower(){
        while(!isShot){
            angularPower += angularPower*0.01f;
            scaleValue += scaleValue*0.05f;
            transform.localScale = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower,ForceMode.Acceleration);
            yield return null;
        }
    }
}
