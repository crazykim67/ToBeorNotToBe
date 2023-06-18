using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Settings;
using static UnityEngine.Rendering.DebugUI;

public class LanguageController : MonoBehaviour
{
    public int languageId;
    public TMP_Dropdown dropdown;

    public void Init()
    {
        languageId = GetLanguage();
        dropdown.value = GetLanguage();
        StartCoroutine(LocalizationInit());
    }

    private IEnumerator LocalizationInit()
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[languageId];
    }

    public void OnValueChanged(int value)
    {
        languageId = value;
    }

    public void OnChangeLanguage()
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[languageId];
        SetLanguage(languageId);
    }

    #region Get, Set Language

    public int GetLanguage()
    {
        if (!PlayerPrefs.HasKey("Language"))
            PlayerPrefs.SetInt("Language", 0);

        return PlayerPrefs.GetInt("Language");
    }

    public void SetLanguage(int value)
    {
        PlayerPrefs.SetInt("Language", value);
    }

    #endregion
}
