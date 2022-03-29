using UnityEngine;
using UnityEngine.UI;

public class GenericInputController : MonoBehaviour
{
    [SerializeField] private Texture2D cursorIcon, clickIcon;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button timeScaleButton;
    [SerializeField] private Button resetButton;

    void Update()
    {
        if (Input.GetButton("Fire1") && MainSceneManager.Instance.CursorVisible())
            Cursor.SetCursor(clickIcon, Vector2.zero, CursorMode.Auto);

        if (Input.GetButtonUp("Fire1") && MainSceneManager.Instance.CursorVisible())
            Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);

        if (Input.GetButtonDown("Jump") && !MainSceneManager.Instance.Configuration)
            pauseButton.onClick.Invoke();

        if(Input.GetButtonDown("Fire3") && !MainSceneManager.Instance.Configuration)
            timeScaleButton.onClick.Invoke();

        if (Input.GetButtonDown("Fire2"))
            MainSceneManager.Instance.SwitchCamera();

        if (Input.GetButtonDown("Fire4") && !MainSceneManager.Instance.Configuration)
            resetButton.onClick.Invoke();
    }
}
