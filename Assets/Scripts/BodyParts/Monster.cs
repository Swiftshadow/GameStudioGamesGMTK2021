using System;
using Channels;
using UnityEngine;

namespace BodyParts
{

    public enum PART_LOCATIONS
    {
        HEAD,
        TORSO,
        L_ARM,
        R_ARM,
        L_LEG,
        R_LEG,
        TAIL
    }

    [System.Serializable]
    public struct MonsterStats
    {
        public int health;
        public int attack;
        public int defense;
    }
    public class Monster : MonoBehaviour
    {
        [Tooltip("Listen for a part changing")]
        public BodyPartChannel partChangedListener;
        
        /// <summary>
        /// Parts currently attached to the monster.
        /// 0 is head
        /// 1 is torso
        /// 2 is left arm
        /// 3 is right arm
        /// 4 is left leg
        /// 5 is right leg
        /// 6 is tail
        /// </summary>
        [SerializeField]
        private BodyPartBaseSO[] attachedParts = new BodyPartBaseSO[7];

        /// <summary>
        /// Monster's base stats
        /// </summary>
        /// <returns></returns>
        private MonsterStats stats = new MonsterStats();

        /// <summary>
        /// Modifications to the monster's stats
        /// </summary>
        private MonsterStats statMods = new MonsterStats();
        
        /// <summary>
        /// Gets the part at the given location
        /// </summary>
        /// <param name="loc">Location to get the part at</param>
        /// <returns>THe requested part</returns>
        public BodyPartBaseSO GetAttachedPart(PART_LOCATIONS loc)
        {
            return attachedParts[(int) loc];
        }

        /// <summary>
        /// Set the part at a given location
        /// </summary>
        /// <param name="loc">Where to set the part</param>
        /// <param name="part">What part to set</param>
        public void SetPart(PART_LOCATIONS loc, BodyPartBaseSO part)
        {
            int intLoc = (int) loc;
            attachedParts[intLoc] = part;
            
            // Change the sprite to the given one
            transform.GetChild(intLoc).GetComponent<SpriteRenderer>().sprite = part.sprite;
        }

        private MonsterStats CalculateStats()
        {
            MonsterStats newStats = new MonsterStats();
            foreach (var part in attachedParts)
            {
                if (part is null) continue;
                
                newStats.attack += part.Attack;
                newStats.defense += part.Defense;
                newStats.health += part.Health;
            }

            newStats.attack += statMods.attack;
            newStats.defense += statMods.defense;
            newStats.health += statMods.health;
            
            return newStats;
        }

        private void PartChangedEvent(BodyPartChangeEvent obj)
        {
            SetPart(obj.location, obj.newPart);
            stats = CalculateStats();
        }

        public void ChangeHealth(int amount)
        {
            statMods.health += amount;
        }

        public void ChangeAttack(int amount)
        {
            statMods.attack += amount;
        }

        public void ChangeDefense(int amount)
        {
            statMods.defense += amount;
        }
        
        private void OnEnable()
        {
            partChangedListener.OnEventRaised += PartChangedEvent;
        }

        private void OnDisable()
        {
            partChangedListener.OnEventRaised -= PartChangedEvent;
        }

        private void Start()
        {
            stats = CalculateStats();
        }
    }
}