using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public string unitLevel;
    public int damage;
    public int maxHealth;
    public int currentHealth;
    //would it need to take in the damage int i already made?
    public void buffDamage(int dmg)
    {
        dmg = dmg + 2;
    }
    public void Defend(int amount)
    {
        damage -= amount;//unsure about this
    }
    //public void buffDefense
    public bool TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   
}
