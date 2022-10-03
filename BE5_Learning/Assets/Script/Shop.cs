using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public RectTransform uiGroup;
    public Animator anim;
    public ObjectManager obj;
    
    public string[] itemName;
    public int[] itemPrice;
    public Transform[] itemPos;
    public string[] talkData;
    public TextMeshProUGUI textTalk;

    Player enterPlayer;

    public void Enter(Player player)
    {
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int i){
        int price = itemPrice[i];
        if(price > enterPlayer.coin){
            StopCoroutine("Talk");
            StartCoroutine("Talk");
            return;
        }
        enterPlayer.coin -= price;
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
        GameObject item = obj.CreateObj(itemName[i]);
        item.transform.position = itemPos[i].position + ranVec;
        item.transform.rotation = Quaternion.identity;
    }

    IEnumerator Talk(){
        textTalk.text = talkData[1];
        yield return new WaitForSeconds(1.2f);
        textTalk.text = talkData[0];
    }
}
