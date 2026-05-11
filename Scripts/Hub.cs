using UnityEngine;
using UnityEngine.UI;

public class Hub : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
    }

    void Start()
    {
        if (type == InfoType.Health && mySlider != null)
        {
            mySlider.minValue = 0f;
            mySlider.maxValue = GameManager.instance.MaxHealth;
        }
    }

    void LateUpdate()
    {
        if (GameManager.instance == null)
            return;

        switch (type)
        {
            case InfoType.Exp:
                if (mySlider != null)
                {
                    float curExp = GameManager.instance.exp;
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                    mySlider.value = maxExp <= 0 ? 0 : curExp / maxExp;
                }
                break;
            case InfoType.Level:
                if (myText != null)
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                if (myText != null)
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                if (myText != null)
                {
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    myText.text = string.Format("{0:D1}:{1:D2}", min, sec);
                }
                break;
            case InfoType.Health:
                if (mySlider != null)
                    mySlider.value = GameManager.instance.health;
                break;
        }
    }
}
