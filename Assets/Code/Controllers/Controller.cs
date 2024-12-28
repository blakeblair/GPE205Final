using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Pawn pawn;

    [field: SerializeField]
    public int Score { get; protected set; }

    public int PlayerNumber;

    public bool Dead;

    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
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
