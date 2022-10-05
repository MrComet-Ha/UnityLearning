using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool isDestroyed = false;
    //누군가 자신을 선언해서 호출되는 것을 방지하기 위해
    //자기 자신을 private로 선언
    private static T instance;
    //이후 자신을 return 해주는 public을 선언
    public static T Instance{
        get{
            if(isDestroyed){
                return null;
            }
            //만약 이 스크립트를 갖고 있는 오브젝트가 없으면
            if(instance == null){
                //우선 한번 찾아보고
                instance = (T)FindObjectOfType(typeof(T));
                //없으면 생성한 뒤
                if(instance == null){
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    //그것을 가져온다.
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake(){
        //중복 확인
        int checkObj = FindObjectsOfType<T>().Length;
        //만약 이 오브젝트가 이미 존재한다면 오브젝트를 파괴하고 코드를 종료한다.
        if(checkObj > 1){
            Destroy(this.gameObject);
            return;
        }
        //씬 넘어가도 파괴되지 않게 처리
        //부모 오브젝트가 있거나, 상위 오브젝트가 있다면
        if(transform.parent != null || transform.root != null)
        //그 오브젝트를 보호하고
            DontDestroyOnLoad(this.transform.root.gameObject);
        else
        //아니면 자기 자신을 보호한다.
            DontDestroyOnLoad(this.gameObject);
    }

    private void OnApplicationQuit()
    {
        isDestroyed = true;
    }
    
    private void OnDestroy() {
        isDestroyed = true;
    }
    
}