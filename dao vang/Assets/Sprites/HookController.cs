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
    private float defaultPullSpeed; // Tốc độ kéo mặc định
    public bool isDropping = false;
    public bool isPulling = false;

    // Giới hạn vùng di chuyển của móc
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = -4.5f;

    // Vị trí ban đầu và LineRenderer
    public Vector3 initialPosition;
    private LineRenderer lineRenderer;

    void Start()
    {
        isRotate = true;
        initialPosition = transform.position;
        lineRenderer = GetComponent<LineRenderer>();
        defaultPullSpeed = pullSpeed;
    }

    void Update()
    {
        // KHÔNG CHO THAO TÁC NẾU ĐÃ HẾT 2 PHÚT
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null && gm.isGameOver) return;

        // 1. Cập nhật đường vẽ sợi dây
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, initialPosition);
            lineRenderer.SetPosition(1, transform.position);
        }

        // 2. Xử lý trạng thái Lắc hoặc Thả móc
        if (isRotate)
        {
            RotationZ();
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
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
            transform.Translate(Vector3.down * dropSpeed * Time.deltaTime);

            if (transform.position.x <= minX || transform.position.x >= maxX || transform.position.y <= minY)
            {
                isDropping = false;
                isPulling = true;
            }
        }
        else if (isPulling)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, pullSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                transform.position = initialPosition;
                isPulling = false;
                isRotate = true;
                ResetPullSpeed();
            }
        }
    }

    public void ResetPullSpeed()
    {
        pullSpeed = defaultPullSpeed;
    }
}