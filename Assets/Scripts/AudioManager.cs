﻿using UnityEngine;
using System;
using System.Collections;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float volumeVariance = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchVariance = 0.1f;

    public bool loop = false;

    private AudioSource source;
    public void SetSource (AudioSource _source) {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play () {
        try {
            source.volume = volume * (1 + UnityEngine.Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
            source.pitch = pitch * (1 + UnityEngine.Random.Range(-pitchVariance / 2f, pitchVariance / 2f));
            source.Play();
        }
        catch (NullReferenceException e) {
            Debug.Log("AudioManager: Referenced audio instance not found. (" + e + ")");
        }
    }

    public void Stop() {
        try {
            source.Stop();
        }
        catch (NullReferenceException e) {
            Debug.Log("AudioManager: Referenced audio instance not found. (" + e + ")");
        }
    }
}

public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    void Awake() {
        if (instance != null) {
            if (instance != this) {
                Destroy(this.gameObject);
            }
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start () {
        for (int i = 0; i < sounds.Length; i++) {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
        }
    }

    public void PlaySound (string _name) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == _name) {
                sounds[i].Play();
                return;
            }
        }

        Debug.LogWarning("Audio Manager: No sound with name '" + _name + "' in sound array");
    }

    public void StopSound (string _name) {
        for (int i = 0; i < sounds.Length; i++) {
            if (sounds[i].name == _name) {
                sounds[i].Stop();
                return;
            }
        }
    }

}