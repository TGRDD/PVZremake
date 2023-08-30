using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nut : Plant
{
    private float FirstStage;
    private float SecondStage;
    private float ThirdStage;

    [SerializeField] private Material[] StagesMat;
    [SerializeField] private MeshRenderer Mesh;

    protected override void Start()
    {
        base.Start();
        float ThirdPart = Health / 3;

        FirstStage = Health;
        SecondStage = Health - ThirdPart;
        ThirdStage = Health - ThirdPart * 2;

        Mesh.material = StagesMat[0];
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health < SecondStage && Health > ThirdStage)
        {
            Mesh.material = StagesMat[1];
        }
        else if (Health < ThirdStage)
        {
            Mesh.material = StagesMat[2];
        }


        if (Health < 0)
        {
            Death();
        }
    }

}
