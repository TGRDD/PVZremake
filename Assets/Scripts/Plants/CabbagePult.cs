using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabbagePult : Plant, IOffensivePlant
{
    [SerializeField] private bool IsEnemySpotted;
    [SerializeField] private LayerMask ZombieLayer;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject HideWhenAttack;

    private float DistanceToRay;
    private Vector3 spotposition;

    new protected void Start()
    {
        base.Start();
        StartCoroutine(Attack());

        DistanceToRay = CalculateRayLength();
        Debug.Log(DistanceToRay);
    }


    public IEnumerator Attack()
    {
        while (true)
        {
            Ray ray = new Ray(transform.position + Vector3.up, -transform.right);
            RaycastHit hit = new RaycastHit();


            if (Physics.Raycast(ray, out hit, DistanceToRay, ZombieLayer))
            {
                spotposition = hit.point;
                _animator.Play("Shoot");
            }
            yield return new WaitForSecondsRealtime(3.5f);
        }
    }

    public void SpawnProjectile()
    {
        _source.PlayOneShot(sounds[0]);
        GameObject Projectile = Instantiate(Bullet, transform.position + Vector3.up * 1.8f, Quaternion.identity);
        Bullet projectile = Projectile.GetComponent<Bullet>();
        projectile.StartPosition = transform.position;
        projectile.Spot = spotposition;
        projectile.G = Vector3.Distance(spotposition, transform.position)/2;
    }


    public void HideModelProjectile()
    {
        HideWhenAttack.SetActive(false);
    }

    public void ShowModelProjectile()
    {
        HideWhenAttack.SetActive(true);
    }
}
