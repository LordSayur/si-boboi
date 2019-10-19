using UnityEngine;
using System.Collections;

public class Gun : Weapon
{
	[Header("General")]
	public float fireRate = .1f;
	public int maxAmmo = 100;

	[Header("Effects")]
	public GameObject muzzleFlash;
	public float muzzleFlashLifeTime = 1;
	public GameObject projectile;
	public GameObject shell;
	public float minShellLifeTime = 2;
	public float maxShellLifeTime = 4;
	public float shellEjectSpeed = 1;
	public AudioClip[] gunShotSounds;
	public float gunShotDuration = 1;

	[Header("Others")]
	public Transform bulletSpawnT;
	public Transform shellEjectT;

	float nextFire;
	int currentAmmo;

	protected override void Start ()
	{
		base.Start ();

		currentAmmo = maxAmmo;
	}

	public override void Action ()
	{
		if (currentAmmo == 0)
			return;
		
		Shoot ();
	}

	void Shoot ()
	{
		if (nextFire < Time.time)
		{
			currentAmmo--;

			GunEffects ();

			nextFire = fireRate + Time.time;
		}
	}

	void GunEffects ()
	{
		if (muzzleFlash)
		{
			GameObject muzzleFlashClone = Instantiate (muzzleFlash, bulletSpawnT.position, bulletSpawnT.rotation) as GameObject;
			muzzleFlashClone.transform.SetParent (bulletSpawnT);

			Destroy (muzzleFlashClone, muzzleFlashLifeTime);
		}

		if (projectile)
		{
			GameObject projectileClone = Instantiate (projectile, bulletSpawnT.position, bulletSpawnT.rotation) as GameObject;

			Projectile newProjectile = projectileClone.GetComponent<Projectile> ();
			newProjectile.SetDamage (damage);
		}

		if (shell)
		{
			GameObject shellClone = Instantiate (shell, shellEjectT.position, shellEjectT.rotation) as GameObject;

			if (shellClone.GetComponent<Rigidbody> ())
			{
				Rigidbody shellRigidbody = shellClone.GetComponent<Rigidbody> ();
				shellRigidbody.AddForce (shellEjectT.forward * shellEjectSpeed, ForceMode.Impulse);
			}

			Destroy (shellClone, Random.Range (minShellLifeTime, maxShellLifeTime));
		}

		PlayGunShotsSound ();
	}

	void PlayGunShotsSound ()
	{
		if (gunShotSounds.Length > 0)
		{
			soundManager.InstantiateClip (bulletSpawnT.position, gunShotSounds [Random.Range (0, gunShotSounds.Length)], gunShotDuration);
		}
	}

	public override void OpenDamageableZone () {}

	public override void CloseDamageableZone () {}
}
