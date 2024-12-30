using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pawn pawn;

    [field: SerializeField]
    public int Score { get; protected set; }

    public int PlayerNumber;


    protected virtual void Awake()
    {
        pawn = GetComponent<Pawn>();
    }

    public virtual void ProcessInputs()
    {
        
    }
}

    public enum ControllerType
    {
        Player,
        AI
    }
