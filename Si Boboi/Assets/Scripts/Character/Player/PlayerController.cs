using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(WeaponController))]
public class PlayerController : MonoBehaviour 
{
	[Header("Mobile Input")]
	public VirtualJoystick leftJoystick;
	public VirtualJoystick rightJoystick;

	Transform cameraTransform;

	Vector3 movement;
	Vector3 lookDirection;
	Vector3 turnDirection;

	PlayerMovement playerMovement;
	WeaponController weaponController;

	void Start ()
	{
		playerMovement = GetComponent<PlayerMovement> ();
		weaponController = GetComponent<WeaponController> ();

		if (Camera.main != null)
			cameraTransform = Camera.main.transform;
	}

	void Update ()
	{
		ControlMovement ();
		ControlWeapon ();
	}

	void ControlMovement ()
	{
		#if UNITY_ANDROID
		movement.Set (leftJoystick.GetAxis ("Horizontal"), 0, leftJoystick.GetAxis ("Vertical"));
		#endif

		#if UNITY_EDITOR
		movement.Set (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		#endif

		playerMovement.Move (movement);

		#if UNITY_ANDROID
		turnDirection = new Vector3 (rightJoystick.GetAxis ("Horizontal"), 0, rightJoystick.GetAxis ("Vertical"));

		if (turnDirection != Vector3.zero)
			lookDirection = GetLookDirection (turnDirection);
		else
			lookDirection = GetLookDirection (movement);
		#endif

		#if UNITY_EDITOR
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayDistance;

		if (groundPlane.Raycast (ray, out rayDistance))
		{
			Vector3 hitPoint = ray.GetPoint (rayDistance);

			lookDirection = hitPoint - transform.position;
			lookDirection.y = 0;
		}
		#endif

		playerMovement.Turn (lookDirection.normalized);
	}

	void ControlWeapon ()
	{
		#if UNITY_ANDROID
		if (turnDirection.magnitude > .8f)
			weaponController.PerformAction ();
		#endif

		#if UNITY_EDITOR
		if (Input.GetMouseButton (0))
			weaponController.PerformAction ();
		#endif
	}

	Vector3 GetLookDirection (Vector3 direction)
	{
		Vector3 lookDirection = Vector3.zero;

		if (cameraTransform != null)
		{
			Vector3 cameraForward = cameraTransform.forward;
			cameraForward.y = 0;

			lookDirection = direction.x * cameraTransform.right + direction.z * cameraForward;
		} else
		{
			lookDirection = direction.x * Vector3.right + direction.z * Vector3.forward;
		}

		return lookDirection;
	}
}
