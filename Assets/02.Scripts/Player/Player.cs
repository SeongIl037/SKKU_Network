using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat Stat;
    public Dictionary<Type, PlayerAbility> _abilitiesCache;
    
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
}
