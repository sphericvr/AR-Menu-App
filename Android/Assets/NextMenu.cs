﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMenu : MonoBehaviour {

    public GameObject menuItemPrefab;

    private Dictionary<int, string[]> menu = new Dictionary<int, string[]>();
    private int currentMenuItem = -1;
    private string[] menu_items = new string[] { "Bowl", "Base", "Protein", "Toppings", "Sauce" };

    private bool busy = false;

    void Start()
    {
        menuItemPrefab = Resources.Load("Menu Item", typeof(GameObject)) as GameObject;
        string[] bowls = new string[] { "Small Bowl", "Large Bowl" };
        menu.Add(0, bowls);
        string[] bases = new string[] { "White Rice", "Brown Rice", "Vermicelli Noodles" };
        menu.Add(1, bases);
        string[] protein = new string[] { "Chicken", "Beef", "Tofu" };
        menu.Add(2, protein);
        string[] toppings = new string[] { "Carrots", "Cucumbers", "Do Chua", "Jalapenos", "Peanuts", "Pickled Daikon", "Scallions", "Sprouts" };
        menu.Add(3, toppings);
        string[] sauces = new string[] { "Lime Fish Sauce", "Peanut Sauce", "Ponzu Ginger Sauce", "Soy Sauce" };
        menu.Add(4, sauces);

        NextMenuItems();
    }

    public void NextMenuItems()
    {
        if (currentMenuItem + 1 < menu_items.Length)
        {
            if (currentMenuItem >= 0)
            {
                StartCoroutine(ExitMenuItems("Right"));
            }
            StartCoroutine(EnterMenuItems("Left"));
        }
        else
        {
            ErrorMenuItemsRight();
        }
    }

    public void PreviousMenuItems()
    {
        if (currentMenuItem > 0)
        {
            StartCoroutine(ExitMenuItems("Left"));
            StartCoroutine(EnterMenuItems("Right"));
        }
        else
        {
            ErrorMenuItemsLeft();
        }
    }

    private IEnumerator EnterMenuItems(string dir)
    {
        while (busy)
        {
            yield return new WaitForSeconds(0.1f);
        }
        busy = true;
        if (dir == "Left")
        {
            currentMenuItem++;
        }
        for (int i = 0; i < menu[currentMenuItem].Length; i++)
        {
            if (dir == "Left")
            {
                StartCoroutine(StartEnterMenuItem(i, dir));
            }

            if (dir == "Right")
            {
                StartCoroutine(StartEnterMenuItem(menu[currentMenuItem].Length - 1 - i, dir));
            }
            yield return new WaitForSeconds(0.25f);
        }
        busy = false;
    }

    private IEnumerator StartEnterMenuItem(int i, string dir)
    {
        GameObject child = Instantiate(menuItemPrefab);
        child.transform.parent = transform;

        MenuItemManager manager = child.GetComponent<MenuItemManager>();
        MenuItemPrefabs prefabs = transform.GetComponent<MenuItemPrefabs>();

        manager.itemPrefab = prefabs.GetPrefab(menu[currentMenuItem][i]);
        manager.angle = 360 - (360 - 30 * (menu[currentMenuItem].Length - 1)) / 2 - 30 * i;
        manager.type = prefabs.GetType(menu[currentMenuItem][i]);
        manager.repeat = prefabs.GetRepeat(menu[currentMenuItem][i]);
        manager.price = prefabs.GetPrice(menu[currentMenuItem][i]);
        manager.calories = prefabs.GetCalories(menu[currentMenuItem][i]);
        manager.recommended = prefabs.GetRecommendation(menu[currentMenuItem][i]);
        manager.allergic = prefabs.GetAllergic(menu[currentMenuItem][i]);

        if (dir == "Left")
        {
            StartCoroutine(child.GetComponent<MenuItemManager>().EnterLeft());
        }
        else
        {
            child.transform.SetAsFirstSibling();
            StartCoroutine(child.GetComponent<MenuItemManager>().EnterRight());
        }
        yield return null;
    }

    private IEnumerator ExitMenuItems(string dir)
    {
        while (busy)
        {
            yield return new WaitForSeconds(0.1f);
        }
        busy = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (dir == "Left")
            {
                StartCoroutine(StartExitMenuItem(transform.childCount - 1 - i, dir));
            }

            if (dir == "Right")
            {
                StartCoroutine(StartExitMenuItem(i, dir));
            }
            yield return new WaitForSeconds(0.25f);
        }
        if (dir == "Left")
        {
            currentMenuItem--;
        }
        busy = false;
    }

    private IEnumerator StartExitMenuItem(int i, string dir)
    {
        if (dir == "Left")
        {
            StartCoroutine(transform.GetChild(i).GetComponent<MenuItemManager>().ExitLeft());
        }
        else
        {
            StartCoroutine(transform.GetChild(i).GetComponent<MenuItemManager>().ExitRight());
        }
        yield return null;
    }

    private void ErrorMenuItemsLeft()
    {

    }

    private void ErrorMenuItemsRight()
    {

    }
}