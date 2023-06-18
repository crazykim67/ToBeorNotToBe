using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentList : MonoBehaviour
{
    public AttachType attachType = AttachType.None;
    
    public AttachItem attachItem = AttachItem.None;

    public List<WeaponAttachmentItem> weaponAttachment = new List<WeaponAttachmentItem>();

    public void OnEquip(AttachItem _item)
    {
        UnEquip();

        foreach (var attachmentItem in weaponAttachment)
        {
            if (_item == attachmentItem.attachItem)
            {

                if(weaponAttachment.Count > 0)
                    weaponAttachment[0].gameObject.SetActive(false);
                
                attachmentItem.gameObject.SetActive(true);
                attachItem = _item;
                break;
            }
        }

    }

    // 착용 해제
    public void UnEquip()
    {
        if (weaponAttachment.Count <= 0)
            return;

        foreach (var attachmentItem in weaponAttachment)
        {
            if (attachmentItem.attachItem == attachItem && attachmentItem.attachItem != AttachItem.None)
            {
                attachmentItem.gameObject.SetActive(false);
                break;
            }
        }

        if (attachItem != AttachItem.None)
            SetDefault();
    }

    // 착용 해제하고나서 기본값으로 설정
    public void SetDefault()
    {
        if (weaponAttachment.Count <= 0)
            return;

        weaponAttachment[0].gameObject.SetActive(true);
        attachItem = AttachItem.None;
    }
}
