using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CarAgent : Agent {
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin() {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(rb.velocity.magnitude); // Velocidade do carro
    }

    public void OnActionReceived(float[] vectorAction) {
        float moveInput = vectorAction[0];
        float turnInput = vectorAction[1];
        rb.velocity = transform.up * moveInput * moveSpeed;
        rb.angularVelocity = -turnInput * turnSpeed;
    }

    public void Heuristic(float[] actionsOut) {
        actionsOut[0] = Input.GetAxis("Vertical"); // Controle de movimento
        actionsOut[1] = Input.GetAxis("Horizontal"); // Controle de direção
    }
}
