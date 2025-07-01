using System.Collections;
using UnityEngine;

public class PlayerShakingAbility : PlayerAbility
{
    // 흔들다? : 무엇을 어떤 힘으로 몇 초동안 흔들 것인가
    public Transform Target;
    public float Duration;
    public float Strength;

    public void Shake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake_Coroutine());
    }

    private IEnumerator Shake_Coroutine()
    {
        float elapsedTime = 0;
        
        Vector3 startPos = Target.localPosition;
        
        while (elapsedTime < Duration)
        {
            elapsedTime += Time.deltaTime;
            Vector3 randomPos = Random.insideUnitSphere.normalized * Strength;
            randomPos.y = startPos.y;
            Target.localPosition = randomPos;
            yield return null;
        }
        
        Target.localPosition = startPos;
    }
}
