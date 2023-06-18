using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class DropItem : MonoBehaviour
{
    public ItemData itemData;

    public int itemCount = 1;

    [SerializeField]
    private LocalizedString localizeString;

    [Header("AttachmentEnum")]
    public AttachItem muzzle = AttachItem.None;
    public AttachItem grib = AttachItem.None;
    public AttachItem sight = AttachItem.None;
    public AttachItem mag = AttachItem.None;
    public AttachItem buttstock = AttachItem.None;

    public void OnHit(TextMeshProUGUI _text)
    {
        if(itemCount <= 1)
        _text.text = $"{itemData.itemName.GetLocalizedString()} \n[E] {localizeString.GetLocalizedString()}";
        else
        _text.text = $"{itemData.itemName.GetLocalizedString()} ({itemCount}) \n[E] {localizeString.GetLocalizedString()}";
    }

    public void SetItemCount(int count)
    {
        itemCount = count;
    }

    public void SetAttachment(AttachmentData _muzzle, AttachmentData _grib, AttachmentData _sight, AttachmentData _mag, AttachmentData _buttstock)
    {
        if (_muzzle != null)
            muzzle = _muzzle.attachItem;

        if (_grib != null)
            grib = _grib.attachItem;

        if (_sight != null)
            sight = _sight.attachItem;

        if (_mag != null)
            mag = _mag.attachItem;

        if (_buttstock != null)
            buttstock = _buttstock.attachItem;
    }
}
