using UnityEngine;

public class ItemController : MonoBehaviour
{
    public bool isMoveFollow = false;
    public GameObject parent; // Ô kéo cái Móc (1_0 hoặc a_0) vào Inspector
    
    private HookController hookController;
    private Item itemScript;
    private bool isScored = false;

    void Start()
    {
        isMoveFollow = false;
        hookController = FindObjectOfType<HookController>();
        itemScript = GetComponent<Item>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parent == null) return;

        // Va chạm với móc hoặc con của móc
        if (collision.gameObject == parent || collision.transform.parent == parent.transform)
        {
            if (!isMoveFollow)
            {
                isMoveFollow = true;

                if (hookController != null)
                {
                    hookController.isDropping = false;
                    hookController.isPulling = true;
                    
                    // Đổi tốc độ kéo của móc dựa trên thuộc tính của Item
                    if (itemScript != null)
                    {
                        hookController.pullSpeed = itemScript.pullSpeed;
                    }
                }

                // Tắt Collider để vật không bị vướng các đồ khác khi đang kéo về
                Collider2D col = GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (isMoveFollow && parent != null)
        {
            MoveFollow(parent);

            // Khi móc đã kéo về vị trí ban đầu (isRotate = true)
            if (hookController != null && hookController.isRotate)
            {
                if (!isScored)
                {
                    isScored = true;
                    // CỘNG ĐIỂM VÀO BẢNG
                    if (itemScript != null)
                    {
                        FindObjectOfType<GameManager>()?.AddScore(itemScript.scoreValue);
                    }
                    else
                    {
                        FindObjectOfType<GameManager>()?.AddScore(5); // Điểm mặc định
                    }

                    // Reset lại tốc độ kéo ban đầu cho móc
                    hookController.ResetPullSpeed();

                    // Xoá vật phẩm
                    Destroy(gameObject);
                }
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