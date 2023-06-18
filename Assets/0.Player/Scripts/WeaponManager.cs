using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // 이 스크립트는 실질적으로 손에 들고있는 무기만 관리하며
    // 실제로 키고 끄는 것은 ArmController에서 함

    private static WeaponManager instance;

    public static WeaponManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WeaponManager();
                return instance;
            }

            return instance;
        }
    }

    public List<WeaponHandler> hendlerList = new List<WeaponHandler>();

    public WeaponHandler currentWeapon;
    public WeaponHandler prevWeapon;

    public WeaponView currentSelectView;

    private void Awake()
    {
        instance = this;
    }

    public void EquipWeapon(WeaponForm weapon)
    {
        UnEquip();

        foreach (WeaponHandler handler in hendlerList)
        { 
            if(handler.weapon == weapon) 
            {
                currentWeapon = handler;
                break;
            }
        }
        //currentWeapon.OnWeapon();
    }

    public void UnEquip()
    {
        ArmController.Instance.UnEquipWeapon();
        prevWeapon = currentWeapon;
        currentWeapon = null;
    }

    public void AttachEquip(WeaponView weaponView, MainWeaponView mainWeaponView)
    {
        if (weaponView == null)
            return;

        if (mainWeaponView == null)
            return;

        GetAttachmentEquip(weaponView.weapon,
            AttachType.Muzzle,
            mainWeaponView.muzzleData);

        GetAttachmentEquip(weaponView.weapon,
            AttachType.Grib,
            mainWeaponView.gribData);

        GetAttachmentEquip(weaponView.weapon,
            AttachType.Sight,
            mainWeaponView.sightData);

        GetAttachmentEquip(weaponView.weapon,
            AttachType.Magazine,
            mainWeaponView.magData);

        GetAttachmentEquip(weaponView.weapon,
            AttachType.Buttstock,
            mainWeaponView.buttstockData);
    }

    public void GetAttachmentEquip(WeaponForm _form, AttachType _attachType, AttachmentData _data)
    {
        foreach (var parent in hendlerList)
        {
            if (parent.IsCheck(_form))
            {
                if (_data != null)
                {
                    parent.OnEquip(_attachType, _data);
                    break;
                }
                else
                {
                    parent.OnEquip(_attachType);
                    break;
                }
            }
        }
    }
}
