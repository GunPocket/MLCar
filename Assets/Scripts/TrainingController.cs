using UnityEngine;

public class TrainingController : MonoBehaviour {
    [SerializeField] CarAgent carAgent;

    void Start() {
        // Encontrar o agente e iniciar o episódio

        if (carAgent != null) {
            carAgent.OnEpisodeBegin();
        } else {
            Debug.LogError("CarAgent não encontrado!");
        }
    }

    void Update() {
        // Verificar condições de término do treinamento
        //if (/* condição de término */) {
        // Terminar o treinamento
        // Exemplo: UnityEditor.EditorApplication.isPlaying = false;
    }
}
