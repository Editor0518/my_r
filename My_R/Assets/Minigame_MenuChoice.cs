using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Minigame_MenuChoice : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameObject thisWhole;
    public StoryBlock nextBlockSame;
    public StoryBlock nextBlock;
    public Button orderBtn;
    public Sprite[] selectedIcons;//0: unselected, 1: selected

    [System.Serializable]
    public class FoodMenu
    {
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
        PlayerPrefs.SetString(cmdFoodMenuName, foodMenu[currentFoodMenuIndex].menuTxt.text);
        if (currentAppeMenuIndex != -1) PlayerPrefs.SetString(cmdAppeMenuName, appeMenu[currentAppeMenuIndex].menuTxt.text);
        else PlayerPrefs.SetString(cmdAppeMenuName, "null");
        if (currentDrinkMenuIndex != -1) PlayerPrefs.SetString(cmdDrinkMenuName, drinkMenu[currentDrinkMenuIndex].menuTxt.text);
        else PlayerPrefs.SetString(cmdDrinkMenuName, "식당생수");
        if (currentDessertMenuIndex != -1) PlayerPrefs.SetString(cmdDessertMenuName, dessertMenu[currentDessertMenuIndex].menuTxt.text);
        else PlayerPrefs.SetString(cmdDessertMenuName, "null");

        if (currentFoodMenuIndex == 0) dialogueManager.ChangeCurrentBlock(nextBlockSame);
        else dialogueManager.ChangeCurrentBlock(nextBlock);
        thisWhole.SetActive(false);
    }

}
