using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class AttachmentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MainWeaponView weaponView;

    public AttachmentData attachData;

    public AttachType attachType = AttachType.None;

    public RawImage viewImage;

    public RawImage image;

    private Transform playerTr;

    [SerializeField]
    private bool isHover;

    private void Update()
    {
        if (attachData == null)
            return;

        if (isHover)
            if (Input.GetMouseButtonDown(1))
                UnEquip();
    }

    public void OnEquip(AttachmentData _data)
    {
        if (attachData != null)
            UnEquip();

        this.attachData = _data;
        this.image.texture = attachData.img;

        int index = Random.Range(0, attachData.clips.Count);

        //weaponView.OnAttachmentEquip(_data);
        weaponView.OnAttachmentData(_data);

        image.gameObject.SetActive(true);
        ItemSoundManager.Instance.Play(attachData.clips[index]);
        //weaponView.equipWeaponView.SetAttachmentEquip(weaponView.equipWeaponView.weapon, attachData);
        // 여기
        WeaponManager.Instance.AttachEquip(ArmController.Instance.currentWeaponView, ArmController.Instance.currentMainWeaponView);
    }

    public void UnEquip()
    {
        if (attachData == null)
            return;

        int index = Random.Range(0, attachData.clips.Count);

        DropItem dropItem = attachData.itemPrefab.GetComponent<DropItem>();

        int sumVolume = EquipManager.Instance.bagController.usePlayerVolume + dropItem.itemData.weight;

        // 부착 해제 > 가방용량 부족 시
        if (EquipManager.Instance.bagController.playerVolume < sumVolume)
        {
            if (playerTr == null)
                playerTr = GameObject.FindWithTag("DropSpot").transform;

            Quaternion rot = Quaternion.Euler(attachData.itemPrefab.transform.eulerAngles.x, playerTr.eulerAngles.y, attachData.itemPrefab.transform.eulerAngles.z);
            //Quaternion rot = Quaternion.Euler(0, playerTr.eulerAngles.y, 0);

            // Drop Item
            DropItem throwItem = Instantiate(
                attachData.itemPrefab,
                playerTr.transform.position,
                rot).GetComponent<DropItem>();

            // Drop Item Set Count
            throwItem.SetItemCount(1);
            // 여기
        }
        // 부착 해제 > 가방용량 충분할 시
        else
        {
            InventoryManager.Instance.Instantiate(dropItem.itemData, 1);
            EquipManager.Instance.bagController.OnAddVolume(dropItem, 1);
        }

        //weaponView.equipWeaponView.OffAttachmentEquip(weaponView.equipWeaponView.weapon, attachData);
        // 여기
        weaponView.UnAttachmentData(attachData);
        //weaponView.AttachmentUnEquip(attachData);

        image.gameObject.SetActive(false);
        ItemSoundManager.Instance.Play(attachData.clips[index]);
        WeaponManager.Instance.AttachEquip(ArmController.Instance.currentWeaponView, ArmController.Instance.currentMainWeaponView);

        DataClear();
    }

    public void DataClear()
    {
        this.image.texture = null;
        this.attachData = null;
    }

    public void OnThrow()
    {
        this.image.texture = null;
        this.attachData = null;

        image.gameObject.SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) => isHover = true;
    public void OnPointerExit(PointerEventData eventData) => isHover = false;

    // ClickItemSlot 용
    public void RaycastTarget(bool isAct)
    {
        if (isAct)
            image.raycastTarget = isAct;
        else
            image.raycastTarget = isAct;
    }

    public void OnRaycastTarget(bool isAct)
    {
        if (isAct)
            viewImage.color = new Color(viewImage.color.r, viewImage.color.g, viewImage.color.b, 0.5f);
        else
            viewImage.color = new Color(viewImage.color.r, viewImage.color.g, viewImage.color.b, 0.2f);
    }
}
