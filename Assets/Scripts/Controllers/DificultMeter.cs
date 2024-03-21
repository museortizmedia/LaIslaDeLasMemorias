using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;
using TMPro;

public class DificultMeter : MonoBehaviour
{
    [SerializeField] Sprite empty, full;
    [SerializeField] TextMeshProUGUI label;
    public int Levels = 3;
    [Range(0,2)][SerializeField]  int _dificult = 0;
    public int Dificult{
        get => _dificult;
        set {

            if (prev.Length == 0)
            {
                prev = gameObject.GetComponentsInChildren<Image>(true);
            }

            _dificult = value;
            label.text = _dificult == 0 ? "Fácil" : _dificult == 1 ? "Normal" : "Difícil";

            for (int i = 0; i < prev.Length; i++)
            {
                prev[i].sprite = i<=_dificult? full:empty;
            }
        }
    }
    Image[] prev = new Image[]{};
}
/*
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
*/