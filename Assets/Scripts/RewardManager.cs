using UnityEngine;

public class RewardManager : MonoBehaviour {
    public float rewardMultiplier = 1f;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Reward")) {
            // Aumentar a recompensa com base na posição Y
            float reward = other.transform.position.y * rewardMultiplier;
            // Aplicar a recompensa ao agente ou à rede neural
            // Por exemplo: neuralNetwork.ApplyReward(reward);
        }
    }
}
