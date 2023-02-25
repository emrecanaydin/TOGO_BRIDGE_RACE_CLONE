using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterCollision : MonoBehaviour
{

    CharacterController characterController;
    GameManager GM;
    UIManager UI;
    CharacterAI characterAI;
    Rigidbody characterRB;
    Animator characterAnimator;

    private void Start()
    {
        characterAnimator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        characterAI = GetComponent<CharacterAI>();
        characterRB = GetComponent<Rigidbody>();
        UI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "FinalPoint":
                TriggerWithFinalPoint();
                break;
            default:
                if (other.tag == characterController.targetTag)
                {
                    TriggerWithCollectable(other.gameObject);
                }
                if (other.tag.StartsWith("Step") && other.tag != "Step-" + characterController.characterColor)
                {
                    TriggerWithStep(other.gameObject);
                }
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "LadderParent")
        {
            characterController.IsInLadder = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LadderParent")
        {
            characterController.IsInLadder = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                StartCoroutine(CollisionWithPlayer(collision.gameObject));
                break;
            default:
                break;
        }
    }

    void TriggerWithCollectable(GameObject other)
    {
        if (other.tag == characterController.targetTag)
        {
            other.tag = "Untagged";
            other.GetComponent<BoxCollider>().enabled = false;
            other.transform.parent = characterController.collectPoint;
            Vector3 position = new Vector3(0, characterController.collectedList.Count * .25f, 0);
            other.transform.DOLocalMove(position, .35f).OnComplete(() => other.transform.Find("Trail").gameObject.SetActive(false));
            other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            other.GetComponent<Renderer>().material = characterController.characterMaterial;
            characterController.collectedList.Add(other.gameObject);
            characterAI.targets.Remove(other.gameObject);
            characterAI.hasTarget = false;
            GM.groundList[characterController.currentLevel].GetComponent<GroundController>().GenerateCube(characterController.targetTag);
        }
    }

    void TriggerWithStep(GameObject other)
    {
        if(characterController.collectedList.Count > 0)
        {
            other.GetComponent<Renderer>().material = characterController.characterMaterial;
            other.GetComponent<MeshRenderer>().enabled = true;
            other.tag = "Step-" + characterController.characterColor;
            characterController.collectedList.RemoveAt(characterController.collectedList.Count - 1);
            Destroy(characterController.collectPoint.GetChild(characterController.collectedList.Count).gameObject, 0f);
            if (characterController.collectedList.Count == 0)
            {
                characterAI.hasTarget = false;
            }
            if(other.name == "FinalStep")
            {
                characterAI.targets.Clear();
                characterAI.hasTarget = false;
                characterAI.alreadySelectedRope = null;
                characterController.currentLevel = characterController.currentLevel + 1;
                if(characterController.currentLevel <= 2)
                {
                    characterAI.DetectTargetsAndAddList(GM.collectableParentList[characterController.currentLevel]);
                }
            }
        }
    }
    
    void TriggerWithFinalPoint()
    {
        GM.isGameOver = true;
        gameObject.tag = "WinnerEnemy";
        UI.gamePlayPanel.SetActive(false);
        UI.lostPanel.SetActive(true);
        characterAI.hasTarget = false;
        characterAnimator.SetBool("IsWinner", true);
    }

    IEnumerator CollisionWithPlayer(GameObject player)
    {

        if (!characterController.IsInLadder)
        {
            PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
            Rigidbody playerRB = player.GetComponent<Rigidbody>();
            int collectedCount = characterController.collectedList.Count;
            int playerCollectedCount = playerController.collectedList.Count;
            if(collectedCount > playerCollectedCount)
            {
                playerRB.isKinematic = true;
                for (int i = playerCollectedCount - 1; i >= 0; i--)
                {
                    GameObject currentGameObject = playerController.collectedList[i];
                    currentGameObject.transform.DOJump(player.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), 3, 1, 1f);
                    currentGameObject.tag = "DroppedCollectable";
                    currentGameObject.GetComponent<Renderer>().material = GM.droppedCollectableMaterial;
                    currentGameObject.transform.parent = GM.groundList[playerController.currentLevel].transform;
                }
                playerController.collectedList.Clear();
                yield return new WaitForSeconds(1f);
                playerRB.isKinematic = false;
                foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("DroppedCollectable"))
                {
                    currentGameObject.transform.Find("Trail").gameObject.SetActive(true);
                    currentGameObject.GetComponent<BoxCollider>().enabled = true;
                    currentGameObject.tag = playerController.targetTag;
                }
            } else
            {
                for (int i = collectedCount - 1; i >= 0; i--)
                {
                    GameObject currentGameObject = characterController.collectedList[i];
                    currentGameObject.transform.DOJump(player.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), 3, 1, 1f);
                    currentGameObject.tag = "DroppedCollectable";
                    currentGameObject.GetComponent<Renderer>().material = GM.droppedCollectableMaterial;
                    currentGameObject.transform.parent = GM.groundList[characterController.currentLevel].transform;
                }
                characterController.collectedList.Clear();
                yield return new WaitForSeconds(1f);
                characterRB.isKinematic = false;
                foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("DroppedCollectable"))
                {
                    currentGameObject.transform.Find("Trail").gameObject.SetActive(true);
                    currentGameObject.GetComponent<BoxCollider>().enabled = true;
                    currentGameObject.tag = characterController.targetTag;
                }
            }
        }
    }

}