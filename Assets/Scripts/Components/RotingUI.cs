using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotingUI : MonoBehaviour
{
    public float rotationSpeed = 50f;
    bool _isActive = true;
    void Update()
    {
        if(_isActive){transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);}
    }
    public void StopRoating(){
        _isActive = false;
        transform.rotation = Quaternion.identity;
    }
}
