using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public float MaxHealth;
    public float Health;
    public Text HealthText;
    public RectTransform HealthMask;

    float maxWidth;

    public void UpdateHealth(float newHealth)
    {
        Health = newHealth;
        HealthMask.sizeDelta = new Vector2(maxWidth * (Health / MaxHealth), HealthMask.rect.size.y);
        HealthText.text = newHealth.ToString();
    }

	// Use this for initialization
	void Awake ()
    {
        maxWidth = HealthMask.rect.size.x;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
