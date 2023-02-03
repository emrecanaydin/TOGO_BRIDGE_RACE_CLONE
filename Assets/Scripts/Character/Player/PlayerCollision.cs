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
        if(other.tag == "CollectableBlue")
        {
            other.tag = "Untagged";
            other.GetComponent<BoxCollider>().enabled = false;
            other.transform.parent = playerController.collectPoint;
            Vector3 position = new Vector3(0, playerController.collectedList.Count * .25f, 0);
            other.transform.DOLocalMove(position, .35f).OnComplete(() => Destroy(other.transform.Find("Trail").gameObject, 0f));
            other.transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerController.collectedList.Add(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Step")
        {
            collision.gameObject.GetComponent<Renderer>().material = playerController.playerMaterial;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = true;
            playerController.collectedList.RemoveAt(playerController.collectedList.Count - 1);
            Destroy(playerController.collectPoint.GetChild(playerController.collectedList.Count).gameObject, 0f);
        }
    }

}
