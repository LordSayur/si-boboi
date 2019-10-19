using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour 
{
	public Slider healthSlider;
	public Image fillImage;
	public Color fullHealthColor = Color.green;
	public Color zeroHealthColor = Color.red;

	public Text scoreText;

	public GameObject pauseObject;
	bool isPaused;

	PlayerStatus playerStatus;

	void Start ()
	{
		GameObject playerObj = GameObject.FindGameObjectWithTag ("Player");
		if (playerObj != null)
			playerStatus = playerObj.GetComponent<PlayerStatus> ();

		Time.timeScale = 1;
		HidePaused ();
	}

	void Update ()
	{
		UpdateHealth ();
		UpdateScore ();
	}

	void UpdateHealth ()
	{
		if (playerStatus)
		{
			float health = playerStatus.GetCurrentHealth () / playerStatus.maximumHealth;

			if (healthSlider != null)
				healthSlider.value = health;

			if (fillImage != null)
				fillImage.color = Color.Lerp (zeroHealthColor, fullHealthColor, health);
		}
	}

	void UpdateScore()
	{
		if (scoreText != null)
			scoreText.text = "Score: " + ScoreManager.score;
	}

	public void LoadLevel (string level)
	{
		SceneManager.LoadScene (level);
	}

	public void ReloadLevel ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void Pause ()
	{
		if (isPaused)
		{
			Time.timeScale = 1;
			HidePaused ();
			isPaused = false;
		} else
		{
			Time.timeScale = 0;
			ShowPaused ();
			isPaused = true;
		}
	}

	void ShowPaused ()
	{
		pauseObject.SetActive (true);
	}

	void HidePaused ()
	{
		pauseObject.SetActive (false);
	}

	public bool IsPaused ()
	{
		return isPaused;
	}
}
