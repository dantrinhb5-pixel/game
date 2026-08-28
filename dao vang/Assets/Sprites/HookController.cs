using UnityEngine;

public class HookController : MonoBehaviour
{
    // Biến điều khiển xoay móc
    public float rotateSpeed = 100f;
    public float minAngle = -70f;
    public float maxAngle = 70f;
    private float currentAngle = 0f;
    private bool rotateRight = true;
    public bool isRotate = true;

    // Biến điều khiển thả và kéo móc
    public float dropSpeed = 5f;
    public float pullSpeed = 5f;
    public bool isDropping = false;
    public bool isPulling = false;

    // Giới hạn vùng di chuyển của móc
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;

    // Vị trí ban đầu và LineRenderer vẽ dây
    public Vector3 initialPosition;
    private LineRenderer lineRenderer;

    void Start()
    {
        isRotate = true;
        initialPosition = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        // 1. Cập nhật đường vẽ sợi dây từ vị trí ban đầu đến vị trí hiện tại của móc
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, initialPosition);
            lineRenderer.SetPosition(1, transform.position);
        }

        // 2. Xử lý trạng thái Lắc hoặc Thả móc
        if (isRotate)
        {
            RotationZ();
            if (Input.GetMouseButtonDown(0))
            {
                isRotate = false;
                isDropping = true;
            }
        }
        else
        {
            MoveHook();
        }
    }

    private void RotationZ()
    {
        if (rotateRight)
        {
            currentAngle += rotateSpeed * Time.deltaTime;
            if (currentAngle >= maxAngle) rotateRight = false;
        }
        else
        {
            currentAngle -= rotateSpeed * Time.deltaTime;
            if (currentAngle <= minAngle) rotateRight = true;
        }

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void MoveHook()
    {
        if (isDropping)
        {
            // Móc di chuyển xuống dưới
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);

            // Kiểm tra nếu chạm giới hạn biên trái, phải hoặc dưới thì bắt đầu kéo về
            if (transform.position.x <= minX || transform.position.x >= maxX || transform.position.y <= minY)
            {
                isDropping = false;
                isPulling = true;
            }
        }
        else if (isPulling)
        {
            // Kéo móc về vị trí ban đầu
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, pullSpeed * Time.deltaTime);

            // Khi khoảng cách gần về vị trí ban đầu (< 0.1) thì đặt hẳn về gốc và lắc tiếp
            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                transform.position = initialPosition;
                isPulling = false;
                isRotate = true;
            }
        }
    }
}