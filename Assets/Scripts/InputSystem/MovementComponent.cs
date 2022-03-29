using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField, Range(5,25)] private float speed = 0.5f;
    private CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(MainSceneManager.Instance.MovementAllowed)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 movement = transform.forward * z + transform.right * x;
            movement = Vector3.ClampMagnitude(movement, 1);
            character.Move(movement * speed * Time.deltaTime);
        }
    }
}
