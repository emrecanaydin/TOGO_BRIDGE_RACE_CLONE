using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Material playerMaterial;
    public Transform collectPoint;
    public string targetTag;
    public string playerColor;
    public int currentLevel = 0;
    public bool IsInLadder;
    public List<GameObject> collectedList = new List<GameObject>();

    private void Start()
    {
        playerMaterial = transform.Find("Stick").GetComponent<Renderer>().material;
    }

}
