using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public GameObject objectPrefab; // Referencia al prefab del objeto
    private GameObject instantiatedObject; // Referencia al objeto instanciado

    // Método para instanciar el objeto
    public void InstantiateObject()
    {
        if (objectPrefab != null)
        {
            if (instantiatedObject == null)
            {
                instantiatedObject = Instantiate(objectPrefab, transform.position, transform.rotation, transform.parent);
                Debug.Log("Objeto instanciado.");
            }
            else
            {
                Debug.LogWarning("Ya hay una instancia del objeto.");
            }
        }
        else
        {
            Debug.LogError("No se ha asignado ningún prefab.");
        }
    }

    // Método para eliminar el objeto instanciado
    public void DestroyObject()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
            Debug.Log("Objeto destruido.");
        }
        else
        {
            Debug.LogWarning("No hay ninguna instancia del objeto para eliminar.");
        }
    }
}
