using Cinemachine;
using UnityEngine;

public class FollowCarCamera : MonoBehaviour {
    private Observer observer;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    public void Initialize(Observer obs) {
        observer = obs;
    }

    private void Update() {
        if (observer != null) {
            PopulationObserver populationObserver = observer.GetPopulationObserver();
            if (populationObserver != null) {
                GameObject bestCar = populationObserver.BestCar;
                if (bestCar != null) {
                    virtualCamera.Follow = bestCar.transform;
                }
            } else {
                Debug.LogWarning("PopulationObserver n�o atribu�do ao FollowCarCamera. Certifique-se de chamar Initialize() com uma refer�ncia v�lida para ObserverHub.");
            }
        } else {
            Debug.LogWarning("ObserverHub n�o atribu�do ao FollowCarCamera. Certifique-se de chamar Initialize() com uma refer�ncia v�lida para ObserverHub.");
        }
    }
}
