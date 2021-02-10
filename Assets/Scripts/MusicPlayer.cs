using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundtrack;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        if (FindObjectsOfType<MusicPlayer>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        _audioSource = GetComponent<AudioSource>();
    }
    
    void Start ()
    {
        if (!_audioSource.playOnAwake)
        {
            _audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
            _audioSource.Play();
        }
    }
     
    // Update is called once per frame
    void Update ()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.clip = soundtrack[Random.Range(0, soundtrack.Length)];
        _audioSource.Play();
    }
}
