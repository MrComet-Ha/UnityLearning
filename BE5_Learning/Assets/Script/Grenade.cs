using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public TrailRenderer trailEff;
    public GameObject meshObj;
    public GameObject exploEff;
    public Rigidbody rigid;
    void OnEnable()
    {
        meshObj.SetActive(true);
        exploEff.SetActive(false);
        trailEff.enabled = false;
        StartCoroutine("Explode");
    }

    void Disable(){
        gameObject.SetActive(false);
    }

    IEnumerator Explode(){
        yield return new WaitForSeconds(0.01f);
        trailEff.enabled = true;
        yield return new WaitForSeconds(3f);
        rigid.velocity = UnityEngine.Vector3.zero;
        rigid.angularVelocity = UnityEngine.Vector3.zero;
        meshObj.SetActive(false);
        exploEff.SetActive(true);
        
        RaycastHit[] rayHits = 
            Physics.SphereCastAll(
                transform.position,
                15f,
                UnityEngine.Vector3.up, 
                0f,
                LayerMask.GetMask("Enemy"));
        foreach(RaycastHit hitObj in rayHits){
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }
        Invoke("Disable",5f);
        yield break;
    }

}
