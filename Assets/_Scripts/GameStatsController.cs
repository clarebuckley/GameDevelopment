using System;
using UnityEngine;

public  class GameStatsController : MonoBehaviour
{
    private static String weapon, timeTaken = "--:--";
    private static int health = 16;
    private static int score = 0;
    private static readonly int weaponHit;


    public static int WeaponHit
    {
        get
        {
            if (weapon == "Ladle-weapon")
            {
                return 2;
            }
            else if(weapon == "Pan-weapon")
            {
                return 4;
            }
            else if (weapon== "Knife-weapon")
            {
                return 7;
            }
            else
            {
                return 1;
            }
              
        }
    }
  
    public static String Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            Debug.Log(value);
            weapon = value;
        }
    }

    public static int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public static int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }

    public static String TimeTaken
    {
        get
        {
            return timeTaken;
        }
        set
        {
            timeTaken = value;
        }
    }
}
