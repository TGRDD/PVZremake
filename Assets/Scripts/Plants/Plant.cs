using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Plant : MonoBehaviour
{

    [SerializeField] protected float Health;
    protected int Damage;

    [SerializeField] LayerMask ScannerLayer;
    [SerializeField] protected LayerMask BlockLayer;
    [SerializeField] private Block _block;

    protected PlantingSystem _system;

    protected AudioSource _source;

    [SerializeField] protected AudioClip[] sounds;

    protected virtual void Start()
    {
        _system = GameObject.FindObjectOfType<PlantingSystem>();
        _source = GetComponent<AudioSource>();
        _source.Play();
    }

    public void SetBlock(Block block)
    {
        block = _block;
    }

    public void Death()
    {
        Ray ray = new Ray(transform.position + Vector3.up, -transform.up);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit, 10f, BlockLayer))
        {
            hit.collider.GetComponent<Block>().Clear();
        }
        _system.DisableAll();
        Destroy(gameObject);
    }

    private void OnMouseOver()
    {
        //Debug.Log("IAM");
        if (_system.IsShowelMod && Input.GetMouseButton(0))
        {
            Death();
        }   
    }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Death();
        }
    }

    protected float CalculateRayLength()
    {
        Ray ray = new Ray(transform.position, Vector3.forward);

        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 1000f, ScannerLayer))
        {
            Debug.Log("Spot");
            return hit.distance;
        }
        else
        {
            return 0;
            throw new Exception("Plant could not found the scan block");
        }
    }
}
