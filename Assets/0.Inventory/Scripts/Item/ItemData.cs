using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization;

public enum ItemType
{
    None,
    Bag,
    Ammo,
    Weapon,
    Cure,
    Kit,
    Material,
}

public enum BulletType
{
    None,
    five,
    seven,
    nine,
    gauge
}

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object Asset/Item", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Common")]
    public ItemType itemType = ItemType.None;
    public LocalizedString itemTypeStr;
    public LocalizedString itemName;
    public int weight;
    public LocalizedString itemExplain;
    public GameObject itemPrefab;

    public int itemCode;

    public AudioClip clip;

    public Texture img;

    [Header("Bag")]
    public BagLevel bagLevel = BagLevel.None;
    public int volume;

    [Header("Ammo")]
    public BulletType bulletType = BulletType.None;

    [Header("Weapon")]
    public WeaponData weaponData;

    [Header("Attachment")]
    public AttachmentData attachmentData;

    [Header("Cure")]
    public float delay;
    public float amount;
    public float duration;
}
