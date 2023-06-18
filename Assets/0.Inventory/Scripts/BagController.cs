using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BagLevel
{
    None,
    Low,
    Midium,
    High,
}

public class BagController : MonoBehaviour
{
    private const int bagVolume = 100;

    [Header("Volume Value")]
    public int playerVolume = 0;
    public int usePlayerVolume = 0;

    //[Header("Current Bag Object")]
    //public GameObject bagObject;

    private void Awake() => playerVolume = bagVolume;

    private void Start()
    {
        InventoryManager.Instance.SetSpareVolume(playerVolume);
    }

    #region Bag Volume

    public int OnEquipBag(DropItem _item)
    {
        EquipManager.Instance.bagLevel = _item.itemData.bagLevel;
        //bagObject = _item.itemData.itemPrefab;
        return playerVolume = bagVolume + _item.itemData.volume;
    }

    public void OnAddVolume(DropItem _item, int itemCount)
    {
        usePlayerVolume += (_item.itemData.weight * itemCount);
        InventoryManager.Instance.SetUseVolume(usePlayerVolume);
    }

    public void OnSubVolume(ItemSlot _item, int itemCount)
    {
        if (usePlayerVolume <= 0 || usePlayerVolume - (_item.itemData.weight * itemCount) < 0)
            return;

        usePlayerVolume -= (_item.itemData.weight * itemCount);
        InventoryManager.Instance.SetUseVolume(usePlayerVolume);
    }

    #endregion
}
