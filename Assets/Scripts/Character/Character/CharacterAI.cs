using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{

    public bool hasTarget;
    public float radius;
    public List<GameObject> targets = new List<GameObject>();

    GameManager GM;
    CharacterController characterController;
    NavMeshAgent navMeshAgent;
    Animator characterAnimator;
    Vector3 targetPosition;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        DetectTargetsAndAddList(GM.firstLevelCollectableParent);
    }

    void DetectTargetsAndAddList(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if(child.tag == characterController.targetTag)
            {
                targets.Add(child.gameObject);
            }
        }
    }


    void Update()
    {
        if(targets.Count > 0 && !hasTarget)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            List <Vector3> targetPositions = new List<Vector3>();
            foreach (var collider in hitColliders)
            {
                if(collider.tag == characterController.targetTag)
                {
                    targetPositions.Add(collider.transform.position);
                }
            }
            if(targetPositions.Count > 0)
            {
                targetPosition = targetPositions[0];
            } else
            {
                int random = Random.Range(0, targets.Count);
                targetPosition = targets[random].transform.position;
            }

            hasTarget = true;
            navMeshAgent.SetDestination(targetPosition);
            characterAnimator.SetFloat("MoveSpeed", 1);
        } else if(targets.Count == 0)
        {
            characterAnimator.SetFloat("MoveSpeed", 0);
        }
    }

}
