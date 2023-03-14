using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrierCarSound : MonoBehaviour
{
    [SerializeField] float MinPitch = 0.3f;
    [SerializeField] float MaxPitch = 1f;
    [SerializeField] float minVol = 0.1f;
    [SerializeField] float maxVol = 0.3f;
    // Start is called before the first frame update
    [SerializeField] AudioSource sound;
    
    void Start()
    {
        if (sound != null)
        {
            sound.pitch = Random.Range(MinPitch, MaxPitch);
            sound.volume = Random.Range(minVol, maxVol);
        }
    }

    void OnEnable()
    {
        PlayerBehaviour.GameOverEvent += StopSound;
    }

    void OnDisable()
    {
        PlayerBehaviour.GameOverEvent -= StopSound;
    }

    private void StopSound()
    {
        sound.Stop();
    }
}
