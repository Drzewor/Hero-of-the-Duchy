using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.StateMachine.NPC;
using UnityEngine;

namespace RPG.Combat
{
    public class NPCWeaponHandler : MonoBehaviour
    {
        [SerializeField] private NPCStateMachine stateMachine;
        [SerializeField] private WeaponLogic weaponLogic;
        [SerializeField] private WeaponItem defaultWeapon;
        [SerializeField] private Transform weaponSlot;
        private float weaponRange;
        private WeaponItem weapon;
        private GameObject weaponInstance;

        private void Start() 
        {
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(WeaponItem weapon)
        {
            this.weapon = weapon;
            if(weaponInstance != null)
            {
                Destroy(weaponInstance);
            }
            weaponInstance = Instantiate(weapon.weaponPrefab, weaponSlot).gameObject;
            weaponRange = weapon.npcAttackRange;
            weaponLogic = weaponInstance.GetComponentInChildren<WeaponLogic>();
            weaponLogic.SetMyCollider(stateMachine.GetComponent<Collider>());
            SetWeapon();
            DisableWepon();
        }

        //called in animation
        public void EnableWepon()
        {
            weaponLogic.EnableWepon();
        }

        //called in animation
        public void DisableWepon()
        {
            weaponLogic.DisableWepon();
        }

        //called in animation
        public void Shoot()
        {
            if(weaponLogic.projectilePrefab == null) return;
            if(stateMachine.NPCTargeter.currentTarget ==null)
            {
                weaponLogic.LaunchProjectile(stateMachine.gameObject);
                return;
            }
            weaponLogic.LaunchProjectile(stateMachine.gameObject,stateMachine.NPCTargeter.currentTarget.gameObject);
        }

        private void SetWeapon()
        {
            stateMachine.SetWeapon(weaponLogic);

            stateMachine.SetAttackRange(weaponRange);

            stateMachine.SetStatDamageBonus(weapon.statDamageBonus);

            stateMachine.SetAttackDamage(weapon.weaponDamage);

            stateMachine.SetAttacks(weapon.attacks);

            stateMachine.SetIsWeaponMagic(weapon.isMagical);
        }
    }

}
