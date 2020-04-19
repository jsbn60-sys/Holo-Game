/* author: SWT-P_SS_2019_Holo */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
/// <summary>
/// This class controls the Waves. It uses the WaveCreator to create Waves of Enemies
/// It also starts the waves(Semesters), detects when all waves are finished and displays the game over screen
/// This script is attached to the GameManager object in the multiplayer scene
/// </summary>
public class WaveController : NetworkBehaviour
{
	// Textfield which is used to display Semester description.
	[SerializeField]
	public Text waveStatusText;
	// Textfield which is used to display the completed Semester counter.
	[SerializeField]
	public Text waveCountText;

	private GameObject winObject;

	public static uint waveCounter = 1;

	public float countdownTimer = 30f;

	List<Wave> waves;
	IEnumerator<Wave> enumerator;

	Wave curWave;

	//private float lastSemEnd = 0;
	[Server]
	private void StartNextSemester()
	{
		SetSemesterStatus("Semester");
		////
		// Increases Skill Points for all Players
		GameObject[] players;
		players = GameObject.FindGameObjectsWithTag("Player");

		foreach (GameObject player in players)
		{
			Player playerController = player.GetComponent<Player>();
			if (playerController != null)
			{
				playerController.addSkillPoints(1);
			}
		}

		////

		if (enumerator.MoveNext())
		{
			StartCoroutine("StartSemester");

		}
		else
		{
			GameObject.FindGameObjectWithTag("WaveCount").SetActive(false);
			GameObject.FindGameObjectWithTag("WaveStatus").SetActive(false);
			winObject.SetActive(true);
			Transform winText = winObject.transform.GetChild(0);
			winText.GetComponent<Text>().text = "You Won!";
			RpcSetSemesterEnd();
		}
	}

	[ClientRpc]
	private void RpcSetSemesterEnd()
	{
		
		GameObject.FindGameObjectWithTag("WaveCount").SetActive(false);
		GameObject.FindGameObjectWithTag("WaveStatus").SetActive(false);
		winObject = GameObject.FindGameObjectWithTag("Win");
		winObject.SetActive(true);
		Transform winText = winObject.transform.GetChild(0);
		winText.GetComponent<Text>().text = "You Won!";
	}

	/// <summary>
	/// starts a semester, heals all profs and spawns all enemies
	/// </summary>
	private void StartSemester()
	{
		//if (!this)
		//{
		//	yield break;
		//}

		NPC.NPCManager.Instance.OnAllNPCsDied += this.OnAllNPCDied;

		//SetSemesterStatus("Semester");

		// restore health of prof players
		GameOverManager.Instance.RecoverProfs();

		// spawn students
		foreach (EnemyGroup group in enumerator.Current.getEnemyGroups())
		{
			NPC.NPCManager.Instance.SpawnNewNPCGroup(group.prefab, group.count);
		}
	}


	/// <summary>
	/// End a the started wave.
	/// If wave wasn't started nothing happens.
	/// </summary>
	public virtual void End()
	{
		NPC.NPCManager.Instance.OnAllNPCsDied -= this.OnAllNPCDied;
		OnSemesterHasFinished();
	}

	private void OnAllNPCDied(object sender, EventArgs e)
	{
		End();
	}


	/// <summary>
	/// First method to be called
	/// </summary>
	public override void OnStartServer()
	{
		winObject = GameObject.FindGameObjectWithTag("Win");
		winObject.SetActive(false);
		waves = new List<Wave>();
		getWaves();
		enumerator = waves.GetEnumerator();
		StartCoroutine(startCountdown());
	}

	/// <summary>
	/// starts the countdown before first spawn
	/// and then starts first spawn
	/// </summary>
	private IEnumerator startCountdown()
	{
		WaitForSeconds wait = new WaitForSeconds(1f);
		for (int i = 0; i < countdownTimer; i++)
		{
			waveStatusText.text = "Start: " + (countdownTimer - i);
			yield return wait;
		}
		StartNextSemester();
	}

	/// <summary>
	/// Gets a List of Waves from the WaveCreator
	/// </summary>
	private void getWaves()
	{
		List<Wave> wavelst = WaveCreator.Instance.CreateAllWaves();
		foreach (Wave wave in wavelst)
		{
			this.waves.Add(wave);
		}
	}






	/// <summary>
	/// Displays the given description with the semester counter on client.
	/// </summary>
	/// <param name="description"></param>
	
	public void SetSemesterStatus(string description)
	{
		string displaySemCnt = "";
		uint semCnt = (waveCounter % 6);
		uint numberOfPlus = (waveCounter - (waveCounter % 6)) / 6;
		displaySemCnt = semCnt.ToString();
		for (int i = 0; i < numberOfPlus; i++)
		{
			displaySemCnt += "+";
		}
		waveStatusText.text = description + " " + displaySemCnt;
		//waveCountText.text = displaySemCnt;
		if (this == null)
		{
			return;
		}

		RpcSetSemesterStatus(description + " " + displaySemCnt);
	}

	/// <summary>
	/// Called on Client by the server to set the current Semester Status.
	/// For each 6 semesters finished, add a + to the semester count indicating a higher difficulty level
	/// </summary>
	/// <param name="description"></param>
	/// <param name="waveCounter"></param>
	[ClientRpc]
	private void RpcSetSemesterStatus(string desc)
	{
		waveStatusText.text = desc;

	}

	/// <summary>
	/// Shall be invoked when a wave is cleared
	/// </summary>
	[Server]
	public void OnSemesterHasFinished()
	{
		//if(waveCounter / 6 == 0)
		//{
		//	lastSemEnd = Time.time + 15; //15s Pause nach 6 Semestern
		//}
		//else
		//{
		//	lastSemEnd = Time.time + 5; //5s Pause nach jedem Semester
		//}
		
		waveCounter++;
		if (waveCounter > 1)
		{
		}
		if (this == null)
		{
			return;
		}
		StartNextSemester();
	}
}
