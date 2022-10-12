using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpd;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + offset;
        transform.RotateAround(target.position,Vector3.up,orbitSpd * Time.deltaTime);
        offset = transform.position - target.position;
    }
}
