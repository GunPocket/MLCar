using System.Collections.Generic;
using UnityEngine;

public class PopulationObserver : MonoBehaviour {
    private Observer observer;
    public GameObject BestCar;

    public void Initialize(Observer obs) {
        observer = obs;
    }

    private void Update() {
        if (observer != null) {
            PopulationManager populationManager = observer.GetPopulationManager();
            if (populationManager != null) {
                List<CarAgent> population = populationManager.GetCarPopulation();
                if (population != null && population.Count > 0) {
                    CarController bestCar = population[0].GetCarController();
                    float bestFitness = bestCar.GetFitness();

                    foreach (CarAgent car in population) {
                        float fitness = car.GetFitness();
                        if (fitness > bestFitness) {
                            BestCar = car.gameObject;
                            bestFitness = fitness;
                        }
                    }
                } else {
                    Debug.LogWarning("Nenhum carro na popula��o. Verifique se a popula��o foi criada corretamente.");
                }
            } else {
                Debug.LogWarning("PopulationManager n�o atribu�do ao PopulationObserver. Certifique-se de chamar Initialize() com uma refer�ncia v�lida para ObserverHub.");
            }
        } else {
            Debug.LogWarning("ObserverHub n�o atribu�do ao PopulationObserver. Certifique-se de chamar Initialize() com uma refer�ncia v�lida para ObserverHub.");
        }
    }
}
