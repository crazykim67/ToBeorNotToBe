using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScreenController : MonoBehaviour
{
    public FullScreenMode screenMode;
    public List<Resolution> resolutions = new List<Resolution>();
    public TMP_Dropdown dropdown;
    public Toggle fullScreenToggle;

    private int resuolutionNum;

    public void Init()
    {
#if UNITY_EDITOR
        resolutions.AddRange(Screen.resolutions);
#else
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
#endif


        dropdown.options.Clear();

        int optionNum = 0;

        foreach(var item in resolutions) 
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = $"{item.width} x {item.height}";
            dropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height) 
                dropdown.value = optionNum;

            optionNum++;
        }

        dropdown.RefreshShownValue();
        fullScreenToggle.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int index)
    {
        resuolutionNum = index;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void ScreenApply()
    {
        Screen.SetResolution(resolutions[resuolutionNum].width, resolutions[resuolutionNum].height, screenMode);
    }
}
