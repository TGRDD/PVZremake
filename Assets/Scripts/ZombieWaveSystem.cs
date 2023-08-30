using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieWaveSystem : MonoBehaviour
{
    public static Action AllZombiesDied;

    [SerializeField] Transform[] SpawnPoints;
    [SerializeField] GameObject[] AllZombies;
    [SerializeField] List<GameObject> ZombiesOnLevel;
    [SerializeField] GameObject[] Zombies;
    [SerializeField] private List<Zombie> ChachedZombiesList;
    [SerializeField] private Zombie[] ChachedZombies;

    [SerializeField] private Slider WaveSlider;
    [SerializeField] private Transform FlagInstantilatePosition;
    [SerializeField] private GameObject FlagPrefab;


    private JSONlevelManager _jsonlevelmanager;
    private JSONlevelManager.Level CurrentLevel;

    private int ZombiesCount;
    private bool IsWavesEnded;

    private PopUpTextManager PopUp;

    private AudioSource _audio;
    [SerializeField] private AudioClip s_finalwave;
    [SerializeField] private AudioClip s_wavesstarted;
    
    private int GargantuaLimitForWave;
    private int MaxGargantuaCount = 2;

    private void Start()
    {
        PopUp = FindObjectOfType<PopUpTextManager>();
        _audio = GetComponent<AudioSource>();

        _jsonlevelmanager = GetComponent<JSONlevelManager>();

        _jsonlevelmanager.LoadLevel(PlayerPrefs.GetString("FileName"));
        CurrentLevel = _jsonlevelmanager.level;

        for (int i = 0; i < CurrentLevel.TypesOnLevel.Length; i++)
        {
            ZombiesOnLevel.Add(AllZombies[Convert.ToInt32(CurrentLevel.TypesOnLevel[i])]);
        }

        Zombies = ZombiesOnLevel.ToArray();

        GameObject.FindObjectOfType<EconomicSystem>().AddSun(CurrentLevel.StartSun);


        //ChacheZombie
        foreach (GameObject item in Zombies)
        {
            ChachedZombiesList.Add(item.GetComponent<Zombie>());
        }
        ChachedZombies = ChachedZombiesList.ToArray();
        //
    }

    private void OnEnable()
    {
        PlantingSystem.StartGameEvent += OnGameStarted;
        Zombie.OnDied += OnZombieDied;
    }

    private void OnDisable()
    {
        PlantingSystem.StartGameEvent -= OnGameStarted;
        Zombie.OnDied -= OnZombieDied;
    }

    private void OnGameStarted()
    {
        StartCoroutine(StartWaves());
    }

    private void OnZombieDied()
    {
        ZombiesCount--;

        if (IsWavesEnded && ZombiesCount == 0)
        {
            Debug.Log("Win");
            AllZombiesDied.Invoke();
        }
    }

    [ContextMenu("Win")]
    private void AutoWin()
    {
        AllZombiesDied.Invoke();
    }

    public void RecalculateAndSetFlags()
    {
        float WaveDelta = 100 / CurrentLevel.WavesCount;
        WaveSlider.value = 100;
        for (int i = 0; i < CurrentLevel.WavesCount; i++)
        {
            WaveSlider.value -= WaveDelta;
            Instantiate(FlagPrefab, FlagInstantilatePosition.position, Quaternion.identity, GameObject.FindObjectOfType<Canvas>().transform);
        }

        WaveSlider.value = 100;

    }

    IEnumerator StartWaves()
    {
        RecalculateAndSetFlags();
        ZombiesCount = 0;

        int HardForWave = CurrentLevel.ZombieIncreasingQuantity;
        float DangerDelta = CalculateZombieDangerDelta();
        float CurrentDangerLevel = CurrentMinDangerLevel();

        float WaveDelta = 100 / CurrentLevel.WavesCount;
        float HalfWaveDelta = WaveDelta / 2;

        Debug.Log(DangerDelta + " DELTA");
        Debug.Log(CurrentDangerLevel + " Current");

        int FinalWave = CurrentLevel.WavesCount;
        int CurrentWave = 0;

        //Prepairing

        if (CurrentLevel.StartWithDefaultZombie)
        {
            yield return new WaitForSeconds(10);
            SpawnZombieFromAllZombiesArray(0);
            _audio.PlayOneShot(s_wavesstarted);

            float tmptime = 0;

            while (ZombiesCount != 0 || tmptime < 10)
            {
                tmptime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }

        }

        //Start Main system
        for (int i = 0; i < CurrentLevel.WavesCount; i++)
        {
            // Small waves
            for (int j = 0; j < 2; j++)
            {
                for (int k = 0; k < UnityEngine.Random.Range(3 + HardForWave, 5 + HardForWave); k++)
                {
                    SpawnRandomZombieWithCurrentDangerLevel(CurrentDangerLevel);
                    yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1, 3));
                }
                while (ZombiesCount > 2 + HardForWave) { yield return new WaitForSeconds(Time.deltaTime); }

                StopCoroutine(SmoothChangeSliderValue(0));
                StartCoroutine(SmoothChangeSliderValue(HalfWaveDelta));
            }
            //
            CurrentDangerLevel += DangerDelta * 1.5f;
            // Big wave
            Debug.Log("BigWave");


            GargantuaLimitForWave = 0;
            MaxGargantuaCount++;

            PopUp.PlayText("A big wave of zombies is coming");
            CurrentWave += 1;
            _audio.Play();

            if (CurrentWave == FinalWave)
            {
                HardForWave += 10;
                yield return new WaitForSeconds(2);
                PopUp.PlayText("FINAL WAVE");
                _audio.PlayOneShot(s_finalwave);
            }


            for (int k = 0; k < UnityEngine.Random.Range(10 + HardForWave, 15 + HardForWave); k++)
            {
                SpawnRandomZombieWithCurrentDangerLevel(CurrentDangerLevel);
                yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(1, 3));
            }
            while (ZombiesCount > 5 + HardForWave) { yield return new WaitForSeconds(Time.deltaTime); }
            //
            HardForWave += 3;


        }

        yield return null;
        IsWavesEnded = true;
    }

    public void SpawnRandomZombieWithCurrentDangerLevel(float dangerlevel)
    {
        ZombiesCount++;

        int rand = UnityEngine.Random.Range(0, Zombies.Length);
        while (ChachedZombies[rand].GetDangerLevel() > dangerlevel)
        {
            rand = UnityEngine.Random.Range(0, Zombies.Length);
            if (Zombies[rand].GetComponent<GarganuaZombie>() != null && GargantuaLimitForWave > MaxGargantuaCount)
            {
                rand--;
            } else if (Zombies[rand].GetComponent<GarganuaZombie>() != null && GargantuaLimitForWave < MaxGargantuaCount)
            {
                GargantuaLimitForWave++;
            }
        }

        Instantiate(Zombies[rand], SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)].position, Quaternion.Euler(0, -90, 0), null);
    }

    public void SpawnZombieFromAllZombiesArray(int id)
    {
        ZombiesCount++;
        Instantiate(AllZombies[id], SpawnPoints[UnityEngine.Random.Range(0, SpawnPoints.Length)].position, Quaternion.Euler(0, -90, 0), null);
    }

    public IEnumerator SmoothChangeSliderValue(float Delta)
    {
        float FinalValue = WaveSlider.value - Delta;
        while (WaveSlider.value > FinalValue)
        {
            WaveSlider.value -= Time.deltaTime * 2;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;
    }

    private float CalculateZombieDangerDelta()
    {
        int mindanger = 999;
        int maxdanger = -1;

        foreach (Zombie item in ChachedZombies)
        {
            int CurrentDangerLevel = item.GetDangerLevel();
            if (mindanger > CurrentDangerLevel)
            {
                mindanger = CurrentDangerLevel;
            }

            if (maxdanger < CurrentDangerLevel)
            {
                maxdanger = CurrentDangerLevel;
            }

        }
        float WavesToFloat = CurrentLevel.WavesCount;

        return (maxdanger - mindanger) / WavesToFloat;

    }

    private int CurrentMinDangerLevel()
    {
        int CurrentDangerLevel = 999;

        foreach (Zombie item in ChachedZombies)
        {
            int tmp = item.GetDangerLevel();
            if (CurrentDangerLevel > tmp)
            {
                CurrentDangerLevel = tmp;
            }
        }

        return CurrentDangerLevel;
    }

}

