using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KillCountUpdater : MonoBehaviour
{
    [Header("Kill Count")] 
    public TextMeshProUGUI text;
    public int killCount = 0;

    private void Start()
    {
        EnemyController.enemyDeath += IncrementKillCount;
    }

    private void IncrementKillCount()
    {
        killCount++;

        text.text = killCount.ToString();
    }
}
