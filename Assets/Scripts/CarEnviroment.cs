using UnityEngine;

public class CarEnvironment : MonoBehaviour {
    void Start() {
        // Configurar o ambiente inicial
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            // Reiniciar o episódio se o carro colidir com uma parede
            Object.FindAnyObjectByType<CarAgent>().EndEpisode(); 
        }
    }
}
