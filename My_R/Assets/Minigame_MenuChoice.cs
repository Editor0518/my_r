using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_MenuChoice : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject thisWhole;
    public int nextBlockSame;
    public int nextBlock;
    public Button orderBtn;
    public Sprite[] selectedIcons;//0: unselected, 1: selected
    public string costName = "ch2foodcost";
    public float defaultAddCost = 0;

    [System.Serializable]
    public class FoodMenu
    {
        public string menuName;
        public float price;
        public Button button;
        public Image icon;
        public TMP_Text menuTxt;

    }

    [Header("Main Dish")]
    public string cmdFoodMenuName = "ch2foodmenu";
    public List<FoodMenu> foodMenu = new List<FoodMenu>();
    public int currentFoodMenuIndex = -1;

    [Header("Appetizer Menu")]
    public string cmdAppeMenuName = "ch2appemenu";
    public List<FoodMenu> appeMenu = new List<FoodMenu>();
    public int currentAppeMenuIndex = -1;

    [Header("Drink")]
    public string cmdDrinkMenuName = "ch2drinkmenu";
    public List<FoodMenu> drinkMenu = new List<FoodMenu>();
    public int currentDrinkMenuIndex = -1;

    [Header("Dessert")]
    public string cmdDessertMenuName = "ch2dessertmenu";
    public List<FoodMenu> dessertMenu = new List<FoodMenu>();
    public int currentDessertMenuIndex = -1;

    public void DessertSelected(int index)
    {
        if (currentDessertMenuIndex == index)
        {
            currentDessertMenuIndex = -1;
            dessertMenu[index].icon.sprite = selectedIcons[0];
            return;
        }
        for (int i = 0; i < dessertMenu.Count; i++)
        {
            dessertMenu[i].icon.sprite = selectedIcons[0];
        }

        currentDessertMenuIndex = index;
        dessertMenu[index].icon.sprite = selectedIcons[1];

    }
    public void AppeSelected(int index)
    {
        if (currentAppeMenuIndex == index)
        {
            currentAppeMenuIndex = -1;
            appeMenu[index].icon.sprite = selectedIcons[0];
            return;
        }

        for (int i = 0; i < appeMenu.Count; i++)
        {
            appeMenu[i].icon.sprite = selectedIcons[0];
        }

        currentAppeMenuIndex = index;
        appeMenu[index].icon.sprite = selectedIcons[1];


    }
    public void DrinkSelected(int index)
    {
        if (currentDrinkMenuIndex == index)
        {
            currentDrinkMenuIndex = -1;
            drinkMenu[index].icon.sprite = selectedIcons[0];
            return;
        }
        for (int i = 0; i < drinkMenu.Count; i++)
        {
            drinkMenu[i].icon.sprite = selectedIcons[0];
        }

        currentDrinkMenuIndex = index;
        drinkMenu[index].icon.sprite = selectedIcons[1];

    }
    public void FoodSelected(int index)
    {

        if (currentFoodMenuIndex == index)
        {
            currentFoodMenuIndex = -1;
            orderBtn.interactable = false;
            foodMenu[index].icon.sprite = selectedIcons[0];
            return;
        }
        for (int i = 0; i < foodMenu.Count; i++)
        {
            foodMenu[i].icon.sprite = selectedIcons[0];
        }

        currentFoodMenuIndex = index;
        foodMenu[index].icon.sprite = selectedIcons[1];
        orderBtn.interactable = true;

    }

    public void ChooseThisMenu()
    {
        if (currentFoodMenuIndex == -1) return;

        float cost = defaultAddCost + foodMenu[currentFoodMenuIndex].price;
        PlayerPrefs.SetString(cmdFoodMenuName, foodMenu[currentFoodMenuIndex].menuName);

        if (cmdAppeMenuName != "")
        {
            if (currentAppeMenuIndex != -1)
            {
                cost += appeMenu[currentAppeMenuIndex].price;
                PlayerPrefs.SetString(cmdAppeMenuName, appeMenu[currentAppeMenuIndex].menuName);
            }
            else PlayerPrefs.SetString(cmdAppeMenuName, "null");
        }
        if (cmdDrinkMenuName != "")
        {
            if (currentDrinkMenuIndex != -1)
            {
                cost += drinkMenu[currentDrinkMenuIndex].price;
                PlayerPrefs.SetString(cmdDrinkMenuName, drinkMenu[currentDrinkMenuIndex].menuName);
            }
            else PlayerPrefs.SetString(cmdDrinkMenuName, "식당생수");
        }
        if (cmdDessertMenuName != "")
        {
            if (currentDessertMenuIndex != -1)
            {
                cost += dessertMenu[currentDessertMenuIndex].price;
                PlayerPrefs.SetString(cmdDessertMenuName, dessertMenu[currentDessertMenuIndex].menuName);
            }
            else PlayerPrefs.SetString(cmdDessertMenuName, "null");
        }
        if (costName != "") PlayerPrefs.SetString(costName, cost.ToString());

        if (currentFoodMenuIndex == 0) dialogueManager.ChangeCurrentBlock(nextBlockSame);
        else dialogueManager.ChangeCurrentBlock(nextBlock);
        thisWhole.SetActive(false);
    }

}
