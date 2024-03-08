using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    [SerializeField] List<int> _users;
    public List<int> Users {get=>_users; set=>_users=value;}
}
