using UnityEngine;

[ExecuteInEditMode]
public class CarAgentGizmos : MonoBehaviour {
    [SerializeField] private LayerMask wallLayer; // Camada das paredes

    private void OnDrawGizmos() {
        CarAgent carAgent = GetComponent<CarAgent>();
        if (carAgent == null)
            return;

        Rigidbody2D rb = carAgent.GetComponent<Rigidbody2D>();

        // Define a cor padrão dos raycasts (clarinho)
        Gizmos.color = new Color(1f, 1f, 1f, 0.2f); // Cor branca com transparência baixa

        // Define os parâmetros dos raycasts
        float maxRaycastDistance = 10f; // Distância máxima do raycast

        // Desenha os raycasts para frente (em diferentes ângulos)
        for (int i = 0; i < 6; i++) {
            float angle = Mathf.Lerp(-30f, 30f, i / 5f); // Ângulo de -30 a 30 graus
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Quaternion.Euler(0, 0, angle) * rb.transform.up, maxRaycastDistance, carAgent.WallLayer);
            Gizmos.DrawLine(rb.position, hit ? hit.point : (Vector3)rb.position + Quaternion.Euler(0, 0, angle) * rb.transform.up * maxRaycastDistance);
        }

        // Desenha os raycasts para os lados (em pares)
        for (int i = 0; i < 2; i++) {
            float angle = i == 0 ? -90f : 90f; // Ângulo de -90 ou 90 graus
            RaycastHit2D hitLeft = Physics2D.Raycast(rb.position, Quaternion.Euler(0, 0, angle) * rb.transform.up, maxRaycastDistance, carAgent.WallLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(rb.position, Quaternion.Euler(0, 0, -angle) * rb.transform.up, maxRaycastDistance, carAgent.WallLayer);
            Gizmos.DrawLine(rb.position, hitLeft ? hitLeft.point : (Vector3)rb.position + Quaternion.Euler(0, 0, angle) * rb.transform.up * maxRaycastDistance);
            Gizmos.DrawLine(rb.position, hitRight ? hitRight.point : (Vector3)rb.position + Quaternion.Euler(0, 0, -angle) * rb.transform.up * maxRaycastDistance);
        }

        // Desenha os raycasts para trás (em diferentes ângulos)
        for (int i = 0; i < 3; i++) {
            float angle = i == 0 ? -150f : (i == 1 ? -180f : -210f); // Ângulos de -150, -180 e -210 graus
            RaycastHit2D hit = Physics2D.Raycast(rb.position, Quaternion.Euler(0, 0, angle) * rb.transform.up, maxRaycastDistance, carAgent.WallLayer);
            Gizmos.DrawLine(rb.position, hit ? hit.point : (Vector3)rb.position + Quaternion.Euler(0, 0, angle) * rb.transform.up * maxRaycastDistance);
        }

        // Desenhar vetores do carro
        DrawArrow(rb.position, rb.position + (Vector2)rb.velocity, Color.blue); // Velocidade
        DrawArrow(rb.position, rb.position + (Vector2)rb.transform.up * carAgent.MoveSpeed, Color.green); // Aceleração
        DrawArrow(rb.position, rb.position + (Vector2)rb.transform.up * -carAgent.DragCoefficient, Color.red); // Arrasto
        DrawArrow(rb.position, rb.position + (Vector2)rb.transform.up * rb.angularVelocity, Color.yellow); // Rotação
    }

    private void DrawArrow(Vector2 start, Vector2 end, Color color) {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);

        Vector2 direction = (end - start).normalized;
        Vector2 right = Quaternion.Euler(0, 0, 45) * direction;
        Vector2 left = Quaternion.Euler(0, 0, -45) * direction;

        Gizmos.DrawLine(end, end - right * 0.25f);
        Gizmos.DrawLine(end, end - left * 0.25f);
    }
}