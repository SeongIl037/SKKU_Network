using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour, IDamaged
{
    private PhotonView _view;
    public PlayerStat Stat;
    public Dictionary<Type, PlayerAbility> _abilitiesCache = new Dictionary<Type, PlayerAbility>();

    public CinemachineImpulseSource Impulse;
    public bool IsDead => Stat.Health <= 0;
    public event Action StaminaChanged;
    public event Action HealthChanged;
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
    
    // 데미지 관련
    [PunRPC]
    public void Damaged(float damage)
    {
        Stat.Health = Mathf.Max(0, Stat.Health - damage);
        HealthRefresh(Stat.Health);
        Impulse.GenerateImpulse();
        
        Debug.Log($"남은 체력 {Stat.Health}");
        if (IsDead)
        {
            GetAbility<PlayerDeadAbility>().DeadAnimation();
        }
    }
    
    // 스태미너 관련
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
    
    public void RecoveryStamina(float value)
    {

        Stat.Stamina += 1 / value * Time.deltaTime;
        StaminaChanged?.Invoke();
    }

    public void HealthRefresh(float value)
    {
        Stat.Health = value;
        HealthChanged?.Invoke();
    }
    private bool CanMove(float value)
    {
       return Stat.Stamina >= value;
    }
    
}
