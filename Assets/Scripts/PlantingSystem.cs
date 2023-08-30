using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantingSystem : MonoBehaviour
{

    public static Action StartGameEvent;
    public static bool IsGameStarted;
    
    private static int SeedsCount;

    private Camera _camera;
    
    private EconomicSystem _ecosystem;


    public PlantsData _plantData;
    private GameObject _objtospawn;
    private PlantIcon _setter;

    private Vector3 _position;
    private Block _block;
    private bool IsBlockSelected;

    public bool IsPlanting;
    public bool IsShowelMod;

    private Vector3 FixDelta;

    [SerializeField] private AudioClip[] _music;
    private AudioSource _source;
    [SerializeField] private Sprite ShowelTexture;
    [SerializeField] private AudioClip ShowelSound;
    [SerializeField] private GameObject ShowelGO;
    private SelectedPlantSprite selectedview;

    [SerializeField] private GameObject SeedsPanel;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _ecosystem = GetComponent<EconomicSystem>();
        _source = GetComponent<AudioSource>();
        _source.clip = _music[0];
        selectedview = GameObject.FindObjectOfType<SelectedPlantSprite>();
    }

    public void SetObjectToSpawn(Vector3 FixDelta, PlantsData data, GameObject obj, PlantIcon Setter)
    {
        _plantData = data;
        _objtospawn = obj;
        this.FixDelta = FixDelta;
        _setter = Setter;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) { DisableAll(); }

        if ((_objtospawn != null && IsPlanting) && IsBlockSelected)
        {
            if (Input.GetMouseButtonDown(0) && (_ecosystem.CurrentSun >= _plantData.PlantCost))
            {
                GameObject go = Instantiate(_plantData.PlantPrefab, _position + FixDelta, Quaternion.Euler(0, 90, 0));
                _block.IsPlantPlaced = true;
                _ecosystem.RemoveSun(_plantData.PlantCost);
                _setter.StartCoolDown();
                DisableAll();

                selectedview.Deactivate();
            }
        }
    }

    public void SetBlockToSpawn(Vector3 pos, Block block)
    {
        _position = pos;
        IsBlockSelected = true;
        _block = block;
    }

    public void BlockDeselected()
    {
        IsBlockSelected = false;
    }

    public void EnterShowelMode()
    {
        DisableAll();
        selectedview.SetSprite(ShowelTexture);
        selectedview.Activate();
        _source.PlayOneShot(ShowelSound);
        ShowelGO.SetActive(false);
        IsShowelMod = true;
    }

    public void DisableAll()
    {
        IsPlanting = false;
        IsShowelMod = false;
        ShowelGO.SetActive(true);
        selectedview.Deactivate();
    }

    public void StartGame()
    {
        if (SeedsCount > 5)
        {
            IsGameStarted = true;
            StartGameEvent.Invoke();
            GameObject.FindObjectOfType<PopUpTextManager>().PlayText("Plant");
            _source.clip = _music[1];
            _source.Play();
            SeedsPanel.SetActive(false);
            _ecosystem.StartPeriodSunGift();
        }
    }

    public static int GetSeedsCount()
    {
        Debug.Log("SeedsCount " + SeedsCount);
        return SeedsCount;
    }

    public static void AddSeed()
    {
        SeedsCount++;
    }

    public static void RemoveSeed()
    {
        SeedsCount--;
    }


}
