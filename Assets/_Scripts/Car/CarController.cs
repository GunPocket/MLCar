using UnityEngine;

public class CarController : MonoBehaviour {
    private NeuralNetwork neuralNetwork;
    private Rigidbody2D rb;
    private CarAgent car;

    private float baseDrag = 0.2f;
    private float maxDrag = 2f;
    private float maxAcceleration = 20f;
    private float initialSpeed = 0f;
    private float score;
    private float fitness;

    public void Initialize(CarAgent car) {
        this.car = car;
        this.neuralNetwork = car.GetNeuralNetwork();
        this.rb = car.GetComponent<Rigidbody2D>();

        if (rb == null) {
            Debug.LogError("Rigidbody2D is not assigned!", this.gameObject);
        }
    }

    void FixedUpdate() {
        if (neuralNetwork != null && rb != null) {
            // Coletar observações
            float[] inputs = new float[] { transform.position.x, transform.position.y, rb.velocity.magnitude, transform.eulerAngles.z };

            // Passar observações para a rede neural e obter as ações
            float[] outputs = neuralNetwork.FeedForward(inputs);

            if (outputs != null && outputs.Length == 2) {
                float acceleration = Mathf.Clamp(outputs[0], -1f, 1f) * maxAcceleration;
                float steering = outputs[1];

                // Aplicar aceleração
                Vector2 force = transform.up * acceleration;
                rb.AddForce(force);

                // Aplicar direção
                float torque = steering * Time.fixedDeltaTime;
                rb.AddTorque(torque);

                // Aplicar resistência do ar
                ApplyDrag();
            }
        }

        if (rb != null) {
            UpdateScore(rb.position.y / 10);
        }
    }

    public void ApplyControl(float acceleration, float steering) {
        if (rb == null) {
            Debug.LogError("Rigidbody2D is not assigned!", this.gameObject);
            return;
        }

        // Apply forces based on acceleration and steering inputs
        Vector2 force = transform.up * acceleration * maxAcceleration;
        rb.AddForce(force);
        rb.AddTorque(-steering);
    }

    void ApplyDrag() {
        if (rb == null) return;

        // Calcular a velocidade atual
        float currentSpeed = rb.velocity.magnitude;

        // Calcular o coeficiente de arrasto com base na velocidade atual
        float drag = Mathf.Lerp(baseDrag, maxDrag, currentSpeed / maxAcceleration);

        // Calcular e aplicar a força de resistência do ar
        Vector2 dragForce = -rb.velocity.normalized * drag;
        rb.AddForce(dragForce);
    }

    public void ResetCar() {
        if (rb == null) {
            rb = car.GetRigidBody();
        }

        rb.velocity = transform.up * initialSpeed; // Aplica velocidade inicial para frente
        score = 0f; // Zera o score
        transform.position = Vector3.zero; // Reposiciona o carro
        transform.rotation = Quaternion.identity; // Reinicia a rotação
    }

    public void UpdateScore(float increment) {
        score += increment;
        UpdateFitness(score / 100);
    }

    public float GetScore() {
        return score;
    }

    public void UpdateFitness(float newFitness) {
        fitness = newFitness;
    }

    public float GetFitness() {
        return fitness;
    }

    public void SetNeuralNetwork(NeuralNetwork nn) {
        neuralNetwork = nn;
    }
}
