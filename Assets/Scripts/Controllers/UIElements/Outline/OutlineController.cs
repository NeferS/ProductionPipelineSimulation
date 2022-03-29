using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class OutlineController : MonoBehaviour
{

    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }

    void OnMouseEnter()
    {
        if (MainSceneManager.Instance.CursorVisible())
            outline.enabled = true;
    }

    void OnMouseExit()
    {
        outline.enabled = false;
    }

    void OnMouseDown()
    {
        if (MainSceneManager.Instance.Configuration)
            TriggerConfiguration();
        else
            TriggerInfo();
    }

    public abstract void TriggerConfiguration();

    public abstract void TriggerInfo();

    public abstract void EventuallyUpdate();

    protected virtual void ClearContent(GameObject container)
    {
        RectTransform[] children = container.GetComponentsInChildren<RectTransform>();
        for (int i = 1; i < children.Length; i++)
            Destroy(children[i].gameObject);
    }
}
