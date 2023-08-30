using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlantIcon : MonoBehaviour
{
    public PlantsData plantData;

    [SerializeField] private GameObject ChooseSeedPrefab;
    private Transform ChoosePanel;

    [SerializeField] private Slider CoolDownIndicator;

    private Image PlantImage;
    private Text PlantCostText;
    private Button SpawnPlant;
    private PlantingSystem plantingSystem;
    private SelectedPlantSprite selectedview;

    private AudioSource _source;
    private void Start()
    {
        selectedview = GameObject.FindObjectOfType<SelectedPlantSprite>();

        _source = GetComponent<AudioSource>();
        ChoosePanel = GameObject.FindGameObjectWithTag("Panel2").transform;

        plantingSystem = GameObject.FindObjectOfType<PlantingSystem>();
        SpawnPlant = GetComponent<Button>();
        PlantCostText = transform.Find("PlantCostText").GetComponent<Text>();
        PlantImage = transform.Find("PlantSprite").GetComponent<Image>();

        PlantImage.sprite = plantData.PlantIcon;
        PlantCostText.text = plantData.PlantCost.ToString();

        SpawnPlant.onClick.AddListener(SetAPlant);
        
        CoolDownIndicator.gameObject.SetActive(false);
    }

    public void SetAPlant()
    {
        if (PlantingSystem.IsGameStarted)
        {
            plantingSystem.SetObjectToSpawn(plantData.FixDelta, plantData, plantData.PlantPrefab, this);
            plantingSystem.IsPlanting = true;
            _source.Play();
            
            selectedview.Activate();
            selectedview.SetSprite(plantData.PlantIcon);
        }
        else
        {
            //RemovePlant
            PlantingSystem.RemoveSeed();
            Instantiate(ChooseSeedPrefab, ChoosePanel).GetComponent<PlantChooseSeed>().plantData = this.plantData;
            Destroy(gameObject);
            _source.Play();
        }
    }
    
    public IEnumerator GoToCoolDown()
    {
        StartCoroutine(ActivateCoolDownIndicator());
        SpawnPlant.enabled = false;
        yield return new WaitForSeconds(plantData.CoolDownTime);
        SpawnPlant.enabled = true;
    }

    public void StartCoolDown()
    {
        StartCoroutine(GoToCoolDown());
    }

    public IEnumerator ActivateCoolDownIndicator()
    {
        CoolDownIndicator.gameObject.SetActive(true);
        CoolDownIndicator.value = 100;
        float Speed;

        Speed = 100 / plantData.CoolDownTime;

        while (CoolDownIndicator.value > 0)
        {
            CoolDownIndicator.value -= Speed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        CoolDownIndicator.gameObject.SetActive(false);
    }
}
