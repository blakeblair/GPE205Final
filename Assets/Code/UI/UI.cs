using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance;

    public AudioMixer audioMixer;
    public TextMeshProUGUI Title;
    public Button OptionsButton;
    public TextMeshProUGUI Options;
    public Button StartButton;
    public TextMeshProUGUI StartText;
    public Button ExitButton;
    public TextMeshProUGUI Exit;
    public Toggle YAxisInvert;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public Toggle DebugMode;

    public GameObject options;

    public CanvasGroup canvasGroup;
    public static bool IsPaused = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnInvertToggleChanged(YAxisInvert.isOn);
        YAxisInvert.onValueChanged.AddListener(OnInvertToggleChanged);
    }

    private void OnInvertToggleChanged(bool invert)
    {
        GameManager.Instance.invertY = invert;

        if(GameManager.Instance.playerPawn)
        {
            GameManager.Instance.playerPawn.Movement.invertY = invert;
        }
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void PauseGame(bool pause)
    {
        if(pause)
        {
            canvasGroup.alpha = 1.0f;
            Time.timeScale = 0f;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0.0f;
            Time.timeScale = 1f;
            canvasGroup.blocksRaycasts = false;
        }
        IsPaused = pause;

    }

    public void ShowOptions(bool show)
    {
        options.SetActive(show);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
