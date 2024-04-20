using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destroyer : MonoBehaviour
{
    public GameObject Target;
    public Component Component;
    public float SecondToDestroy=0;
    public UnityEvent BeforeDestroy, AfterDestroy;
    public void DestroyIt(){
        if(Target!=null){
            StartCoroutine(nameof(Destroying));
        }else{
            StartCoroutine(nameof(Destroying));
        }
    }
    IEnumerator Destroying(){
        BeforeDestroy?.Invoke();
        yield return new WaitForSeconds(SecondToDestroy);
        AfterDestroy?.Invoke();
        if(Target!=null){Destroy(Target);} else
        if(Component!=null){Destroy(Component);} else
        Destroy(gameObject);
    }
}
