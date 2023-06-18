using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToastController : MonoBehaviour
{
    private static ToastController instance;


    public static ToastController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ToastController();
                return instance;
            }

            return instance;
        }
    }

    private bool isOn = true;

    private TextMeshProUGUI toastText;

    private float timer = 0f;

    private IEnumerator coroutine;

    private void Awake()
    {
        instance = this;
        toastText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isOn)
        {
            if(timer <= 3f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0f;
                isOn = false;
                coroutine = HideToast();
                StartCoroutine(coroutine);
            }
        }
    }

    public void OnToast(string text)
    {
        StartCoroutine(ShowToast(text));
    }

    public IEnumerator ShowToast(string text)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        toastText.color = new Color(toastText.color.r, toastText.color.g, toastText.color.b, 0);
        toastText.enabled = true;
        toastText.text = text;

        while (toastText.color.a < 1f)
        {
            toastText.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }

        timer = 0f;
        isOn = true;
    }

    public IEnumerator HideToast()
    {
        while (toastText.color.a > 0f && !isOn)
        {
            toastText.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
        toastText.enabled = false;
    }
}
