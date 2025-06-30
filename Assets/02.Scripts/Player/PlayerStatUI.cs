using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    public Slider HealthSlider;
    public Slider StaminaSlider;
    public Player MyPlayer;
    
    public void Init(Player player)
    {
        MyPlayer = player;
        StaminaSlider.maxValue = MyPlayer.Stat.MaxStamina;
        StaminaSlider.value = MyPlayer.Stat.Stamina;
        
        HealthSlider.maxValue = MyPlayer.Stat.MaxHealth;
        HealthSlider.value = MyPlayer.Stat.Health;
        
        MyPlayer.HealthChanged += HealthRefresh;
        MyPlayer.StaminaChanged += Refresh;
    }

    private void Refresh()
    {
        StaminaSlider.value = MyPlayer.Stat.Stamina;
    }

    private void HealthRefresh()
    {
        HealthSlider.value = MyPlayer.Stat.Health;
        Debug.Log("감소");
    }
}
