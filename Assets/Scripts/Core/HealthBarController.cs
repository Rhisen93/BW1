using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField]private Slider slider;
    [SerializeField] LifeController hp;

    public void Start()
    {
     SetMaxHp();   
    }
    public void Update()
    {
        SetHp();
    }
    public void SetMaxHp ()
    {
        slider.maxValue = hp.GetMaxHealth();
        slider.value= hp.GetCurrentHealth();
    }
    public void SetHp()
    {
        slider.value = hp.GetCurrentHealth();
    }
}
