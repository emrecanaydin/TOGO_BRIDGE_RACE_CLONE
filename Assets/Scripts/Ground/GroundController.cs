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
        InvokeRepeating("GenerateCube", 0f, 0.5f);
    }

    public void GenerateCube()
    {
        GameObject createdCollectable = Instantiate(GM.blueCollectable);
        Vector3 createdPosition = GenerateRandomPosition();
        createdCollectable.transform.parent = parent;
        createdCollectable.transform.localPosition = createdPosition;
        createdCollectable.SetActive(true);
        Debug.Log(Physics.CheckSphere(createdPosition, 1));
        //foreach (var collider in hitColliders)
        //{
        //    Debug.Log(collider.tag);
        //}
    }

    Vector3 GenerateRandomPosition()
    {
        //Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0.065f, Random.Range(minZ, maxZ));
        Vector3 randomPosition = new Vector3(-1.51f, 0.065f, -19.94f);
        return randomPosition;
    }

}
