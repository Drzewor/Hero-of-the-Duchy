using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Factions;
using RPG.StateMachine;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponLogic : MonoBehaviour
    {
        [SerializeField] private Collider mycollider;
        [SerializeField] public ParticleSystem trails;
        [SerializeField] public Projectile projectilePrefab;
        
        private float damage = 0;
        private float knockback = 0;
        private float armourPiercing = 0;
        private bool isMagic = false;
        public event Action<GameObject> OnKill;
        

        private List<Collider> alreadyCollidedWith = new List<Collider>();

        private void OnTriggerEnter(Collider other) 
        {
            if(other == mycollider) return;

            if(alreadyCollidedWith.Contains(other)) return;
            alreadyCollidedWith.Add(other);

            if(other.TryGetComponent<FactionManager>(out FactionManager fManager))
            {
                if(mycollider.GetComponent<FactionManager>().isAlly(fManager.GetFaction())) return;
            }
            
            if(other.TryGetComponent<Health>(out Health health))
            {
                health.DealDamage(damage, mycollider.gameObject, armourPiercing, isMagic);
                if(health.isDead)
                {
                    OnKill?.Invoke(other.gameObject);
                }
            }
            if(other.TryGetComponent<ForceReceiver>(out ForceReceiver forceReceiver))
            {
                Vector3 direction = (other.transform.position - mycollider.transform.position).normalized;
                forceReceiver.AddForce(direction * knockback);
            }
        }

        public void SetAttack
        (
            float damage, 
            float damageStatBonus, 
            float knockback, 
            float damageMultiplier,
            float armourPiercing, 
            bool isMagic)
            {
                this.damage = (damage+damageStatBonus) * damageMultiplier;
                this.knockback = knockback;
                this.isMagic = isMagic;
                this.armourPiercing = armourPiercing;
            }

        public Projectile LaunchProjectile(GameObject shooter, GameObject target)
        {
            projectilePrefab.target = target;
            projectilePrefab.shooter = shooter;
            projectilePrefab.damage = damage;
            projectilePrefab.isMagic = isMagic;

            Instantiate(projectilePrefab,gameObject.transform.position,shooter.transform.rotation);

            return projectilePrefab.GetComponent<Projectile>();
        }
        public Projectile LaunchProjectile( GameObject shooter)
        {
            projectilePrefab.target = null;
            projectilePrefab.shooter = shooter;
            projectilePrefab.damage = damage;
            projectilePrefab.isMagic = isMagic;

            Instantiate(projectilePrefab,gameObject.transform.position,shooter.transform.rotation);

            return projectilePrefab.GetComponent<Projectile>();
        }

        public void SetMyCollider(Collider mycollider)
        {
            this.mycollider = mycollider;
        }

        public void ClearColidersList()
        {
            alreadyCollidedWith.Clear();
        }

        public void SwitchTrails(bool play)
        {
            if(trails == null) return;
            if(play)
            {
                trails.Play();
            }
            else
            {
                trails.Stop();
            }
        }
    }
}

