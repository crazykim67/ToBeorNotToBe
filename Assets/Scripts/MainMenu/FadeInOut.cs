using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public MapSwitchSystem mapSystem;

    [SerializeField]
    private RawImage fadeImage;

    private bool firstTime;

    public IEnumerator FadeOut()
    {
        float fadeCount = 0;
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }

        StartCoroutine(FadeIn());
        
    }
    public IEnumerator FadeIn()
    {
        if (!firstTime)
            firstTime = true;
        else
        {
            mapSystem.ChangeCurrentMap();
            mapSystem.MapSwitch((int)mapSystem.currentMap);
        }

        float fadeCount = 1.0f;
        while (fadeCount > 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            fadeImage.color = new Color(0, 0, 0, fadeCount);
        }


    }
}
