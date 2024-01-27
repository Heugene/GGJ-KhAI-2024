using UnityEngine;

public class LeverScript : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            animator.SetBool("isSwitched", true);
        }
    }
}
