using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    private Slider healthSlider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    void Awake()
    {
        healthSlider = GetComponent<Slider>();
    }
    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = Camera.main.transform.rotation;
    }
    public void UpdateHealthSlider(float currentValue, float maxValue)
    {
        healthSlider.value = currentValue / maxValue;
    }
}
