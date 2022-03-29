using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ComponentBlock))]
public class BlockOutline : OutlineController
{
    private ComponentBlock selfComponent;
    private int originalHeight;

    void Start()
    {
        selfComponent = GetComponent<ComponentBlock>();
        originalHeight = (int)MainSceneManager.Instance.elementPanes.content.GetComponent<RectTransform>().sizeDelta.y;
    }

    public override void TriggerConfiguration() { }

    public override void TriggerInfo()
    {
        MainSceneManager.Instance.elementPanes.scrollable.SetActive(true);
        MainSceneManager.Instance.elementPanes.scrollable.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        GameObject content = MainSceneManager.Instance.elementPanes.content;
        ClearContent(content);

        if(selfComponent.GetChildrenCount() == 0)
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, originalHeight);
            GenerateInfo(selfComponent, 0, 0);
        }
        else
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0,
                            MainSceneManager.Instance.elementPanes.blockFrame.rect.height * selfComponent.GetChildrenCount());
            int top = (int)Mathf.Abs(MainSceneManager.Instance.elementPanes.blockFrame.rect.height - 
                                     (content.GetComponent<RectTransform>().sizeDelta.y / 2));
            for (int i = 0; i < selfComponent.GetChildrenCount(); i++)
                GenerateInfo(selfComponent.GetChildAt(i), i, top);
        }
    }

    public override void EventuallyUpdate() { }

    private void GenerateInfo(ComponentBlock block, int frameNum, int top)
    {
        RectTransform frame = Instantiate(MainSceneManager.Instance.elementPanes.blockFrame,
                                          MainSceneManager.Instance.elementPanes.content.transform);
        frame.position = frame.position - new Vector3(0, 
                            MainSceneManager.Instance.elementPanes.blockFrame.rect.height  * frameNum - top, 0);
        Text[] textBoxes = frame.GetComponentsInChildren<Text>();
        textBoxes[0].text = block.gameObject.name;
        textBoxes[2].text = block.Id;
        textBoxes[4].text = block.Value;
        textBoxes[6].text = "RGB(" + block.BlockColor.r * 255 + "," 
                                   + block.BlockColor.g * 255 + "," 
                                   + block.BlockColor.b * 255 + ")";
    }
}
