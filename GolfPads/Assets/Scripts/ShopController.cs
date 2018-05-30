using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ShopController : MonoBehaviour {

	public GameObject itemMenu;
	public Image itemImage;
	public Image menuImage;
	string buttonName;

	int menuNo, buttonNo;

	int cost;
	public int credits;
	public Text creditText, costText;

	bool  purchasedItem, equipedItem;

	public void Start()
	{
		if (PlayerPrefs.HasKey ("Credits")) {
			credits = PlayerPrefs.GetInt ("Credits");
			creditText.text = credits.ToString ();
		} else {
			credits = 0;
			PlayerPrefs.SetInt ("Credits", credits);
		}
	}

	public void UpdateItemStatus()
	{
		purchasedItem = GameObject.Find (buttonName).GetComponent<TestShopScript> ().itemPurchased;
		equipedItem = GameObject.Find (buttonName).GetComponent<TestShopScript> ().itemEquiped;
	}

	public void OnMenuClick()
	{
		SceneManager.LoadScene ("Menu");
		PlayerPrefs.SetInt ("Credits", credits);
	}

	public void OnButtonClick()
	{
		buttonName = EventSystem.current.currentSelectedGameObject.name;
		UpdateItemStatus ();
		GameObject.Find (buttonName).GetComponent<TestShopScript> ().SendMessage ("Clicked");
		itemMenu.SetActive (true);

		//Reset Status of Buy Button when the menu is opened.
		GameObject.Find ("BuyButton").GetComponentInChildren<Text> ().text = "Purchase";
		GameObject.Find ("BuyButton").GetComponent<Button> ().interactable = true;

		if (purchasedItem && !equipedItem){
			GameObject.Find ("BuyButton").GetComponentInChildren<Text> ().text = "Equip";
		} else if (purchasedItem && equipedItem) {
			GameObject.Find ("BuyButton").GetComponentInChildren<Text> ().text = "Equiped";
			GameObject.Find ("BuyButton").GetComponent<Button> ().interactable = false;
		}

	}

	public void OnCloseClick()
	{
		itemMenu.SetActive (false);
	}

	public void OnBackButton()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Menu");
		}
	}

	public void UpdateCost(int itemCost)
	{
		cost = itemCost;
		costText.text = "Cost: " + cost.ToString ();
	}

	public void UpdateImage(Image menuItemImage)
	{
		menuImage.overrideSprite = menuItemImage.sprite;
	}

	//public void OnBuyClick()
	//{
	//	if (!purchased) {
	//		GameObject.Find("BuyButton").GetComponentInChildren<Text> ().text = "Equip";
	//		credits -= cost;
	//		creditText.text = credits.ToString ();
	//		purchased = true;
	//	} else if (purchased) {
	//		GameObject.Find("BuyButton").GetComponentInChildren<Text> ().text = "Equiped";
	//		equiped = true;
	//		GameObject.Find ("BuyButton").GetComponent<Button> ().interactable = false;
	//	}
	//}

	public void OnBuyClick()
	{
			if (!purchasedItem && cost <= credits) {
				GameObject.Find ("BuyButton").GetComponentInChildren<Text> ().text = "Equip";
				credits -= cost;
				creditText.text = credits.ToString ();
				GameObject.Find (buttonName).GetComponent<TestShopScript> ().SendMessage ("ItemPurchased");
				purchasedItem = GameObject.Find (buttonName).GetComponent<TestShopScript> ().itemPurchased;
			} else if (purchasedItem) {
				GameObject.Find ("BuyButton").GetComponentInChildren<Text> ().text = "Equiped";
				GameObject.Find (buttonName).GetComponent<TestShopScript> ().SendMessage ("ItemEquiped");
				GameObject.Find ("BuyButton").GetComponent<Button> ().interactable = false;
			} else {
				Debug.Log ("Dont have enough credits to buy this.");
			}
	}
}