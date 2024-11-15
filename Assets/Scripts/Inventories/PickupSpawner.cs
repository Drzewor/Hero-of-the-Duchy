using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using RPG.StateMachine.Player;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickup;
    [SerializeField] Item item;
    [SerializeField] int amount = 1;
    public bool isPicked = false;

    private void Awake() 
    {
        if(!isPicked)
        {
            GameObject pickupInstantion= Instantiate(pickup,gameObject.transform);
            pickupInstantion.GetComponent<Pickup>().spawner = this;
            pickupInstantion.GetComponent<Pickup>().item = item;
            pickupInstantion.GetComponent<Pickup>().amount = amount;
        }
    }
}
