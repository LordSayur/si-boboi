using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckObstacle : MonoBehaviour 
{
	public LayerMask obstacleLayer;
	public bool debugRay;

	Transform playerTransform;
	Transform mainCameraTransform;

	List<Transform> hiddenObjects = new List<Transform>();


	void Start ()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		if (playerObj != null)
			playerTransform = playerObj.transform;
		
		if (Camera.main != null)
			mainCameraTransform = Camera.main.transform;
	}

	void Update ()
	{
		if (playerTransform != null && mainCameraTransform != null)
		{
			Vector3 direction = playerTransform.position - mainCameraTransform.position;
			float distance = direction.magnitude;

			RaycastHit[] hits = Physics.RaycastAll (mainCameraTransform.position, direction, distance, obstacleLayer);

			if (debugRay)
				Debug.DrawLine (mainCameraTransform.position, playerTransform.position, Color.red);

			for (int i = 0; i < hits.Length; i++)
			{
				if (!hiddenObjects.Contains (hits [i].transform))
				{
					hiddenObjects.Add (hits [i].transform);

					ChangeShader ("Particles/Alpha Blended", hits [i].transform);
				}
			}

			for (int i = 0; i < hiddenObjects.Count; i++)
			{
				bool isHit = false;

				for (int j = 0; j < hits.Length; j++)
				{
					if (hits [j].transform == hiddenObjects [i])
					{
						isHit = true;
						break;
					}
				}

				if (!isHit)
				{
					ChangeShader ("Standard", hiddenObjects [i].transform);
					hiddenObjects.RemoveAt (i);
				}
			}
		}
	}

	void ChangeShader(string shaderName, Transform t)
	{
		Renderer[] meshes = t.GetComponentsInChildren<Renderer> ();

		foreach (Renderer mesh in meshes)
		{
			Material[] materials = mesh.materials;
			foreach (Material material in materials)
			{
				material.shader = Shader.Find (shaderName);
			}
		}
	}
}
