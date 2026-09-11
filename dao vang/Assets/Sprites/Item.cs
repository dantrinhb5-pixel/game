using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Cấu hình vật phẩm")]
    public string itemName = "Gold";
    public int scoreValue = 5;     // Số điểm cộng khi gắp về
    public float pullSpeed = 3f;   // Đồ càng nặng kéo càng chậm (Đá=2, Vàng to=3, Kim cương=6)
}