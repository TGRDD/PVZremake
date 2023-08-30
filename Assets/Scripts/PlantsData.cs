using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Plant", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class PlantsData : ScriptableObject
{
    public string PlantName;
    public int PlantCost;
    public Sprite PlantIcon;
    public GameObject PlantPrefab;
    public Vector3 FixDelta;
    public float CoolDownTime;
}
