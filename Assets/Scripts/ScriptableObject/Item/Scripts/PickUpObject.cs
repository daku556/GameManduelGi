using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Health,
    Mana,
    Exp
}

[CreateAssetMenu(fileName = "new PickUp", menuName = "Item/PickUp")]
public class PickUpObject : ItemObject
{
    public List<EffectType> effects;
    public float effectValue;
    // Start is called before the first frame update
    void Awake()
    {
        this.type = ItemType.PickUp;
    }

    override
    public bool UseItem(PlayerManager player)
    {
        foreach (var effect in effects)
        {
            switch (effect)
            {
                case EffectType.Health:
                    player.currentHealth += Mathf.FloorToInt(effectValue);
                    break;

                case EffectType.Mana:
                    Debug.Log($"마나 {effectValue} 회복!");
                    break;

                case EffectType.Exp:
                    player.currentExp += effectValue;
                    break;
            }
        }
        return true;
    }
}