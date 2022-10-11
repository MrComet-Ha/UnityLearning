using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectState : MonoBehaviour
{
    //정적 변수 선언(why? 동일한 스크립트를 가진 모든 오브젝트에서 사용할 예정이기 때문에)
    private static int m_NextValidId = 0;
    //BaseObjectState를 받는 모든 오브젝트에 ID를 매김
    //고유 id, 수정할 수 없게 해둠.
    private int id;
    //id를 외부로 보여주는 역할
    public int ID{
        set{
            id = value;     //id를 Setup에서 확정한 ID의 값으로 변경해줌
            m_NextValidId++;    //함수가 끝난 후 ID값을 증가
        }
        get => id;    //ID값을 id값으로 덮어씌워서 확정  
    }
    string entityName;  //이름
    string personalColor;   //색상

    
    public virtual void Setup(string name){
        // 고유 id 설정
        ID = m_NextValidId;
        entityName = name;

        int color = Random.Range(0,100000);
        personalColor = $"#{color.ToString("X6")}";
    }

    // 차후 Controller 에서 모든 에이전트의 Updated()를 호출해 에이전트를 구동한다.
    public abstract void Updated();

    public void PrintText(string text){
        Debug.Log($"<color={personalColor}><b>{entityName}</b></color> : {text}");
    }
}
