using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        public GameObject shooter;
        public GameObject target;
        [SerializeField] private bool isHoming = false;
        [SerializeField] private float speed = 10;
        public float damage = 10;
        

        private void Update() 
        {
            if(target != null && isHoming)
            {
                gameObject.transform.LookAt(CorrectedLookAt(target));
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other) 
        {
            if(other.gameObject == shooter) return;
            shooter.TryGetComponent<PlayerWeaponHandler>(out PlayerWeaponHandler handler);
            Health health = other.gameObject.GetComponent<Health>();
            if(health == null)
            {
                Destroy(gameObject);
                return;
            }
            health.DealDamage(damage, shooter);
            if(health.isDead)
            {
                handler?.HandleKill(other.gameObject);
            }
            Destroy(gameObject);
        }

        private Vector3 CorrectedLookAt(GameObject target)
        {
            Vector3 correctPosition = new Vector3(0f,0.5f,0f);
            correctPosition += correctPosition + target.transform.position;
            return correctPosition;
        }
    }

}
