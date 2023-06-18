using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster gr;

    //[HideInInspector]
    public MainWeaponView weaponView;

    public RawImage weaponImage;

    public TextMeshProUGUI weaponNameText;

    [Header("Active Attachment")]
    public GameObject muzzleView;
    public GameObject gribView;
    public GameObject sightView;
    public GameObject magView;
    public GameObject buttStockView;

    [Header("Attachment Image")]
    public RawImage muzzleImage;
    public RawImage gribImage;
    public RawImage sightImage;
    public RawImage magImage;
    public RawImage buttStockImage;

    [HideInInspector]
    public bool isAct = false;

    public void SetItemData(MainWeaponView _view)
    {
        if (_view == null)
            return;

        if (_view.weaponData == null)
            return;

        weaponView = _view;

        weaponImage.texture = weaponView.weaponImage.texture;
        weaponNameText.text = _view.weaponNameText.StringReference.GetLocalizedString();
        OnViewActive(weaponView.weaponData);

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        rect.position = Input.mousePosition;

        this.gameObject.SetActive(true);

        isAct = true;
    }

    public void OffClickItem()
    {
        this.gameObject.SetActive(false);

        weaponView = null;

        weaponImage.texture = null;
        weaponNameText.text = null;

        OnViewActive();

        isAct = false;
    }

    public void OnViewActive(WeaponData data = null)
    {
        if (data != null)
        {
            #region Muzzle

            if (data.isMuzzle)
            {
                muzzleView.SetActive(true);

            }
            else
            {
                muzzleView.SetActive(false);

            }

            #endregion

            #region Grib

            if (data.isGrib)
                gribView.SetActive(true);
            else
                gribView.SetActive(false);

            #endregion

            #region Sight

            if (data.isSight)
                sightView.SetActive(true);
            else
                sightView.SetActive(false);

            #endregion

            #region Magazine

            if (data.isMag)
                magView.SetActive(true);
            else
                magView.SetActive(false);

            #endregion

            #region ButtStock

            if (data.isButt)
                buttStockView.SetActive(true);
            else
                buttStockView.SetActive(false);

            #endregion
        }
        else
        {
            muzzleView.SetActive(false);
            gribView.SetActive(false);
            sightView.SetActive(false);
            magView.SetActive(false);
            buttStockView.SetActive(false);
        }
    }

    public void Raycast()
    {
        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if (OptionManager.Instance != null)
            if (OptionManager.Instance.isMenu)
                return;

        if (results[0].gameObject.tag == "Outside")
        {
            if (InventoryManager.Instance.isUse)
                ItemManager.Instance.UseCancel();

            InventoryManager.Instance.weaponInventory.OnDrop((int)weaponView.slotType);
            if ((int)EquipManager.Instance.equipSlot == (int)weaponView.slotType)
                EquipManager.Instance.OnEquip((int)EquipManager.Instance.equipSlot);
        }
        else if (results[0].gameObject.tag == "WeaponSlot")
        {
            if (InventoryManager.Instance.isUse)
                ItemManager.Instance.UseCancel();

            MainWeaponView swapView = results[0].gameObject.GetComponent<MainWeaponView>();

            if (weaponView.slotType == swapView.slotType)
                return;

            if(swapView.weaponData == null)
                InventoryManager.Instance.weaponInventory.OnSwap((int)swapView.slotType, null);
            else
                InventoryManager.Instance.weaponInventory.OnSwap((int)swapView.slotType, weaponView.weaponData);
            EquipManager.Instance.OnSwapEquip((int)swapView.slotType);
        }
    }
}
