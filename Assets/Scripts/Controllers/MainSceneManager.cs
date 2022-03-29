using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    [Serializable]
    public struct UIElemPanes
    {
        public GameObject scrollable;
        public GameObject content;
        public GameObject conveyorsConfig;
        public GameObject pipelineConfig;
        public GameObject pipelineConfigContent;
        public RectTransform blockFrame;
        public RectTransform pipelineFrame;
        public RectTransform pipelineInfoRow;
    }
    [SerializeField] public UIElemPanes elementPanes;

    [SerializeField] private GameObject configurationHide;
    [SerializeField] private GameObject startButton, menuButton, conveyorsButton;
    [SerializeField] private Texture2D cursorIcon;
    [SerializeField] private Camera[] cameras;
    private int currentCamera;

    private bool pauseSimulation = false;
    private float scaledTime = 3f;
    private ConveyorsController conveyorsController;

    public bool Configuration { get; private set; }
    public bool MovementAllowed { get; private set; }
    public float TimeScale { get; private set; }
    public float TimeMultiplier { get; private set; }
    public BlockObserver BlockObs { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        BlockObs = GetComponent<BlockObserver>();
        conveyorsController = GetComponent<ConveyorsController>();
        pauseSimulation = true;
        TimeScale = 0f;
        configurationHide.SetActive(false);
        TimeMultiplier = 1f;
        MovementAllowed = false;
        Configuration = true;
        currentCamera = 0;
        Cursor.SetCursor(cursorIcon, Vector2.zero, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void TriggerConveyorsConfig() 
    { 
        elementPanes.conveyorsConfig.SetActive(!elementPanes.conveyorsConfig.activeSelf);
        if (conveyorsController != null)
        {
            if (elementPanes.conveyorsConfig.activeSelf)
                conveyorsController.OnActivateSelf();
            else
                conveyorsController.OnDeactivateSelf();
        }
    }

    public void BeginSimulation()
    {
        startButton.SetActive(false);
        elementPanes.pipelineConfig.SetActive(false);
        elementPanes.conveyorsConfig.SetActive(false);
        conveyorsController.OnDeactivateSelf();
        conveyorsButton.SetActive(false);
        menuButton.SetActive(true);
        configurationHide.SetActive(true);
        Configuration = false;
        DoPause();
        if(currentCamera == 0)
        {
            cameras[currentCamera].gameObject.SetActive(false);
            currentCamera = 1;
        }
        SwitchCamera();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void DoPause()
    {
        if (!pauseSimulation)
            TimeScale = 0f;
        else
            TimeScale = 1f;
        pauseSimulation = !pauseSimulation;
    }

    public void ScaleTime()
    {
        TimeMultiplier = TimeMultiplier == 1f ? scaledTime : 1f;
    }

    public bool CursorVisible() { return currentCamera < cameras.Length - 1; }

    public void SwitchCamera()
    {
        cameras[currentCamera].gameObject.SetActive(false);
        if (Configuration)
            currentCamera = 1 - currentCamera;
        else
            currentCamera = (currentCamera + 1) % cameras.Length;
        MovementAllowed = currentCamera == 2;
        Cursor.visible = !MovementAllowed;
        Cursor.lockState = MovementAllowed ? CursorLockMode.Locked : CursorLockMode.Confined;
        cameras[currentCamera].gameObject.SetActive(true);
    }

    public void ResetSimulation()
    {
        Configuration = true;
        cameras[currentCamera].gameObject.SetActive(false);
        currentCamera = 1;
        SwitchCamera();
        menuButton.SetActive(false);
        elementPanes.scrollable.SetActive(false);
        startButton.SetActive(true);
        conveyorsButton.SetActive(true);
        pauseSimulation = false;
        DoPause();
        configurationHide.SetActive(false);
        TimeMultiplier = 1f;

        PipelineElement[] pipelineElements = GameObject.FindObjectsOfType<PipelineElement>();
        foreach (PipelineElement elem in pipelineElements)
            elem.ResetSelf();

        Conveyor[] conveyors = GameObject.FindObjectsOfType<Conveyor>();
        foreach (Conveyor conv in conveyors)
            conv.ResetSelf();

        BlockObs.Action();
    }
}
