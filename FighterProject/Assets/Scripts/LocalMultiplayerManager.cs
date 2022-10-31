using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{
    [SerializeField] bool spawnTestPlayer;


    PlayerInputManager playerInputManager;

    [Header("Parameters")]
    // Empty object that contains all the players as childs
    [SerializeField] Transform playerContainer;


    [SerializeField] CharacterData FirstPlayerData_Debug;
    [SerializeField] CharacterData SecondPlayerData_Debug;

    private void Awake()
    {
        //if (spawnTestPlayer)
        //{
        //    //GameManager.GetInstance().currentCharactersData = new CharacterData[4];

        //    //GameManager.GetInstance().currentCharactersData[0] = defaultinaDATA;
        //    //GameManager.GetInstance().currentCharactersData[1] = mrBruhDATA;


        //    GameObject characterPrefab = Array.Find(GameManager.GetInstance().characterPrefs, p => p.GetComponent<PlayerController_2D>().
        //        characterData.character == GameManager.Character.Defaultina);

        //    PlayerInput.Instantiate(characterPrefab, pairWithDevice: Keyboard.current);

        //    GameObject characterPrefab_2 = Array.Find(GameManager.GetInstance().characterPrefs, p => p.GetComponent<PlayerController_2D>().
        //        characterData.character == GameManager.Character.MrBruh);

        //    PlayerInput.Instantiate(characterPrefab_2, pairWithDevice: Gamepad.all[0]);

        //    //PlayerInput.Instantiate(characterPrefab);
        //    //PlayerInput.Instantiate(characterPrefab_2);

        //    return;
        //}


        if (GameManager.GetInstance().currentCharactersData[0] == null)
        {
            GameManager.GetInstance().currentCharactersData[0] = FirstPlayerData_Debug;
            GameManager.GetInstance().currentCharactersData[1] = SecondPlayerData_Debug;


            //InputDevice player1_input = Keyboard.current.device;
            //InputDevice player2_input = Gamepad.current;

            InputDevice player1_input = Gamepad.current;
            InputDevice player2_input = Keyboard.current.device;


            GameManager.GetInstance().currentDevices[0] = player1_input;

            if (player2_input != null)
                GameManager.GetInstance().currentDevices[1] = player2_input;
        }


        playerInputManager = GetComponent<PlayerInputManager>();

        CharacterData[] characters = GameManager.GetInstance().currentCharactersData;
        InputDevice[] devices = GameManager.GetInstance().currentDevices;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null) continue;

            GameObject characterPrefab = Array.Find(GameManager.GetInstance().characterPrefs, p => p.GetComponent<CharacterActions>().
               characterData.character == GameManager.GetInstance().currentCharactersData[i].character);

            PlayerController_2D newPlayer = PlayerInput.Instantiate(characterPrefab, pairWithDevice: devices[i]).GetComponent<PlayerController_2D>();

            newPlayer.playerIndex = i;
        }
    }

    // This function is called everytime a player joined
    //public void PlayerJoined(PlayerInput newplayer)
    //{
    //    // Get a reference
    //    PlayerController newPlayerController = newplayer.GetComponent<PlayerController>();

    //    // Set player index
    //    int playerIndex = playerInputManager.playerCount - 1;
    //    newPlayerController.playerIndex = playerIndex;
    //    newPlayerController.transform.SetParent(playerContainer);

    //    allPlayers.Add(newPlayerController);
    //}
}
