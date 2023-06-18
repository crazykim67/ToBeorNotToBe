using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Localization;

public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;

    public static ItemManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ItemManager();
                return instance;
            }

            return instance;
        }
    }

    private ItemSlot coolItem;
    private ItemSlot noneCoolItem;

    [Header("GameObject")]
    public GameObject activePrefab;

    [Header("UI")]
    public TextMeshProUGUI itemNameUseText;
    public TextMeshProUGUI timerText;
    public Image gauge;

    [Header("Localize")]
    [SerializeField]
    private LocalizedString usingLocal;
    [SerializeField]
    private LocalizedString secLocal;

    private float coolTime;
    private float leftTime;
    private float fillTime;

    private IEnumerator coroutine;

    public List<AttachmentData> attachmentList = new List<AttachmentData>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        MoveItemCancel();
    }

    public IEnumerator UseItem(float _cool = 0)
    {
        InventoryManager.Instance.isUse = true;

        gauge.fillAmount = 0f;
        coolTime = _cool;
        leftTime = _cool;
        fillTime = 0f;

        activePrefab.SetActive(true);

        while (leftTime > 0.0f) 
        {
            leftTime -= Time.deltaTime;
            fillTime += Time.deltaTime;

            if(OptionManager.Instance == null)
            {
                itemNameUseText.text = $"{usingLocal.GetLocalizedString()} {coolItem.itemData.itemName.GetLocalizedString()}... ";
            }
            else
            {
                if(OptionManager.Instance.languageController.languageId == 0)
                {
                    itemNameUseText.text = $"{coolItem.itemData.itemName.GetLocalizedString()} {usingLocal.GetLocalizedString()}... ";
                }
                else
                {
                    itemNameUseText.text = $"{usingLocal.GetLocalizedString()} {coolItem.itemData.itemName.GetLocalizedString()}... ";
                }
            }
            
            timerText.text = $"{fillTime.ToString("F1")} / {coolTime} {secLocal.GetLocalizedString()}";
            gauge.fillAmount = 1.0f - (leftTime / coolTime);

            yield return null;
        }

        activePrefab.SetActive(false);
        itemNameUseText.text = "";

        if(coolItem != null)
        {
            EquipManager.Instance.bagController.OnSubVolume(coolItem, 1);
            coolItem.Subtraction(true, 1);
            coolItem = null;

            InventoryManager.Instance.isUse = false;
            StopCoroutine(coroutine);
            coroutine = null;
        }

    }

    public void UseItem()
    {
        if (noneCoolItem != null)
        {
            EquipManager.Instance.bagController.OnSubVolume(noneCoolItem, 1);
            noneCoolItem.Subtraction(true, 1);
            noneCoolItem = null;
        }
    }

    public void Use(ItemSlot _item)
    {
        if (_item == null)
            return;

        ItemSlot itemSlot = _item;

        switch (itemSlot.itemData.itemType) 
        {
            case ItemType.Weapon:
                {
                    noneCoolItem = itemSlot;

                    // ºÎÂø¹°
                    if (noneCoolItem.itemData.weaponData == null)
                        UseItem();
                    else
                    {

                    }
                    break;
                }
            case ItemType.Cure:
                {
                    if (coolItem == _item)
                        return;

                    coolItem = itemSlot;

                    if(coroutine != null)
                    {
                        InventoryManager.Instance.isUse = false;
                        StopCoroutine(coroutine);
                        coroutine = null;
                    }

                    coroutine = UseItem(coolItem.itemData.delay);
                    StartCoroutine(coroutine);
                    break;
                }
            case ItemType.Kit:
                {


                    break;
                }
            case ItemType.Material:
                {

                    break;
                }
            default:
                    return;
        }
    }

    public void UseCancel()
    {
        activePrefab.SetActive(false);
        itemNameUseText.text = "";

        coolItem = null;

        InventoryManager.Instance.isUse = false;
        StopCoroutine(coroutine);
        coroutine = null;
    }

    public void MoveItemCancel()
    {
        if (OptionManager.Instance != null)
            if (OptionManager.Instance.isMenu)
                return;

        if (InventoryManager.Instance.isUi)
            return;

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector2 moveDir = new Vector2(xAxis, yAxis);
        if (moveDir.magnitude > 0 && coroutine != null)
            UseCancel();
        else
            return;
    }

    public AttachmentData GetAttachmentData(AttachItem _item)
    {
        AttachmentData attachmentItem = null;

        foreach (var attachment in attachmentList)
        {
            if (attachment.attachItem == _item)
            {
                attachmentItem = attachment;
                break;
            }
        }
        

        return attachmentItem;
    }
}
