using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponLogic : MonoBehaviour
    {
        [SerializeField] private Collider mycollider;
        [SerializeField] public Projectile projectilePrefab;
        private float damage = 0;
        private float knockback = 0;
        public event Action<string> OnKill;
        

        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnTriggerEnter(Collider other) 
        {
            if(other == mycollider) return;
            if(alreadyCollidedWith.Contains(other)) return;
            alreadyCollidedWith.Add(other);
            if(other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage, mycollider.gameObject);
                if(health.isDead)
                {
                    OnKill?.Invoke(health.gameObject.name);
                }
            }
            if(other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - mycollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }

        public void SetAttack(float damage, float damageStatBonus, float knockback)
        {
            this.damage = damage + damageStatBonus;
            this.knockback = knockback;
        }

        public void SetAttack(float damage, float damageStatBonus, float knockback, float damageMultiplier)
        {
            this.damage = (damage+damageStatBonus) * damageMultiplier;
            this.knockback = knockback;
        }

        public void LaunchProjectile(GameObject shooter, GameObject target)
        {

            projectilePrefab.damage = damage;
            projectilePrefab.target = target;
            projectilePrefab.shooter = shooter;
            Instantiate(projectilePrefab,gameObject.transform.position,shooter.transform.rotation);
        }
        public void LaunchProjectile( GameObject shooter)
        {
            projectilePrefab.damage = damage;
            projectilePrefab.target = null;
            projectilePrefab.shooter = shooter;
            Instantiate(projectilePrefab,gameObject.transform.position,shooter.transform.rotation);
        }

        public void SetMyCollider(Collider mycollider)
        {
            this.mycollider = mycollider;
        }

        public void ClearColidersList()
        {
            alreadyCollidedWith.Clear();
        }
    }
}

