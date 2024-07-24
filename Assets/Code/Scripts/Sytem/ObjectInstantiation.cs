using UnityEngine;

public class ObjectInstantiation : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab của đối tượng bạn muốn tạo ra
   

    void Update()
    {
        // Kiểm tra xem người dùng đã nhấn vào màn hình chưa
        if (Input.GetMouseButtonDown(0)) // 0 ở đây đại diện cho nút chuột trái
        {
            // Lấy vị trí của điểm nhấn trên màn hình
            Vector3 touchPosition = Input.mousePosition;
            touchPosition.z = 10; // Đặt z-coordinate là 10 để đảm bảo nó ở phía trước camera

            // Chuyển đổi vị trí từ màn hình sang thế giới 3D
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

            // Tạo ra một đối tượng từ prefab tại vị trí của điểm nhấn
            GameObject newObject = Instantiate(objectPrefab, worldPosition, Quaternion.identity);

            
        }
    }
}
