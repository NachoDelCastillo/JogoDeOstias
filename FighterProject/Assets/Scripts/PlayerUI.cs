using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public int playerIndex;

    public TMP_Text name;
    public Image profile;
    public TMP_Text health;

    //public PlayerUI(CharacterData characterData, int i)
    //{
    //    playerIndex = i;

    //    name.text = characterData.name;
    //    profile.sprite = characterData.characterSprite;
    //    health.text = "0 %";
    //}

    public void InitializeThis(CharacterData characterData, int i)
    {
        playerIndex = i;

        name.text = characterData.name;
        profile.sprite = characterData.characterSprite;
        health.text = "0 %";
    }
}
