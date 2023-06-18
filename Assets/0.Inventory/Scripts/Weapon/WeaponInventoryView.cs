using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;
using UnityEngine.Localization;
using Unity.VisualScripting;

public class WeaponInventoryView : MonoBehaviour
{
    // 주무기 인벤토리 관련 UI
    public MainWeaponView firstWeapon;
    public MainWeaponView secondWeapon;

    [Header("Selected")]
    public List<RawImage> selected = new List<RawImage>();

    public void OnPickUp(DropItem _item, int index)
    {
        // 주무기를 모두 소유 시에 현재 사용중인 슬롯에 무기를 버리고 새로 픽업함. x
        // 아무것도 보유하고 있지 않을 시에 현재 사용중인 슬롯에 무기를 픽업함(3, 4번일 시 강제로 1번에 픽업) x
        // 둘 중 하나만 보유하고 있을 시에 비어있는 슬롯에 무기를 픽업함 x
        switch (index) 
        {
            case 1:
                {
                    EquipManager.Instance.weaponController.firstWeapon.SetEquip((int)_item.itemData.weaponData.weaponForm);
                    if(firstWeapon.weaponData == null)
                    {
                        firstWeapon.PickUpWeapon(_item);
                    }
                    else
                    {
                        firstWeapon.DropWeapon();
                        firstWeapon.PickUpWeapon(_item);
                    }
                    break;
                }
            case 2:
                {
                    EquipManager.Instance.weaponController.secondWeapon.SetEquip((int)_item.itemData.weaponData.weaponForm);
                    if (secondWeapon.weaponData == null)
                    {
                        secondWeapon.PickUpWeapon(_item);
                    }
                    else
                    {
                        secondWeapon.DropWeapon();
                        secondWeapon.PickUpWeapon(_item);
                    }
                    break;
                }
        }
    }

    public void OnDrop(int index)
    {
        switch (index)
        {
            case 1:
                {
                    firstWeapon.DropWeapon();
                    EquipManager.Instance.weaponController.firstWeapon.UnEquip();
                    break;
                }
            case 2:
                {
                    secondWeapon.DropWeapon();
                    EquipManager.Instance.weaponController.secondWeapon.UnEquip();
                    break;
                }
        }
    }

// Index에는 피대상자의 index가 들어감 (1에 있는걸 2에 옮기면 index에 2가 들어감)
public void OnSwap(int index, WeaponData _data = null)
    {
        if(_data != null)
        {
            // 첫 번째 슬롯 기준
            WeaponData firData = GetWeaponData(firstWeapon);
            WeaponData secData = GetWeaponData(secondWeapon);

            AttachmentData _muzzleData = firstWeapon.muzzleData;
            AttachmentData _gribData = firstWeapon.gribData;
            AttachmentData _sightData = firstWeapon.sightData;
            AttachmentData _magData = firstWeapon.magData;
            AttachmentData _buttstockData = firstWeapon.buttstockData;

            EquipManager.Instance.weaponController.firstWeapon.SetEquip((int)secData.weaponForm);
            EquipManager.Instance.weaponController.secondWeapon.SetEquip((int)firData.weaponForm);

            // Swap
            firstWeapon.SwapWeapon(secondWeapon, secData);

            secondWeapon.muzzleData = _muzzleData;
            secondWeapon.gribData = _gribData;
            secondWeapon.sightData = _sightData;
            secondWeapon.magData = _magData;
            secondWeapon.buttstockData = _buttstockData;

            // 첫 번째 슬롯이 바뀌고 두 번째 슬롯을 정상적으로 바꾸기 위해 첫 번째 슬롯 데이터를 가지고 있음
            secondWeapon.SwapWeapon(secondWeapon, firData);
            
            ItemSoundManager.Instance.Play(_data.clip);
        }
        else
        {
            switch(index) 
            {
                case 1:
                    {
                        firstWeapon.SwapWeapon(secondWeapon, secondWeapon.weaponData);
                        ItemSoundManager.Instance.Play(secondWeapon.weaponData.clip);
                        secondWeapon.SwapEmpty();
                        EquipManager.Instance.weaponController.firstWeapon.SetEquip((int)firstWeapon.weaponData.weaponForm);
                        EquipManager.Instance.weaponController.secondWeapon.UnEquip();
                        break;
                    }
                case 2:
                    {
                        secondWeapon.SwapWeapon(firstWeapon, firstWeapon.weaponData);
                        ItemSoundManager.Instance.Play(firstWeapon.weaponData.clip);
                        firstWeapon.SwapEmpty();

                        EquipManager.Instance.weaponController.secondWeapon.SetEquip((int)secondWeapon.weaponData.weaponForm);
                        EquipManager.Instance.weaponController.firstWeapon.UnEquip();
                        break;
                    }
            }
        }
    }

    public WeaponData GetWeaponData(MainWeaponView _view)
    {
        return _view.weaponData;
    }

    public void OnSelected(int index)
    {
        for(int i = 0; i < selected.Count; i++)
        {
            if(i == index)
            {
                selected[index].gameObject.SetActive(true);
            }
            else
                selected[i].gameObject.SetActive(false);
        }
    }

    #region AttachmentCheck

    // 두 무기 전부 껴져 있을 때
    // 두 무기 중 하나만 껴져 있을 때 (현재 들고 있는 무기가 안껴져 있을 때) (현재 들고있지 않은 무기가 껴져 있을 때)
    // 두 무기 전부 안 껴져 있을 때
    // 착용할 수 없을 때

    public MainWeaponView IsAttachmentEquip(AttachmentData data)
    {
         EquipSlot slot = EquipManager.Instance.equipSlot;

        // 무기가 있는지 없는지 부터 체크
        // 두 개 다 있는지
        if(firstWeapon.isPosses && secondWeapon.isPosses)
        {
            switch (slot)
            {
                // 현재 슬롯이 2번
                case EquipSlot.SECOND:
                    {
                        // 2번에 부착물 착용 가능
                        if (secondWeapon.weaponData.IsAttchmentCheck(data.attachType))
                        {
                            if (data.IsPossible(secondWeapon.weaponData.weaponForm))
                                return secondWeapon;
                            else
                            {
                                if (firstWeapon.weaponData.IsAttchmentCheck(data.attachType))
                                {
                                    if (data.IsPossible(firstWeapon.weaponData.weaponForm))
                                        return firstWeapon;
                                    else
                                        return null;
                                }
                                else
                                    return null;
                            }
                        }
                        // 1번에 부착물 착용 준비
                        else
                        {
                            // 1번에 부착물 착용 가능
                            if (firstWeapon.weaponData.IsAttchmentCheck(data.attachType))
                            {
                                if (data.IsPossible(firstWeapon.weaponData.weaponForm))
                                    return firstWeapon;
                                else
                                {
                                    if (secondWeapon.weaponData.IsAttchmentCheck(data.attachType))
                                    {
                                        if (data.IsPossible(secondWeapon.weaponData.weaponForm))
                                            return secondWeapon;
                                        else
                                            return null;
                                    }
                                    else
                                        return null;
                                }
                            }
                            // 1번에 부착물 착용 불가능
                            else
                                return null;
                        }
                    }
                // 현재 슬롯이 1, 3, 4번
                default:
                    {
                        // 1번에 부착물 착용 가능
                        if (firstWeapon.weaponData.IsAttchmentCheck(data.attachType))
                        {
                            if (data.IsPossible(firstWeapon.weaponData.weaponForm))
                                return firstWeapon;
                            else
                            {
                                if (secondWeapon.weaponData.IsAttchmentCheck(data.attachType))
                                {
                                    if (data.IsPossible(secondWeapon.weaponData.weaponForm))
                                        return secondWeapon;
                                    else
                                        return null;
                                }
                                else
                                    return null;
                            }
                        }
                        // 2번에 부착물 착용 준비
                        else
                        {
                            // 2번에 부착물 착용 가능
                            if (secondWeapon.weaponData.IsAttchmentCheck(data.attachType))
                            {
                                if (data.IsPossible(secondWeapon.weaponData.weaponForm))
                                    return secondWeapon;
                                else
                                {
                                    if (firstWeapon.weaponData.IsAttchmentCheck(data.attachType))
                                    {
                                        if (data.IsPossible(firstWeapon.weaponData.weaponForm))
                                            return firstWeapon;
                                        else
                                            return null;
                                    }
                                    else
                                        return null;
                                }

                            }
                            // 2번에 부착물 착용 불가능
                            else
                                return null;
                        }
                    }
            }
        }
        // 둘 중 1번 무기만 있는지
        else if(firstWeapon.isPosses && !secondWeapon.isPosses)
        {
            if (firstWeapon.weaponData.IsAttchmentCheck(data.attachType))
            {
                if (data.IsPossible(firstWeapon.weaponData.weaponForm))
                    return firstWeapon;
                else
                    return null;
            }
            else
                return null;
        }
        // 둘 중 2번 무기만 있는지
        else if(!firstWeapon.isPosses && secondWeapon.isPosses)
        {
            if (secondWeapon.weaponData.IsAttchmentCheck(data.attachType))
            {
                if (data.IsPossible(secondWeapon.weaponData.weaponForm))
                    return secondWeapon;
                else
                    return null;
            }
            else
                return null;
        }
        // 둘 다 없는지
        else
            return null;
    }

    #endregion
}
