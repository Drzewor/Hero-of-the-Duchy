using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] GameObject pickup;

    public void DropItem(Item item, int amount = 1) 
    {
        GameObject pickupInstantion= Instantiate(pickup, GetDropLocation(), transform.rotation);
        pickupInstantion.GetComponent<Pickup>().item = item;
        pickupInstantion.GetComponent<Pickup>().amount = amount;
    }

    protected virtual Vector3 GetDropLocation()
    {
        return transform.position;
    }
}
