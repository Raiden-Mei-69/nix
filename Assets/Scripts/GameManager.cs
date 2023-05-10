using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Save;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public Transform lootHolder;
    Player.PlayerController[] players;
    public static GameManager Instance;
    //public DiscordRich.DiscordRichManager discord=new();
    private void Awake()
    {
        Instance = this;
        lootHolder=new GameObject("loot-holder").transform;
        lootHolder.parent = transform;
        players=FindObjectsOfType<Player.PlayerController>();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].id = i;
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
#endif
    }

#if UNITY_EDITOR
    private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
    {
        if (obj == PlayModeStateChange.ExitingPlayMode)
        {
            //discordRichManager.Close().ConfigureAwait(true).GetAwaiter().GetResult();
        }
    }
#endif

    private void Update()
    {
        //discordRichManager.OnUpdate();
    }

    void Verify()
    {
        bool discordRunning = false;
        // loops through open processes
        for (int i = 0; i < System.Diagnostics.Process.GetProcesses().Length; i++)
        {
            // checks if current process is discord
            if (System.Diagnostics.Process.GetProcesses()[i].ToString() == "System.Diagnostics.Process (Discord)")
            {
                discordRunning = true;
                break;
            }
        }
        if (discordRunning)
        {
            Debug.Log("Discord is running");
        }
        else
        {
            Debug.Log("Discord is not running");
        }
    }

    /// <summary>
    /// Put the game on pause or not
    /// </summary>
    /// <param name="state">true if paused, false for not</param>
    public void PauseGame(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }
}
