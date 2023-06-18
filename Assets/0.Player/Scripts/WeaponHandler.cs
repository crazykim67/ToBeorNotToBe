using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponHandler : MonoBehaviour
{
    public WeaponForm weapon;

    public GameObject weaponObject;

    public WeaponData weaponData;

    [Header("Attachment List Parent")]
    public AttachmentList muzzleParent;
    public AttachmentList gribParent;
    public AttachmentList sightParent;
    public AttachmentList magParent;
    public AttachmentList buttParent;

    public void OnWeapon()
    {
        if(ArmController.Instance.currentWeaponView != null && ArmController.Instance.currentMainWeaponView != null)
            WeaponManager.Instance.AttachEquip(ArmController.Instance.currentWeaponView, ArmController.Instance.currentMainWeaponView);

        weaponObject.SetActive(true);
    }

    public void HideWeapon()
    {
        weaponObject.SetActive(false);
    }

    public bool IsCheck(WeaponForm _form)
    {
        if (weapon == _form)
            return true;
        else
            return false;
    }

    public void OnEquip(AttachType type, AttachmentData item = null)
    {
        switch (type)
        {
            case AttachType.Muzzle:
                {
                    if (muzzleParent != null)
                    {
                        if (item != null)
                            muzzleParent.OnEquip(item.attachItem);
                        else
                            muzzleParent.UnEquip();
                    }
                    break;
                }
            case AttachType.Grib:
                {
                    if (gribParent != null)
                    {
                        if (item != null)
                            gribParent.OnEquip(item.attachItem);
                        else
                            gribParent.UnEquip();
                    }
                    break;
                }
            case AttachType.Sight:
                {
                    if (sightParent != null)
                    {
                        if (item != null)
                            sightParent.OnEquip(item.attachItem);
                        else
                            sightParent.UnEquip();
                    }
                    break;
                }
            case AttachType.Magazine:
                {
                    if (magParent != null)
                    {
                        if (item != null)
                            magParent.OnEquip(item.attachItem);
                        else
                            magParent.UnEquip();

                    }
                    break;
                }
            case AttachType.Buttstock:
                {
                    if (buttParent != null)
                    {
                        if (item != null)
                            buttParent.OnEquip(item.attachItem);
                        else
                            buttParent.UnEquip();
                    }
                    break;
                }
        }
    }
}
