using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public DynamicJoystick dynamicJoystick;

    GameManager GM;
    Animator playerAnimator;
    PlayerController playerController;

    private void Start()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerAnimator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!GM.isGameOver)
        {
            Movement();
        } else
        {
            playerAnimator.SetFloat("MoveSpeed", 0);
        }
    }

    void Movement()
    {
        float horizontal = dynamicJoystick.Horizontal;
        float vertical = dynamicJoystick.Vertical;

        playerAnimator.SetFloat("MoveSpeed", Mathf.Abs(dynamicJoystick.Vertical + dynamicJoystick.Horizontal));

        Vector3 position = new Vector3(horizontal * GM.playerMoveSpeed * Time.deltaTime, 0, vertical * GM.playerMoveSpeed * Time.deltaTime);
        Vector3 rotation = Vector3.forward * vertical + Vector3.right * horizontal;

        if (playerController.IsInLadder)
        {
            position.x = Mathf.Clamp(position.x, -1.6f, 1.54f);
        }

        transform.position += position;
        if (rotation != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), GM.playerTurnSpeed * Time.deltaTime);
        }
    }

}
