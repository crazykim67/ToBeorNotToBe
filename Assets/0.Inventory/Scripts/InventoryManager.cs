using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static InventoryManager;
using UnityEngine.Rendering;
using UnityEngine.Localization.Components;

public class InventoryManager : MonoBehaviour
{
    public enum InventoryType
    {
        All,
        Weapon,
        Cure,
        Ammo,
        Material,
    }

    private static InventoryManager instance; 

    public static  InventoryManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new InventoryManager();
                return instance;
            }

            return instance;
        }
    }

    [Header("Master Canvas")]
    public CanvasGroup master;

    [SerializeField]
    private GameObject hudUI;
    [SerializeField]
    private GameObject outSideUI;

    [Header("Throw Menu")]
    public ThrowMenu throwMenu;

    [HideInInspector]
    public bool isUi;
    //[HideInInspector]
    public bool isUse;

    [SerializeField]
    private InventoryType inventoryType = InventoryType.All;
    [SerializeField]
    private GameObject itemSlot;

    public List<ItemSlot> slots = new List<ItemSlot>();

    [SerializeField]
    private Transform content;

    [Header("Toggle Group")]
    public List<Toggle> toggles = new List<Toggle>();

    private AudioSource ad;
    [Header("Audio Clips")]
    [SerializeField]
    private List<AudioClip> clips = new List<AudioClip>();

    [Header("Bag UI")]
    public RawImage bagImage;
    public LocalizeStringEvent bagName;
    public Image spareVolume;
    public Image bagVolume;

    [Header("Weapon Inventory")]
    public WeaponInventoryView weaponInventory;

    private void Awake()
    {
        instance = this;
        ad = GetComponent<AudioSource>();
    }

    public void Update()
    {
        OnMenu();
    }

    // Input Key On Show Menu
    public void OnMenu()
    {
        if(OptionManager.Instance != null)
        {
            if (OptionManager.Instance.isMenu)
                return;
        }

        // 전체
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isUi)
            {
                OnShow(InventoryType.All);
                toggles[(int)InventoryType.All].SetIsOnWithoutNotify(true);

                // 효과음 실행
                Play(0);
            }
            else
            {
                if(inventoryType == InventoryType.All)
                    OnHide();
                else
                {
                    CategoryFilter(InventoryType.All);
                    toggles[(int)InventoryType.All].SetIsOnWithoutNotify(true);
                }
            }
        }
        // 무기
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (!isUi)
            {
                OnShow(InventoryType.Weapon);
                toggles[(int)InventoryType.Weapon].SetIsOnWithoutNotify(true);

                // 효과음 실행
                Play(0);
            }
            else
            {
                if (inventoryType == InventoryType.Weapon)
                    OnHide();
                else
                {
                    CategoryFilter(InventoryType.Weapon);
                    toggles[(int)InventoryType.Weapon].SetIsOnWithoutNotify(true);
                }
            }
        }
        // 치료
        else if (Input.GetKeyDown(KeyCode.C))
        {
            if (!isUi)
            {
                OnShow(InventoryType.Cure);
                toggles[(int)InventoryType.Cure].SetIsOnWithoutNotify(true);

                // 효과음 실행
                Play(0);
            }
            else
            {
                if (inventoryType == InventoryType.Cure)
                    OnHide();
                else
                {
                    CategoryFilter(InventoryType.Cure);
                    toggles[(int)InventoryType.Cure].SetIsOnWithoutNotify(true);
                }
            }
        }
        // 탄약
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isUi)
            {
                OnShow(InventoryType.Ammo);
                toggles[(int)InventoryType.Ammo].SetIsOnWithoutNotify(true);

                // 효과음 실행
                Play(0);
            }
            else
            {
                if (inventoryType == InventoryType.Ammo)
                    OnHide();
                else
                {
                    CategoryFilter(InventoryType.Ammo);
                    toggles[(int)InventoryType.Ammo].SetIsOnWithoutNotify(true);
                }
            }
        }
        // 재료
        else if (Input.GetKeyDown(KeyCode.K))
        {
            if (!isUi)
            {
                OnShow(InventoryType.Material);
                toggles[(int)InventoryType.Material].SetIsOnWithoutNotify(true);

                // 효과음 실행
                Play(0);
            }
            else
            {
                if (inventoryType == InventoryType.Material)
                    OnHide();
                else
                {
                    CategoryFilter(InventoryType.Material);
                    toggles[(int)InventoryType.Material].SetIsOnWithoutNotify(true);
                }
            }
        }
    }

    #region UI Relation

    public void OnShow(InventoryType type)
    {
        if (throwMenu.isAct)
            return;

        hudUI.SetActive(true);
        outSideUI.SetActive(true);
        isUi = true;

        // 아이템 코드순으로 정렬
        foreach (var item in slots)
        {
            item.transform.SetSiblingIndex(item.itemData.itemCode);
        }

        CategoryFilter(type);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        inventoryType = type;
    }

    public void OnHide()
    {
        if (throwMenu.isAct)
            return;

        hudUI.SetActive(false);
        outSideUI.SetActive(false);
        isUi = false;

        // 효과음 실행
        Play(1);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CategoryFilter(InventoryType type)
    {
        // 카테고리 필터
        if (type != InventoryType.All)
        {
            foreach (var item in slots)
            {
                if (CheckType(type, item.type))
                    item.gameObject.SetActive(true);
                else
                    item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in slots)
                item.gameObject.SetActive(true);
        }

        inventoryType = type;
    }

    // Change Category
    public void OnValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                {
                    //OnShow(InventoryType.All);
                    CategoryFilter(InventoryType.All);
                    break;
                }
            case 1:
                {
                    //OnShow(InventoryType.Weapon);
                    CategoryFilter(InventoryType.Weapon);
                    break;
                }
            case 2:
                {
                    //OnShow(InventoryType.Cure);
                    CategoryFilter(InventoryType.Cure);
                    break;
                }
            case 3:
                {
                    //OnShow(InventoryType.Ammo);
                    CategoryFilter(InventoryType.Ammo);
                    break;
                }
            case 4:
                {
                    //OnShow(InventoryType.Material);
                    CategoryFilter(InventoryType.Material);
                    break;
                }
        }
    }

    // Inventory Item Instantiate
    public void Instantiate(ItemData _itemData, int _itemCount)
    {
        foreach(var _item in slots)
        {
            if(_item.itemData.itemCode == _itemData.itemCode) 
            {
                _item.AddItem(_itemCount);
                return;
            }
        }

        ItemSlot item = Instantiate(itemSlot, content).GetComponent<ItemSlot>();
        item.SetItemData(_itemData, _itemCount);
        slots.Add(item);
    }

    // On Menu Category Check Return Boolean
    public bool CheckType(InventoryType invenType, ItemType _type)
    {
        bool isCheck = false;

        switch (_type)
        {
            case ItemType.Ammo:
                {
                    if (invenType == InventoryType.Ammo)
                        isCheck = true;
                    else
                        isCheck = false;
                    break;
                }
            case ItemType.Weapon:
                {
                    if (invenType == InventoryType.Weapon)
                        isCheck = true;
                    else
                        isCheck = false;
                    break;
                }
            case ItemType.Cure:
                {
                    if (invenType == InventoryType.Cure)
                        isCheck = true;
                    else
                        isCheck = false;
                    break;
                }
            case ItemType.Kit:
            case ItemType.Material:
                {
                    if (invenType == InventoryType.Material)
                        isCheck = true;
                    else
                        isCheck = false;
                    break;
                }
        }

        return isCheck;
    }

    #endregion

    // Inventory Item Destroy
    public void Destroy(ItemSlot _item)
    {
        Destroy(_item.gameObject);
    }

    // Audio Play
    private void Play(int index)
    {
        ad.clip = clips[index];

        ad.Play();
    }

    #region Bag

    public void SetSpareVolume(int volume)
    {
        spareVolume.rectTransform.sizeDelta = new Vector2(spareVolume.rectTransform.sizeDelta.x, volume);
    }

    public void SetUseVolume(int volume)
    {
        bagVolume.rectTransform.sizeDelta = new Vector2(bagVolume.rectTransform.sizeDelta.x, volume);
    }

    public void PickUpBag(DropItem bagItem)
    {
        // Bag Image Init
        if (bagImage.color.a == 0)
            bagImage.color = new Color(1, 1, 1, 1);

        // Set Texture
        bagImage.texture = bagItem.itemData.img;
        bagName.StringReference = bagItem.itemData.itemName;

        // Volume Extension
        SetSpareVolume(EquipManager.Instance.bagController.OnEquipBag(bagItem));
    }

    #endregion

    #region Master Canvas

    public void OnBackMenu(bool isAct)
    {
        if (isAct)
            master.alpha = 0;
        else
            master.alpha = 1;
    }

    #endregion

    #region ItemCheck

    public string GetAmmoAmount(BulletType _bullet)
    {
        string ammoAmount = "0";

        foreach(var slot in slots)
        {
            if(slot.type == ItemType.Ammo)
            {
                if(slot.itemData.bulletType == _bullet)
                {
                    ammoAmount = slot.itemCount.ToString();
                    break;
                }
            }
        }

        return ammoAmount;
    }

    #endregion
}
