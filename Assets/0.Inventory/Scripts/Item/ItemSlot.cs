using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

[System.Serializable]
public class ItemSlot : MonoBehaviour
{
    public ItemData itemData;

    public ItemType type = ItemType.None;

    public RawImage itemImage;
    public LocalizeStringEvent itemName;
    public LocalizeStringEvent itemType;
    public TextMeshProUGUI itemCountText;

    [HideInInspector]
    public LocalizedString itemExplain;
    [HideInInspector]
    public string itemWeight;
    [HideInInspector]
    public int itemCount;

    private Transform playerTr;

    public void SetItemData(ItemData _data, int _itemCount)
    {
        itemData = _data;

        type = _data.itemType;

        itemImage.texture= _data.img;
        itemName.StringReference = _data.itemName;
        itemType.StringReference = _data.itemTypeStr;

        itemExplain = _data.itemExplain;
        itemWeight = _data.weight.ToString();

        itemCount = _itemCount;
        itemCountText.text = itemCount.ToString();
    }

    public void AddItem(int _itemCount)
    {
        itemCount += _itemCount;
        itemCountText.text = itemCount.ToString();
    }

    public void Subtraction(bool isUse, int _itemCount)
    {
        if (!isUse)
        {
            if (playerTr == null)
                playerTr = GameObject.FindWithTag("DropSpot").transform;

            //Quaternion rot = Quaternion.Euler(0, playerTr.eulerAngles.y, 0);
            Quaternion rot = Quaternion.Euler(itemData.itemPrefab.transform.eulerAngles.x, playerTr.eulerAngles.y, itemData.itemPrefab.transform.eulerAngles.z);

            // Drop Item
            DropItem throwItem = Instantiate(
                itemData.itemPrefab,
                playerTr.transform.position,
                rot).GetComponent<DropItem>();

            // Drop Item Set Count
            throwItem.SetItemCount(_itemCount);

            // Sound Play
            ItemSoundManager.Instance.Play(throwItem.itemData.clip);
        }

        itemCount -= _itemCount;
        itemCountText.text = itemCount.ToString();

        if (itemCount == 0)
        {
            foreach (var item in InventoryManager.Instance.slots)
            {
                if (item == this)
                {
                    InventoryManager.Instance.slots.Remove(item);
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
        
    }
}
