using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Transform collectPoint;
    public string targetTag;
    public List<GameObject> collectedList = new List<GameObject>();
}
