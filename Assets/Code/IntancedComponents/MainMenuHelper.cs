using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuHelper : MonoBehaviour
{
    public Slider BGVolumeSlider;
    public Slider SFXVolumeSlider;
    public Slider MasterVolumeSlider;


    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        BGVolumeSlider.value = PlayerPrefs.GetFloat("BGVolume", 1f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        BGVolumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SFXVolumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        MasterVolumeSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });


        ValueChangeCheck();
    }

    private void ValueChangeCheck()
    {

        PlayerPrefs.SetFloat("BGVolume", BGVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeSlider.value);


        SetVolume("SFXVolume", SFXVolumeSlider.value);
        SetVolume("BGMVolume", BGVolumeSlider.value);
        SetVolume("MasterVolume", MasterVolumeSlider.value);
    }

    private void SetVolume(string keyName, float value)
    {
        var logValue = Mathf.Clamp(Mathf.Log10(value) * 20.0f, -80, 0);
        mixer.SetFloat(keyName, logValue);
    }

}
