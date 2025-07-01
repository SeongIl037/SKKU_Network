using System;
using TMPro;
using UnityEngine;

public class UI_RoomLog : MonoBehaviour
{
    public TextMeshProUGUI LogTextUI;

    private string _logMessage = "방에 입장했습니다.";

    private void Start()
    {

        RoomManager.Instance.OnPlayerEntered += PlayerEnterLog;
        RoomManager.Instance.OnPlayerExited += PlayerExitLog;
        RoomManager.Instance.OnPlayerDead += PlayerDeathLog;
        Refresh();
    }

    private void Refresh()
    {
        LogTextUI.text = _logMessage;
    }

    public void PlayerEnterLog(string playerName)
    {
        // 유니티 rich text
        _logMessage += $"\n<color=Green>{playerName}</color>님이 <color=Green>입장</color>하였습니다.";
        Refresh();
    }

    public void PlayerExitLog(string playerName)
    { 
        _logMessage += $"\n<color=Red>{playerName}</color>님이 <color=Red>퇴장</color>하였습니다.";
        Refresh();
    }

    public void PlayerDeathLog(string playerName, string attackerName)
    {
        _logMessage += $"\n<color=Red>{attackerName}</color>님이 <color=Green>{playerName}</color>님을 <color=Red>처치</color>하였습니다.";
        Refresh();
    }
}
