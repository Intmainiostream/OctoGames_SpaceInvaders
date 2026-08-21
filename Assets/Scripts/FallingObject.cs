using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 2f;
    [SerializeField] private Vector3 fallDirection = new Vector3(0f, 0f, -1f);
    [SerializeField] private float destroyAtZ = -12f;

    private void Update()
    {
        transform.position += fallDirection.normalized * fallSpeed * Time.deltaTime;

        if (transform.position.z < destroyAtZ)
        {
            Destroy(gameObject);
        }
    }
}