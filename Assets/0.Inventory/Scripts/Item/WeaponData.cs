using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public enum WeaponForm
{
    None,
    HK45,
    M16A1,
    M4Carbine,
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Object Asset/Weapon", order = 2)]
public class WeaponData : ScriptableObject
{
    public enum WeaponType
    {
        Melee,
        Pistol,
        SMG,
        AR,
        DMR,
        LMG,
        SR,
        TW,
    }

    public GameObject dropPrefab;

    public enum WeaponStyle
    {
        Primary,
        Auto,
    }

    public AudioClip clip;

    public LocalizedString weaponName;

    public LocalizedString description;

    public WeaponForm weaponForm = WeaponForm.None;
    public BulletType bulletType = BulletType.nine;

    public int damage;

    public int magazineSize;
    public int extensionMagSize;
    public float fireRate;
    public int range;
    public float upDelay;

    public WeaponType weaponType;
    public WeaponStyle weaponStyle;

    [Header("Attachment/Muzzle")]
    public AttachmentData muzzle;
    public bool isMuzzle;
    [Header("Attachment/Grib")]
    public AttachmentData grib;
    public bool isGrib;
    [Header("Attachment/Sight")]
    public AttachmentData sight;
    public bool isSight;
    [Header("Attachment/Magazine")]
    public AttachmentData mag;
    public bool isMag;
    [Header("Attachment/ButtStock")]
    public AttachmentData buttstock;
    public bool isButt;

    [Header("Throw Weapon Check")]
    public bool isThrowing;

    #region AttachmentCheck

    public bool IsMuzzle()
    {
        if (isMuzzle)
            return true;
        else
            return false;
    }
    public bool IsGrib()
    {
        if (isGrib)
            return true;
        else
            return false;
    }
    public bool IsSight()
    {
        if (isSight)
            return true;
        else
            return false;
    }
    public bool IsMagazine()
    {
        if (isMag)
            return true;
        else
            return false;
    }
    public bool IsButtStock()
    {
        if (isButt)
            return true;
        else
            return false;
    }

    public bool IsAttchmentCheck(AttachType _type)
    {
        bool isCheck = false;
        switch (_type)
        {
            case AttachType.Muzzle:
                {
                    isCheck = IsMuzzle();
                    break;
                }
            case AttachType.Grib:
                {
                    isCheck = IsGrib();
                    break;
                }
            case AttachType.Sight:
                {
                    isCheck = IsSight();
                    break;
                }
            case AttachType.Magazine:
                {
                    isCheck = IsMagazine();
                    break;
                }
            case AttachType.Buttstock:
                {
                    isCheck = IsButtStock();
                    break;
                }
        }

        return isCheck;
    }

    #endregion
}
