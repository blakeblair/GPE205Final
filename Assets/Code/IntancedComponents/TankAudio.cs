using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAudio : MonoBehaviour
{
    public AudioSource MovingSource;
    public AudioSource ShootingSource;
    NoiseMaker noiseMaker;

    public AudioClip deathClip;
    public AudioClip shootClip;

    public float moveNoiseThresholdMax = 2f;
    public float maxMovingVolume = 0.75f;
    private void Awake()
    {
        noiseMaker = GetComponent<NoiseMaker>();
    }
    public void PlayFireSound()
    {
        ShootingSource.PlayOneShot(shootClip);
    }

    public void PlayDeathSound()
    {
        MovingSource.PlayOneShot(deathClip);
    }

    private void Update()
    {
        MovingSource.volume = Mathf.Lerp(MovingSource.volume, Mathf.Min(noiseMaker.movingVolume, maxMovingVolume), Time.deltaTime);
    }
}
