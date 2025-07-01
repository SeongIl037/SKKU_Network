using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //IDamaged 인터페이스를 가지고 있으면 데미지 주기
        if (other.GetComponent<IDamaged>() != null)
        { 
            IDamaged damaged = other.GetComponent<IDamaged>();
            damaged.Damaged(100, 0);
        } 
    }
}
