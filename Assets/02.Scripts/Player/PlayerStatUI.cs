using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    private PhotonView _view;
    public Slider StaminaSlider;
    public Player MyPlayer;
    private void Start()
    {
        
        _view = GetComponentInParent<PhotonView>();
        MyPlayer = GetComponentInParent<Player>();

        if(_view.IsMine)
        {
            StaminaSlider.maxValue = MyPlayer.Stat.MaxStamina;
            StaminaSlider.value = MyPlayer.Stat.Stamina;
            MyPlayer.StaminaChanged += Refresh;
        }
    }

    private void Refresh()
    {
        StaminaSlider.value = MyPlayer.Stat.Stamina;
        Debug.Log("UI 리프레시");
    }
}
