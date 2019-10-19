using UnityEngine;
using System.Collections;

public class EnemyStatus : Status 
{
	public int scoreValue = 10;

	EnemyAI enemyAI;
	UnityEngine.AI.NavMeshAgent agent;
	CapsuleCollider myCollider;

	protected override void Start()
	{
		base.Start ();

		enemyAI = GetComponent<EnemyAI> ();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		myCollider = GetComponent<CapsuleCollider> ();

		onDeath += OnEnemyDeath;
	}

	void OnEnemyDeath ()
	{
		ScoreManager.score += scoreValue;

		enemyAI.enabled = false;
		agent.enabled = false;
		myCollider.isTrigger = true;

		Destroy (gameObject, 3);
	}
}
