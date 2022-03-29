using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField, Range(1, 5)] private float speed = 2.5f;
    [SerializeField, Range(0, 1)] private float maxAngle = 0.6f;
    private Camera attachedCamera;

    private string highlightLayer = "Highlightable";
    private GameObject lastHit;

    void Start()
    {
        attachedCamera = GetComponentInChildren<Camera>(true);
    }

    void Update()
    {
        if(MainSceneManager.Instance.MovementAllowed)
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y") * -1;
            Vector3 bodyRotation = new Vector3(0, x, 0);
            Vector3 cameraRotation = new Vector3(y, 0, 0);
            transform.Rotate(bodyRotation * speed);
            float yRotation = attachedCamera.transform.localRotation.x;
            float direction = Mathf.Sign(y);
            if ((direction == -1 && yRotation > -maxAngle) || (direction == 1 && yRotation < maxAngle))
                attachedCamera.transform.Rotate(cameraRotation * speed);

            RaycastHit hit;
            Ray ray = attachedCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask(highlightLayer)))
            {
                if (hit.transform.gameObject != lastHit) 
                {
                    if (lastHit != null && lastHit.GetComponent<Outline>() != null)
                        lastHit.GetComponent<Outline>().enabled = false;

                    lastHit = hit.transform.gameObject;
                    if (lastHit.GetComponent<Outline>() != null)
                        lastHit.GetComponent<Outline>().enabled = true;
                }
            } 
            else
            {
                if (lastHit != null && lastHit.GetComponent<Outline>() != null)
                    lastHit.GetComponent<Outline>().enabled = false;
                lastHit = null;
            }
        }
    }
}
