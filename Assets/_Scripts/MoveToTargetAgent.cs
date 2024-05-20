using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToTargetAgent : Agent {
    [SerializeField] private Transform env;
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer background;

    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(Random.Range(-4.7f, 4.7f), Random.Range(-4.7f, 4.7f));
        target.localPosition = new Vector3(Random.Range(-4.7f, 4.7f), Random.Range(-4.7f, 4.7f));

        env.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        transform.rotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float movementSpeed = 5f;

        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Target")) {
            AddReward(10);
            background.color = Color.green;
            EndEpisode();

        } else if (collision.CompareTag("Wall")) {
            AddReward(-2);
            background.color = Color.red;
            EndEpisode();
        }
    }
}
