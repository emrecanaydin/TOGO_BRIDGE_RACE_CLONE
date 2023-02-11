using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterCollision : MonoBehaviour
{

    CharacterController characterController;
    GameManager GM;
    CharacterAI characterAI;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAI = GetComponent<CharacterAI>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "CollectableGreen":
                TriggerWithCollectable(other.gameObject);
                break;
            case "CollectableOrange":
                TriggerWithCollectable(other.gameObject);
                break;
            case "InvisibleLadderStart":
                TriggerWithLadderStart(other.gameObject);
                break;
            case "Step":
                TriggerWithStep(other.gameObject);
                break;
            default:
                break;
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
        if(other.tag == characterController.targetTag)
        {
            other.tag = "Untagged";
            other.GetComponent<BoxCollider>().enabled = false;
            other.transform.parent = characterController.collectPoint;
            Vector3 position = new Vector3(0, characterController.collectedList.Count * .25f, 0);
            other.transform.DOLocalMove(position, .35f).OnComplete(() => Destroy(other.transform.Find("Trail").gameObject, 0f));
            other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            characterController.collectedList.Add(other.gameObject);
            characterAI.targets.Remove(other.gameObject);
            characterAI.hasTarget = false;
        }
    }

    void TriggerWithLadderStart(GameObject other)
    {
        Transform invisibleObstacle = other.transform.parent.Find("InvisibleObstacle");
        invisibleObstacle.GetComponent<BoxCollider>().isTrigger = true;
    }

    void TriggerWithStep(GameObject other)
    {
        other.GetComponent<Renderer>().material = characterController.characterMaterial;
        other.GetComponent<MeshRenderer>().enabled = true;
        other.tag = "PassiveStep";
        characterController.collectedList.RemoveAt(characterController.collectedList.Count - 1);
        Destroy(characterController.collectPoint.GetChild(characterController.collectedList.Count).gameObject, 0f);
    }

    IEnumerator CollisionWithPlayer(GameObject player)
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        int collectedCount = characterController.collectedList.Count;
        int playerCollectedCount = player.gameObject.GetComponent<PlayerController>().collectedList.Count;
        for (int i = playerCollectedCount - 1; i >= 0; i--)
        {
            GameObject currentGameObject = player.gameObject.GetComponent<PlayerController>().collectedList[i];
            currentGameObject.transform.DOJump(player.transform.position + new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-3f, 3f)), 3, 1, 1f);
            currentGameObject.transform.DOLocalRotate(new Vector3(0, 0, 0), .5f);
            currentGameObject.tag = "DroppedCollectable";
            currentGameObject.GetComponent<Renderer>().material = GM.droppedCollectableMaterial;
            currentGameObject.transform.parent = GameObject.Find("FirstLevelCollectable").transform;
        }
        player.gameObject.GetComponent<PlayerController>().collectedList.Clear();
        yield return new WaitForSeconds(1f);
        player.GetComponent<Rigidbody>().isKinematic = false;
        foreach (var currentGameObject in GameObject.FindGameObjectsWithTag("DroppedCollectable"))
        {
            currentGameObject.transform.Find("Trail").gameObject.SetActive(true);
            currentGameObject.GetComponent<BoxCollider>().enabled = true;
            currentGameObject.tag = "CollectableBlue";
        }
    }

}