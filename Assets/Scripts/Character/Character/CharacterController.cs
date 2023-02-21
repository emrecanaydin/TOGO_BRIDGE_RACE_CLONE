using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int maximumLoad;
    public Material characterMaterial;
    public Transform collectPoint;
    public string targetTag;
    public string characterColor;
    public int currentLevel = 0;
    public List<GameObject> collectedList = new List<GameObject>();

    private void Start()
    {
        characterMaterial = transform.Find("Stick").GetComponent<Renderer>().material;
    }

}
