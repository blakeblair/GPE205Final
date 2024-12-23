using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public abstract void Start();
    
    //find out what the arguments are for the Move and Rotate functions
    public abstract void HullMove();

    public abstract void HullRotate();
    
    public abstract void TurretRotate();
}
