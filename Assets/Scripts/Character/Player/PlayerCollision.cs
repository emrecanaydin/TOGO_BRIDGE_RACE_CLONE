using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCollision : MonoBehaviour
{

    PlayerController playerController;
    GameManager GM;
    UIManager UI;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        UI = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "InvisibleLadderStart":
                TriggerWithLadderStart(other.gameObject);
                break;
            case "Booster":
                StartCoroutine(TriggerWithBooster(other.gameObject));
                break;
            case "Obstacle":
                StartCoroutine(TriggerWithObstacle(other.gameObject));
                break;
            case "FinalPoint":
                TriggerWithFinalPoint();
                break;
            default:
                if(other.tag == playerController.targetTag)
                {
                    TriggerWithCollectable(other.gameObject);
                }
                if(other.tag.StartsWith("Step") && other.tag != "Step-" + playerController.playerColor)
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
            playerController.IsInLadder = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "LadderParent")
        {
            playerController.IsInLadder = false;
        }
    }

    IEnumerator TriggerWithObstacle(GameObject other)
    {
        Destroy(other.gameObject, 0f);
        int count = other.GetComponent<ObstacleController>().count;
        int collectedCount = playerController.collectedList.Count;
        for (int i = collectedCount - 1; i >= collectedCount - count; i--)
        {
            if(playerController.collectedList.Count > 0)
            {
                yield return new WaitForSeconds(.1f);
                GameObject currentGameObject = playerController.collectedList[i];
                Destroy(currentGameObject.gameObject);
                playerController.collectedList.RemoveAt(i);
            } else
            {
                break;
            }
        }
    }

    IEnumerator TriggerWithBooster(GameObject other)
    {
        int count = other.GetComponent<BoosterController>().count;
        Destroy(other.gameObject, 0f);
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(.1f);
            GameObject clonedCollectable = Instantiate(GM.blueCollectable, transform);
            TriggerWithCollectable(clonedCollectable);
        }
    }

    void TriggerWithStep(GameObject other)
    {
        other.GetComponent<Renderer>().material = playerController.playerMaterial;
        other.GetComponent<MeshRenderer>().enabled = true;
        other.tag = "Step-" + playerController.playerColor;
        playerController.collectedList.RemoveAt(playerController.collectedList.Count - 1);
        Destroy(playerController.collectPoint.GetChild(playerController.collectedList.Count).gameObject, 0f);
        if(other.name == "FinalStep")
        {
            playerController.currentLevel = playerController.currentLevel + 1;
        }
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
        GM.groundList[playerController.currentLevel].GetComponent<GroundController>().GenerateCube(playerController.targetTag);
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
                int count = 0;
                foreach (Transform child in other.transform.parent)
                {
                    if(child.gameObject.tag == "Step" && child.gameObject.tag != "Step-" + playerController.playerColor)
                    {
                        count = count + 1;
                    }
                }
                GameObject maxStepObject = GameObject.FindGameObjectsWithTag("Step")[playerController.collectedList.Count - 1];
                Vector3 obstaclePosition = maxStepObject.transform.position;
                obstaclePosition.y = obstaclePosition.y * 1.5f;
                invisibleObstacle.transform.position = obstaclePosition;
            }
        }
    }

    void TriggerWithFinalPoint()
    {
        GM.isGameOver = true;
        UI.gamePlayPanel.SetActive(false);
        UI.winPanel.SetActive(true);
        //int collectedCount = playerController.collectedList.Count;
        //for (int i = 0; i < collectedCount; i++)
        //{
        //    GameObject currentGameObject = playerController.collectedList[i];
        //    Vector3 position = currentGameObject.transform.position;
        //    position.y = 50;
        //    currentGameObject.transform.DOLocalJump(position, 1.5f, 1, .45f).OnComplete(() => Debug.Log(1) );
        //}
        //playerController.collectedList.Clear();
    }

}
