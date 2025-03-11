using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public enum Infotype { Exp, Level, kill, Time, Health, Speed, Score };
    public Infotype type;
    private TextMeshProUGUI text;
    private Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (type)
        {
            case Infotype.Exp:
                float curExp = PlayerManager.Instance.currentExp;
                float maxExp = PlayerManager.Instance.maxExp;
                slider.value = curExp / maxExp;
                break;
            case Infotype.Level:
                text.SetText("Level : " + PlayerManager.Instance.level);
                break;
            case Infotype.kill:
                break;
            case Infotype.Time:
                break;
            case Infotype.Health:
                float curHp = PlayerManager.Instance.currentHealth;
                float maxHp = PlayerManager.Instance.maxHealth;
                slider.value = curHp / maxHp;
                break;
            case Infotype.Speed:
                break;
            case Infotype.Score:
                text.SetText("Score" + PlayerManager.Instance.score);
                break;
            default:
                break;
        }
    }
}
