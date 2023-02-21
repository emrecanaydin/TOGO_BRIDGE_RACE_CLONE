using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CharacterAI : MonoBehaviour
{

    public bool hasTarget;
    public bool isGoingRope;
    public GameObject alreadySelectedRope;
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
        DetectTargetsAndAddList(GM.collectableParentList[0]);
    }

    void Update()
    {
        if (!GM.isGameOver)
        {
            ChooseTarget();
        } else
        {
            characterAnimator.SetFloat("MoveSpeed", 0);
        }
    }

    public void DetectTargetsAndAddList(Transform parent)
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
                Transform target;
                GameObject choosedRope = ChooseRope();
                if(characterController.currentLevel == 2)
                {
                    target = choosedRope.transform;
                }
                else
                {
                    target = choosedRope.transform.Find("Middle").Find("FinalStep");
                }
                targetPosition = target.position;
                isGoingRope = true;
            } else
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, GM.overlapSphereRadius);
                var orderedByProximity = hitColliders.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();

                List<Vector3> targetPositions = new List<Vector3>();
                foreach (var collider in orderedByProximity)
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
                hasTarget = false;
                isGoingRope = false;
            }
        }
        else if (targets.Count == 0)
        {
            characterAnimator.SetFloat("MoveSpeed", 0);
        }
    }

    bool IsCharacterReachedRope()
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

    GameObject ChooseRope()
    {

        int random = Random.Range(0, GM.firstLevelLaddersList.Count);
        GameObject selectedRope = GM.firstLevelLaddersList[random];

        if (alreadySelectedRope)
        {
            return alreadySelectedRope;
        }

        if(characterController.currentLevel == 1)
        {
            int randomSecondLevel = Random.Range(0, GM.secondLevelLaddersList.Count);
            selectedRope = GM.secondLevelLaddersList[randomSecondLevel];
        } else if(characterController.currentLevel == 2)
        {
            selectedRope = GameObject.Find("FinalPoint");
        }

        alreadySelectedRope = selectedRope;

        return selectedRope;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5);
    }

}