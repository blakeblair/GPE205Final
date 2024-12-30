using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    //for bullet hit VFX
    public float lifetime = 4f;
    void Start()
    {
        Destroy(gameObject, lifetime);        
    }

}
