using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


// Define una clase para el ScriptableObject
[CreateAssetMenu(fileName = "ScriptableActivitiesData", menuName = "ScriptableObject/ActivitieData", order = 52)]
public class ScriptableActivitiesData : ScriptableObject
{
    [Serializable]
    public struct ActivityData{
        public string Name;
        public Sprite Imagen;
        public string Text;
        public AudioClip Sound;
        public int BadgeInt;
        public bool IsAbsurd;
	}
    public string ActivityName;
    public List<ActivityData> Data;
    public List<ActivityData> AbsurdData;
    [Range(0,100)]
    public float AbsurdPercent = 0;
    public float AbsurdWait = 0;
}
