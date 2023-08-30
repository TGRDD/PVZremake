using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuLoader : MonoBehaviour
{
    public void LoadWinMenu()
    {
        SceneManager.LoadScene("GameOverWin");
    }
}
