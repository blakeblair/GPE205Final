using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{ 
    public abstract void HullMove(float vertical);

    public abstract void HullRotate(float horizontal);
    
    public abstract void TurretRotate(float horizontal);
}
