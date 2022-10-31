using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI_Manager : MonoBehaviour
{
    static GameplayUI_Manager instance;
    static public GameplayUI_Manager GetInstance() { return instance; }

    private void Awake()
    { instance = this; }

    [SerializeField] Transform playerCard_Container;

    PlayerUI[] allPlayerCards;

    [SerializeField] GameObject playerUI_Prefab;

    private void Start()
    {
        allPlayerCards = new PlayerUI[4];

        CharacterData[] characters = GameManager.GetInstance().currentCharactersData;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) continue;

            PlayerUI newPlayerUI = Instantiate(playerUI_Prefab, playerCard_Container).GetComponent<PlayerUI>();

            allPlayerCards[i] = newPlayerUI;

            newPlayerUI.InitializeThis(characters[i], i);
        }
    }

    public void UpdatePlayerDamage(int playerIndex, float newHealth)
    {
        if (allPlayerCards[playerIndex] != null)
            allPlayerCards[playerIndex].health.text = newHealth + " %";

        // Cambiarlo de color
    }
}
