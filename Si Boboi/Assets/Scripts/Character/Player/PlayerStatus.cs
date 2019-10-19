using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(WeaponController))]
public class PlayerStatus : Status 
{
	PlayerController playerController;
	PlayerMovement playerMovement;
	WeaponController weaponController;

	protected override void Start()
	{
		base.Start ();

		playerController = GetComponent<PlayerController> ();
		playerMovement = GetComponent<PlayerMovement> ();
		weaponController = GetComponent<WeaponController> ();

		onDeath += OnPlayerDeath;
	}

	void OnPlayerDeath ()
	{
		playerController.enabled = false;
		playerMovement.enabled = false;
		weaponController.enabled = false;
	}
}
