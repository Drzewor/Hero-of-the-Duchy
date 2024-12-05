using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentMenager : MonoBehaviour
{
    [SerializeField] private Transform headSlot;
    [SerializeField] private Transform torsoSlot;
    [SerializeField] private Transform lHandSlot;
    [SerializeField] private Transform rHandSlot;
    [SerializeField] private Transform lFootSlot;
    [SerializeField] private Transform rFootSlot;
    [SerializeField] private Transform shieldSlot;
    [SerializeField] private ArmorItem defaultHead;
    [SerializeField] private ArmorItem defaultTorso;
    [SerializeField] private ArmorItem defaultHands;
    [SerializeField] private ArmorItem defaultFoots;
    [SerializeField] private ArmorItem defaultShield;
    private GameObject head;
    private GameObject torso;
    private GameObject lHand;
    private GameObject rHand;
    private GameObject lFoot;
    private GameObject rFoot;
    private GameObject shield;
    private bool hasShield = false;
    public event Action shieldChange;

    private void Awake() 
    {
        EquipItem(defaultHead);
        EquipItem(defaultTorso);
        EquipItem(defaultHands);
        EquipItem(defaultFoots);
        EquipItem(defaultShield);
    }

    public void EquipItem(ArmorItem item)
    {
        if(item == null) return;
        switch (item.EquipmentType)
        {
            case EquipmentType.Helmet:
                if(head != null)
                {
                    Destroy(head);
                }
                head = Instantiate(item.EquipmentPrefab, headSlot);
                break;
            case EquipmentType.Armour:
                if(torso != null)
                {
                    Destroy(torso);
                }
                torso = Instantiate(item.EquipmentPrefab, torsoSlot);
                break;
            case EquipmentType.Gloves:
                if(lHand != null)
                {
                    Destroy(lHand);
                }
                if(rHand != null)
                {
                    Destroy(rHand);
                }
                lHand = Instantiate(item.EquipmentPrefab, lHandSlot);
                rHand = Instantiate(item.SecondEquipmentPrefab, rHandSlot);
                break;
            case EquipmentType.Boots:
                if(lFoot != null)
                {
                    Destroy(lFoot);
                }
                if(rFoot != null)
                {
                    Destroy(rFoot);
                }
                lFoot = Instantiate(item.EquipmentPrefab, lFootSlot);
                rFoot = Instantiate(item.SecondEquipmentPrefab, rFootSlot);
                break;
            case EquipmentType.OffHand:
                if(shield != null)
                {
                    Destroy(shield);
                }
                shield = Instantiate(item.EquipmentPrefab, shieldSlot);
                hasShield = true;
                shieldChange.Invoke();
                break;
            default:
                break;
        }
    }

    public void UnequipItem(EquipmentType type)
    {
        ArmorItem item;
        switch (type)
        {
            case EquipmentType.Helmet:
                item = defaultHead;
                break;
            case EquipmentType.Armour:
                item = defaultTorso;
                break;
            case EquipmentType.Gloves:
                item = defaultHands;
                break;
            case EquipmentType.Boots:
                item = defaultFoots;
                break;
            case EquipmentType.OffHand:
                if(shield != null)
                {
                    Destroy(shield);
                }
                item = defaultShield;
                hasShield = false;
                shieldChange.Invoke();
                break;
            default:
                item = null;
                break;
        }
        EquipItem(item);
    }

    public bool GetHasShield()
    {
        return hasShield;
    }
}
