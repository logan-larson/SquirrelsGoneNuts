using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [SerializeField] private Rigidbody rb;
  [SerializeField] private float movementSpeed = 3f;
  [SerializeField] private float forwardMuliplier = 2f;
  [SerializeField] private float sprintMulitplier = 1.5f;

  public void Move(float horizontal, float vertical, bool isSprinting)
  {
    Vector3 v = new Vector3(horizontal, 0f, vertical).normalized * movementSpeed;

    v = vertical > 0f ? v * forwardMuliplier : v;

    v = isSprinting ? v * sprintMulitplier : v;

    rb.MovePosition(rb.position + (v * Time.deltaTime));

  }

}
