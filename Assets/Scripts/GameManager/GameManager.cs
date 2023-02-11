using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject blueCollectable;
    public GameObject greenCollectable;
    public GameObject orangeCollectable;

    [Header("General Settings")]
    public Material droppedCollectableMaterial;

    [Header("Player Settings")]
    public float playerMoveSpeed;
    public float playerTurnSpeed;

    [Header("Ladders")]
    public List<GameObject> laddersList = new List<GameObject>();

    [Header("AI Settings")]
    public Transform firstLevelCollectableParent;
    public Transform secondLevelCollectableParent;
    public float overlapSphereRadius;


}
