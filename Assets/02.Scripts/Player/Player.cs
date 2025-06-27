using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PhotonView _view;
    public PlayerStat Stat;
    public Dictionary<Type, PlayerAbility> _abilitiesCache = new Dictionary<Type, PlayerAbility>();
    
    public event Action StaminaChanged;
    
    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);
        
        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability as T;
            
            return ability as T;
        }
        
        throw new Exception($"어빌리티{type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        
        if (_view.IsMine)
        {
            PlayerStatUI ui = GameObject.FindGameObjectWithTag("StatUI").GetComponent<PlayerStatUI>();
            ui.Init(this);
        }
    }

    private void Update()
    {
        if (Stat.Stamina >= Stat.MaxStamina)
        {
            return;
        }
        
        if (GetAbility<PlayerAttackAbility>().IsAttacking && GetAbility<PlayerController>().CanRecovery() == false)
        {
            return;
        }
        
        RecoveryStamina(Stat.StaminRecovery);
    }

    public bool ImmediateReduceStamina(float value)
    {
        if (CanMove(value) == false)
        {
            return false;
        }
        
        Stat.Stamina -= value;
        StaminaChanged?.Invoke();
        return true;
    }
    
    public bool SlowReduceStamina(float value)
    {
        if (CanMove(value) == false)
        {
            return false;
        }
        Stat.Stamina -= 1/ value * Time.deltaTime;
        StaminaChanged?.Invoke();
        return true;
    }
    
    public bool RecoveryStamina(float value)
    {
        if (CanMove(value) == false)
        {
            return false;
        }
        Stat.Stamina += 1 / value * Time.deltaTime;
        StaminaChanged?.Invoke();
        return true;
    }

    private bool CanMove(float value)
    {
       return Stat.Stamina >= value;
    }
}
