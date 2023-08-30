using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultZombie : Zombie
{
    [Header("Elements ho hide")]
    [SerializeField] private SkinnedMeshRenderer Hand;
    [SerializeField] private SkinnedMeshRenderer[] Head;

    [Header("Elements to instantilate after hide")]
    [SerializeField] private GameObject[] ToSpawn;

    private bool IsAlsoLoseHand;
    private float HalfHealth;

    private void Start()
    {
        HalfHealth = Health / 2;
    }

    private void Update()
    {
        MoveForward();
    }

    public override void TakeDamage(int Damage)
    {

        Health -= Damage;
        _audiosource.PlayOneShot(Sounds[0]);
        if (Health < HalfHealth && !IsAlsoLoseHand) 
        {
            Hand.enabled = false;
            IsAlsoLoseHand = true;
            Instantiate(ToSpawn[0], Hand.gameObject.transform.position, Hand.transform.rotation);

        }

        if (Health < 0)
        {
            Head[0].enabled = false;
            Head[1].enabled = false;
            Instantiate(ToSpawn[1], Head[0].gameObject.transform.position, Hand.transform.rotation);
            base._animator.Play("Death");
        }
    }
}
