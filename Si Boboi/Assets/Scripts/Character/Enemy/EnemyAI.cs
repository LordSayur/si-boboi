using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyStatus))]
[RequireComponent(typeof(CapsuleCollider))]
public class EnemyAI : MonoBehaviour 
{
	[Header("General")]
	public Global.EnemyState currentState;
	public float attackDistance = .5f;
	public float attackRate = 1;
	public float damage = 10;

	[Header("Others")]
	public Transform damageableZone;
	public LayerMask damageableMask;

	Transform target;
	float myColliderRadius;
	float targetColliderRadius;

	float nextAttack;
	float attackDuration;
	bool hasAppliedAttack;

	float speed;

	bool enableDamageableZone;
	List<EntityInDamageableZone> entitiesInDamageableZone = new List<EntityInDamageableZone> ();

	UnityEngine.AI.NavMeshAgent agent;
	Animator animator;
	Status targetStatus;
	EnemyStatus myStatus;

	void Start ()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		myStatus = GetComponent<EnemyStatus> ();

		agent.updateRotation = false;
		agent.updatePosition = true;

		if (GameObject.FindGameObjectWithTag ("Player"))
		{
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetStatus = target.GetComponent<Status> ();
			targetStatus.onDeath += OnTargetDeath;

			currentState = Global.EnemyState.Chasing;

			myColliderRadius = GetComponent<CapsuleCollider> ().radius;
			targetColliderRadius = target.GetComponent<CapsuleCollider> ().radius;

			agent.stoppingDistance = myColliderRadius + targetColliderRadius + attackDistance;

			StartCoroutine (UpdateAI());
		}
	}

	IEnumerator UpdateAI ()
	{
		while (true)
		{
			if (!myStatus.IsDead())
			{
				switch (currentState)
				{
				case Global.EnemyState.Idle:
					agent.enabled = false;
					Move (Vector3.zero);
					break;
				case Global.EnemyState.Chasing:
					StartCoroutine (UpdatePath ());

					if (nextAttack < Time.time)
					{
						float squareDistance = (target.position - transform.position).sqrMagnitude;
						if (squareDistance <= Mathf.Pow (myColliderRadius + targetColliderRadius + attackDistance, 2))
						{
							currentState = Global.EnemyState.Attacking;
							nextAttack = attackRate + Time.time;
						}
					}

					break;
				case Global.EnemyState.Attacking:
					StartCoroutine (Attack ());
					break;
				}
			}
			yield return null;
		}
	}

	IEnumerator Attack ()
	{
		Vector3 lookDirection = (target.position - transform.position).normalized;

		agent.enabled = false;
		Move (Vector3.zero);
		Turn (lookDirection);

		if (!hasAppliedAttack)
			animator.SetBool ("Attack", true);
		
		if (animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack") && animator.GetBool("Attack"))
		{
			animator.SetBool ("Attack", false);
			attackDuration = animator.GetCurrentAnimatorStateInfo (0).length;
			hasAppliedAttack = true;
		}

		if (enableDamageableZone)
		{
			Collider[] colliders = Physics.OverlapBox (damageableZone.position, Vector3.one, Quaternion.identity, damageableMask);

			if (colliders.Length > 0)
			{
				foreach (Collider c in colliders)
				{
					if (entitiesInDamageableZone.Count > 0)
					{
						foreach (EntityInDamageableZone dimz in entitiesInDamageableZone)
						{
							if (dimz.entity != c)
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

		if (hasAppliedAttack)
		{
			yield return new WaitForSeconds (attackDuration);

			hasAppliedAttack = false;
			agent.enabled = true;
			currentState = Global.EnemyState.Chasing;
		}

		yield return null;
	}

	IEnumerator UpdatePath ()
	{
		float refreshRate = .25f;

		if (!myStatus.IsDead())
		{
			agent.SetDestination (target.position);

			Vector3 velocity = agent.velocity.normalized;

			Move (velocity);
			Turn (velocity);
		}

		yield return new WaitForSeconds (refreshRate);
	}

	void Move (Vector3 velocity)
	{
		speed = Mathf.Abs (velocity.x) + Mathf.Abs (velocity.z);
		speed = Mathf.Clamp (speed, 0, 1);

		animator.SetFloat ("Speed", speed, .1f, Time.deltaTime);
	}

	void Turn (Vector3 lookDirection)
	{
		if (lookDirection != Vector3.zero)
		{
			Quaternion lookRotation = Quaternion.LookRotation (lookDirection);
			Quaternion newRotation = Quaternion.Slerp (transform.rotation, lookRotation, 10 * Time.deltaTime);
			transform.rotation = newRotation;
		}
	}

	void OnTargetDeath ()
	{
		currentState = Global.EnemyState.Idle;
	}

	public void OpenDamageableZone ()
	{
		enableDamageableZone = true;
	}

	public void CloseDamageableZone ()
	{
		enableDamageableZone = false;
		entitiesInDamageableZone.Clear ();
	}

	void OnDrawGizmos ()
	{
		Gizmos.color = Color.blue;

		Gizmos.DrawWireCube (damageableZone.position, Vector3.one);
	}
}
