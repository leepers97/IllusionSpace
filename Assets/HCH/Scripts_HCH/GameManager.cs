using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player_HCH player;
    public Grab_HCH grab;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
