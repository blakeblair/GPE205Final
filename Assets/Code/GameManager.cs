using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private InputActionAsset _controls;

    [SerializeField]
    private TankPawn tankPrefab;
    
    public static InputActionAsset Controls => Instance._controls;

    public Transform WaypointsRoot;
    public Transform[] Waypoints;

    public AISkill[] Skills;

    private void Awake()
    {
        Instance = this;
        _controls.Enable();
        Waypoints = WaypointsRoot.GetComponentsInChildren<Transform>();
    }


    public TankPawn CreatePlayerPawn()
    {
        var tank = Instantiate(tankPrefab);

        tank.AddComponent<PlayerController>();
    
        return tank;
    }

    public TankPawn CreateEnemyPawn()
    {
        var tank = Instantiate(tankPrefab);
        tank.AddComponent<AIController>();
        var senses = tank.AddComponent<AISenses>();

        senses.Skill = Skills[UnityEngine.Random.Range(0, Skills.Length)];
        tank.name = senses.Skill.name;

        return tank;
    }

    public void GameOver(PlayerController playerController)
    {
        
    }

    public void Respawn(Pawn pawn)
    {
        
    }

}

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineVirtualCamera VirtualCamera;

    public void Attach(TankPawn tankPawn)
    {
        VirtualCamera.Follow = tankPawn.cameraFollow;
        VirtualCamera.LookAt = tankPawn.cameraLookAt;
    }

}
