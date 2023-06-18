using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    private static ArmController instance;

    public static ArmController Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ArmController();
                return instance;
            }

            return instance;
        }
    }

    private Animator anim;

    private WeaponForm currentStateParam = WeaponForm.None;

    public WeaponView currentWeaponView = null;
    public MainWeaponView currentMainWeaponView = null;

    private float currentValue = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }

    public void SetSlot(EquipSlot slot)
    {
        switch(slot) 
        {
            case EquipSlot.FIRST:
                {
                    currentWeaponView = EquipManager.Instance.weaponController.firstWeapon;
                    currentMainWeaponView = InventoryManager.Instance.weaponInventory.firstWeapon;

                    if (currentWeaponView == null)
                        return;

                    currentStateParam = currentWeaponView.weapon;
                    anim.SetInteger("weaponType", (int)currentStateParam);
                    WeaponManager.Instance.EquipWeapon(currentStateParam);

                    //WeaponManager.Instance.AttachEquip(currentWeaponView, currentMainWeaponView);
                    break;
                }
            case EquipSlot.SECOND:
                {
                    currentWeaponView = EquipManager.Instance.weaponController.secondWeapon;
                    currentMainWeaponView = InventoryManager.Instance.weaponInventory.secondWeapon;
                    if (currentWeaponView == null)
                        return;

                    currentStateParam = currentWeaponView.weapon;
                    anim.SetInteger("weaponType", (int)currentStateParam);
                    WeaponManager.Instance.EquipWeapon(currentStateParam);

                    //WeaponManager.Instance.AttachEquip(currentWeaponView, currentMainWeaponView);
                    break;
                }
            case EquipSlot.THIRD: 
                {

                    break;
                }
            case EquipSlot.FOURTH:
                {

                    break;
                }
        }
    }

    public void UnEquipWeapon()
    {
        if(EquipManager.Instance.isPossible)
            anim.SetTrigger("unEquip");
    }

    public void OnShowWeapon() => WeaponManager.Instance.currentWeapon.OnWeapon();

    //public void OnHideWeapon() => WeaponManager.Instance.prevWeapon.HideWeapon();

    public void OnRun(Vector3 dir)
    {
        if(dir.magnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
                Debug.Log("누름");
            }
            else if (!Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetFloat("Speed", 0.5f, 0.2f, Time.deltaTime);
                Debug.Log("안누름");
            }
        }
        else
            anim.SetFloat("Speed", 0f, 0.2f, Time.deltaTime);
    }
}
