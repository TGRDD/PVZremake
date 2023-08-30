using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLevelButton : MonoBehaviour
{
    private Button button;
    public string filename;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(StartLevel);
    }

    public void StartLevel()
    {

        PlayerPrefs.SetString("FileName", filename);
        SceneManager.LoadScene("SampleScene");

    }
}
