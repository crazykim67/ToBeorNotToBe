using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class MainWeaponView : MonoBehaviour
{
    public enum SlotType
    {
        None,
        Main_1,
        Main_2,
        Sub_1,
        Sub_2,
    }

    public SlotType slotType = SlotType.Main_1;

    // 주무기 UI Slot 관련, 습득 후 UI Slot 추가 및 수정
    public WeaponData weaponData;

    public RawImage weaponImage;

    public TextMeshProUGUI ammoText;
    public LocalizeStringEvent weaponNameText;
    public TextMeshProUGUI nameText;

    [Header("Active Attachment")]
    public AttachmentItem muzzleView;
    public AttachmentItem gribView;
    public AttachmentItem sightView;
    public AttachmentItem magView;
    public AttachmentItem buttStockView;

    [Header("WeaponData AttachmentData")]
    public AttachmentData muzzleData;
    public AttachmentData gribData;
    public AttachmentData sightData;
    public AttachmentData magData;
    public AttachmentData buttstockData;

    // 현재 슬롯에 무기가 있는 지 없는 지 체크
    public bool isPosses;

    private Transform playerTr;

    [Header("EquipManager WeaponView")]
    public WeaponView equipWeaponView;
    
    public void PickUpWeapon(DropItem _data)
    {
        weaponData = _data.itemData.weaponData;

        SetActiveText(weaponData);
        GetAttachmentView(_data);

        isPosses = true;

        OnAttachmentData();
    }

    public void DropWeapon()
    {
        Intantiate(this);

        OnAttachmentData();

        weaponData = null;

        SetActiveText();
        SetAttachmentView();

        isPosses = false;

    }

    public void SwapWeapon(MainWeaponView _view, WeaponData _data)
    {
        weaponData = null;

        SetActiveText();
        SetAttachmentView();

        weaponData = _data;

        nameText = weaponNameText.GetComponent<TextMeshProUGUI>();

        SetActiveText(weaponData);
        SetAttachmentView(_view);

        isPosses = true;
        OnAttachmentData();
    }

    public void SwapEmpty()
    {
        weaponData = null;

        SetActiveText();
        SetAttachmentView();

        isPosses = false;
    }

    public void SetAttachmentView(MainWeaponView _view = null)
    {
        if(_view != null)
        {
            if(_view.weaponData != null) 
            {
                #region Muzzle

                if (_view.weaponData.isMuzzle)
                {
                    muzzleView.gameObject.SetActive(true);

                    if(_view.muzzleData != null)
                    {
                        muzzleView.OnEquip(ItemManager.Instance.GetAttachmentData(_view.muzzleData.attachItem));

                    }
                    else
                    {

                    }
                }
                else
                {
                    muzzleView.gameObject.SetActive(false);
                    muzzleView.OnThrow();
                }

                #endregion

                #region Grib

                if (_view.weaponData.isGrib)
                {
                    gribView.gameObject.SetActive(true);

                    if(_view.gribData != null)
                        gribView.OnEquip(ItemManager.Instance.GetAttachmentData(_view.gribData.attachItem));
                }
                else
                {
                    gribView.gameObject.SetActive(false);
                    gribView.OnThrow();
                }

                #endregion

                #region Sight

                if (_view.weaponData.isSight)
                {
                    sightView.gameObject.SetActive(true);

                    if(_view.sightData != null)
                    sightView.OnEquip(ItemManager.Instance.GetAttachmentData(_view.sightData.attachItem));
                }
                else
                {
                    sightView.gameObject.SetActive(false);
                    sightView.OnThrow();
                }

                #endregion

                #region Magazine

                if (_view.weaponData.isMag)
                {
                    magView.gameObject.SetActive(true);

                    if(_view.magData != null)
                    magView.OnEquip(ItemManager.Instance.GetAttachmentData(_view.magData.attachItem));
                }
                else
                {
                    magView.gameObject.SetActive(false);
                    magView.OnThrow();
                }

                #endregion

                #region ButtStock

                if (_view.weaponData.isButt)
                {
                    buttStockView.gameObject.SetActive(true);

                    if(_view.buttstockData != null)
                    buttStockView.OnEquip(ItemManager.Instance.GetAttachmentData(_view.buttstockData.attachItem));
                }
                else
                {
                    buttStockView.gameObject.SetActive(false);
                    buttStockView.OnThrow();
                }

                #endregion
            }
            else
            {
                muzzleView.OnThrow();
                gribView.OnThrow();
                sightView.OnThrow();
                magView.OnThrow();
                buttStockView.OnThrow();
            }
        }
        else
        {
            muzzleView.OnThrow();
            gribView.OnThrow();
            sightView.OnThrow();
            magView.OnThrow();
            buttStockView.OnThrow();
        }

    }

    public void GetAttachmentView(DropItem _item = null)
    {
        if (_item != null)
        {
            #region Muzzle

            if (_item.itemData.weaponData.isMuzzle)
            {
                muzzleView.gameObject.SetActive(true);

                if (_item.muzzle != AttachItem.None)
                {
                    muzzleView.OnEquip(ItemManager.Instance.GetAttachmentData(_item.muzzle));
                    //equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, muzzleData);
                }
                else
                    muzzleData = null;
            }
            else
            {
                muzzleView.gameObject.SetActive(false);
                muzzleView.OnThrow();
            }

            #endregion

            #region Grib

            if (_item.itemData.weaponData.isGrib)
            {
                gribView.gameObject.SetActive(true);

                if (_item.grib != AttachItem.None)
                {
                    gribView.OnEquip(ItemManager.Instance.GetAttachmentData(_item.grib));
                    //equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, gribData);
                }
                else
                    gribData = null;
            }
            else
            {
                gribView.gameObject.SetActive(false);
                gribView.OnThrow();
            }

            #endregion

            #region Sight

            if (_item.itemData.weaponData.isSight)
            {
                sightView.gameObject.SetActive(true);

                if (_item.sight != AttachItem.None)
                {
                    sightView.OnEquip(ItemManager.Instance.GetAttachmentData(_item.sight));
                    //equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, sightData);
                }
                else
                    sightData = null;
            }
            else
            {
                sightView.gameObject.SetActive(false);
                sightView.OnThrow();
            }

            #endregion

            #region Magazine

            if (_item.itemData.weaponData.isMag)
            {
                magView.gameObject.SetActive(true);

                if (_item.mag != AttachItem.None)
                {
                    magView.OnEquip(ItemManager.Instance.GetAttachmentData(_item.mag));
                    //equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, magData);
                }
                else
                    magData = null;
            }
            else
            {
                magView.gameObject.SetActive(false);
                magView.OnThrow();
            }

            #endregion

            #region ButtStock

            if (_item.itemData.weaponData.isButt)
            {
                buttStockView.gameObject.SetActive(true);

                if (_item.buttstock != AttachItem.None)
                {
                    buttStockView.OnEquip(ItemManager.Instance.GetAttachmentData(_item.buttstock));
                    //equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, buttstockData);
                }
                else
                    buttstockData = null;
            }
            else
            {
                buttStockView.gameObject.SetActive(false);
                buttStockView.OnThrow();
            }

            #endregion
        }
        else
        {
            muzzleView.OnThrow();
            gribView.OnThrow();
            sightView.OnThrow();
            magView.OnThrow();
            buttStockView.OnThrow();
        }
    }

    public void SetActiveText(WeaponData _weaponData = null)
    {
        if (_weaponData != null)
        {
            // 탄약 수 추가 필요.
            SetAmmoText(_weaponData);
            weaponNameText.StringReference = weaponData.weaponName;
            nameText.enabled = true;
        }
        else
        {
            SetAmmoText();
            ammoText.enabled = false;
            nameText.enabled = false;
        }
    }

    public void SetAmmoText(WeaponData _weaponData = null)
    {
        if (_weaponData != null)
        {
            // 탄약 수 추가 필요.
            ammoText.text = $" / {InventoryManager.Instance.GetAmmoAmount(_weaponData.bulletType)}";
            ammoText.enabled = true;
        }
        else
        {
            ammoText.text = "";
            ammoText.enabled = false;
        }
    }

    public void Intantiate(MainWeaponView view)
    {
        if (playerTr == null)
            playerTr = GameObject.FindWithTag("DropSpot").transform;

        Quaternion rot = Quaternion.Euler(view.weaponData.dropPrefab.transform.eulerAngles.x, playerTr.eulerAngles.y, view.weaponData.dropPrefab.transform.eulerAngles.z);

        DropItem throwItem = Instantiate(
            view.weaponData.dropPrefab,
            playerTr.position,
            rot).GetComponent<DropItem>();

        throwItem.SetAttachment(view.muzzleData, view.gribData, view.sightData, view.magData, view.buttstockData);

        ItemSoundManager.Instance.Play(throwItem.itemData.clip);
    }

    #region Attachment Type Check

    public AttachmentItem GetAttachmentItem(AttachType type)
    {
        AttachmentItem item = null;
        switch (type)
        {
            case AttachType.Muzzle:
                {
                    item = muzzleView;
                    break;
                }
            case AttachType.Grib:
                {
                    item = gribView;
                    break;
                }
            case AttachType.Sight:
                {
                    item = sightView;
                    break;
                }
            case AttachType.Magazine:
                {
                    item = magView;
                    break;
                }
            case AttachType.Buttstock:
                {
                    item = buttStockView;
                    break;
                }
        }

        return item;
    }

    public bool IsPossible(AttachType type)
    {
        bool isPossible = false;

        if (isPosses)
        {
            switch (type)
            {
                case AttachType.Muzzle:
                    {
                        if (weaponData.isMuzzle)
                            isPossible = true;
                        else
                            isPossible = false;
                        break;
                    }
                case AttachType.Grib:
                    {
                        if (weaponData.isGrib)
                            isPossible = true;
                        else
                            isPossible = false;
                        break;
                    }
                case AttachType.Sight:
                    {
                        if (weaponData.isSight)
                            isPossible = true;
                        else
                            isPossible = false;
                        break;
                    }
                case AttachType.Magazine:
                    {
                        if (weaponData.isMag)
                            isPossible = true;
                        else
                            isPossible = false;
                        break;
                    }
                case AttachType.Buttstock:
                    {
                        if (weaponData.isButt)
                            isPossible = true;
                        else
                            isPossible = false;
                        break;
                    }
            }

            return isPossible;
        }
        else
            return false;
        
    }

    #endregion

    #region RaycastTarget On/Off

    public void OnRaycastTarget(bool isAct, AttachmentData _data = null)
    {
        if(_data != null)
        {
            switch (_data.attachType)
            {
                case AttachType.Muzzle:
                    {
                        muzzleView.OnRaycastTarget(isAct);
                        break;
                    }
                case AttachType.Grib:
                    {
                        gribView.OnRaycastTarget(isAct);
                        break;
                    }
                case AttachType.Sight:
                    {
                        sightView.OnRaycastTarget(isAct);
                        break;
                    }
                case AttachType.Magazine:
                    {
                        magView.OnRaycastTarget(isAct);
                        break;
                    }
                case AttachType.Buttstock:
                    {
                        buttStockView.OnRaycastTarget(isAct);
                        break;
                    }
            }
        }

        muzzleView.RaycastTarget(!isAct);
        gribView.RaycastTarget(!isAct);
        sightView.RaycastTarget(!isAct);
        magView.RaycastTarget(!isAct);
        buttStockView.RaycastTarget(!isAct);
    }

    #endregion

    #region AttachmentData

    public void OnAttachmentData(AttachmentData _data = null)
    {
        if (weaponData == null)
            return;

        if(_data == null)
        {
            muzzleData = muzzleView.attachData;
            equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Muzzle, muzzleData);
            gribData = gribView.attachData;
            equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Grib, gribData);
            sightData = sightView.attachData;
            equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Sight, sightData);
            magData = magView.attachData;
            equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Magazine, magData);
            buttstockData = buttStockView.attachData;
            equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Buttstock, buttstockData);
            return;
        }

        switch (_data.attachType)
        {
            case AttachType.Muzzle:
                {
                    muzzleData = muzzleView.attachData;
                    equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Muzzle, muzzleData);
                    break;
                }
            case AttachType.Grib:
                {
                    gribData = gribView.attachData;
                    equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Grib, gribData);
                    break;
                }
            case AttachType.Sight:
                {
                    sightData = sightView.attachData;
                    equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Sight, sightData);
                    break;
                }
            case AttachType.Magazine:
                {
                    magData = magView.attachData;
                    equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Magazine, magData);
                    break;
                }
            case AttachType.Buttstock:
                {
                    buttstockData = buttStockView.attachData;
                    equipWeaponView.SetAttachmentEquip(weaponData.weaponForm, AttachType.Buttstock, buttstockData);
                    break;
                }
        }
    }

    public void UnAttachmentData(AttachmentData _data)
    {
        if (weaponData == null)
            return;

        if(_data != null)
        {
            switch (_data.attachType)
            {
                case AttachType.Muzzle:
                    {
                        muzzleData = null;
                        equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, AttachType.Muzzle, muzzleData);
                        break;
                    }
                case AttachType.Grib:
                    {
                        gribData = null;
                        equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, AttachType.Grib, gribData);
                        break;
                    }
                case AttachType.Sight:
                    {
                        sightData = null;
                        equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, AttachType.Sight, sightData);
                        break;
                    }
                case AttachType.Magazine:
                    {
                        magData = null;
                        equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, AttachType.Magazine, magData);
                        break;
                    }
                case AttachType.Buttstock:
                    {
                        buttstockData = null;
                        equipWeaponView.SetAttachmentEquip(equipWeaponView.weapon, AttachType.Buttstock, buttstockData);
                        break;
                    }
            }
        }
    }

    #endregion
}
