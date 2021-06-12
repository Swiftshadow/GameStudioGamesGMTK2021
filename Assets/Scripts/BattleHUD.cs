using System.Collections;
using System.Collections.Generic;
using Monsters;
using UnityEngine;
using UnityEngine.UI;
public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text levelText;
    public Slider hpSlider;

    public void setHUD(MonsterStats stats)
    {
        nameText.text = stats.name;
        levelText.text = "Lvl " + stats.level;
        hpSlider.maxValue = stats.health;
        hpSlider.value = stats.health;
    }

    public void setHealth(int health)
    {
        hpSlider.value = health;
    }
}
