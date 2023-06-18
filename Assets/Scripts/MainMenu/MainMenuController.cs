using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static MainMenuController;

public class MainMenuController : MonoBehaviour
{
    public enum MenuType
    {
        MainMenu,
        Difficulty,
    }

    [Header("MainMenu")]
    public Button startBtn;
    public Button optionBtn;
    public Button exitBtn;

    [Header("Difficulty")]
    public Button easyBtn;
    public Button normalBtn;
    public Button hardBtn;

    [Header("BackBtn")]
    public Button backBtn;

    [Header("Menu Category")]
    public GameObject mainMenu;
    public GameObject difficultyMenu;

    [Header("CurrentMenuType")]
    public MenuType menuType = MenuType.MainMenu;

    public ExitController exitController;

    public void Awake()
    {
        startBtn.onClick.AddListener(() => { OnMenuType(MenuType.Difficulty); });
        optionBtn.onClick.AddListener(() => { OptionManager.Instance.SetActive(true); });
        exitBtn.onClick.AddListener(() => { exitController.OnShowNHide(true); });

        easyBtn.onClick.AddListener(() => 
        {
            OptionManager.Instance.soundController.OnSceneAsyncLoaded("Motion");
            LoadingSceneController.LoadScene("MainGame");
            MainGameManager.Instance.SetDifficult(MainGameManager.Diffculty.Easy);
        });
        normalBtn.onClick.AddListener(() => 
        { 
            OptionManager.Instance.soundController.OnSceneAsyncLoaded("Motion");
            LoadingSceneController.LoadScene("MainGame");
            MainGameManager.Instance.SetDifficult(MainGameManager.Diffculty.Normal);
        });
        hardBtn.onClick.AddListener(() =>
        {
            OptionManager.Instance.soundController.OnSceneAsyncLoaded("Motion");
            LoadingSceneController.LoadScene("MainGame");
            MainGameManager.Instance.SetDifficult(MainGameManager.Diffculty.Hard);

        });

        backBtn.onClick.AddListener(() => { OnMenuType(MenuType.MainMenu); });
    }

    public void OnMenuType(MenuType _menuType)
    {
        switch(_menuType) 
        {
            case MenuType.MainMenu:
                {
                    menuType = MenuType.MainMenu;
                    mainMenu.SetActive(true);
                    difficultyMenu.SetActive(false);
                    break;
                }
            case MenuType.Difficulty:
                {
                    menuType = MenuType.Difficulty;
                    mainMenu.SetActive(false);
                    difficultyMenu.SetActive(true);
                    break;
                }
        }
    }

}
