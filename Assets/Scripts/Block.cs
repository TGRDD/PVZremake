using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private Material[] materials;
    private MeshRenderer meshRenderer;
    private PlantingSystem plantingSystem;
    public bool IsPlantPlaced;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        plantingSystem = GameObject.FindObjectOfType<PlantingSystem>();
    }

    private void OnMouseEnter()
    {
        if (plantingSystem.IsPlanting)
        {

            if (!IsPlantPlaced)
            {
                plantingSystem.SetBlockToSpawn(transform.position + Vector3.up, this);
                meshRenderer.material = materials[1];
            }
            else
            {
                meshRenderer.material = materials[2];
            }
        }
    }

    private void OnMouseExit()
    {
        meshRenderer.material = materials[0];
        plantingSystem.BlockDeselected();
    }

    public void Clear()
    {
        IsPlantPlaced = false;
    }
}
