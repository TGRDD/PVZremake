using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmouredZombie : Zombie
{
    [SerializeField] private float Armor;

    [Header("Elements ho hide")]
    [SerializeField] private SkinnedMeshRenderer Hand;
    [SerializeField] private SkinnedMeshRenderer[] Head;

    [Header("Elements to instantilate after hide")]
    [SerializeField] private GameObject[] ToSpawn;


    [SerializeField] private GameObject ArmorGO;

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

        if (Armor > 0)
        {
            _audiosource.PlayOneShot(Sounds[1]);
            Armor -= Damage;

            if (Armor <= 0)
            {
                ArmorGO.transform.parent = null;
                ArmorGO.AddComponent<Rigidbody>();
            }

            return;
        }

        _audiosource.PlayOneShot(Sounds[0]);
        Health -= Damage;

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
