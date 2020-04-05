/* author: SWT-P_SS_2019_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//ScriptableObject.Instantiate() to create a instance of the class
/// <summary>
///  An instance of this class represents one wave(Semester) in the Game
///  It gets created by the WaveCreator and used by the WaveController
/// </summary>
public class Wave : ScriptableObject
{
	//list of different enemygroups
	List<EnemyGroup> enemyGroups;

	public Wave()
	{
		enemyGroups = new List<EnemyGroup>();
	}

	public void addEnemyGroup(EnemyGroup egroup)
	{
		enemyGroups.Add(egroup);
	}

	public List<EnemyGroup> getEnemyGroups()
	{
		return enemyGroups;
	}
}
