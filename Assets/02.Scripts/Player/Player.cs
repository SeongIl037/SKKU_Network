using System;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EPlayerState
{
    Live,
    Dead
    
}
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour, IDamaged
{
    public int Score = 0;
    private PhotonView _view;
    public PlayerStat Stat;
    public Dictionary<Type, PlayerAbility> _abilitiesCache = new Dictionary<Type, PlayerAbility>();
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    
    private EPlayerState _state;
    public EPlayerState State => _state;
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
        _impulseSource = GetComponent<CinemachineImpulseSource>();
                    
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
    public void Damaged(float damage, int actorNumber)
    {
        Stat.Health = Mathf.Max(0, Stat.Health - damage);
        HealthRefresh(Stat.Health);
        if (_view.IsMine)
        { 
            _impulseSource.GenerateImpulse();
        }
        Debug.Log($"남은 체력 {Stat.Health}");
        if(Stat.Health <= 0)
        {
            _state = EPlayerState.Dead;
            GetAbility<PlayerDeadAbility>().DeadAnimation();
            
            RoomManager.Instance.OnPlayerDeath(_view.Owner.ActorNumber, actorNumber);
            if (_view.IsMine)
            {
                MakeItem(Random.Range(1,4));
            }
        }
        else
        {
            GetAbility<PlayerShakingAbility>().Shake();
        }
    }
    
    [PunRPC]
    public void Heal(int value)
    {
        Stat.Health += value;
        Stat.Health = Mathf.Clamp(Stat.Health, 0, Stat.MaxHealth);
        HealthRefresh(Stat.Health);
    }
    private void MakeItem(int count)
    {
        for (int i = 0; i < count; i++)
        {
            int index = Random.Range(0, 10);
            
            if (index >= 8)
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Health, transform.position + new Vector3(0, 2f, 0));
            }
            else if (index >= 6)
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Stamina, transform.position + new Vector3(0, 2f, 0));
            }
            else
            {
                ItemObjectFactory.Instance.RequestCreate(EItemType.Score, transform.position + new Vector3(0, 2f, 0));
            }
            // player : 플레이어가 생겅하고, 플레이어    가 나가면 이 플레이어가 만든 오브젝트들은 모두 삭제된다.(PhotonNetwork.Instantiate/Destroy)
            // room : 룸이 생성하고, 룸이 없어지면 오브젝트들을 삭제한다. (PhotonNetwork.InstantiateRoomObject / Destroy)
            // ㄴ 룸이 생성한다. = 방장만 생성할 수 있다. 
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

    public void StaminaRefresh(float value)
    {
        Stat.Stamina += value;
        if (Stat.Stamina >= Stat.MaxStamina)
        {
            Stat.Stamina = Stat.MaxStamina;
        }
        StaminaChanged?.Invoke();
    }
    public void HealthRefresh(float value)
    {
        if (Stat.Stamina >= Stat.MaxStamina)
        {
            Stat.Stamina = Stat.MaxStamina;
        }
        Stat.Health = value;
        HealthChanged?.Invoke();
    }
    private bool CanMove(float value)
    {
       return Stat.Stamina >= value;
    }
    
}
