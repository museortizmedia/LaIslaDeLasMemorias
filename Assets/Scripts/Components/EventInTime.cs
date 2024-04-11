using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventInTime : MonoBehaviour
{
    public int Seconds;
    public UnityEvent OnWaitForTimer, OnFinishTimer;
    public UnityEvent<int> OnEachTimeCount;

    IEnumerator TimerCorrutine(){
        OnWaitForTimer?.Invoke();
        int _currentTime = 0;
        while(_currentTime < Seconds){
            yield return new WaitForSeconds(1);
            _currentTime++;
            OnEachTimeCount?.Invoke(_currentTime);
        }
        OnFinishTimer?.Invoke();
    }
    public void ComenzarTimer(){
        StartCoroutine(TimerCorrutine());
    }
}
