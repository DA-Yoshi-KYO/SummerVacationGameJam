using UnityEngine;

public class CS_Bullet : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5.0f;

    private void Start()
    {
        Destroy(gameObject, _lifeTime);
    }
}
