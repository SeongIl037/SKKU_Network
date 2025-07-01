using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarAbility : PlayerAbility
{
    public Slider HealthBarSlider;

    private void Start()
    {
        RefreshHealthBar();
        _owner.HealthChanged += RefreshHealthBar;
    }


    private void RefreshHealthBar()
    {
        HealthBarSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }

    // public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    // {
    //     if (stream.IsWriting)
    //     {
    //         stream.SendNext(_owner.Stat.Health / _owner.Stat.MaxHealth);
    //     }
    //     else if (stream.IsReading)
    //     {
    //         float value =(float)stream.ReceiveNext();
    //         HealthBarSlider.value = value;
    //     }
    // }
}
