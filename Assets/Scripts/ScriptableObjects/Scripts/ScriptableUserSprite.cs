using System.Collections.Generic;
using UnityEngine;

// Define una clase para el ScriptableObject
[CreateAssetMenu(fileName = "ScriptableUsersSprite", menuName = "ScriptableObject/UsersSprite", order = 51)]
public class ScriptableUserSprite : ScriptableObject
{
    public List<Sprite> UserSprites;
}
