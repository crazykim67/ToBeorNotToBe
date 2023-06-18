using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentParent : MonoBehaviour
{
    public WeaponForm weaponForm = WeaponForm.None;

    public AttachmentList muzzleList;
    public AttachmentList gribList;
    public AttachmentList sightList;
    public AttachmentList magList;
    public AttachmentList buttstockList;

    public bool IsCheck(WeaponForm _form)
    {
        if (weaponForm == _form)
            return true;
        else
            return false;
    }

    public void OnEquip(AttachType type, AttachmentData item = null)
    {
        switch(type) 
        {
            case AttachType.Muzzle:
                {
                    if(muzzleList != null)
                    {
                        if (item != null)
                            muzzleList.OnEquip(item.attachItem);
                        else
                            muzzleList.UnEquip();
                    }
                    break;
                }
            case AttachType.Grib:
                {
                    if(gribList != null)
                    {
                        if(item != null)
                            gribList.OnEquip(item.attachItem);
                        else
                            gribList.UnEquip();
                    }
                    break;
                }
            case AttachType.Sight:
                {
                    if(sightList != null)
                    {
                        if(item != null)
                            sightList.OnEquip(item.attachItem);
                        else
                            sightList.UnEquip();
                    }
                    break;
                }
            case AttachType.Magazine:
                {
                    if(magList != null)
                    {
                        if(item != null)
                            magList.OnEquip(item.attachItem);
                        else
                            magList.UnEquip();

                    }
                    break;
                }
            case AttachType.Buttstock:
                {
                    if(buttstockList != null)
                    {
                        if(item != null)
                            buttstockList.OnEquip(item.attachItem);
                        else
                            buttstockList.UnEquip();
                    }
                    break;
                }
        }
    }
}
