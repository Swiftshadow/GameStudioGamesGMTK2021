using System.Collections.Generic;
using Channels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Monsters
{
     
    public enum PART_LOCATIONS
    {
        BODY,
        FRONT_ARM,
        BACK_ARM,
        EXTRA_1,
        EXTRA_2,
        EXTRA_3,
        TAIL
    }

    public enum MONSTER_ACTIONS
    {
        BASE_ATTACK,
        SPECIAL_ATTACK,
        BUFF_ATTACK,
        BUFF_DEFENSE,
        STALL,
        RISK_REWARD,
        DEFEND,
        HEAL
    }
    
    [System.Serializable]
    public struct MonsterStats
    {
        public string name;
        public int level;
        public int health;
        public int attack;
        public int defense;
    }
    
    /// <summary>
    /// Class for a monster. Works for both player and enemy monsters
    /// </summary>
    public class Monster : MonoBehaviour
    {
        [Tooltip("Listen for a part changing")]
        [SerializeField]
        private BodyPartChannel partChangedListener;

        [Tooltip("Send damage to the opponent")]
        [SerializeField]
        private IntChannel sendDamage;

        [FormerlySerializedAs("recieveDamage")]
        [Tooltip("Recieve damage from the opponent")]
        [SerializeField]
        private IntChannel receiveDamage;

        [Tooltip("Add an action to the player's queue")]
        [SerializeField]
        private MonsterActionChannel addAction;

        [Tooltip("Do the next action in the queue")]
        [SerializeField]
        private VoidChannel doAction;
        
        [Tooltip("Triggered when monster dies")]
        [SerializeField]
        private VoidChannel onDie;

        [SerializeField]
        private IntChannel modifyAttack;

        [SerializeField]
        private IntChannel modifyDefense;

        [SerializeField]
        private VoidChannel resetAttack;

        [SerializeField]
        private VoidChannel resetDefense;

        [SerializeField]
        private VoidChannel resetHealth;
        
        [SerializeField]
        private MonsterStatsChannel updateBattleSystem;

        [SerializeField]
        private VoidChannel requestStats;

        [SerializeField] private VoidChannel clearQueue;

        [SerializeField] private StringChannel changeName;

        [SerializeField] private GameObject attackBuffIcon;
        [SerializeField] private GameObject defendBuffIcon;
        [SerializeField] private GameObject brokenShieldIcon;
        
        [SerializeField] private int attackBuffValue;
        [SerializeField] private int defendBuffValue;
        [SerializeField] private int healValue;
        [SerializeField] private int defensiveAttackDefBuffValue;
        [SerializeField] private int riskyAttackValue;
        [SerializeField] private int riskyDefenseValue;
        
        private Animator anim;
        /// <summary>
        /// Parts currently attached to the monster.
        /// 0 is body
        /// 1 is front arm
        /// 2 is back arm
        /// 3 is extra slot 1
        /// 4 is extra slot 2
        /// 5 is tail
        /// </summary>
        [SerializeField]
        private BodyPartBaseSO[] attachedParts = new BodyPartBaseSO[7];

        /// <summary>
        /// Monster's base stats
        /// </summary>
        /// <returns></returns>
        [SerializeField]
        private MonsterStats stats = new MonsterStats();

        /// <summary>
        /// Modifications to the monster's stats
        /// </summary>
        [SerializeField]
        private MonsterStats statMods = new MonsterStats();

        /// <summary>
        /// Actions the monster is going to take
        /// </summary>
        [SerializeField]
        private Queue<MONSTER_ACTIONS> actions = new Queue<MONSTER_ACTIONS>();
        
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

        /// <summary>
        /// Recalculates the stats including modifiers
        /// </summary>
        /// <returns>Calculated stats</returns>
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
            newStats.name = statMods.name;
            newStats.level = statMods.level;
            
            
            return newStats;
        }

        public MonsterStats GetCurrentStats()
        {
            MonsterStats newStats = new MonsterStats();
            newStats.name = stats.name;
            newStats.level = stats.level;
            newStats.attack = stats.attack + statMods.attack;
            newStats.defense = stats.defense + statMods.defense;
            newStats.health = stats.health + statMods.health;

            return newStats;
        }
        /// <summary>
        /// Triggered when a part changes. Swaps the part and recalcualtes stats.
        /// </summary>
        /// <param name="obj">Part and location to insert</param>
        private void PartChangedEvent(BodyPartChangeEvent obj)
        {
            SetPart(obj.location, obj.newPart);
            stats = CalculateStats();
        }

        /// <summary>
        /// Apply a health modifier
        /// </summary>
        /// <param name="amount">How much to modify by</param>
        public void ChangeHealth(int amount)
        {
            statMods.health += amount;

            updateBattleSystem.RaiseEvent(GetCurrentStats());
            if (stats.health + statMods.health <= 0)
            {
                onDie.RaiseEvent();
            }
        }

        /// <summary>
        /// Reset health modifier
        /// </summary>
        public void ResetHealth()
        {
            statMods.health = 0;
        }
        
        /// <summary>
        /// Apply an attack modifier
        /// </summary>
        /// <param name="amount">How much to modify by</param>
        public void ChangeAttack(int amount)
        {
            statMods.attack += amount;
            attackBuffIcon.SetActive(true);
        }

        /// <summary>
        /// Reset attack modifier
        /// </summary>
        public void ResetAttack()
        {
            statMods.attack = 0;
            attackBuffIcon.SetActive(false);
        }
        
        /// <summary>
        /// Apply a defense modifier
        /// </summary>
        /// <param name="amount">How much to modify by</param>
        public void ChangeDefense(int amount)
        {
            statMods.defense += amount;
            if (statMods.defense >= 0)
            {
                defendBuffIcon.SetActive(true);
            }
            else
            {
                brokenShieldIcon.SetActive(false);
            }
        }

        /// <summary>
        /// Reset defense modifier
        /// </summary>
        public void ResetDefense()
        {
            statMods.defense = 0;
            defendBuffIcon.SetActive(false);
        }
        
        /// <summary>
        /// Adds a new action to the queue
        /// </summary>
        /// <param name="action">Action to add</param>
        public void AddAction(MONSTER_ACTIONS action)
        {
            actions.Enqueue(action);
        }

        /// <summary>
        /// Does the given action. If harming the enemy, triggers the event
        /// </summary>
        public void DoAction()//can prolly put the icon poppy uppy in here
        {
           
           MONSTER_ACTIONS action = actions.Dequeue();

            MonsterStats currentStats = GetCurrentStats();
            // Defines what each action does. Not good architecture, but its a gamejam
            switch (action)
            {
                case MONSTER_ACTIONS.BASE_ATTACK:
                    sendDamage.RaiseEvent(currentStats.attack);
                    anim.SetTrigger("Walk");
                    break;
                case MONSTER_ACTIONS.SPECIAL_ATTACK:
                    Debug.Log("Special Attack!");
                    sendDamage.RaiseEvent(currentStats.attack/2);
                    anim.SetTrigger("Walk");
                    ChangeDefense(defensiveAttackDefBuffValue);
                    break;
                case MONSTER_ACTIONS.DEFEND://show the shield icon
                    Debug.Log("Defend!");
                    ChangeDefense(defendBuffValue);
                    //setActive defend icon
                    break;
                case MONSTER_ACTIONS.BUFF_ATTACK:
                    ChangeAttack(attackBuffValue);
                    break;
                case MONSTER_ACTIONS.BUFF_DEFENSE:
                    ChangeDefense(defendBuffValue);
                    break;
                case MONSTER_ACTIONS.HEAL:
                    ChangeHealth(healValue);
                    break;
                case MONSTER_ACTIONS.RISK_REWARD:
                    ChangeAttack(riskyAttackValue);
                    ChangeDefense(riskyDefenseValue);
                    sendDamage.RaiseEvent(GetCurrentStats().attack);
                    anim.SetTrigger("Walk");
                    break;
                case MONSTER_ACTIONS.STALL:
                    Debug.Log("Stall!");
                    break;
            }
        }
        
        /// <summary>
        /// Triggered when monster receives damage. Damage adjusted by defense, floored at 1
        /// </summary>
        /// <param name="obj">Amount of raw damage received</param>
        private void ReceiveDamage(int obj)
        {
            int damage = obj - GetCurrentStats().defense;
            if (damage < 0)
            {
                statMods.defense -= obj;
                damage = 0;
            }
            else
            {
                statMods.defense = 0;
            }
            anim.SetTrigger("Damaged");
            ChangeHealth(-damage);
            MonsterStats newStats = GetCurrentStats();
            updateBattleSystem.RaiseEvent(newStats);
        }
        
        private void OnEnable()
        {
            if (partChangedListener != null)
                partChangedListener.OnEventRaised += PartChangedEvent;
            receiveDamage.OnEventRaised += ReceiveDamage;
            addAction.OnEventRaised += AddAction;
            doAction.OnEventRaised += DoAction;
            modifyAttack.OnEventRaised += ChangeAttack;
            modifyDefense.OnEventRaised += ChangeDefense;
            resetAttack.OnEventRaised += ResetAttack;
            resetDefense.OnEventRaised += ResetDefense;
            resetHealth.OnEventRaised += ResetHealth;
            requestStats.OnEventRaised += RequestStats;
            clearQueue.OnEventRaised += ClearActionQueue;
            changeName.OnEventRaised += ChangeName;
        }

        private void OnDisable()
        {
            if (partChangedListener != null)
                partChangedListener.OnEventRaised -= PartChangedEvent;
            receiveDamage.OnEventRaised -= ReceiveDamage;
            addAction.OnEventRaised -= AddAction;
            doAction.OnEventRaised -= DoAction;
            modifyAttack.OnEventRaised -= ChangeAttack;
            modifyDefense.OnEventRaised -= ChangeDefense;
            resetAttack.OnEventRaised -= ResetAttack;
            resetDefense.OnEventRaised -= ResetDefense;
            resetDefense.OnEventRaised -= ResetHealth;
            requestStats.OnEventRaised -= RequestStats;
            clearQueue.OnEventRaised -= ClearActionQueue;
            changeName.OnEventRaised -= ChangeName;
        }

        private void ClearActionQueue()
        {
            actions.Clear();
        }

        private void RequestStats()
        {
            updateBattleSystem.RaiseEvent(GetCurrentStats());
        }

        private void ChangeName(string obj)
        {
            stats.name = obj;
            statMods.name = obj;
        }
        
        private void Start()
        {
            stats = CalculateStats();
            anim = gameObject.GetComponent<Animator>();
        }
    }
}