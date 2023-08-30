using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantChooseSeed : MonoBehaviour
{
    public PlantsData plantData;

    private GameObject PlantsBar;
    [SerializeField] private GameObject PlantbuttonPrefab;

    private Image PlantImage;
    private Text PlantCostText;
    private Button ChooseSeedButton;

    private void Start()
    {

        PlantsBar = GameObject.FindGameObjectWithTag("Panel");

        ChooseSeedButton = GetComponent<Button>();
        PlantCostText = transform.Find("PlantCostText").GetComponent<Text>();
        PlantImage = transform.Find("PlantSprite").GetComponent<Image>();

        PlantImage.sprite = plantData.PlantIcon;
        PlantCostText.text = plantData.PlantCost.ToString();

        ChooseSeedButton.onClick.AddListener(AddPlant);
    }

    private void AddPlant()
    {
        if (PlantingSystem.GetSeedsCount() < 7)
        {
            Instantiate(PlantbuttonPrefab, PlantsBar.transform).GetComponent<PlantIcon>().plantData = this.plantData;
            PlantingSystem.AddSeed();
            Destroy(gameObject);
        }
    }
}
