using UnityEngine;

public class ItemController : MonoBehaviour
{
    public bool isMoveFollow = false;
    public GameObject parent; // Kéo Cái Móc (a_0) vào ô này
    
    [Header("Speed Setting")]
    public float pullSpeed = 2f; // Tốc độ kéo riêng cho vật phẩm này
    
    private HookController hookController;

    void Start()
    {
        isMoveFollow = false;
        hookController = FindObjectOfType<HookController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == parent || collision.transform.parent == parent.transform)
        {
            isMoveFollow = true;

            if (hookController != null)
            {
                hookController.isDropping = false;
                hookController.isPulling = true;
                
                // Đổi tốc độ kéo của móc bằng tốc độ của vật phẩm này
                hookController.pullSpeed = pullSpeed;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoveFollow && parent != null)
        {
            MoveFollow(parent);

            if (hookController != null && hookController.isRotate)
            {
                Destroy(gameObject);
            }
        }
    }

    private void MoveFollow(GameObject target)
    {
        float heightOffset = GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        transform.position = new Vector3(
            target.transform.position.x,
            target.transform.position.y - heightOffset,
            target.transform.position.z
        );
    }
}