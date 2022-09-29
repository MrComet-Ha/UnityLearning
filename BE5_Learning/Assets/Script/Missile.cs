using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public ParticleSystem particle;
    void OnEnable()
    {
        particle.Play();
        transform.rotation = UnityEngine.Quaternion.identity;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * 30 * Time.deltaTime);
    }
}
