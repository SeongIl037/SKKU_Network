using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    public Slider StaminaSlider;
    public Player MyPlayer;
    
    public void Init(Player player)
    {
        MyPlayer = player;
        StaminaSlider.maxValue = MyPlayer.Stat.MaxStamina;
        StaminaSlider.value = MyPlayer.Stat.Stamina;
        MyPlayer.StaminaChanged += Refresh;
    }

    private void Refresh()
    {
        StaminaSlider.value = MyPlayer.Stat.Stamina;
        Debug.Log("UI 리프레시");
    }
}
