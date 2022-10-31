using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New CharacterData", menuName = "CharacterData")]
public class CharacterData : ScriptableObject
{
    public GameManager.Character character;
    public string characterName;

    public float speed;
    public float jumpForce;
    public float mass;



    [Header("PlayerCard")]
    public Sprite characterSprite;
    public Vector2 characterSpritePosition;
    public Vector2 characterSpriteScale;
}
