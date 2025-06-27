using System;
using UnityEngine;
using TMPro;
public class NicknameAbility : PlayerAbility
{
    public TextMeshProUGUI NicknameText;
    public SpriteRenderer Person;
    private void Start()
    {
        NicknameText.text = $"{_photonView.Owner.NickName}_{_photonView.Owner.ActorNumber}";

        if (_photonView.IsMine)
        {
            NicknameText.color = Color.green;
            Person.color = Color.green;
        }
        else
        {
            NicknameText.color = Color.red;
            Person.color = Color.red;
        }
    }
}
