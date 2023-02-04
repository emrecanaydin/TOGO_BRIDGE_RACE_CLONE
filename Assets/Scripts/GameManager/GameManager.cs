using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Player Settings")]
    public float playerMoveSpeed;
    public float playerTurnSpeed;


    [Header("AI Settings")]
    public Transform firstLevelCollectableParent;
    public Transform secondLevelCollectableParent;
    public float overlapSphereRadius;


}
