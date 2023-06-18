using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Localization;

public class SlotInteraction : MonoBehaviour
{
    private GraphicRaycaster gr;

    [SerializeField]
    private ItemSlot currentHitItem;
    [SerializeField]
    private MainWeaponView currentHitWeaponItem;

    [SerializeField]
    private ExplainHud explainHud;
    private RectTransform explainHudRect;

    [SerializeField]
    private bool isHover = false;

    [SerializeField]
    private ClickItemSlot clickItemSlot;
    [SerializeField]
    private WeaponSlot clickWeaponSlot;

    private RectTransform clickItemRect;
    private RectTransform clickWeaponRect;

    [SerializeField]
    private ScrollRect scrollView;

    [SerializeField]
    private LocalizedString noneString;
    [SerializeField]
    private LocalizedString noItemString;

    private void Awake()
    {
        gr = GetComponent<GraphicRaycaster>();
        explainHudRect = explainHud.GetComponent<RectTransform>();
        clickItemRect = clickItemSlot.GetComponent<RectTransform>();
        clickWeaponRect = clickWeaponSlot.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // Mouse Hover Check
        IsHover();
        
        // Following ExplainHud
        OnUpdateRect();
        // Following ClickItemSlot
        OnUpdateClickRect();

        // Following ClickWeaponSlot
        OnUpdateClickWeaponUpdate();

        // Show Explain Hud
        OnExplain();

        OnGraphicRaycast();
    }

    public void OnGraphicRaycast()
    {
        if (!InventoryManager.Instance.isUi)
        {
            if(clickItemSlot.isAct)
                clickItemSlot.OffClickItem();

            if (clickWeaponSlot.isAct)
                clickWeaponSlot.OffClickItem();

            currentHitItem = null;
            currentHitWeaponItem = null;
            isHover = false;
            return;
        }

        // 아이템 드래그
        if (Input.GetMouseButton(0))
        {
            if(currentHitItem != null)
            {
                clickItemSlot.SetItemData(currentHitItem);
                scrollView.vertical = false;
            }
            else if(currentHitWeaponItem != null)
            {
                clickWeaponSlot.SetItemData(currentHitWeaponItem);
                scrollView.vertical = false;
            }

            currentHitItem = null;
            currentHitWeaponItem = null;
            isHover = false;
            return;
        }
        // 아이템 왼쪽 클릭
        else if (Input.GetMouseButtonUp(0))
        {
            if(clickItemSlot.isAct)
                clickItemSlot.Raycast();
            else if(clickWeaponSlot.isAct)
                clickWeaponSlot.Raycast();

            clickItemSlot.OffClickItem();
            clickWeaponSlot.OffClickItem();

            scrollView.vertical = true;
        }
        // 낱개로 아이템 버리기(드래그)
        else if(Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftControl))
        {
            if (InventoryManager.Instance.isUse)
            {
                // Toast
                return;
            }

            InventoryManager.Instance.throwMenu.OnShow(currentHitItem);
            clickItemSlot.OffClickItem();
        }
        // 아이템 사용
        else if (Input.GetMouseButtonDown(1))
        {
            // 인벤토리 인 뷰
            if(currentHitItem != null)
            {
                switch (currentHitItem.type)
                {
                    case ItemType.Cure:
                        {
                            ItemManager.Instance.Use(currentHitItem);
                            break;
                        }
                    case ItemType.Weapon:
                        {
                            // 부착물
                            if(currentHitItem.itemData.weaponData == null)
                            {
                                MainWeaponView weaponView = null;
                                AttachmentData attachmentData = currentHitItem.itemData.attachmentData;
                                if (InventoryManager.Instance.weaponInventory.IsAttachmentEquip(attachmentData) != null)
                                {
                                    weaponView = InventoryManager.Instance.weaponInventory.IsAttachmentEquip(attachmentData);

                                    //if (!attachmentData.IsPossible(weaponView.weaponData.weaponForm))
                                    //{
                                    //    ToastController.Instance.OnToast(noItemString.GetLocalizedString());
                                    //    return;
                                    //}

                                    AttachmentItem attachmentItem = weaponView.GetAttachmentItem(attachmentData.attachType);
                                    attachmentItem.OnEquip(attachmentData);
                                    ItemManager.Instance.Use(currentHitItem);
                                }
                                else
                                    ToastController.Instance.OnToast(noneString.GetLocalizedString());

                                break;
                            }
                            // 투척 무기
                            else
                            {

                            }
                            break;
                        }
                }
            }
            // 무기 뷰
            else if(currentHitWeaponItem != null) 
            {
                if (currentHitWeaponItem.weaponData != null)
                    switch(currentHitWeaponItem.weaponData.weaponType) 
                    {
                        case WeaponData.WeaponType.Melee:
                            {

                                break;
                            }
                        case WeaponData.WeaponType.TW:
                            {

                                break;
                            }
                        default:
                            {
                                InventoryManager.Instance.weaponInventory.OnDrop((int)currentHitWeaponItem.slotType);

                                if ((int)EquipManager.Instance.equipSlot == (int)currentHitWeaponItem.slotType)
                                    EquipManager.Instance.OnEquip((int)EquipManager.Instance.equipSlot);
                                break;
                            }
                    }
                else
                {

                }
            }
        }

        var ped = new PointerEventData(null);

        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);

        if (results.Count <= 0)
        {
            currentHitItem = null;
            currentHitWeaponItem = null;
            isHover = false;
            return;
        }

        if (results[0].gameObject.tag == "ItemSlot")
            currentHitItem = results[0].gameObject.transform.GetComponent<ItemSlot>();
        else if (results[0].gameObject.tag == "WeaponSlot")
            currentHitWeaponItem = results[0].gameObject.transform.GetComponent<MainWeaponView>();
        else
        {
            currentHitItem = null;
            currentHitWeaponItem = null;
            isHover = false;
        }
    }

    public void IsHover()
    {
        if(currentHitItem != null)
            isHover = true;
        else if(currentHitWeaponItem != null) 
            isHover = true;
        else
        {
            currentHitItem = null;
            currentHitWeaponItem = null;
            isHover = false;
        }
    }

    public void OnExplain()
    {
        if (isHover && currentHitItem != null)
            explainHud.OnExplain(currentHitItem);
        if(isHover && currentHitWeaponItem != null)
            explainHud.OnWeaponExplain(currentHitWeaponItem);
        else if (!isHover)
        {
            currentHitItem = null;
            currentHitWeaponItem = null;
            explainHud.OffExplain();
            explainHud.OffWeaponExplain();
        }
    }

    public void OnUpdateRect()
    {
        if (isHover)
        {
            Vector2 mousePos = Input.mousePosition;
            explainHudRect.position = mousePos;
        }
    }

    public void OnUpdateClickRect()
    {
        if (!clickItemSlot.isAct)
            return;

        Vector2 mousePos = Input.mousePosition;
        clickItemRect.position = mousePos;
    }

    public void OnUpdateClickWeaponUpdate()
    {
        if (!clickWeaponSlot.isAct)
            return;

        Vector2 mousePos = Input.mousePosition;
        clickWeaponRect.position = mousePos;
    }
}
