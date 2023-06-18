using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class DayNightController : MonoBehaviour
{
    [SerializeField]
    private Light directionalLight;
    [SerializeField]
    private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeofDay;

    [SerializeField]
    private float targetTime = 60f;
    public void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            timeofDay += Time.deltaTime / targetTime;
            timeofDay %= 24; // Clamp between 0 - 24
            UpdateLighting(timeofDay / 24f);
        }
        else
            UpdateLighting(timeofDay / 24f);
    }

    private void UpdateLighting(float _time)
    {
        RenderSettings.ambientEquatorColor = preset.ambientColor.Evaluate(_time);
        RenderSettings.fogColor = preset.fogColor.Evaluate(_time);

        if(directionalLight != null)
        {
            directionalLight.color = preset.directionalColor.Evaluate(_time);

            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((_time * 360f) - 90f, 170f, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
            directionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional) 
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
