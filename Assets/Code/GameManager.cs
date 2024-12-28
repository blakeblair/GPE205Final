using System;
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

    public Transform RandomWaypoint
    {
        get
        {
            return GameManager.Instance.Waypoints[UnityEngine.Random.Range(0, Instance.Waypoints.Length)];
        }
    }

    private void Awake()
    {
        Instance = this;
        _controls.Enable();
        Waypoints = WaypointsRoot.GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        var player = CreatePlayerPawn();

        player.transform.GetComponent<Rigidbody>().MovePosition(RandomWaypoint.position);
        player.transform.rotation = RandomWaypoint.rotation;

        CameraManager.Instance.Attach(player);

        var enemy = CreateEnemyPawn();

        enemy.transform.GetComponent<Rigidbody>().MovePosition(RandomWaypoint.position);
        enemy.transform.rotation = RandomWaypoint.rotation;

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
        tank.AddComponent<AIWaypointing>();
        var senses = tank.AddComponent<AISenses>();
        tank.AddComponent<AIController>();

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
