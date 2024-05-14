using UnityEngine;

public class CarController : MonoBehaviour {
    public NeuralNetwork neuralNetwork;
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // Obter a posição Y do carro
        float positionY = transform.position.y;

        // Entradas para a rede neural (apenas posição Y)
        float[] inputs = { positionY };

        // Calcular as saídas da rede neural
        float[] outputs = neuralNetwork.FeedForward(inputs);

        // Controlar o carro com base nas saídas da rede neural
        float moveInput = outputs[0]; // Saída para acelerar (positivo ou negativo)
        float turnInput = outputs[1]; // Saída para girar (direita ou esquerda)

        // Movimentar o carro
        rb.velocity = transform.up * moveInput * moveSpeed;
        rb.angularVelocity = -turnInput * turnSpeed;
    }
}
