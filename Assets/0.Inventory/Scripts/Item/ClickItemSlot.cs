using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class ClickItemSlot : MonoBehaviour
{
    [SerializeField]
    private GraphicRaycaster gr;

    [HideInInspector]
    public ItemSlot itemSlot;

    public RawImage itemImage;
    public LocalizeStringEvent itemName;
    public LocalizeStringEvent itemType;
    public TextMeshProUGUI itemCountText;

    [HideInInspector]
    public bool isAct = false;

    [SerializeField]
    private LocalizedString noneString;
    [SerializeField]
    private LocalizedString noItemString;

    public void SetItemData(ItemSlot _item)
    {
        if (_item == null)
            return;

        itemSlot = _item;

        AttachmentData attachmentData = itemSlot.itemData.attachmentData;

        if (attachmentData != null)
        {
            if(InventoryManager.Instance.weaponInventory.firstWeapon.weaponData != null)
            {
                WeaponData data = InventoryManager.Instance.weaponInventory.firstWeapon.weaponData;

                if (attachmentData.IsPossible(data.weaponForm))
                    InventoryManager.Instance.weaponInventory.firstWeapon.  OnRaycastTarget(true, attachmentData);
            }
            if(InventoryManager.Instance.weaponInventory.secondWeapon.weaponData != null)
            {
                WeaponData data = InventoryManager.Instance.weaponInventory.secondWeapon.weaponData;

                if (attachmentData.IsPossible(data.weaponForm))
                    InventoryManager.Instance.weaponInventory.secondWeapon.OnRaycastTarget(true, attachmentData);
            }
        }

        itemImage.texture = itemSlot.itemImage.texture;
        itemName.StringReference = itemSlot.itemName.StringReference;
        itemType.StringReference = itemSlot.itemType.StringReference;
        itemCountText.text = itemSlot.itemCountText.text;

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        rect.position = Input.mousePosition;

        this.gameObject.SetActive(true);

        isAct = true;
    }

    public void OffClickItem()
    {
        if (itemSlot == null)
            return;

        AttachmentData attachmentData = itemSlot.itemData.attachmentData;
        if (attachmentData != null)
        {
            InventoryManager.Instance.weaponInventory.firstWeapon.OnRaycastTarget(false, attachmentData);
            InventoryManager.Instance.weaponInventory.secondWeapon.OnRaycastTarget(false, attachmentData);

            //MainWeaponView weaponView = InventoryManager.Instance.weaponInventory.IsAttachmentEquip(itemSlot.itemData.attachmentData);

            //if (weaponView != null)
            //    weaponView.OnRaycastTarget(false, itemSlot.itemData.attachmentData);
        }

        itemSlot = null;

        itemImage.texture = null;
        //itemName.StringReference = null;
        //itemType.StringReference = null;
        itemCountText.text = null;

        isAct = false;

        this.gameObject.SetActive(false);
    }

    public void Raycast()
    {
        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
            return;

        if(OptionManager.Instance != null)
            if (OptionManager.Instance.isMenu)
                return;

        if (results[0].gameObject.tag == "Outside")
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (InventoryManager.Instance.isUse)
                {
                    // Toast
                    return;
                }
                InventoryManager.Instance.throwMenu.OnShow(itemSlot);
            }
            else
            {
                if(InventoryManager.Instance.isUse)
                ItemManager.Instance.UseCancel();

                EquipManager.Instance.bagController.OnSubVolume(itemSlot, itemSlot.itemCount);
                itemSlot.Subtraction(false, itemSlot.itemCount);

                // 총알 일때
                if(itemSlot.type == ItemType.Ammo)
                {
                    if(InventoryManager.Instance.weaponInventory.firstWeapon.weaponData != null)
                        InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.firstWeapon.weaponData);
                    else
                        InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText();

                    if(InventoryManager.Instance.weaponInventory.secondWeapon.weaponData != null)
                        InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.secondWeapon.weaponData);
                    else
                        InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText();
                }
            }
        }
        else if (results[0].gameObject.tag == "WeaponSlot")
        {
            if(itemSlot.itemData.attachmentData != null)
            {
                MainWeaponView weaponView = results[0].gameObject.GetComponent<MainWeaponView>();

                // 무기 중에 착용 가능한 무기가 있는지 체크
                if (InventoryManager.Instance.weaponInventory.IsAttachmentEquip(itemSlot.itemData.attachmentData))
                {
                    if (!weaponView.IsPossible(itemSlot.itemData.attachmentData.attachType))
                        weaponView = InventoryManager.Instance.weaponInventory.IsAttachmentEquip(itemSlot.itemData.attachmentData);

                    if (!itemSlot.itemData.attachmentData.IsPossible(weaponView.weaponData.weaponForm))
                    {
                        ToastController.Instance.OnToast(noItemString.GetLocalizedString());
                        return;
                    }

                    AttachmentItem attachmentItem = weaponView.GetAttachmentItem(itemSlot.itemData.attachmentData.attachType);
                    attachmentItem.OnEquip(itemSlot.itemData.attachmentData);
                    ItemManager.Instance.Use(itemSlot);
                }
                else
                {
                    ToastController.Instance.OnToast(noneString.GetLocalizedString());
                    return;
                }
            }
            
        }
    }
}
