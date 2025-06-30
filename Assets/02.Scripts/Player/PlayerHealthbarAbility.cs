using System;
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

    public void RefreshHealthBar()
    {
        HealthBarSlider.value = _owner.Stat.Health / _owner.Stat.MaxHealth;
    }
}
