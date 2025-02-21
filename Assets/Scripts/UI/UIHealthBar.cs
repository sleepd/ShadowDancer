using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    private Image healthBar;
    [SerializeField] PlayerController characterController;
    [SerializeField] float maxWidth = 200f;

    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float hpPercentage = characterController.GetHpPercentage();
        healthBar.rectTransform.sizeDelta = new Vector2(hpPercentage * maxWidth, healthBar.rectTransform.sizeDelta.y);
    }
}
