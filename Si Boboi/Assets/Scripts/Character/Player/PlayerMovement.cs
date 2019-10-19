using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 5;
	public float turnSpeed = 10;

	float rightAmount;
	float forwardAmount;

	Vector3 velocity;
	Vector3 lookDirection;

	Rigidbody playerRigidbody;
	Animator animator;

	void Start ()
	{
		playerRigidbody = GetComponent<Rigidbody> ();
		animator = GetComponent<Animator> ();

		playerRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	void FixedUpdate ()
	{
		playerRigidbody.MovePosition (transform.position + velocity * Time.fixedDeltaTime);

		if (lookDirection != Vector3.zero)
		{
			Quaternion lookRotation = Quaternion.LookRotation (lookDirection);
			Quaternion newRotation = Quaternion.Slerp (transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
			transform.rotation = newRotation;
		}

		Animate ();
	}

	public void Move (Vector3 movement)
	{
		Vector3 localMovement = transform.InverseTransformDirection (movement);
		rightAmount = localMovement.x;
		forwardAmount = localMovement.z;

		velocity = movement.normalized * moveSpeed;
	}

	public void Turn (Vector3 lookDirection)
	{
		this.lookDirection = lookDirection;
	}

	void Animate ()
	{
		animator.SetFloat ("Right", rightAmount, .1f, Time.deltaTime);
		animator.SetFloat ("Forward", forwardAmount, .1f, Time.deltaTime);
	}
}
