using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        Instance = this;
        _controls.Enable();
    }

    public static GameManager Instance;

    [SerializeField]
    InputActionAsset _controls;

    public static InputActionAsset Controls => Instance._controls;


    public void GameOver(PlayerController playerController)
    {
        
    }

    public void Respawn(Pawn pawn)
    {
        
    }

}
