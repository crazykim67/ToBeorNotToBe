using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameMenuController : MonoBehaviour
{
    public GameObject backMenu;

    public Button settingBtn;
    public Button exitBtn;
    public Button backBtn;

    [Header("Exit Btns")]
    public Button yesBtn;
    public Button noBtn;

    [Header("Exit Repeat")]
    public GameObject exitRepeatMenu;

    public void Awake()
    {
        settingBtn.onClick.AddListener(() => { OptionManager.Instance.SetActive(true); });
        exitBtn.onClick.AddListener(() => { OnRepeat(true); });
        backBtn.onClick.AddListener(() => { ShowNHideMenu(false); });

        yesBtn.onClick.AddListener(() => 
        { 
            OptionManager.Instance.isMenu = false;
            Cursor.visible = true;
            LoadingSceneController.LoadScene("MainMenu");
        });
        noBtn.onClick.AddListener(() => { OnRepeat(false); });
    }

    public void Update()
    {
        if (OptionManager.Instance == null)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            ShowNHideMenu(true);
    }

    public void ShowNHideMenu(bool isAct)
    {
        backMenu.SetActive(isAct);

        OptionManager.Instance.isMenu = isAct;

        if (isAct)
        {
            Cursor.visible = isAct;
            Cursor.lockState = CursorLockMode.None;
            InventoryManager.Instance.OnBackMenu(true);
        }
        else
        {
            if(InventoryManager.Instance != null)
                if (!InventoryManager.Instance.isUi)
                {
                    Cursor.visible = isAct;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            
            InventoryManager.Instance.OnBackMenu(false);
        }
    }

    public void OnRepeat(bool isAct)
    {
        exitRepeatMenu.SetActive(isAct);
    }
}
