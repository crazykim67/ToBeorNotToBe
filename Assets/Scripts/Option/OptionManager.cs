using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionManager : MonoBehaviour
{
    private static OptionManager instance;

    public static OptionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new OptionManager();
                return instance;
            }
            return instance;
        }
    }

    [Header("OptionBG")]
    public GameObject optionBG;

    [Header("Apply")]
    public Button applyBtn;

    [Header("Controllers")]
    public SoundController soundController;
    public ScreenController screenController;
    public LanguageController languageController;

    //[HideInInspector]
    public bool isMenu;

    public void Awake()
    {
        if (instance == null) 
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        applyBtn.onClick.AddListener(() => { SetActive(false); });
    }

    public void Start()
    {
        screenController.Init();
        languageController.Init();
    }

    public void SetActive(bool isAct)
    {
        if(!isAct) 
        {
            screenController.ScreenApply();
            languageController.OnChangeLanguage();
        }

        optionBG.SetActive(isAct);
    }
}
