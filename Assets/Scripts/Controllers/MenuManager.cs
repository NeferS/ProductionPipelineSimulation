using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorIcon, clickIcon;
    [SerializeField] private GameObject informationPane;
    private bool information;

    void Awake()
    {
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
        information = false;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
            Cursor.SetCursor(clickIcon, Vector2.zero, CursorMode.Auto);
        if (Input.GetButtonUp("Fire1"))
            Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
    }

    public void LoadMain()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void ToggleInstructions()
    {
        informationPane.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        information = !information;
        informationPane.SetActive(information);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
