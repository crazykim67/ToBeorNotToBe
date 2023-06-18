using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public enum AttachType
{
    None,
    Muzzle,
    Grib,
    Sight,
    Magazine,
    Buttstock,
}

public enum AttachItem
{
    None,
    Hologram,
    Dot,
    Vertical_Grib,
    Angle_Grib,
    Extension_Mag_Pistol,
    Extension_Mag_AR,
}

[System.Serializable]
[CreateAssetMenu(fileName = "Attachment", menuName = "Scriptable Object Asset/Attachment", order = 3)]
public class AttachmentData : ScriptableObject
{
    public AttachType attachType = AttachType.None;

    public AttachItem attachItem = AttachItem.None;

    public LocalizedString itemTypeStr;
    public LocalizedString itemName;
    public int weight;
    public LocalizedString itemExplain;
    public GameObject itemPrefab;

    public List<WeaponForm> possibleWeapons = new List<WeaponForm>();

    public int itemCode;

    public List<AudioClip> clips = new List<AudioClip>();

    public Texture img;

    public bool IsPossible(WeaponForm item)
    {
        bool isPossible = false;
        foreach(var weapon in possibleWeapons) 
        {
            if (weapon == item)
            {
                isPossible = true;
                break;
            }
        }

        return isPossible;
    }
}
