using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollision : MonoBehaviour
{

    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "CollectableBlue":
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

    void TriggerWithStep(GameObject other)
    {
        other.GetComponent<Renderer>().material = playerController.playerMaterial;
        other.GetComponent<MeshRenderer>().enabled = true;
        other.tag = "PassiveStep";
        playerController.collectedList.RemoveAt(playerController.collectedList.Count - 1);
        Destroy(playerController.collectPoint.GetChild(playerController.collectedList.Count).gameObject, 0f);
    }

    void TriggerWithCollectable(GameObject other)
    {
        other.tag = "Untagged";
        other.GetComponent<BoxCollider>().enabled = false;
        other.transform.parent = playerController.collectPoint;
        Vector3 position = new Vector3(0, playerController.collectedList.Count * .25f, 0);
        other.transform.DOLocalJump(position, 1.5f, 1, .45f).OnComplete(() => other.transform.Find("Trail").gameObject.SetActive(false) );
        other.transform.localRotation = Quaternion.Euler(0, 0, 0);
        other.GetComponent<Renderer>().material = playerController.playerMaterial;
        playerController.collectedList.Add(other.gameObject);
    }

    void TriggerWithLadderStart(GameObject other)
    {
        if (playerController.collectedList.Count > 0)
        {
            Transform invisibleObstacle = other.transform.parent.Find("InvisibleObstacle");
            invisibleObstacle.GetComponent<BoxCollider>().isTrigger = false;
            if (playerController.collectedList.Count >= 15)
            {
                invisibleObstacle.gameObject.SetActive(false);
            }
            else
            {
                GameObject maxStepObject = GameObject.FindGameObjectsWithTag("Step")[playerController.collectedList.Count - 1];
                Vector3 obstaclePosition = maxStepObject.transform.position;
                obstaclePosition.y = obstaclePosition.y * 1.5f;
                invisibleObstacle.transform.position = obstaclePosition;
            }
        }
    }

}
