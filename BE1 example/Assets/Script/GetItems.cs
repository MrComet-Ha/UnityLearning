using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItems : MonoBehaviour
{
    AudioSource sound;
    ScoreManagement manager;
    public GameObject other;
    void Start(){
        sound = GetComponent<AudioSource>();
        manager = other.GetComponent<ScoreManagement>();
    }
    
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Coin"){
            manager.itemget += 1;
            sound.Play();
            other.gameObject.SetActive(false);
        }
    }
}
