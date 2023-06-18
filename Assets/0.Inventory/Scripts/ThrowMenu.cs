using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class ThrowMenu : MonoBehaviour
{
    public ItemSlot itemSlot;

    [Header("GameObject")]
    public GameObject throwMenu;

    [Header("UI")]
    public RawImage itemImage;
    public LocalizeStringEvent itemName;

    public TMP_InputField countInputField;
    public Button prevBtn;
    public Button nextBtn;

    public Button acceptBtn;

    public Button closeBtn;

    [HideInInspector]
    public bool isAct = false;

    [Header("Amount")]
    public int currentCount;
    public int maxCount;

    private void Update()
    {
        prevBtn.interactable = currentCount <= 1 ? false : true;
        nextBtn.interactable = currentCount == maxCount ? false : true;

        if (itemSlot == null)
            return;

        countInputField.onEndEdit.AddListener((str) => 
        {
            int count = int.Parse(str);

            if(count > maxCount)
            {
                countInputField.text = maxCount.ToString();
                count = maxCount;
            }
            else if(count < 1)
            {
                countInputField.text = 1.ToString();
                count = 1;
            }

            currentCount = count;
        });
    }

    public void OnShow(ItemSlot _item)
    {
        if (_item == null)
        {
            Debug.Log("ItemSlot Data is Null...!!");
            return;
        }

        itemSlot = _item;

        itemImage.texture = itemSlot.itemImage.texture;
        itemName.StringReference = itemSlot.itemData.itemName;

        maxCount = itemSlot.itemCount;
        currentCount = (itemSlot.itemCount / 2) > 0 ? itemSlot.itemCount / 2 : 1;

        countInputField.text = currentCount.ToString();

        throwMenu.SetActive(true);
        isAct = true;
    }

    public void OnClose()
    {
        itemSlot = null;

        itemImage.texture = null;

        maxCount = 0;
        currentCount = 0;

        throwMenu.SetActive(false);
        isAct = false;
    }

    public void OnAccept()
    {
        if (itemSlot == null)
            return;

        EquipManager.Instance.bagController.OnSubVolume(itemSlot, currentCount);
        itemSlot.Subtraction(false, currentCount);

        if (itemSlot.type == ItemType.Ammo)
        {
            if (InventoryManager.Instance.weaponInventory.firstWeapon.weaponData != null)
                InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.firstWeapon.weaponData);
            else
                InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText();

            if (InventoryManager.Instance.weaponInventory.secondWeapon.weaponData != null)
                InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.secondWeapon.weaponData);
            else
                InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText();
        }
        OnClose();
    }

    // 0 add, -1 sub
    public void OnClick(int num)
    {
        if(num == 0 && currentCount < maxCount)
            currentCount++;
        else if(num == -1 && currentCount > 1)
            currentCount--;

        countInputField.text = currentCount.ToString();
    }
}
