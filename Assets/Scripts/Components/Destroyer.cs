using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroyer : MonoBehaviour
{
    public GameObject Target;
    public float SecondToDestroy=0;
    public UnityEvent BeforeDestroy, AfterDestroy;
    public void DestroyIt(){
        if(Target!=null){
            StartCoroutine(Destroying(Target));
        }else{
            StartCoroutine(Destroying(gameObject));
        }
    }
    IEnumerator Destroying(GameObject eliminateGO){
        BeforeDestroy?.Invoke();
        yield return new WaitForSeconds(SecondToDestroy);
        AfterDestroy?.Invoke();
        Destroy(eliminateGO);
    }
}
