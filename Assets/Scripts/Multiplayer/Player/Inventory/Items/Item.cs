/* author: SWT-P_WS_2018_Holo */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


///<summary>
/// This class provides the general structure of an item. All items inherit from this class. 
///</summary>
public class Item : NetworkBehaviour {
	public Sprite sprite; // the sprite to represent the item in the inventory
	
	///<summary>
	/// Performs the unique ability of an item.
	///</summary>
	public virtual void Ability(PlayerController player)
	{
		
	}
	
    void Update()
    {
		// prevent objects from falling through map
        if (gameObject.transform.position.y < 0)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z);
            rb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

	///<summary>
	/// This function displays the icon of the item. It creates a new GameObject and puts it on the canvas in a predefined position.
	/// The icon will consists of the sprite image.
	///</summary>
	public void DisplayIcon(GameObject icon, Vector3 position)
	{
		Image img = icon.AddComponent<Image>();
		img.sprite = sprite;
		icon.GetComponent<RectTransform>().SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
		icon.GetComponent<RectTransform>().transform.position = position;
		icon.GetComponent<RectTransform>().sizeDelta = new Vector2(50f,50f);
		icon.SetActive(true);
	}
}
