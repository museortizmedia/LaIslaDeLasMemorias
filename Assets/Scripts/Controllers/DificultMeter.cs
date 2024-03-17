using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class DificultMeter : MonoBehaviour
{
    [SerializeField] Sprite empty, full;
    public int Levels = 3;
    int _dificult;
    public int Dificult{
        get => _dificult;
        set {
            _dificult = value;
            if(prev!=null){prev.sprite = empty;}
            prev = transform.GetChild(value).GetComponent<Image>();
            prev.sprite = full;
        }
    }
    Image prev;
}

[CustomEditor(typeof(DificultMeter))]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Llamar al inspector predeterminado
        DrawDefaultInspector();

        // Obtener una referencia al componente que se está editando
        DificultMeter myComponent = (DificultMeter)target;

        myComponent.Dificult = EditorGUILayout.IntSlider("Dificult", myComponent.Dificult, 0, myComponent.Levels-1);
    }
}
