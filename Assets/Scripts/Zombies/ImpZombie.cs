using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ImpZombie : Zombie
{
    private bool IsEndFly;
    private void Start()
    {
        StartCoroutine(FlyToTarget());
        _collider.enabled = false;
    }

    private void Update()
    {
        if (IsEndFly)
        {
            MoveForward();
        }
    }

    private IEnumerator FlyToTarget()
    {
        Vector3 target = new Vector3(transform.position.x, -7.43f, -2.88f);
        float DistanceToSpot = Vector3.Distance(transform.position, target);
        float G = DistanceToSpot/3;
        float flyspeed = 10f;
        float CurrentDistance = 200f;

        while (CurrentDistance > 13)
        {
            if (G < 0) G = 1;
            CurrentDistance = Vector3.Distance(transform.position, target);
            Debug.Log(CurrentDistance);

            if (Vector3.Distance(new Vector3(transform.position.x, -7.43f, transform.position.z), target) > DistanceToSpot / 2)
            {
                G -= Time.deltaTime * 12;
                transform.position += ((-Vector3.forward * flyspeed) + (Vector3.up * G)) * Time.deltaTime;
            }
            else
            {
                G += Time.deltaTime * 12;
                transform.position += ((-Vector3.forward * flyspeed) + (-Vector3.up * G)) * Time.deltaTime;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, 1.443f, transform.position.z);
        _collider.enabled = true;
        IsEndFly = true;
        _animator.Play("ImpWalk");
    }
    

}
