using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Animator _animator;

    [SerializeField] private Transform ButtonsPosition;
    [SerializeField] private GameObject ButtonPrefab;

    private AudioSource _audioSource;
   [SerializeField] private AudioClip _buttonaudio;

    [SerializeField] private TextAsset[] StartLevels;
    

    public void ButtonSound()
    {
        _audioSource.PlayOneShot(_buttonaudio);
    }

    private void Start()
    {
        _animator = _camera.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        GetLevelsData();

    }
    public void StartLevel(string Filename)
    {
        PlayerPrefs.SetString("FileName", Filename);
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenLevelChooser()
    {
        _animator.Play("GoToLevels");
    }

    public void CloseLevelChooser()
    {
        _animator.Play("FlevelsTmenu");
    }

    [ContextMenu("GetData")]
    public void GetLevelsData()
    {
        Debug.Log("StartDataCollecting");

        if (!Directory.Exists(PathCollection.LevelsPath))
        {
            Directory.CreateDirectory(PathCollection.LevelsPath);
        }


        string[] JsonFiles = Directory.GetFiles(PathCollection.LevelsPath, "*.json");
        foreach (string JsonFile in JsonFiles)
        {
            GameObject go = Instantiate(ButtonPrefab, ButtonsPosition);
            go.GetComponent<StartLevelButton>().filename = JsonFile;
            JSONlevelManager.Level level = JsonUtility.FromJson<JSONlevelManager.Level>(File.ReadAllText(JsonFile));
            go.transform.Find("LevelName").GetComponent<Text>().text = level.Name;

        }
    }

    public void ExitFromGame()
    {
        Application.Quit();
    }
}
