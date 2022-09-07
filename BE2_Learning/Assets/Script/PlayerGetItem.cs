using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetItem : MonoBehaviour
{
    AudioClip snd;
    PlayerSendData send;
    public int bronze;
    public int silver;
    public int gold;
    // Start is called before the first frame update
    void Awake()
    {
        snd = GetComponent<AudioClip>();
        send = GetComponent<PlayerSendData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Item"){
        bool isBronze = other.gameObject.name.Contains("BronzeCoin");
        bool isSilver = other.gameObject.name.Contains("SilverCoin");
        bool isGold = other.gameObject.name.Contains("GoldCoin");
            if(isBronze){
                send.gm.SendScore(bronze);
                send.gm.SoundManagement("sndCOIN1");
            }
            else if(isSilver){
                send.gm.SendScore(silver);
                send.gm.SoundManagement("sndCOIN2");
            }
            else if(isGold){
                send.gm.SendScore(gold);
                send.gm.SoundManagement("sndCOIN3");
            }
        other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "Finish"){
            
        }
    }
}
