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
        // Obter a posi��o Y do carro
        float positionY = transform.position.y;

        // Entradas para a rede neural (apenas posi��o Y)
        float[] inputs = { positionY };

        // Calcular as sa�das da rede neural
        float[] outputs = neuralNetwork.FeedForward(inputs);

        // Controlar o carro com base nas sa�das da rede neural
        float moveInput = outputs[0]; // Sa�da para acelerar (positivo ou negativo)
        float turnInput = outputs[1]; // Sa�da para girar (direita ou esquerda)

        // Movimentar o carro
        rb.velocity = transform.up * moveInput * moveSpeed;
        rb.angularVelocity = -turnInput * turnSpeed;
    }
}
