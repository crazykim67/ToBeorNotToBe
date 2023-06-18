using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExplainHud : MonoBehaviour
{
    public GameObject explainHud;
    public GameObject weaponExplainHud;

    [Header("Item UI")]
    public TextMeshProUGUI itemNameText;
    public RawImage itemImage;
    public TextMeshProUGUI itemExplainText;
    public CanvasGroup itemgroup;

    [Header("Weapon UI")]
    public TextMeshProUGUI weaponNameText;
    public RawImage weaponImage;
    public TextMeshProUGUI weaponExplainText;
    public CanvasGroup weaponGroup;

    #region Item

    public void OnExplain(ItemSlot _item)
    {
        explainHud.SetActive(true);
        StartCoroutine(FadeIn());

        itemNameText.text = _item.itemData.itemName.GetLocalizedString();
        itemImage.texture = _item.itemData.img;
        itemExplainText.text = _item.itemData.itemExplain.GetLocalizedString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(explainHud.GetComponent<RectTransform>());
    }

    public void OffExplain()
    {
        itemgroup.alpha = 0f;
        explainHud.SetActive(false);

        itemNameText.text = "";
        itemImage.texture = null;
        itemExplainText.text = "";
    }

    #endregion

    #region Item

    public void OnWeaponExplain(MainWeaponView view)
    {
        if (view.weaponData == null)
        {
            OffWeaponExplain();
            return;
        }

        weaponExplainHud.SetActive(true);
        StartCoroutine(WeaponFadeIn());

        weaponNameText.text = view.weaponData.weaponName.GetLocalizedString();
        weaponImage.texture = view.weaponImage.texture;
        weaponExplainText.text = view.weaponData.description.GetLocalizedString();

        LayoutRebuilder.ForceRebuildLayoutImmediate(weaponExplainHud.GetComponent<RectTransform>());
    }

    public void OffWeaponExplain()
    {
        weaponGroup.alpha = 0f;
        weaponExplainHud.SetActive(false);

        weaponNameText.text = "";
        weaponImage.texture = null;
        weaponExplainText.text = "";
    }

    #endregion

    private IEnumerator FadeIn()
    {
        while(itemgroup.alpha < 1.0f)
        {
            itemgroup.alpha += 0.1f * Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator WeaponFadeIn()
    {
        while (weaponGroup.alpha < 1.0f)
        {
            weaponGroup.alpha += 0.1f * Time.deltaTime;

            yield return null;
        }
    }
}
