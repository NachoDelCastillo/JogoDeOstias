using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerCard : MonoBehaviour
{
    [HideInInspector] public int playerIndex;

    // Si es null, no hay ninguno seleccionado
    [SerializeField] public CharacterData characterSelected;
    // Si tiene un script referenciado, esque esta encima de esa carta, si es null, no esta encima de ninguna
    [SerializeField] CharacterCard hoveringThisCharacterCard;

    [HideInInspector] public CharacterSelectionManager characterSelectionManager;

    // Referencias
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text playerNumber;

    [SerializeField] Image characterImage;

    [SerializeField] Image playerNumberPanel;
    [SerializeField] Image panelLightColor;
    [SerializeField] Image panelDarkColor;

    [SerializeField] Transform movablePointer_Tr;
    [SerializeField] Image movablePointer_Img;
    [SerializeField] float pointerSpeed;

    [SerializeField] GameObject ready;

    [SerializeField] CharacterData defaultina;
    CharacterData mrBruh;

    // Cuando los valores ya estan asignados, 
    private void Start()
    {
        playerNumber.text = "P " + (playerIndex + 1);

        panelLightColor.color = characterSelectionManager.playerColors[playerIndex].light;
        //panelDarkColor.color = characterSelectionManager.playerColors[playerIndex].dark;
        playerNumberPanel.color = characterSelectionManager.playerColors[playerIndex].dark;
        movablePointer_Img.color = characterSelectionManager.playerColors[playerIndex].light;

        //if (playerIndex == 0)
        //    characterSelected = defaultina;
        //else if (playerIndex == 1)
        //    characterSelected = mrBruh;

        UpdateUI(characterSelected);
    }

    void UpdateUI(CharacterData characterData_)
    {
        if (characterData_ != null)
        {
            // Hay seleccionado algun personaje
            nameText.text = characterData_.characterName;

            characterImage.enabled = true;
            characterImage.sprite = characterData_.characterSprite;
            characterImage.transform.localPosition = characterData_.characterSpritePosition;
            characterImage.transform.localScale = characterData_.characterSpriteScale;
        }
        else
        {
            // No hay seleccionado ningun personaje
            nameText.text = "?";
            characterImage.enabled = false;
        }
    }

    private void Update()
    {
        Vector2 translateVector = new Vector2(input_hor, input_ver).normalized;
        movablePointer_Tr.Translate(translateVector * Time.deltaTime * pointerSpeed);
    }


    public void CharacterCardEnter(OnTriggerDelegation delegation)
    {
        CharacterCard characterCardTriggered = delegation.Other.GetComponent<CharacterCard>();
        // Si no es una carta de personaje, ignorarlo
        if (characterCardTriggered == null) return;

        characterCardTriggered.PointerEnter();

        hoveringThisCharacterCard = characterCardTriggered;

        UpdateUI(hoveringThisCharacterCard.characterData);
    }

    public void CharacterCardExit(OnTriggerDelegation delegation)
    {
        CharacterCard characterCardTriggered = delegation.Other.GetComponent<CharacterCard>();
        characterCardTriggered.PointerExit();

        hoveringThisCharacterCard = null;

        // Si este evento se ha llamado porque se ha elegido un personaje,
        // no quitar el personaje actual (que sera el seleccionado) de la carta
        if (characterSelected == null)
            UpdateUI(null);
    }

    #region Input

    float input_hor;
    float input_ver;
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        input_hor = context.ReadValue<Vector2>().x;
        input_ver = context.ReadValue<Vector2>().y;
    }

    public virtual void Select_Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (hoveringThisCharacterCard != null)
            {
                // Si el pointer esta encima de una carta de personaje, seleccionarla
                characterSelected = hoveringThisCharacterCard.characterData;

                // El UI de la carta no necesita ser actualizado ya que ya esta puesto los datos del personaje
                // de cuando el pointer entro en la carta de personaje
                // Despues de actualizar la carta, poner a false esto
                movablePointer_Tr.gameObject.SetActive(false);

                // Mostrar el ready
                ready.SetActive(true);
            }
        }
    }

    public virtual void Unselect_Input(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            characterSelected = null;

            movablePointer_Tr.gameObject.SetActive(true);

            ready.SetActive(false);
        }
    }

    #endregion
}
