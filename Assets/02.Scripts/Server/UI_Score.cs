using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class UI_Score : MonoBehaviour
{
    public List<UI_ScoreSlot> Slots;
    public UI_ScoreSlot MySlot;
    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += Refresh;
    }

    private void Refresh()
    {
        // 1~4
        Dictionary<string, int> playerScores = ScoreManager.Instance.Scores;
        var sortedScores = playerScores.ToList().OrderByDescending(x => x.Value).ToList();

        for (int i = 0; i < Slots.Count; i++)
        {
            if (i < sortedScores.Count())
            {
                Slots[i].gameObject.SetActive(true);
                Slots[i].Set($"{i+1}",sortedScores[i].Key,sortedScores[i].Value);
            }
            else
            {
                Slots[i].gameObject.SetActive(false);   
            }
        }
        
        string myNickname = PhotonNetwork.NickName + "_" + PhotonNetwork.LocalPlayer.ActorNumber;
        int index = sortedScores.FindIndex(x => x.Key == myNickname);
        if (index < 0)
        {
            return;
        }
        MySlot.Set($"{index+1}", sortedScores[index].Key, sortedScores[index].Value);
        //
        // MySlot.Set(sortedScores.Count.ToString(), playerScores.Count.ToString(), playerScores.Count);
    }
}
