using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public TankParameters BaseTankParameters;

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
        Waypoints = new Transform[WaypointsRoot.childCount];
        int index = 0;
        foreach(Transform t in WaypointsRoot)
        {
            Waypoints[index] = t;
            index++;
        }
        
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

        player.SetParameters(BaseTankParameters);
        
        CameraManager.Instance.Attach(player);

        var skill = Skills[UnityEngine.Random.Range(0, Skills.Length)];
        var enemy = CreateEnemyPawn(skill);

        enemy.transform.GetComponent<Rigidbody>().MovePosition(RandomWaypoint.position);
        enemy.transform.rotation = RandomWaypoint.rotation;
        if (skill.CustomParameters)
            enemy.SetParameters(skill.CustomParameters);
        else
            enemy.SetParameters(BaseTankParameters);

    }

    public TankPawn CreatePlayerPawn()
    {
        var tank = Instantiate(tankPrefab);

        tank.AddComponent<PlayerController>();
    
        return tank;    
    }

    public TankPawn CreateEnemyPawn(AISkill skill)
    {
        var tank = Instantiate(tankPrefab);
        tank.AddComponent<AIWaypointing>();
        var senses = tank.AddComponent<AISenses>();
        tank.AddComponent<AIController>();

        senses.Skill = skill;
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
