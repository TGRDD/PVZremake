using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public UnityEvent StartGameWinAction;
    private void OnEnable()
    {
        ZombieWaveSystem.AllZombiesDied += OnGameWinned;
    }

    private void OnDisable()
    {
        ZombieWaveSystem.AllZombiesDied -= OnGameWinned;
    }


    private void OnGameWinned()
    {
        StartGameWinAction.Invoke();
    }
}
