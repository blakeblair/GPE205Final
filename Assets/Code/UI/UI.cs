using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public AudioMixer audioMixer;
    public TextMeshProUGUI Title;
    public Button OptionsButton;
    public TextMeshProUGUI Options;
    public Button StartButton;
    public TextMeshProUGUI Start;
    public Button ExitButton;
    public TextMeshProUGUI Exit;
    public Toggle YAxisInvert;
    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SFXVolume;
    public Toggle DebugMode;
}
