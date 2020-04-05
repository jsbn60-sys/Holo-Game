/* author: SWT-P_SS_2019_Holo */
using System;
using UnityEngine;

/// <summary>
/// Instances of this class define a Enemygroup.
/// The group is defined out of its size and the Enemytype.
/// </summary>
[Serializable]
public class EnemyGroup
{
	// this is the Enemytype
	[SerializeField]
	public GameObject prefab;
	// the count how many enemys are in this group
	[SerializeField]
	public int count;
	/// <summary>
	/// Called by the Wavecreator to create Enemygroup Objects
	/// </summary>
	/// <returns>New initialized Enemygroup</returns>
	public EnemyGroup CreateEnemyGroup(GameObject prefab, int count)
	{
		EnemyGroup enemyGroup = new EnemyGroup();
		enemyGroup.prefab = prefab;
		enemyGroup.count = count;
		return enemyGroup;
	}

}

