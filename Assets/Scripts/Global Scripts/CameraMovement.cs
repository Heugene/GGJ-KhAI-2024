using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] Transform player;
    private Vector3? targetPosition = null;


    void Update()
    {
        if (targetPosition != null)
        {
            Vector3 movePos = targetPosition.Value;
            transform.position = Vector3.Lerp(transform.position, movePos, speed * Time.deltaTime);
        }
        else if (transform.position != player.position)
        {
            Vector3 moveVector = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, moveVector, speed * Time.deltaTime);
        } 
    }

    public void ChangeTarget(Vector3 movePoint)
    {
        targetPosition = new Vector3(movePoint.x, movePoint.y, -8f);
    }

    public void FollowPlayer()
    {
        targetPosition = null;
        
    }

    public bool isOnTarget()
    {
        return transform.position == targetPosition;
    }
}
