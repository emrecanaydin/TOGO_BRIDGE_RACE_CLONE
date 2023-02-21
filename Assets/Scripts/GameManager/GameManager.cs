using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    [Header("Prefabs")]
    public GameObject blueCollectable;
    public GameObject greenCollectable;
    public GameObject orangeCollectable;

    [Header("General Settings")]
    public Material droppedCollectableMaterial;
    public CinemachineVirtualCamera vcam;
    public bool isGameOver;

    [Header("Player Settings")]
    public float playerMoveSpeed;
    public float playerTurnSpeed;

    [Header("Ladders")]
    public List<GameObject> firstLevelLaddersList = new List<GameObject>();
    public List<GameObject> secondLevelLaddersList = new List<GameObject>();

    [Header("Grounds")]
    public List<GameObject> groundList = new List<GameObject>();

    [Header("AI Settings")]
    public List<Transform> collectableParentList = new List<Transform>();
    public float overlapSphereRadius;

    private void Start()
    {
        DisableRopeMeshRenderer();
    }

    private void Update()
    {
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (isGameOver)
        {
            if (GameObject.FindGameObjectsWithTag("WinnerEnemy").Length > 0)
            {
                GameObject winner = GameObject.FindGameObjectWithTag("WinnerEnemy");
                List<GameObject> collectedList = winner.GetComponent<CharacterController>().collectedList;
                int collectedCount = collectedList.Count;
                for (int i = 0; i < collectedCount; i++)
                {
                    GameObject currentGameObject = winner.GetComponent<CharacterController>().collectedList[i];
                    Destroy(currentGameObject.gameObject);
                }
                collectedList.Clear();
                vcam.Follow = winner.transform;
                Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            } else
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    Destroy(enemy);
                }
            }
        }
    }

    void DisableRopeMeshRenderer()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("InvisiblePlane"))
        {
            item.GetComponent<MeshRenderer>().enabled = false;
        }
    }

}
