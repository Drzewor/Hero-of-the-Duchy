using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Factions
{
    [CreateAssetMenu(fileName = "New Faction", menuName = "RPG/Faction")]
    public class FactionSO : ScriptableObject
    {
        public Faction faction;
        public List<Faction> allyFactions;
        public List<Faction> enemyFactions;

        public bool isEnemy(Faction potentialEnemy)
        {
            if(potentialEnemy == faction) return false;

            foreach(Faction enemyfaction in enemyFactions)
            {
                if(potentialEnemy == enemyfaction) return true;
            }

            return false;
        }

        public bool isAlly(Faction potentialAlly)
        {
            if(potentialAlly == faction) return true;

            foreach(Faction allyFaction in allyFactions)
            {
                if(potentialAlly == allyFaction) return true;
            }

            return false;
        }
    }
}
