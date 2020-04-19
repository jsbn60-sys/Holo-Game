/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class can create infinite Waves of Enemies
/// To add new Enemies: Add their Prefab as field of the class
/// And then add a newWave.addEnemyGroup statement in the switch below(One Statement for each case)
/// If you want to create more or less waves, modify the NUMBER_OF_WAVES constant
/// If you want to modify the current Waves, you have to change the Calculation of the Enemycount
/// </summary>
public class WaveCreator : MonoBehaviour
{
	private const int NUMBER_OF_WAVES = 30;

	public static WaveCreator Instance = new WaveCreator();



	[SerializeField]
	public GameObject enemy_Big;
	[SerializeField]
	public GameObject enemy_Gesundheit;
	[SerializeField]
	public GameObject enemy_LSE;
	[SerializeField]
	public GameObject enemy_MNI;
	[SerializeField]
	public GameObject enemy_Wirtschaft;
	[SerializeField]
	public GameObject enemy_Fire;
	[SerializeField]
	public GameObject enemy_Kamikaze;

	//Object to create EnemyGroups
	EnemyGroup eGroupGenerator = new EnemyGroup();
	public static int playerCount = 0;


	private void OnEnable()
	{
		Instance = this;
	}

	public int ReturnPlayerCount()
	{
		return playerCount;
	}

	public void ReducePlayerCount()
	{
		playerCount--;
	}

	public void SetPlayerCountToZero()
	{
		playerCount = 0;
	}

	/// <summary>
	/// Creates a preset number of waves
	/// </summary>
	/// <returns></returns>
	public List<Wave> CreateAllWaves()
	{
		List<Wave> waves = new List<Wave>();
		for(int i = 1; i < NUMBER_OF_WAVES; i++)
		{
			waves.Add(CreateWave(i, playerCount));
		}
		return waves;
	}
	/// <summary>
	/// Creates a new Wave based on entered Parameters
	/// </summary>
	/// <param name="waveCount"> The level that the wave is created for</param>
	/// <param name="playerCount">The amount of players in the game</param>
	/// <returns>A new Wave of Enemies that can be spawned</returns>
	private Wave CreateWave(int waveCount, int playerCount)
	{
		Wave newWave = (Wave) ScriptableObject.CreateInstance("Wave");
		
		switch (playerCount) //Difficulty Changes with different player count
		{
			case 1:
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_MNI, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Gesundheit, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_LSE, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Wirtschaft, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				if (waveCount % 6 >= 2 || waveCount % 6 == 0) //from wave 2 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Kamikaze, playerCount * waveCount));
				}
				if (waveCount % 6 >= 4 || waveCount % 6 == 0) //from wave 4 and higher
				{
					//Enemycount is calculated by: playercount + (number of times 6 semesters have been played) * playercount:
					//eg: 12 semesters with 1 player = 1 + 2 * 1 = 3
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Fire, (1 * playerCount) + ((waveCount - (waveCount % 6)) / 6) * playerCount));
				}
				if (waveCount % 6 == 0) //in every 6th wave 
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Big, 1 * (waveCount / 6)));
				}
				break;
			case 2:
				Debug.Log("2 Spieler gefunden");
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_MNI, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Gesundheit, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_LSE, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Wirtschaft, 3 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				if (waveCount % 6 >= 2 || waveCount % 6 == 0) //from wave 2 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Kamikaze, playerCount * waveCount));
				}
				if (waveCount % 6 >= 4 || waveCount % 6 == 0) //from wave 4 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Fire, (1 * playerCount) + ((waveCount - (waveCount % 6)) / 6) * playerCount));
				}
				if (waveCount % 6 == 0) //in every 6th wave 
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Big, waveCount / 6 + playerCount));
				}
				break;
			case 3:
				Debug.Log("3 Spieler gefunden");
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_MNI, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Gesundheit, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_LSE, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Wirtschaft, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				if (waveCount % 6 >= 2 || waveCount % 6 == 0) //from wave 2 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Kamikaze, playerCount * waveCount));
				}
				if (waveCount % 6 >= 4 || waveCount % 6 == 0) //from wave 4 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Fire, (1 * playerCount) + ((waveCount - (waveCount % 6)) / 6) * playerCount));
				}
				if (waveCount % 6 == 0) //in every 6th wave 
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Big, waveCount / 6 + playerCount));
				}
				break;
			case 4:
				Debug.Log("4 Spieler gefunden");
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_MNI, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Gesundheit, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_LSE, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Wirtschaft, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				if (waveCount % 6 >= 2 || waveCount % 6 == 0) //from wave 2 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Kamikaze, playerCount * waveCount));
				}
				if (waveCount % 6 >= 4 || waveCount % 6 == 0) //from wave 4 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Fire, (1 * playerCount) + ((waveCount - (waveCount % 6)) / 6) * playerCount));
				}
				if (waveCount % 6 == 0) //in every 6th wave 
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Big, waveCount / 6 + playerCount));
				}
				break;
			case 5:
				Debug.Log("5 Spieler gefunden");
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_MNI, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Gesundheit, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_LSE, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Wirtschaft, 4 + playerCount + ((waveCount % 6) == 0 ? 6 : waveCount % 6)));
				if (waveCount % 6 >= 2 || waveCount % 6 == 0) //from wave 2 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Kamikaze, playerCount * waveCount));
				}
				if (waveCount % 6 >= 4 || waveCount % 6 == 0) //from wave 4 and higher
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Fire, (1 * playerCount) + ((waveCount - (waveCount % 6)) / 6) * playerCount));
				}
				if (waveCount % 6 == 0) //in every 6th wave 
				{
					newWave.addEnemyGroup(eGroupGenerator.CreateEnemyGroup(enemy_Big, waveCount / 6 + playerCount));
				}
				break;
			default:
				Debug.Log("Komische Sachen passieren...");
				break;
		}
		return newWave;
	}

}
