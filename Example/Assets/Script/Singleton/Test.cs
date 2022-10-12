using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public GameObject test;
    // 싱글톤 스크립트 호출
    void Start(){
        Instantiate(test,transform.position,transform.rotation);
        DataManager.Instance.Save();
    }
}
