using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UISwitch : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();
    private void OnOpenInventory(InputValue value)
    {
        foreach (GameObject target in targets)
        {
            bool isActive = target.activeSelf;
            target.SetActive(!isActive);
        }
    }
}
