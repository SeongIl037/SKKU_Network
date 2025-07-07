using Photon.Pun;
using UnityEngine;

public class Waepon : MonoBehaviour,IPunObservable
{
    [SerializeField] private PlayerAttackAbility _attackAbility;
    public GameObject Effect;

    private Vector3 _receiveTransform;
    private float _ratio = 0.1f;
    private int _num = 10000;
    private void Start()
    {
        _attackAbility = GetComponentInParent<PlayerAttackAbility>();
        ScoreManager.Instance.OnScoreChanged += RefreshWeaponSize;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localScale);
        }
        else if (stream.IsReading)
        {
            _receiveTransform = (Vector3)stream.ReceiveNext();
            transform.localScale = _receiveTransform;
        }
    }
    private void RefreshWeaponSize()
    {
        int score = ScoreManager.Instance.Score;
        int scale = (int)(score / _num);    

        float add = 1 + _ratio * scale;
        
        transform.localScale = new Vector3(add, add, add);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // 자기 자신과 부딪히면 return
        if (other.transform == _attackAbility.transform)
        {
            return;
        }
        
        //IDamaged 인터페이스를 가지고 있으면 데미지 주기
        if (other.GetComponent<IDamaged>() != null)
        { 
            _attackAbility.Hit(other);
            Instantiate(Effect, transform.position, Quaternion.identity);
        } 
    }
}
