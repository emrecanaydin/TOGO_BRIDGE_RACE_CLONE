using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAI : MonoBehaviour
{

    public bool hasTarget;
    public List<GameObject> targets = new List<GameObject>();

    GameManager GM;
    CharacterController characterController;
    NavMeshAgent navMeshAgent;
    Animator characterAnimator;
    Vector3 targetPosition;
    public bool isGoingRope;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        DetectTargetsAndAddList(GM.firstLevelCollectableParent);
    }

    void Update()
    {
        ChooseTarget();
    }

    void DetectTargetsAndAddList(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.tag == characterController.targetTag)
            {
                targets.Add(child.gameObject);
            }
        }
    }

    void ChooseTarget()
    {
        if (targets.Count > 0 && !hasTarget)
        {

            bool goToRope = DetectGoingRope();

            if (goToRope)
            {
                Transform target = GM.laddersList[0].transform.Find("Middle").GetChild(characterController.collectedList.Count - 2);
                targetPosition = target.position;
                isGoingRope = true;
            } else
            {
                Debug.Log(2);
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GM.overlapSphereRadius);
                List<Vector3> targetPositions = new List<Vector3>();
                foreach (var collider in hitColliders)
                {
                    if (collider.tag == characterController.targetTag)
                    {
                        targetPositions.Add(collider.transform.position);
                    }
                }
                if (targetPositions.Count > 0)
                {
                    targetPosition = targetPositions[0];
                }
                else
                {
                    int random = Random.Range(0, targets.Count);
                    targetPosition = targets[random].transform.position;
                }
            }
            hasTarget = true;
            navMeshAgent.SetDestination(targetPosition);
            characterAnimator.SetFloat("MoveSpeed", 1);
        } else if (isGoingRope)
        {
            bool isReached = IsCharacterReachedRope();
            if (isReached)
            {
                isGoingRope = false;
            }
        }
        else if (targets.Count == 0)
        {
            characterAnimator.SetFloat("MoveSpeed", 0);
        }
    }

    public bool IsCharacterReachedRope()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool DetectGoingRope()
    {
        int random = Random.Range(0, 2);
        return characterController.collectedList.Count >= characterController.maximumLoad && random == 0;
    }

}
