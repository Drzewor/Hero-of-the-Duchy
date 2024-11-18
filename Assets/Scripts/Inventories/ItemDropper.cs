using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private GameObject pickup;
    [SerializeField] private float scatterDistance = 1;
    private const int Attempts = 20;

    public void DropItem(Item item, int amount = 1) 
    {
        GameObject pickupInstantion= Instantiate(pickup, GetDropLocation(), transform.rotation);
        pickupInstantion.GetComponent<Pickup>().item = item;
        pickupInstantion.GetComponent<Pickup>().amount = amount;
    }

    protected virtual Vector3 GetDropLocation()
    {
        for(int i = 0; i < Attempts; i++)
        {
            Vector3 randomPoint = transform.position + (Random.insideUnitSphere * scatterDistance);
            NavMeshHit hit;
            if(NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return transform.position;
    }
}
