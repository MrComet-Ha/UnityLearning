using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    Dictionary<int, string[]> diagData;
    Dictionary<int, Sprite> portData;

    public Sprite[] portArr;

    void Awake()
    {
        diagData = new Dictionary<int, string[]>();
        portData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        //dialog data(idle)
        diagData.Add(1000,new string[]{
            "못생겼어.:1", 
            "진짜 못생겼어.:0", 
            "역할 정도로 못생겼어.:2", 
            "나도 못생겼다고?:1", 
            "야!:3"
        });
        diagData.Add(2000,new string[]{
            "안아줘요!:2", 
            "안아줘요.:1", 
            "안아줘.:0", 
            "안아달라고.:0", 
            "안아.:3"
        });
        diagData.Add(100,new string[]{
            "박스다.", "부숴볼까 싶었는데, 너무 더럽다."
        });
        diagData.Add(200,new string[]{
            "책상이다.", "누구 책상인지는 몰라도 더럽다."
        });

        //quest data
        diagData.Add(10+1000, new string[]{
            "너 진짜 못생겼다.:2",
            "좀 잘 생겨보이게 해줄게. \n일 하나만 해라.:1",
            "저기, 옆집 안아줘요 빌런있지? \n그 집 옆에 상자가 하나 있어.:1",
            "그 안에 숨겨둔 화장품이 있거든?. \n그거 훔쳐와봐.:1",
            "그러면 그 못 생긴 얼굴에도 \n바를 수 있게 해줄게.:2",
            "뭐해? 빨리 안가고!:3"});
        diagData.Add(11+100, new string[]{
            "상자가 있다...",
            "일단 박살내보자.",
            "주먹으로 박살내니 화장품이 나왔다.",
            "빨리 주워가자."});
        diagData.Add(12+300, new string[]{
            "화장품을 얻었다.",
            "이제 그 여자한테 돌아가자.",
            "잠깐. 남자 목소리가 들리는데...",
            "남자에게 가보도록 하자."});
        diagData.Add(20+2000, new string[]{
            "안아줘요!:3",
            "안아줘요.:1",
            "안아줘요?:2",
            "안아줘요!:3",
            "...:0",
            "아니 말을 못 알아듣나...:3",
            "가라. 훔친거는 봐준다.:0"});
        diagData.Add(21+1000, new string[]{
            "갖고 왔어?:1",
            "이야, 우리 호... \n아니, 일 잘하네?:2",
            "한번 발라 볼까? :1",
            "아이, 촉촉해.:2",
            "... 뭘 봐? 뭐? 너도 발라달라고?:1",
            "저리 꺼져.:3"
        });

        //portrait data
        //0:idle 1:speak 2:Smile 3:Angry
        portData.Add(1000 + 0,portArr[0]);
        portData.Add(1000 + 1,portArr[1]);
        portData.Add(1000 + 2,portArr[2]);
        portData.Add(1000 + 3,portArr[3]);
        portData.Add(2000 + 0,portArr[4]);
        portData.Add(2000 + 1,portArr[5]);
        portData.Add(2000 + 2,portArr[6]);
        portData.Add(2000 + 3,portArr[7]);
    }


    public string GetDiag(int id, int DiagIndex){
        if(!diagData.ContainsKey(id)){
            if(!diagData.ContainsKey(id-id%10))
                return GetDiag(id-id%100,DiagIndex);
            else
                return GetDiag(id-id%10,DiagIndex);
        }
        if(DiagIndex == diagData[id].Length)
            return null;
        else
            return diagData[id][DiagIndex];
    }
    public Sprite GetPort(int id, int portIndex){
        return portData[id + portIndex];
    }
}
