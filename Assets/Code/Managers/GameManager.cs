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

    [SerializeField]
    private InputActionAsset _controls;

    [SerializeField]
    private TankPawn tankPrefab;
    
    public static InputActionAsset Controls => Instance._controls;

    public Transform WaypointsRoot;
    public Transform[] Waypoints;

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

    public int maxEnemies = 10;

    public float spawnTimer;
    public float spawnInterval = 10;

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

    public void StartGame()
    {
        if (GameStarted) return;
        
        pawnRoot = new GameObject("PawnRoot").transform;

        var player = CreatePlayerPawn();

        player.transform.GetComponent<Rigidbody>().MovePosition(RandomWaypoint.position);
        player.transform.rotation = RandomWaypoint.rotation;

        player.SetParameters(BaseTankParameters);

        player.Health.HealthChanged += Health_HealthChanged;
        playerPawn = player;
        healthText.text = player.Health.CurrentHealth.ToString();
        CameraManager.Instance.Attach(player);

        player.transform.parent = pawnRoot;


        GameStarted = true;
    }

    private void Health_HealthChanged(int health, TankPawn damager)
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

            if (!Physics.CheckSphere(spawn.position, 3f, LayerMask.GetMask("Tank"), QueryTriggerInteraction.Ignore))
            {
                break;
            }

            count++;
        }

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

    IEnumerator SwitchScene()
    {
        Debug.Log("Waiting..");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(0);
        Debug.Log("Switch!");

    }

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
