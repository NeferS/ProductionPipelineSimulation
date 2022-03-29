using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum DoorState { Open, Opening, Closing, Closed }
    private enum EditorDoorState { Opening, Closing }

    [SerializeField] private EditorDoorState initialState = EditorDoorState.Opening;
    [SerializeField] public Vector3 openYPos = new Vector3(0, 0, 0.1f);
    [SerializeField] public Vector3 closedYPos = new Vector3(0, -0.4f, 0.1f);
    [SerializeField] private Renderer lightBulb;
    [SerializeField] private Color lightOnColor;
    [SerializeField] private bool lightWhenClosed = true;

    private DoorState state;
    private Vector3 openYScale = new Vector3(0.7f, 0.85f, 0.8f);
    private Vector3 closedYScale = new Vector3(0.7f, 0.1f, 0.8f);
    private float doorSpeed = 0.5f;
    private float threshold = 0.025f;

    private PipelineElement element;

    void Start()
    {
        state = (DoorState)(((int)initialState) + 1);
        lightBulb.material.SetColor("_EmissionColor", Color.gray);
        element = GetComponentInParent<PipelineElement>();
    }

    void Update()
    {
        if(state.Equals(DoorState.Opening))
        {
            transform.localScale += new Vector3(0, 2 * doorSpeed * MainSceneManager.Instance.TimeScale 
                                                     * MainSceneManager.Instance.TimeMultiplier * Time.deltaTime, 0);
            transform.localPosition += new Vector3(0, doorSpeed * MainSceneManager.Instance.TimeScale 
                                                                * MainSceneManager.Instance.TimeMultiplier * Time.deltaTime, 0);
            if (Vector3.Distance(transform.localPosition, openYPos) < threshold)
            {
                state = DoorState.Open;
                if(!lightWhenClosed)
                {
                    lightBulb.material.SetColor("_EmissionColor", lightOnColor);
                    lightBulb.GetComponentInChildren<Light>().enabled = true;
                }
            }

        }
        if(state.Equals(DoorState.Closing))
        {
            transform.localScale -= new Vector3(0, 2 * doorSpeed * MainSceneManager.Instance.TimeScale 
                                                     * MainSceneManager.Instance.TimeMultiplier * Time.deltaTime, 0);
            transform.localPosition -= new Vector3(0, doorSpeed * MainSceneManager.Instance.TimeScale 
                                                                * MainSceneManager.Instance.TimeMultiplier * Time.deltaTime, 0);
            if (Vector3.Distance(transform.localPosition, closedYPos) < threshold)
            {
                state = DoorState.Closed;
                if(lightWhenClosed)
                {
                    lightBulb.material.SetColor("_EmissionColor", lightOnColor);
                    lightBulb.GetComponentInChildren<Light>().enabled = true;
                }
            }
        }
    }

    public void Trigger()
    {
        if ((state.Equals(DoorState.Closed) && lightWhenClosed) || (state.Equals(DoorState.Open) && !lightWhenClosed))
        {
            lightBulb.material.SetColor("_EmissionColor", Color.gray);
            lightBulb.GetComponentInChildren<Light>().enabled = false;
        }

        if (state.Equals(DoorState.Closed) || state.Equals(DoorState.Closing))
            state = DoorState.Opening;
        else if (state.Equals(DoorState.Open) || state.Equals(DoorState.Opening)) 
            state = DoorState.Closing;
    }

    private void OnTriggerEnter(Collider other)
    {
        element.TriggerInput(this, other);
        if (state.Equals(DoorState.Open) && other.GetComponent<ComponentBlock>() != null)
            Trigger();
    }

    public void ResetSelf()
    {
        state = (DoorState)(((int)initialState) + 1);
        lightBulb.material.SetColor("_EmissionColor", Color.gray);
        lightBulb.GetComponentInChildren<Light>().enabled = false;
        transform.localPosition = closedYPos;
        transform.localScale = closedYScale;
    }

    public DoorState GetState() { return state; }
}
