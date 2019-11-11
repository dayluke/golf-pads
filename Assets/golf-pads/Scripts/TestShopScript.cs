using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShopScript : MonoBehaviour {

	public int cost;
	public int buttonNo;
	string buttonNoString;
	public bool itemPurchased, itemEquiped;
	string itemStatus;

	public Image itemImage;
	public GameObject itemMenu;

	public void Start()
	{
		buttonNoString = buttonNo.ToString();

		if (PlayerPrefs.HasKey (buttonNoString)) {
			// "f" means the item hasn't been purchased and therefore isn't equiped.
			//"t" means that the item has been purchased but isn't equiped.
			//"tt" means that the item is both purchased and equiped.
			itemStatus = PlayerPrefs.GetString (buttonNoString);
		} else {
			PlayerPrefs.SetString (buttonNoString, "f");
			itemStatus = PlayerPrefs.GetString (buttonNoString);
		}

		if (itemStatus == "t") {
			itemPurchased = true;
		} else if (itemStatus == "tt") {
			itemPurchased = true;
			itemEquiped = true;
		}

		if (itemEquiped)
			PlayerPrefs.SetInt ("Current Ball", buttonNo);
	}

	public void Update()
	{
		//Makes the Item buttons unselectable once an Item menu is opened.
		if (itemMenu.activeInHierarchy) {
			this.GetComponent<Button> ().interactable = false;
		} else {
			this.GetComponent<Button> ().interactable = true;
		}

		//Constantly updates the values of the PlayerPrefs of the BallID -- whether it is purchased and equiped.
		if (itemPurchased && !itemEquiped) {
			PlayerPrefs.SetString (buttonNoString, "t");
		} else if (itemPurchased && itemEquiped) {
			PlayerPrefs.SetString (buttonNoString, "tt");
		} else {
			PlayerPrefs.SetString (buttonNoString, "f");
		}

		//Sets the item to unequiped if the "Current Ball" PlayerPrefs isn't equal to the ButtonNo
		if (!(PlayerPrefs.GetInt ("Current Ball") == buttonNo))
			itemEquiped = false;

	}

	public void Clicked()
	{
		GameObject.Find ("ShopController").GetComponent<ShopController> ().SendMessage ("UpdateCost", cost);
		GameObject.Find ("ShopController").GetComponent<ShopController> ().SendMessage ("UpdateImage", itemImage);
	}

	public void ItemPurchased()
	{
		itemPurchased = true;
		PlayerPrefs.SetString (buttonNoString, "t");
	}

	public void ItemEquiped()
	{
		itemEquiped = true;
		PlayerPrefs.SetString (buttonNoString, "tt");
		PlayerPrefs.SetInt ("Current Ball", buttonNo);
	}
}
