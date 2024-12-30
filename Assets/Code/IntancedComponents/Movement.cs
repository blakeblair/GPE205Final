using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public bool invertY = false;

    public abstract void HullMove(float vertical);

    public abstract void HullRotate(float horizontal);
    
    public abstract void TurretRotate(float horizontal);

    public abstract void TurretPitch(float vertical);
}
