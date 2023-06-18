using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [HideInInspector]
    public AudioSource bgSound;

    [Header("Background Sound List")]
    public AudioClip[] bg;

    public AudioMixer mixer;

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        bgSound = GetComponent<AudioSource>();
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource= go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BackGroundPlay(AudioClip clip) 
    {
        if (bgSound == null)
            return;

        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BackGround")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        if (bgSound != null)
            bgSound.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        for(int i = 0; i < bg.Length; i++) 
        {
            if (scene.name == bg[i].name)
            {
                BackGroundPlay(bg[i]);
            }
        }
    }
    
    public void OnSceneAsyncLoaded(string _scene)
    {
        // 수정 예정
        if (_scene.Equals("Motion"))
        {
            bgSound.Stop();
        }
    }

    public void MasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void BGVolume(float volume)
    {
        mixer.SetFloat("BgVolume", Mathf.Log10(volume) * 20);
    }

    public void SfxVolume(float volume)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
    }
}
