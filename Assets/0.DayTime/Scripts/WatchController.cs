using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WatchController : MonoBehaviour
{
    public enum TimeDay
    {
        morning,
        afternooon,
        evening,
    }

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI meridiemText;

    public int currentTime = 360;
    public float targetTimer = 10f;

    private float timer;

    [SerializeField]
    private TimeDay currentTimeDay = TimeDay.morning;

    public void Awake()
    {
        InitTime();
    }

    public void Update()
    {
        SetTimer();
        UpdateTimeDay(currentTime);
    }

    // 시계 초기화
    public void InitTime()
    {
        timeText.text = $"{SetHour()}:{(currentTime % 60).ToString("00")}";
        SetMeridiem(currentTime);
        UpdateTimeDay(currentTime);
    }

    // 시계 업데이트
    public void SetTimer()
    {
        timer += Time.deltaTime;

        if(timer >= targetTimer)
        {
            timer = 0;
            currentTime += 10;

            // 시간 초기화
            if(currentTime >= 1440)
                currentTime = 0;

            InitTime();
        }
    }

    // 시간 설정
    public string SetHour()
    {
        string hour = "";
        if (currentTime >= 720)
        {
            if (currentTime >= 720 && currentTime < 780)
                return hour = 12.ToString("00");
            else
                return hour = ((currentTime / 60) % 12).ToString("00");
        }
        else
            return hour = (currentTime / 60).ToString("00");
    }

    // AM, PM 설정
    public void SetMeridiem(int _time)
    {
        if (_time < 720)
            meridiemText.text = "AM";
        else
            meridiemText.text = "PM";
    }

    // 아침, 낮, 밤 설정
    public void UpdateTimeDay(int _time)
    {
        // 아침
        if (_time >= 300 && _time < 720)
            currentTimeDay = TimeDay.morning;
        else if (_time >= 720 && _time < 1080)
            currentTimeDay = TimeDay.afternooon;
        else
            currentTimeDay = TimeDay.evening;
    }
}
