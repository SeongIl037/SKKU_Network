using System;
using System.Collections.Generic;
using Photon.Pun;
using ExitGames.Client.Photon;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance { get; private set; }

    private Dictionary<string, int> _scores = new Dictionary<string, int>();
    public Dictionary<string, int>Scores => _scores;
    
    public event Action OnScoreChanged;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnJoinedRoom()
    {
        Refresh();
    }

    public void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            // 가면 내 점수가 0이다. 라는 내용으로 커스텀 프로퍼티를 초기화해준다.
            Refresh();
        }
        // 방에 들어가면 내 점수가 0이다. 라는 내용으로 커스텀 프로퍼티를 초기화해준다.
    }

    private int _score = 0;
    public int Score => _score;
    
    private int _killCount = 0;
    public int KillCount => _killCount;

    private int _killScore = 5000;

    public void Refresh()
    {
        // 최초 등록
        Hashtable hash = new Hashtable();
        hash.Add("Score", _score);
        // 딕셔너리가 들어가야한다.
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void ReduceScore()
    {
        int value = _score / 2 ;
        _score -= value;
        Refresh();
    }
    public void AddScore(int addScore) 
    { 
        _score += addScore;
        // 프로퍼티 밸류 수정
        Refresh();
    }
    
    public void AddKillCount()
    {
        _killCount += 1;
        AddScore(_killScore);
    }
    
    
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player newPlayer, Hashtable changedProps)
    {
        //Debug.Log($"Player {newPlayer.NickName}_{newPlayer.ActorNumber}의 점수  {changedProps["Score"]}");
        
        var roomplayers = PhotonNetwork.PlayerList;

        foreach (Photon.Realtime.Player player in roomplayers)
        {
            if (player.CustomProperties.ContainsKey("Score"))
            {
             
                _scores[$"{player.NickName}_{player.ActorNumber}"] = (int)player.CustomProperties["Score"];

            }
        }
        OnScoreChanged?.Invoke();
    }
}

