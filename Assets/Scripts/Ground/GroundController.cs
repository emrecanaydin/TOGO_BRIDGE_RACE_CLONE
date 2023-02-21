using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{

    GameManager GM;

    public int minX, maxX, minZ, maxZ;
    public Transform parent;
    public LayerMask layerMask;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void GenerateCube(string relatedTag)
    {

        GameObject createdCollectable;

        switch (relatedTag)
        {
            case "CollectableOrange":
                createdCollectable = Instantiate(GM.orangeCollectable);
                break;
            case "CollectableGreen":
                createdCollectable = Instantiate(GM.greenCollectable);
                break;
            case "CollectableBlue":
                createdCollectable = Instantiate(GM.blueCollectable);
                break;
            default:
                createdCollectable = Instantiate(GM.blueCollectable);
                break;
        }

        Vector3 createdPosition = GenerateRandomPosition();
        createdCollectable.transform.parent = parent;
        createdCollectable.transform.localPosition = createdPosition;
        if (CheckIfCollisionWithCollectble(createdCollectable.transform.position))
        {
            Destroy(createdCollectable, 0f);
            //GenerateCube(relatedTag);
        }

    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        return randomPosition;
    }

    bool CheckIfCollisionWithCollectble(Vector3 position)
    {
        return Physics.CheckSphere(position, .5f, layerMask);
    }

}
