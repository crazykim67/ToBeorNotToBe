using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class InteractionController : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;

    [SerializeField]
    private float distance = 3f;

    private int layer;

    [Header("UI")]
    public TextMeshProUGUI itemNameText;

    //[SerializeField]
    private DropItem currentHitItem;

    [SerializeField]
    private float coolTime = 0.8f;
    private float timer = 0f;
    private bool isCool = false;

    [SerializeField]
    private LocalizedString sameString; 
    [SerializeField]
    private LocalizedString highString; 
    [SerializeField]
    private LocalizedString enoughString;

    private void Start() => layer = 1 << LayerMask.NameToLayer("Item");

    private void Update()
    {
        if (InventoryManager.Instance.isUi)
        {
            if (currentHitItem != null)
                currentHitItem = null;

            if (itemNameText.enabled)
                itemNameText.enabled = false;

            return;
        }

        // Pick Up Delay
        OnCoolTime();

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(currentHitItem != null)
            OnPickUp(currentHitItem);

        if (Physics.Raycast(ray, out hit, distance, layer))
        {
            if(hit.transform != null)
            {
                if (currentHitItem != hit.transform.GetComponent<DropItem>())
                {
                    if (!itemNameText.enabled)
                        itemNameText.enabled = true;

                    currentHitItem = hit.transform.GetComponent<DropItem>();
                    currentHitItem.OnHit(itemNameText);
                }
            }

            return;
        }

        if(currentHitItem != null)
            currentHitItem = null;

        if(itemNameText.enabled)
            itemNameText.enabled = false;
    }

    public void OnPickUp(DropItem _item)
    {
        if (isCool)
            return;

        if(Input.GetKeyDown(KeyCode.E)) 
        {
            int sumVolume = EquipManager.Instance.bagController.usePlayerVolume + (_item.itemData.weight * _item.itemCount);
            // Weight Compare
            if (EquipManager.Instance.bagController.playerVolume < sumVolume)
            {
                ToastController.Instance.OnToast(enoughString.GetLocalizedString());
                return;
            }

            switch (_item.itemData.itemType)
            {
                case ItemType.Bag:
                    {
                        if ((int)EquipManager.Instance.bagLevel > (int)_item.itemData.bagLevel)
                        {
                            ToastController.Instance.OnToast(highString.GetLocalizedString());
                            return;
                        }
                        else if((int)EquipManager.Instance.bagLevel == (int)_item.itemData.bagLevel)
                        {
                            ToastController.Instance.OnToast(sameString.GetLocalizedString());
                            return;
                        }
                        InventoryManager.Instance.PickUpBag(_item);
                        break;
                    }
                    // ���������� ���������� weaponData ������ �Ǵ� (������ ������, ������ ����)
                case ItemType.Weapon:
                    {
                        if(_item.itemData.weaponData != null)
                        switch (_item.itemData.weaponData.weaponType)
                        {
                            case WeaponData.WeaponType.Melee:
                                {

                                    break;
                                }
                            case WeaponData.WeaponType.TW:
                                { 

                                    break;
                                }
                            default:
                                {
                                    // �ֹ��⸦ ��� ���� �ÿ� ���� ������� ���Կ� ���⸦ ������ ���� �Ⱦ���. o
                                    if (InventoryManager.Instance.weaponInventory.firstWeapon.isPosses && InventoryManager.Instance.weaponInventory.secondWeapon.isPosses)
                                        switch (EquipManager.Instance.equipSlot)
                                        {
                                            case EquipSlot.FIRST:
                                                {
                                                    InventoryManager.Instance.weaponInventory.OnPickUp(_item, (int)EquipManager.Instance.equipSlot);
                                                    EquipManager.Instance.OnEquip((int)EquipManager.Instance.equipSlot);
                                                    break;
                                                    }
                                            default:
                                                {
                                                    InventoryManager.Instance.weaponInventory.OnPickUp(_item, 2);
                                                    EquipManager.Instance.OnEquip(2);
                                                    break;
                                                    }
                                        }
                                    // �ƹ��͵� �����ϰ� ���� ���� �ÿ� ���� ������� ���Կ� ���⸦ �Ⱦ���(3, 4���� �� ������ 1���� �Ⱦ�) o
                                    else if (!InventoryManager.Instance.weaponInventory.firstWeapon.isPosses && !InventoryManager.Instance.weaponInventory.secondWeapon.isPosses)
                                    {
                                        int index = EquipManager.Instance.EquipIndex();
                                        InventoryManager.Instance.weaponInventory.OnPickUp(_item, index);
                                        EquipManager.Instance.OnEquip(index);
                                    }
                                    // �� �� �ϳ��� �����ϰ� ���� �ÿ� ����ִ� ���Կ� ���⸦ �Ⱦ��� o
                                    else
                                    {
                                        int index = EquipManager.Instance.EquipIndex();
                                        InventoryManager.Instance.weaponInventory.OnPickUp(_item, EquipManager.Instance.EquipIndex());

                                        if((int)EquipManager.Instance.equipSlot == index)
                                            EquipManager.Instance.OnEquip((int)EquipManager.Instance.equipSlot);
                                    }
                                    break;
                                    }
                        }
                        else
                        {
                            InventoryManager.Instance.Instantiate(_item.itemData, _item.itemCount);
                            EquipManager.Instance.bagController.OnAddVolume(_item, _item.itemCount);
                        }
                        break;
                    }
                case ItemType.Ammo:
                    {
                        InventoryManager.Instance.Instantiate(_item.itemData, _item.itemCount);
                        EquipManager.Instance.bagController.OnAddVolume(_item, _item.itemCount);

                        if (InventoryManager.Instance.weaponInventory.firstWeapon.weaponData != null)
                            InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.firstWeapon.weaponData);
                        else
                            InventoryManager.Instance.weaponInventory.firstWeapon.SetAmmoText();

                        if (InventoryManager.Instance.weaponInventory.secondWeapon.weaponData != null)
                            InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText(InventoryManager.Instance.weaponInventory.secondWeapon.weaponData);
                        else
                            InventoryManager.Instance.weaponInventory.secondWeapon.SetAmmoText();
                        break;
                    }
                case ItemType.Cure:
                case ItemType.Kit:
                case ItemType.Material:
                    {
                        InventoryManager.Instance.Instantiate(_item.itemData, _item.itemCount);
                        EquipManager.Instance.bagController.OnAddVolume(_item, _item.itemCount);
                        break;
                    }
            }

            isCool = true;

            ItemSoundManager.Instance.Play(_item.itemData.clip);
            Destroy(_item.gameObject);
        }
    }

    public void OnCoolTime()
    {
        if (isCool)
        {
            if(timer <= coolTime)
            {
                timer += Time.deltaTime;
            }
            else
            {
                isCool = false;
                timer = 0f;
            }
        }
    }
}
