using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VibrateImage : MonoBehaviour
{
    [SerializeField] float totalDuration = 0.5f;
    [SerializeField] float amplitude = 10f;
    public UnityEvent OnStopped;

    public void Vibrar(){
        StartCoroutine(StartVibration());
    }

    IEnumerator StartVibration()
    {
        // Inicializar la rotación inicial y el temporizador
        float startTime = Time.time;
        Quaternion startRotation = transform.rotation;

        // Realizar la vibración
        while (Time.time - startTime < totalDuration)
        {
            // Calcular el progreso de la vibración
            float progress = (Time.time - startTime) / totalDuration;

            // Calcular el ángulo de rotación basado en una función sinusoidal para una vibración suave
            float angle = Mathf.Sin(progress * Mathf.PI * 2) * amplitude;

            // Aplicar rotación a la imagen
            transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);

            // Esperar al siguiente frame
            yield return null;
        }

        // Restaurar la rotación inicial al final de la vibración
        transform.rotation = startRotation;
        OnStopped?.Invoke();
    }
}
