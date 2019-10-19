using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Melee : Weapon
{
	float comboRate = .5f;
	bool attacking;

	public LayerMask damageableMask;
	public AudioClip[] meleeSounds;

	bool enableDamageableZone;
	List<EntityInDamageableZone> entitiesInDamageableZone = new List<EntityInDamageableZone> ();

	Transform damageableZone;

	protected override void Start ()
	{
		base.Start ();

		damageableZone = user.damageableZone;
	}

	void Update ()
	{
		if (enableDamageableZone)
		{
			Collider[] colliders = Physics.OverlapBox (damageableZone.position, Vector3.one, Quaternion.identity, damageableMask);

			if (colliders.Length > 0)
			{
				foreach (Collider c in colliders)
				{
					if (entitiesInDamageableZone.Count > 0)
					{
						foreach (EntityInDamageableZone dz in entitiesInDamageableZone)
						{
							if (dz.entity != c)
							{
								entitiesInDamageableZone.Add (new EntityInDamageableZone (c, false));
								break;
							}
						}
					} else
					{
						entitiesInDamageableZone.Add (new EntityInDamageableZone (c, false));
					}
				}
			}

			foreach (EntityInDamageableZone dimz in entitiesInDamageableZone)
			{
				if (dimz.isDamage == false)
				{
					IDamageable damageableObject = dimz.entity.GetComponent<IDamageable> ();
					if (damageableObject != null)
						damageableObject.TakeDamage (damage);

					dimz.isDamage = true;
				}
			}
		}
	}

	public override void Action ()
	{
		Attack ();
	}

	public override void OpenDamageableZone ()
	{
		enableDamageableZone = true;
	}

	public override void CloseDamageableZone ()
	{
		enableDamageableZone = false;
		entitiesInDamageableZone.Clear ();
	}

	void Attack ()
	{
		user.GetAnimator ().SetBool ("Attack", true);

		PlayMeleeSound ();
		StartCoroutine (StopAttacking ());
	}

	IEnumerator StopAttacking ()
	{
		yield return new WaitForSeconds (comboRate);
		user.GetAnimator ().SetBool ("Attack", false);
	}

	void PlayMeleeSound ()
	{
		if (meleeSounds.Length > 0)
		{
			soundManager.InstantiateClip (damageableZone.position, meleeSounds [Random.Range (0, meleeSounds.Length)], 1);
		}
	}
}
