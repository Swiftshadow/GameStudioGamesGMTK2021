using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Body/Body Part")]
public class BodyPartBaseSO : ScriptableObject
{
    public string description;
    
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

    public float xLocOffset = 0;
    public float yLocOffset = 0;

    public float xScaleOffset = 0;
    public float yScaleOffset = 0;
    
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
