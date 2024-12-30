using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //sets up the inputs from the new unity input system to be used by the game
    [SerializeField]
    private InputActionAsset _controls;

    [SerializeField]
    private TankPawn tankPrefab;
    
    //accessor for the controls
    public static InputActionAsset Controls => Instance._controls;
    
    //Root holds all the waypoints, makes dragging in all waypoints easier in unity
    public Transform WaypointsRoot;
    //array of all Patrol Points in the scene, populated at start
    public Transform[] Waypoints;

    //AI Personalities
    public List<AISkill> Skills;

    public TankParameters BaseTankParameters;

    public int numEnemies;

    bool GameStarted = false;
    public Transform pawnRoot;

    public List<TankPawn> AllPawns;

    public int score;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI healthText;

    //max number of enemies that can exist simultaneously
    public int maxEnemies = 10;

    //time between enemy spawn attempts
    public float spawnTimer;
    public float spawnInterval = 10;

    //the most important bool in the game
    public bool invertY;

    public TankPawn playerPawn;

    public GameObject gameOver;

    private void Update()
    {
        if (!GameStarted) return;

        SpawnTimerUpdate();

        Pause();
    }

    private void Pause()
    {
        if (_controls.FindAction("Pause").WasPressedThisFrame())
        {
            if (GameStarted)
            {
                UI.Instance.PauseGame(!UI.IsPaused);
            }
        }
    }

    //timer
    //attempts to spawn enemy tanks on a timer, restricted by maxEnemies
    private void SpawnTimerUpdate()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer < 0)
        {
            if (AllPawns.Count < maxEnemies)
                SpawnRandomEnemy();

            spawnTimer = spawnInterval;
        }
    }

    //returns a random Patrol Point
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
        PopulateWaypoints();
    }

    private void PopulateWaypoints()
    {
        Waypoints = new Transform[WaypointsRoot.childCount];
        int index = 0;
        foreach(Transform t in WaypointsRoot)
        {
            Waypoints[index] = t;
            index++;
        }
    }

    public void StartGame()
    {
        if (GameStarted) return;
        
        pawnRoot = new GameObject("PawnRoot").transform;

        var player = CreatePlayerPawn();

        player.transform.GetComponent<Rigidbody>().MovePosition(RandomWaypoint.position);
        player.transform.rotation = RandomWaypoint.rotation;

        player.SetParameters(BaseTankParameters);

        player.Health.HealthChanged += OnPlayerHealthChanged;
        playerPawn = player;
        healthText.text = player.Health.CurrentHealth.ToString();
        CameraManager.Instance.Attach(player);

        player.transform.parent = pawnRoot;


        GameStarted = true;
    }
    
    //Updates HUD
    private void OnPlayerHealthChanged(int health, TankPawn damager)
    {
        healthText.text = health.ToString();
    }

    public TankPawn SpawnRandomEnemy()
    {
        int iterations = 10;
        int count = 0;

        Transform spawn = null;
        while (count < iterations) 
        {
            spawn = RandomWaypoint;
            //checks spawnpoint for preexisting occupant before allowing a new tank to spawn
            if (!Physics.CheckSphere(spawn.position, 3f, LayerMask.GetMask("Tank"), QueryTriggerInteraction.Ignore))
            {
                break;
            }

            count++;
        }
        //for now enemies are random, but this could be updated to increase difficulty of enemies as the game progresses
        var skill = Skills[UnityEngine.Random.Range(0, Skills.Count)];
        var enemy = CreateEnemyPawn(skill);
        enemy.transform.position = spawn.position;

        return enemy;
    }

    public void EndGame()
    {
        AllPawns.Clear();
        if(pawnRoot != null)
        {
            Destroy(pawnRoot.gameObject);
        }

        ShowGameOverScreen();
        StartCoroutine(SwitchScene());
    }

    private void ShowGameOverScreen()
    {
        gameOver.SetActive(true);
    }

    //shows game over screen for 5 seconds before returning to main menu
    IEnumerator SwitchScene()
    {
        Debug.Log("Waiting..");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(0);
        Debug.Log("Switch!");

    }

    //similar to my previous spawner adding components at runtime, allows modular changes to spawnable tanks later
    public TankPawn CreatePlayerPawn()
    {
        var tank = Instantiate(tankPrefab);

        tank.AddComponent<PlayerController>();

        tank.Movement.invertY = invertY;

        AllPawns.Add(tank);
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
        AllPawns.Add(tank);

        if (skill.CustomParameters)
            tank.SetParameters(skill.CustomParameters);
        else
            tank.SetParameters(BaseTankParameters);
        tank.transform.parent = Instance.pawnRoot;
        SetMaterial(tank, skill);

        return tank;
    }

    //sets the color of the tank based on personality
    private void SetMaterial(TankPawn tank, AISkill skill)
    {
        var material = tank.GetComponentInChildren<SkinnedMeshRenderer>().material;
        material.SetColor("_Color", skill.TankColor);
    }

    internal static void AddScore(int score)
    {
        Instance.score += score;
        Instance.ScoreText.text = Instance.score.ToString();
        Instance.gameOverScoreText.text = Instance.score.ToString();
    }
}
