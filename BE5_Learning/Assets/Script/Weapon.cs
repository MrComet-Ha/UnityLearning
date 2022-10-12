using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee, Range };
    public Type type;
    public string nameOfWeapon;
    public int dmg;
    public float rate;
    public int maxAmmo;
    public int curAmmo;

    //Melee
    public BoxCollider meleeArea;
    public TrailRenderer trailEft;

    //Range
    public Transform bulletPos;
    public Transform bulletCasePos;
    public ObjectManager obj;

    public void Use(){
        if(type == Type.Melee){
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Range && curAmmo > 0){
            curAmmo --;
            StartCoroutine("Shot");
        }
    }

    IEnumerator Swing(){

        yield return new WaitForSeconds(0.1f);
        meleeArea.enabled = true;
        trailEft.enabled = true;
        yield return new WaitForSeconds(0.4f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.2f);
        trailEft.enabled = false;

    }

    IEnumerator Shot(){
        string type = "";
        switch(nameOfWeapon){
            case "HG" : type = "HandGunBullet"; 
                break;
            case "SMG" : type = "SubMachineGunBullet"; 
                break;
        }
        GameObject bullet = obj.CreateObj(type);
        bullet.transform.position = bulletPos.position;
        Bullet bulletLog = bullet.GetComponent<Bullet>();
        yield return new WaitForSeconds(0.01f);
        bulletLog.trail.enabled = true;
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        GameObject bulletCase = obj.CreateObj("BulletCase");
        bulletCase.transform.position = bulletCasePos.position;
        Rigidbody caseRigid = bulletCase.GetComponent<Rigidbody>();
        UnityEngine.Vector3 caseVec = bulletCasePos.forward * UnityEngine.Random.Range(-3,-2) + UnityEngine.Vector3.up*UnityEngine.Random.Range(2,3);
        caseRigid.AddForce(caseVec,ForceMode.Impulse);
        caseRigid.AddTorque(UnityEngine.Vector3.up * 10,ForceMode.Impulse);
        yield break;
    }
}
