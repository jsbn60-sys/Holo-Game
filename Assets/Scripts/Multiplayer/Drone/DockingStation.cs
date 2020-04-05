/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the docking station where the drone can craft an item
//  depending on the combination of collected students.
/// </summary>
public class DockingStation : MonoBehaviour
{
	public List<GameObject> itemPrefabs = new List<GameObject>();
	private string craftNumber = null;

	/// <summary>
	/// Crafts an item.
	/// </summary>
	/// <param name="students">The list of collected student types</param>
	/// <returns>crafted item or null if the combination was not in one of the recipes</returns>
	public GameObject Craft(int[] students)
	{
		int mniFlag = 0;
		int lseFLag = 0;
		int wirFlag = 0;
		int gesFlag = 0;
		//counts studenttypes
		for (int i = 0; i < students.Length; i++)
		{
			if (students[i] != -1)
			{
				switch (students[i])
				{
					case 0:
						mniFlag++;
						break;
					case 1:
						lseFLag++;
						break;
					case 2:
						wirFlag++;
						break;
					case 3:
						gesFlag++;
						break;
				}
			}
		}
		//cases of combinations for a item to craft
		if (mniFlag ==3  || wirFlag == 3 || lseFLag == 3 || gesFlag == 3)
		{
			Debug.Log("oneHit");
			craftNumber = "3";
			return itemPrefabs[3];//oneHIt
		}
		else if (lseFLag == 1 && gesFlag == 2)
		{
			Debug.Log("healAll");
			craftNumber = "2";
			return itemPrefabs[2];//healAll
		}
		else if (lseFLag == 1 && wirFlag == 2)
		{
			craftNumber = "0";
			return itemPrefabs[0];//distraction
		}
		else if (lseFLag == 1 && wirFlag == 1 && mniFlag == 1)
		{
			craftNumber = "5";
			return itemPrefabs[5];//speedBoost
		}
		else if (lseFLag == 1 && wirFlag == 1 && gesFlag == 1)
		{
			craftNumber = "4";
			return itemPrefabs[4];//orientationLoss
		}
		else if (mniFlag == 2 && wirFlag == 1)
		{
			craftNumber = "1";
			return itemPrefabs[1];//globalStun
		}
		return null; // unknown combination
	}

	public string GetLastCraftNumber() {
		return craftNumber;
	}

	public void SetLastCraftNumber()
	{
		craftNumber = null;
	}

}
