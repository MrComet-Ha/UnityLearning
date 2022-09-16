using System.Net.Mime;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEffect : MonoBehaviour
{
    public GameManager gm;
    public int visibleChar;
    TextMeshProUGUI diagTMP;
    AudioSource sound;
    public float spd;
    public bool isAnim;
    public GameObject textCurser;

    // Update is called once per frame
    void Awake()
    {
        diagTMP = gameObject.GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();
        sound = gameObject.GetComponent<AudioSource>() ?? gameObject.AddComponent<AudioSource>();
    }
    IEnumerator TextAnim(int length)
    {
        isAnim = true;
        if(textCurser.activeSelf == true){
            textCurser.SetActive(false);
        }
        int totalChar = length;
        int counter = 0;

        while(true){
            visibleChar = counter%(totalChar+1);
            diagTMP.maxVisibleCharacters = visibleChar;
            if(counter<length && (diagTMP.text[counter] !=' ' && diagTMP.text[counter] !='.'))
                sound.Play();
            if (visibleChar>=totalChar || isAnim==false){
                diagTMP.maxVisibleCharacters = totalChar;  
                textCurser.SetActive(true);
                isAnim=false;
                yield break;
            }
            counter++;
            yield return new WaitForSeconds(spd);
        }
    }
}
