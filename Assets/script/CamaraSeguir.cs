using UnityEngine;

public class CamaraSeguir : MonoBehaviour
{
    public Transform jugador;
    public float speed = 5.0f;
    void Update()
    {
        Vector3 destino = new Vector3 (jugador.position.x, jugador.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, destino, speed * Time.deltaTime);
    }
}
