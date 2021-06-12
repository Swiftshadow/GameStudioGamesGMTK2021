using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Body/Body Part")]
public class BodyPartBaseSO : ScriptableObject
{
    [SerializeField]
    private int baseHealth;
    private int healthMod;

    [SerializeField]
    private int baseAttack;
    private int attackMod;

    [SerializeField]
    private int baseDefense;
    private int defenseMod;

    [SerializeField]
    private int baseDurability;
    private int durabilityMod;

    public Sprite sprite;
    public int Health
    {
        get { return baseHealth + healthMod; }

        set { healthMod += value; }
    }
    
    public int Attack
    {
        get { return baseAttack + attackMod; }

        set { attackMod += value; }
    }
    
    public int Defense
    {
        get { return baseDefense + defenseMod; }

        set { defenseMod += value; }
    }
    
    public int Durability
    {
        get { return baseDurability + durabilityMod; }

        set { durabilityMod += value; }
    }
}
