using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform follow;
    public float leftLimit = -4.3f;
    public float rightLimit = 4.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(follow.position.x, leftLimit, rightLimit);
        transform.position = position;
    }
}
