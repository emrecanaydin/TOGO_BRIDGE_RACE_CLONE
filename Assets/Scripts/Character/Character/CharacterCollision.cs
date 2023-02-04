using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterCollision : MonoBehaviour
{

    CharacterController characterController;
    CharacterAI characterAI;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterAI = GetComponent<CharacterAI>();
    }

    private void OnTriggerEnter(Collider other)
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
}
