using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	public LayerMask collisionMask;

	public float lifeTime = 2;
	public float speed = 100;

	float damage = 1;
	float collisionCheckRadius = .2f;

	void Start ()
	{
		Destroy (gameObject, lifeTime);

		Collider[] initialCollision = Physics.OverlapSphere (transform.position, collisionCheckRadius, collisionMask);
		if (initialCollision.Length > 0)
			OnHitObject (initialCollision [0]);
	}

	void Update ()
	{
		float moveDistance = speed * Time.deltaTime;
		CheckCollision (moveDistance);

		transform.Translate (Vector3.forward * moveDistance);
	}

	void CheckCollision (float moveDistance)
	{
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if (Physics.SphereCast (ray, collisionCheckRadius, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject (hit);
		}
	}

	void OnHitObject (RaycastHit hit)
	{
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();
		if (damageableObject != null)
			damageableObject.TakeHit (damage, hit.point);
		
		Destroy (gameObject);
	}

	void OnHitObject (Collider col)
	{
		IDamageable damageableObject = col.GetComponent<IDamageable> ();
		if (damageableObject != null)
			damageableObject.TakeDamage (damage);

		Destroy (gameObject);
	}

	public void SetDamage (float damage)
	{
		this.damage = damage;
	}
}
