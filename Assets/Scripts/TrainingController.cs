using UnityEngine;

public class TrainingController : MonoBehaviour {
    [SerializeField] CarAgent carAgent;

    void Start() {
        // Encontrar o agente e iniciar o epis�dio

        if (carAgent != null) {
            carAgent.OnEpisodeBegin();
        } else {
            Debug.LogError("CarAgent n�o encontrado!");
        }
    }

    void Update() {
        // Verificar condi��es de t�rmino do treinamento
        //if (/* condi��o de t�rmino */) {
        // Terminar o treinamento
        // Exemplo: UnityEditor.EditorApplication.isPlaying = false;
    }
}
