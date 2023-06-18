using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSoundManager : MonoBehaviour
{
    private static ItemSoundManager instance;

    public static ItemSoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemSoundManager();
                return instance;
            }

            return instance;
        }
    }

    [HideInInspector]
    public AudioSource ad;

    private void Awake()
    {
        instance = this;
        ad = GetComponent<AudioSource>();
    }

    public void Play(AudioClip _clip)
    {
        ad.clip = _clip;
        ad.Play();
    }
}
