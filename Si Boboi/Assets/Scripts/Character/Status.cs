using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Status : MonoBehaviour, IDamageable
{
	[Header("General")]
	public float maximumHealth = 100;

	[Header("Effects")]
	public GameObject hitParticles;

	float currentHealth;
	bool dead;

	public event System.Action onDeath;

	Animator animator;

	protected virtual void Start ()
	{
		animator = GetComponent<Animator> ();

		currentHealth = maximumHealth;
	}

	public void TakeHit (float damage, Vector3 hitPoint)
	{
		HitEffects (hitPoint);

		TakeDamage (damage);
	}

	public void TakeDamage (float damage)
	{
		if (dead)
			return;
		
		currentHealth -= damage;

		if (currentHealth <= 0 && !dead)
		{
			Die ();
		}
	}

	public void Heal (float amount)
	{
		currentHealth += amount;

		if (currentHealth > 0)
			currentHealth = maximumHealth;
	}

	void HitEffects (Vector3 hitPoint)
	{
		if (hitParticles != null)
		{
			GameObject hitParticlesClone = Instantiate (hitParticles, hitPoint, Quaternion.identity) as GameObject;
			Destroy (hitParticlesClone, 1);
		}
	}

	void Die ()
	{
		dead = true;
		animator.SetTrigger ("Die");

		if (onDeath != null)
			onDeath ();
	}

	public float GetCurrentHealth ()
	{
		return currentHealth;
	}

	public bool IsDead ()
	{
		return dead;
	}
}
