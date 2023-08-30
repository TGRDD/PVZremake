using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EconomicSystem : MonoBehaviour
{
    public UnityEvent OnSunAdded;

    public int CurrentSun { get; private set; }

    [SerializeField] private Text SunCountText;


    public void AddSun(int count)
    {
        CurrentSun += count;
        SunCountText.text = CurrentSun.ToString();
        OnSunAdded.Invoke();
    }

    public void RemoveSun(int count)
    {
        CurrentSun -= count;
        SunCountText.text = CurrentSun.ToString();
    }

    private IEnumerator SunGiveSun()
    {
        while (true)
        {
            AddSun(25);
            yield return new WaitForSeconds(10);
        }
    }

    public void StartPeriodSunGift()
    {
        StartCoroutine(SunGiveSun());
    }
}
