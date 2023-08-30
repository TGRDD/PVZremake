using System.Collections;
using UnityEngine;

public class Repeater : Plant, IOffensivePlant
{
    [SerializeField] private bool IsEnemySpotted;
    [SerializeField] private LayerMask ZombieLayer;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private Animator _animator;

    private float DistanceToRay;

    new protected void Start()
    {
        base.Start();
        StartCoroutine(Attack());

        DistanceToRay = CalculateRayLength();
    }


    public IEnumerator Attack()
    {
        while (true)
        {
            Ray ray = new Ray(transform.position + Vector3.up, -transform.right);
            RaycastHit hit = new RaycastHit();


            if (Physics.Raycast(ray, out hit, DistanceToRay, ZombieLayer))
            {
                _animator.Play("RepeaterShoot");

            }
            yield return new WaitForSecondsRealtime(2);
        }
    }

    public void SpawnProjectile()
    {
        _source.PlayOneShot(sounds[0]);
        Instantiate(Bullet, transform.position + Vector3.up, Quaternion.identity);
    }
}
