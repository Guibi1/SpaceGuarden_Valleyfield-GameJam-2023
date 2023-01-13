using UnityEngine;

public class cameraIdle : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public float speed;

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, endPosition.position, step);

        if (transform.position == endPosition.position)
        {
            Transform temp = startPosition;
            startPosition = endPosition;
            endPosition = temp;
        }
    }
}
