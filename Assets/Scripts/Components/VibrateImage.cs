using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VibrateImage : MonoBehaviour
{
    public bool IsVibrating;
    [SerializeField] float totalDuration = 0.5f;
    [SerializeField] float amplitude = 10f;
    public UnityEvent OnStopped;

    [SerializeField] Transform target;

    private void Start() {
        if(target==null){
            target = gameObject.transform;
        }
    }

    public void Vibrar(){
        StartCoroutine(StartVibration());
    }

    IEnumerator StartVibration()
    {
        IsVibrating=true;
        // Inicializar la rotación inicial y el temporizador
        float startTime = Time.time;
        Quaternion startRotation = target.rotation;

        // Realizar la vibración
        while (Time.time - startTime < totalDuration)
        {
            // Calcular el progreso de la vibración
            float progress = (Time.time - startTime) / totalDuration;

            // Calcular el ángulo de rotación basado en una función sinusoidal para una vibración suave
            float angle = Mathf.Sin(progress * Mathf.PI * 2) * amplitude;

            // Aplicar rotación a la imagen
            target.rotation = startRotation * Quaternion.Euler(0, 0, angle);

            // Esperar al siguiente frame
            yield return null;
        }

        // Restaurar la rotación inicial al final de la vibración
        target.rotation = startRotation;
        OnStopped?.Invoke();
        IsVibrating=false;
    }
}
