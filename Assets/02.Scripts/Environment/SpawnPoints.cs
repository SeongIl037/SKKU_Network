using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private static SpawnPoints _instance;
    public static SpawnPoints Instance => _instance;
    
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    public List<Transform> BearSpanwPoints = new List<Transform>();
    private void Awake()
    {
        _instance = this;
    }

    public Vector3 GetSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
    }

    public Vector3 GetBearSpanwPoint()
    {
        return BearSpanwPoints[Random.Range(0, BearSpanwPoints.Count)].position;
    }
}
