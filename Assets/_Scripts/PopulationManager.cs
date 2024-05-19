using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
    public GameObject carPrefab;
    public int populationSize = 50;
    public float simulationTime = 30f;
    public float mutationRate = 0.01f;

    [SerializeField] private float elapsedTime = 0f;
    [SerializeField] private int generation = 1;

    public Transform carSpawnPoint;

    private List<CarAgent> population = new List<CarAgent>();
    private List<CarAgent> elitePopulation = new List<CarAgent>();

    void Start() {
        elapsedTime = 0f;
        generation = 1;

        if (carPrefab == null) {
            Debug.LogError("Car prefab is not assigned!");
            return;
        }

        if (carSpawnPoint == null) {
            Debug.LogError("CarSpawnPoint is not assigned!");
            return;
        }
        CreatePopulation();
    }

    void Update() {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= simulationTime) {
            EvaluatePopulation();
            elapsedTime = 0f;
            generation++;
        }
    }

    void CreatePopulation() {
        foreach (var car in population) {
            Destroy(car.gameObject);
        }
        population.Clear();
        elitePopulation.Clear();

        for (int i = 0; i < populationSize; i++) {
            GameObject newCarObject = Instantiate(carPrefab, carSpawnPoint.position, Quaternion.identity);
            CarAgent newCar = newCarObject.GetComponent<CarAgent>();
            if (newCar == null) {
                Debug.LogError("CarAgent component missing on car prefab!");
                continue;
            }
            newCar.SetNeuralNetwork(new NeuralNetwork(4, 2)); // Certifique-se de criar a rede neural aqui
            newCar.Initialize();
            population.Add(newCar);
        }
    }

    void EvaluatePopulation() {
        if (population.Count == 0) {
            Debug.LogWarning("No cars in population to evaluate.");
            return;
        }

        population.Sort((a, b) => b.GetFitness().CompareTo(a.GetFitness()));
        int eliteCount = Mathf.CeilToInt(populationSize * 0.1f);
        elitePopulation = population.GetRange(0, Mathf.Min(eliteCount, population.Count));

        foreach (CarAgent car in population) {
            car.ResetCar();
        }

        Reproduce();
    }

    void Reproduce() {
        List<CarAgent> newPopulation = new List<CarAgent>();

        foreach (CarAgent parent1 in elitePopulation) {
            foreach (CarAgent parent2 in elitePopulation) {
                if (parent1 != parent2) {
                    CarAgent offspring = Crossover(parent1, parent2);
                    Mutate(offspring);
                    newPopulation.Add(offspring);
                }
            }
        }

        while (newPopulation.Count < populationSize) {
            GameObject newCarObject = Instantiate(carPrefab, carSpawnPoint.position, Quaternion.identity);
            CarAgent newCar = newCarObject.GetComponent<CarAgent>();
            NeuralNetwork parentNN = elitePopulation[Random.Range(0, elitePopulation.Count)].GetNeuralNetwork();
            newCar.SetNeuralNetwork(parentNN.Copy()); // Copy the parent neural network
            newCar.Initialize();
            newPopulation.Add(newCar);
        }

        foreach (var car in population) {
            Destroy(car.gameObject);
        }
        population.Clear();
        population = newPopulation;

        Debug.Log("New population created with size: " + population.Count);
    }

    CarAgent Crossover(CarAgent parent1, CarAgent parent2) {
        GameObject newCarObject = Instantiate(carPrefab, carSpawnPoint.position, Quaternion.identity);
        CarAgent offspring = newCarObject.GetComponent<CarAgent>();

        offspring.SetNeuralNetwork(parent1.GetNeuralNetwork().Crossover(parent2.GetNeuralNetwork()));
        offspring.Initialize();

        return offspring;
    }

    void Mutate(CarAgent car) {
        car.GetNeuralNetwork().Mutate(mutationRate);
    }

    public CarAgent GetRandomCar() {
        if (population.Count > 0) {
            int randomIndex = UnityEngine.Random.Range(0, population.Count);
            return population[randomIndex];
        } else {
            Debug.LogWarning("Population is empty. No cars to select.");
            return null;
        }
    }

    public List<CarAgent> GetCarPopulation() {
        return population;
    }
}
