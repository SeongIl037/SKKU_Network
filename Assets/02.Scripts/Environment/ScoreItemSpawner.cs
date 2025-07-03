using Photon.Pun;
using UnityEngine;

public class ScoreItemSpawner : MonoBehaviour
{
    public float Interval; // 몇초마다 생성할 것인가
    private float _timer = 0;
    public float Range;    // 랜덤한 범위

    
    
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        _timer += Time.deltaTime;

        if (_timer >= Interval)
        {
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * Range;
            randomPosition.y = 20f;
            
            ItemObjectFactory.Instance.RequestCreate(EItemType.Score, randomPosition);
            _timer = 0;
        }
    }
}
