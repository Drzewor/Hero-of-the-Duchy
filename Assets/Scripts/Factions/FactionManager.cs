using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Factions
{
    public class FactionManager : MonoBehaviour
    {
        [SerializeField] public FactionSO factionSO;

        public bool isEnemy(Faction potentialEnemy)
        {
            if(potentialEnemy == factionSO.faction) return false;

            foreach(Faction faction in factionSO.enemyFactions)
            {
                if(potentialEnemy == faction) return true;
            }

            return false;
        }

        public bool isAlly(Faction potentialAlly)
        {
            if(potentialAlly == factionSO.faction) return true;

            foreach(Faction allyFaction in factionSO.allyFactions)
            {
                if(potentialAlly == allyFaction) return true;
            }

            return false;
        }

        public Faction GetFaction()
        {
            return factionSO.faction;
        }
    }
}

