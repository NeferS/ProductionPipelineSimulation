using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PipelineElement))]
public class PipelineOutline : OutlineController
{
    protected PipelineElement selfElement;
    protected string pipelineName;
    private float contentHeight;

    void Start()
    {
        selfElement = GetComponent<PipelineElement>();
        pipelineName = gameObject.transform.parent.name;
        contentHeight = MainSceneManager.Instance.elementPanes.content.GetComponent<RectTransform>().rect.height;
    }

    public override void TriggerConfiguration() 
    {
        MainSceneManager.Instance.elementPanes.pipelineConfig.SetActive(true);
        MainSceneManager.Instance.elementPanes.pipelineConfig.GetComponentInChildren<Text>().text = pipelineName;
        GameObject content = MainSceneManager.Instance.elementPanes.pipelineConfigContent;
        ClearContent(content);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight);
    }

    public override void TriggerInfo()
    {
        MainSceneManager.Instance.elementPanes.scrollable.SetActive(true);
        MainSceneManager.Instance.elementPanes.scrollable.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        GameObject content = MainSceneManager.Instance.elementPanes.content;
        ClearContent(content);
        content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentHeight);

        RectTransform frame = Instantiate(MainSceneManager.Instance.elementPanes.pipelineFrame,
                                          MainSceneManager.Instance.elementPanes.content.transform);
        frame.gameObject.name = pipelineName;
        Text[] textBoxes = frame.GetComponentsInChildren<Text>();
        textBoxes[0].text = pipelineName;
        textBoxes[2].text = "" + selfElement.outBlocks;
        textBoxes[4].text = "" + selfElement.discardedBlocks;
    }

    public override void EventuallyUpdate()
    {
        if(MainSceneManager.Instance.elementPanes.scrollable.activeSelf)
        {
            RectTransform frame = MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>()[1];
            if(frame.gameObject.name.Equals(pipelineName))
            {
                Text[] textBoxes = frame.GetComponentsInChildren<Text>();
                textBoxes[2].text = "" + selfElement.outBlocks;
                textBoxes[4].text = "" + selfElement.discardedBlocks;
            }
        }
    }
}
