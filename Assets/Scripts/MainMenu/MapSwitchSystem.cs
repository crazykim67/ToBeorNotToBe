using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSwitchSystem : MonoBehaviour
{
    public enum Map
    {
        Area01,
        Area02,
        Area03,
    }

    public Map currentMap = Map.Area01;

    public List<GameObject> maps = new List<GameObject>();
    public List<Transform> cameras = new List<Transform>();

    public float maxTimer;
    private float timer;
    private bool isTime;

    private int maxIndex;

    public FadeInOut fadeSystem;

    public void Awake()
    {
        maxIndex = maps.Count;

        int ran = Random.Range(0, maps.Count);
        currentMap = (Map)ran;
        MapSwitch((int)currentMap);

        StartCoroutine(fadeSystem.FadeIn());
    }

    public void Update()
    {
        CameraRotation();
        OnStartTime();
    }
    
    public void CameraRotation()
    {
        switch (currentMap)
        {
            case Map.Area01:
            case Map.Area03:
                {
                    cameras[(int)currentMap].transform.Rotate(new Vector3(0, 1 * Time.deltaTime, 0));
                    break;
                }
            case Map.Area02:
                {
                    cameras[(int)currentMap].transform.Rotate(new Vector3(-1 * Time.deltaTime, 0, 0));
                    break;
                }
        }
    }

    public void OnStartTime()
    {
        if (isTime)
        {
            if(timer < maxTimer)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                isTime = false;
                StartCoroutine(fadeSystem.FadeOut());
            }
        }
    }

    public void MapSwitch(int index)
    {
        foreach (var map in maps)
            map.SetActive(false);

        cameras[index].rotation = Quaternion.identity;

        maps[index].SetActive(true);
        isTime = true;
    }

    public void ChangeCurrentMap()
    {
        if ((int)currentMap < maxIndex - 1)
        {
            int nextIndex = (int)currentMap + 1;
            currentMap = (Map)nextIndex;
        }
        else
        {
            currentMap = (Map)0;
        }
    }
}
