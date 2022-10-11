using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Locations {Home = 0, Dungeon, Studio, Colosseum, Inn}
public class Controller : MonoBehaviour
{
    [SerializeField]
    private string[] arrayPlayers;  //플레이어 이름 배열

    [SerializeField]
    private GameObject playerPrefab; //플레이어 오브젝트 프리펩

    // 재생 제어를 위한 모든 플레이어 리스트
    private List<BaseObjectState> entities;
    // 재생/정지를 위한 bool값
    public static bool isGameStop { set; get; } = false;

    void Awake(){
        entities = new List<BaseObjectState>();
        for(int i = 0; i < arrayPlayers.Length; i++){
            GameObject clone = Instantiate(playerPrefab);
            Player entity = clone.GetComponent<Player>();
            entity.Setup(arrayPlayers[i]);
            entities.Add(entity);
        }
    }

    void Update(){
        if(isGameStop) return;
        for(int i = 0; i < entities.Count; i++){
            entities[i].Updated();
        }
    }

    public static void Stop(BaseObjectState entity){
        isGameStop = true;
        entity.PrintText("100 승 를! 했어요. 이제 그만~");
    }
}
