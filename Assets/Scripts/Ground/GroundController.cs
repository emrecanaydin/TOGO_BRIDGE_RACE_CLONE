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

    public void GenerateCube()
    {
        GameObject createdCollectable = Instantiate(GM.blueCollectable);
        Vector3 createdPosition = GenerateRandomPosition();
        createdCollectable.transform.parent = parent;
        createdCollectable.transform.localPosition = createdPosition;
        if (CheckIfCollisionWithCollectble(createdCollectable.transform.position))
        {
            Destroy(createdCollectable, 0f);
            GenerateCube();
        }
    }

    Vector3 GenerateRandomPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0.065f, Random.Range(minZ, maxZ));
        return randomPosition;
    }

    bool CheckIfCollisionWithCollectble(Vector3 position)
    {
        return Physics.CheckSphere(position, 1, layerMask);
    }

}
