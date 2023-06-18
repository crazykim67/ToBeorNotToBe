using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static EquipManager;

public enum EquipSlot
{
    None,
    FIRST,
    SECOND,
    THIRD,
    FOURTH,
}
public class EquipManager : MonoBehaviour
{
    public const float noneUpDelay = 1.2f;

    private static EquipManager instance;

    public static EquipManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EquipManager();
                return instance;
            }

            return instance;
        }
    }

    public BagController bagController;
    public WeaponController weaponController;

    [Header("Bag Level")]
    public BagLevel bagLevel = BagLevel.None;

    [Header("Current Use Slot")]
    public EquipSlot equipSlot = EquipSlot.None;

    //[HideInInspector]
    public bool isPossible;
    public float timer;
    public float delay;

    private void Awake()
    {
        instance = this;
        // 임시 초기 설정
    }

    private void Start()
    {
        OnEquip(1);
    }
    public void Update()
    {
        if (!isPossible)
        {
            if(timer <= delay)
            timer += Time.deltaTime;
            else
            {
                timer = 0;
                isPossible = true;
            }
        }

        if(OptionManager.Instance != null)
            if (OptionManager.Instance.isMenu)
                return;

        if (!isPossible)
            return;

        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4))
            InputAlpha();
    }

    public void InputAlpha()
    {

        switch (Input.inputString)
        {
            case "1":
                {
                    SlotCheck(1);
                    break;
                }
            case "2":
                {
                    SlotCheck(2);
                    break;
                }
            case "3":
                {
                    SlotCheck(3);
                    break;
                }
            case "4":
                {
                    SlotCheck(4);
                    break;
                }
        }
    }

    public void SlotCheck(int alpha)
    {
        if (InventoryManager.Instance.isUse)
            ItemManager.Instance.UseCancel();

        if ((int)equipSlot == alpha)
            return;

        switch (equipSlot)
        {
            case EquipSlot.FIRST:
                {
                    OnEquip(alpha);
                    break;
                }
            case EquipSlot.SECOND:
                {
                    OnEquip(alpha);
                    break;
                }
            case EquipSlot.THIRD:
                {
                    OnEquip(alpha);
                    break;
                }
            case EquipSlot.FOURTH:
                {
                    OnEquip(alpha);
                    break;
                }
        }
    }

    public void OnEquip(int index)
    {
        switch (index)
        {
            case 1:
                {
                    equipSlot = EquipSlot.FIRST;
                    //ArmController.Instance.SetSlot(equipSlot);
                    InventoryManager.Instance.weaponInventory.OnSelected(index - 1);
                    ArmController.Instance.SetSlot(equipSlot);
                    delay = GetDelay(InventoryManager.Instance.weaponInventory.firstWeapon);
                    isPossible = false;
                    break;
                }
            case 2:
                {
                    equipSlot = EquipSlot.SECOND;
                    //ArmController.Instance.SetSlot(equipSlot);
                    InventoryManager.Instance.weaponInventory.OnSelected(index - 1);
                    ArmController.Instance.SetSlot(equipSlot);
                    delay = GetDelay(InventoryManager.Instance.weaponInventory.secondWeapon);
                    isPossible = false;
                    break;
                }
            case 3:
                {
                    equipSlot = EquipSlot.THIRD;
                    //ArmController.Instance.SetSlot(equipSlot);
                    InventoryManager.Instance.weaponInventory.OnSelected(index - 1);
                    break;
                }
            case 4:
                {
                    equipSlot = EquipSlot.FOURTH;
                    //ArmController.Instance.SetSlot(equipSlot);
                    InventoryManager.Instance.weaponInventory.OnSelected(index - 1);
                    break;
                }
        }
    }

    public void OnSwapEquip(int index)
    {
        switch (equipSlot)
        {
            case EquipSlot.FIRST: 
                {
                    OnEquip(2);
                    break;
                }
            case EquipSlot.SECOND:
                {
                    OnEquip(1);
                    break;
                }
            default:
                return;
        }
    }

    public int EquipIndex()
    {
        int index = 0;

        if(weaponController.firstWeapon.currentWeaponIndex == 0 && weaponController.secondWeapon.currentWeaponIndex == 0)
        {
            switch (equipSlot)
            {
                case EquipSlot.SECOND:
                    {
                        index = 2;
                        break;
                    }
                default:
                    {
                        index = 1;
                        break;
                    }
            }
        }
        else
        {
            if(weaponController.firstWeapon.currentWeaponIndex == 0)
                index = 1;
            else if (weaponController.secondWeapon.currentWeaponIndex == 0)
                index = 2;
        }

        return index;
    }

    public float GetDelay(MainWeaponView view)
    {
        timer = 0;

        if (view.weaponData == null)
            return noneUpDelay;

        switch (view.weaponData.weaponForm) 
        {
            case WeaponForm.None:
                return noneUpDelay;
            default:
                return view.weaponData.upDelay;
        }
    }
}
