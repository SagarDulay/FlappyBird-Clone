using UnityEngine;

public class PipeObstacle : MonoBehaviour
{
    [SerializeField] private float yRange;
    [SerializeField] private float moveSpeed;



    void Start()
    {
        transform.position = new Vector3(transform.position.x, Random.Range(-yRange, yRange), 0); 
    }

    
    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }
}
