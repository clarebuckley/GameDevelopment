using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour, InteractiveObjectBase
{
    [SerializeField] GameObject weaponType;

    public void OnInteraction()
    {
        GameStatsController.Weapon = weaponType.name;
        Debug.Log(weaponType.name);
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
        foreach(GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        weaponType.SetActive(true);
        Debug.Log(GameStatsController.Weapon);

    }
}
