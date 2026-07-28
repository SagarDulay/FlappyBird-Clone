using Unity.VisualScripting;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D birdRigidbody;
    [SerializeField] private float jumpForce;

    private Vector2 touchStartedPos;
    private Vector2 touchEndedPos;
    void Update()
    {
        if (Input.touchCount > 0)
        {

            Touch firstTouch = Input.GetTouch(0);

            if (firstTouch.phase == TouchPhase.Began)
            {
                touchStartedPos = firstTouch.position;
                FlapTheBird();
            }
            else if (firstTouch.phase == TouchPhase.Ended)
            {
                touchEndedPos = firstTouch.position;

                Vector2 result = touchEndedPos - touchStartedPos;
                Vector2.Dot(result.normalized, Vector2.right);

            }
        }
          
    }

    public void FlapTheBird()
    {
        birdRigidbody.linearVelocity = Vector2.zero;
        birdRigidbody.AddForce(Vector2.up * jumpForce);
    }
}
