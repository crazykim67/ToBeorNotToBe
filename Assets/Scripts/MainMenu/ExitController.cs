using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitController : MonoBehaviour
{
    public GameObject exitBG;

    [Header("Button Group")]
    public Button exitBtn;
    public Button cancelBtn;

    private void Awake()
    {
        exitBtn.onClick.AddListener(OnExit);
        cancelBtn.onClick.AddListener(OnCancel);
    }
    public void OnShowNHide(bool isAct)
    {
        exitBG.SetActive(isAct);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnCancel()
    {
        OnShowNHide(false);
    }
}
