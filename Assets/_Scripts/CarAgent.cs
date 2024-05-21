using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dragCoefficient = 0.5f;
    [SerializeField] private float angularDragCoefficient = 0.5f;
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float mass = 1;
    private float maxSpeed = 0.0f;

    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private Transform target;
    private int targetCount = 0;

    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int numFrontRaycasts = 10;
    [SerializeField] private int numSideRaycasts = 4;
    [SerializeField] private int numBackRaycasts = 6;

    [HideInInspector] public LayerMask WallLayer { get { return wallLayer; } set { return; } }
    [HideInInspector] public float DragCoefficient { get { return dragCoefficient; } set { return; } }
    [HideInInspector] public float MoveSpeed { get { return moveSpeed; } set { return; } }

    public override void OnEpisodeBegin() {
        rb.mass = mass;
        rb.drag = dragCoefficient;
        rb.angularDrag = angularDragCoefficient;

        transform.localPosition = SpawnPoint.localPosition;
        transform.rotation = Quaternion.Euler(0f, 0f, -90);

        target.localPosition = new Vector3(Random.Range(-18f, 18f), Random.Range(-18f, 18f));
        targetCount = 0;
    }

    public override void CollectObservations(VectorSensor sensor) {
        //sensor.AddObservation(rb.velocity.magnitude);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(transform.rotation.eulerAngles.z);
        sensor.AddObservation(target.localPosition);

        float maxRaycastDistance = 10f;

        for (int i = 0; i < 6; i++) {
            float angle = Mathf.Lerp(-30f, 30f, i / 5f);
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, Quaternion.Euler(0, 0, angle) * transform.up, maxRaycastDistance);
            float distanceNormalized = hit ? hit.distance / maxRaycastDistance : 1f;
            sensor.AddObservation(distanceNormalized);
        }

        for (int i = 0; i < 2; i++) {
            float angle = i == 0 ? -90f : 90f;
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.localPosition, Quaternion.Euler(0, 0, angle) * transform.up, maxRaycastDistance);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.localPosition, Quaternion.Euler(0, 0, -angle) * transform.up, maxRaycastDistance);
            float distanceLeftNormalized = hitLeft ? hitLeft.distance / maxRaycastDistance : 1f;
            float distanceRightNormalized = hitRight ? hitRight.distance / maxRaycastDistance : 1f;
            sensor.AddObservation(distanceLeftNormalized);
            sensor.AddObservation(distanceRightNormalized);
        }

        for (int i = 0; i < 3; i++) {
            float angle = i == 0 ? -150f : (i == 1 ? -180f : -210f);
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, Quaternion.Euler(0, 0, angle) * transform.up, maxRaycastDistance);
            float distanceNormalized = hit ? hit.distance / maxRaycastDistance : 1f;
            sensor.AddObservation(distanceNormalized);
        }

        sensor.AddObservation(rb.angularVelocity);

        float steerAngleNormalized = (transform.eulerAngles.z - 90) / maxSteerAngle;
        sensor.AddObservation(steerAngleNormalized);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float acceleration = Mathf.Clamp(actions.ContinuousActions[0], -0.5f, 1f);
        float turn = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        // Verificar se o carro está acelerando para aplicar rotação
        if (acceleration > 0) {
            // Aplicar rotação ao carro
            float rotation = turn * maxSteerAngle * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation + rotation);
        }

        // Aplicar aceleração ao carro
        Vector2 forward = transform.up * acceleration * moveSpeed;
        rb.AddForce(forward);

        // Aplicar arrasto (drag) baseado na velocidade
        Vector2 dragForce = -rb.velocity.normalized * dragCoefficient * rb.velocity.sqrMagnitude;
        rb.AddForce(dragForce);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Vertical");
        continuousActions[1] = Input.GetAxis("Horizontal");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Target")) {
            AddReward(20 * (targetCount + 1));
            targetCount++;
            if (targetCount >= 5) {
                AddReward(maxSpeed);
                EndEpisode();
            } else {
                target.localPosition = new Vector3(Random.Range(-4.7f, 4.7f), Random.Range(-4.7f, 4.7f));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Wall")) {
            return;
        }
        AddReward(maxSpeed);
        AddReward(-20);
        EndEpisode();
    }

    private void FixedUpdate() {
        if (rb.velocity.sqrMagnitude > maxSpeed) {
            maxSpeed = rb.velocity.sqrMagnitude;
        }

        for (int i = 0; i < numFrontRaycasts; i++) {
            float angle = Mathf.Lerp(-45f, 45f, i / (float)(numFrontRaycasts - 1));
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, direction, 1f, wallLayer);

            if (hit.collider != null) {
                AddReward(-0.01f);
            }
        }

        for (int i = 0; i < numSideRaycasts; i++) {
            float angle = Mathf.Lerp(-90f, 90f, i / (float)(numSideRaycasts - 1));
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.right;
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, direction, 1f, wallLayer);

            if (hit.collider != null) {
                AddReward(-0.01f);
            }
        }

        for (int i = 0; i < numBackRaycasts; i++) {
            float angle = Mathf.Lerp(135f, -135f, i / (float)(numBackRaycasts - 1));
            Vector2 direction = Quaternion.Euler(0, 0, angle) * transform.up;
            RaycastHit2D hit = Physics2D.Raycast(transform.localPosition, direction, 1f, wallLayer);

            if (hit.collider != null) {
                AddReward(-0.01f);
            }
        }
    }
}
