using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test1 : MonoBehaviour
{
    //테스트용 코드
    public void Click(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
