using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponView : MonoBehaviour
{
    // ·»´õ ÅØ½ºÃÄ
    public WeaponForm weapon = WeaponForm.None;

    public List<GameObject> weaponList = new List<GameObject>();

    [Header("Attachment Parent List")]
    public List<AttachmentParent> attachmentLists = new List<AttachmentParent>();
    [HideInInspector]
    public int currentWeaponIndex = 0;

    // ·»´õÅØ½ºÃÄ Âø¿ë
    public void SetEquip(int index)
    {
        UnEquip();

        weapon = (WeaponForm)index;
        weaponList[index].SetActive(true);
        currentWeaponIndex = index;

        if (currentWeaponIndex != 0)
            weaponList[0].SetActive(false);

    }

    // ·»´õÅØ½ºÃÄ Âø¿ë ÇØÁ¦
    public void UnEquip()
    {
        weaponList[0].SetActive(true);

        weaponList[currentWeaponIndex].SetActive(false);
        weapon = 0;
        currentWeaponIndex = 0;
    }

    public void SetAttachmentEquip(WeaponForm _form, AttachType _attachType, AttachmentData _data)
    {
        foreach (var parent in attachmentLists) 
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
