using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterStats
{
    maxHealth,
    currentHealth,
    maxExp,
    currentExp,
    score,
    level
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public InventoryObject mainInventory;

    // player stats
    public float maxHealth;
    public float currentHealth;
    public float maxExp;
    public float currentExp;
    public float score;
    public int level;
    public bool isAlive = true;

    // player movement
    public float baseSpeed;
    public float baseJumpPower;
    public float maxSpeed;

    // player inventory 나중에 옮기
    public Gun playerGun;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        playerGun = GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        checkLevelUp();
        checkGameOver();
    }

    void checkLevelUp()
    {
        if (maxExp <= currentExp)
        {
            level++;
            currentExp -= maxExp;
        }
    }

    void checkGameOver()
    { 
        if (currentHealth <= 0)
        {
            isAlive = false;
            GameManager.Instance.gameOverSequence();
        }
    }

    public bool AddItemToInventory(Item item, int amount)
    {
        return mainInventory.AddItem(item, amount);
    }

    public void setStats(CharacterStats targetStat, int value)
    {
        switch (targetStat)
        {
            case CharacterStats.currentHealth:
                currentHealth += value;
                break;
            case CharacterStats.maxHealth:
                maxHealth += value;
                break;
            case CharacterStats.maxExp:
                maxExp += value;
                break;
            case CharacterStats.currentExp:
                currentExp += value;
                break;
            case CharacterStats.score:
                score += value;
                break;
            case CharacterStats.level:
                level += value;
                break;
        }
    }


    //Set asset to default state
    private void OnApplicationQuit()
    {
        mainInventory.container.items = new InventorySlot[42];
}
}
