using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float turnSpeed;
    public DynamicJoystick dynamicJoystick;

    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {

        float horizontal = dynamicJoystick.Horizontal;
        float vertical = dynamicJoystick.Vertical;

        playerAnimator.SetFloat("MoveSpeed", Mathf.Abs(dynamicJoystick.Vertical + dynamicJoystick.Horizontal));

        Vector3 position = new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, vertical * moveSpeed * Time.deltaTime);
        Vector3 rotation = Vector3.forward * vertical + Vector3.right * horizontal;

        transform.position += position;
        if (rotation != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotation), turnSpeed * Time.deltaTime);
        }

    }

}
