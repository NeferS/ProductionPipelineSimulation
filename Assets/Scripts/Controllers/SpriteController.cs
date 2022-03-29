using UnityEngine;
using UnityEngine.UI;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private int currentSprite = 0;
    private Image innerImage;

    private Button button;
    private Color[] colors = new Color[2];

    void Start()
    {
        innerImage = GetComponentInChildren<Image>(true);
        innerImage.sprite = sprites[0];

        button = GetComponentInParent<Button>();
        colors[0] = button.colors.normalColor;
        colors[1] = button.colors.pressedColor;
    }

    public void Clicked()
    {
        currentSprite = (currentSprite + 1) % sprites.Length;
        innerImage.sprite = sprites[currentSprite];
        ColorBlock cb = button.colors;
        cb.normalColor = colors[currentSprite];
        cb.selectedColor = colors[currentSprite];
        button.colors = cb;
    }

    public void ResetSelf()
    {
        currentSprite = sprites.Length - 1;
        Clicked();
    }
}
