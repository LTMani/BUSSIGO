using System;
using UnityEngine;

namespace Bussigo.Core
{
    /// <summary>
    /// Master orchestrator for the BUSSIGO game lifecycle and service coordination.
    /// </summary>
    public class BussigoGameManager : MonoBehaviour, IService
    {
        public static BussigoGameManager Instance { get; private set; }

        public GameStateMachine StateMachine { get; private set; }

        [Header("Corridor Configuration")]
        public string activeCorridorName = "NH65: Vijayawada PNBS -> Hyderabad MGBS";
        public float totalRouteDistanceKm = 275.0f;
        public float currentDistanceDrivenKm = 0.0f;

        [Header("Trip Metrics")]
        public int boardedPassengers = 0;
        public int maxPassengerCapacity = 45;
        public float fuelLiters = 340.0f;
        public int fastagBalanceINR = 2500;
        public int earnedRevenueINR = 0;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize()
        {
            StateMachine = new GameStateMachine();
            StateMachine.RegisterState(new MainMenuState());
            StateMachine.RegisterState(new TerminalBoardingState());
            StateMachine.RegisterState(new HighwayDrivingState());
            StateMachine.RegisterState(new DestinationArrivalState());
            StateMachine.RegisterState(new TripSummaryState());

            StateMachine.ChangeState(GamePhase.MainMenu);

            ServiceLocator.Register<BussigoGameManager>(this);
            Debug.Log("[BUSSIGO] Core BussigoGameManager initialized successfully.");
        }

        public void Shutdown()
        {
            EventBus.Clear();
            Debug.Log("[BUSSIGO] BussigoGameManager shutdown complete.");
        }

        private void Update()
        {
            StateMachine?.Update(Time.deltaTime);
        }

        public void StartTrip()
        {
            currentDistanceDrivenKm = 0f;
            boardedPassengers = 0;
            StateMachine.ChangeState(GamePhase.TerminalBoarding);
            EventBus.Publish(new TripStartedEvent(activeCorridorName, totalRouteDistanceKm));
        }

        public void CompleteBoarding()
        {
            boardedPassengers = maxPassengerCapacity;
            earnedRevenueINR = boardedPassengers * 850;
            StateMachine.ChangeState(GamePhase.HighwayDriving);
            EventBus.Publish(new PassengerBoardingCompletedEvent(boardedPassengers, earnedRevenueINR));
        }

        public void DeductFastagToll(string plazaName, int tollAmount)
        {
            fastagBalanceINR = Mathf.Max(0, fastagBalanceINR - tollAmount);
            EventBus.Publish(new TollPlazaCrossedEvent(plazaName, tollAmount, fastagBalanceINR));
        }

        public void ArriveAtDestination()
        {
            StateMachine.ChangeState(GamePhase.DestinationArrival);
        }

        public void CompleteTrip()
        {
            int netProfit = earnedRevenueINR - 135;
            int driverXP = 850;
            boardedPassengers = 0;
            StateMachine.ChangeState(GamePhase.TripSummary);
            EventBus.Publish(new TripCompletedEvent("Hyderabad MGBS Platform 12", earnedRevenueINR, netProfit, driverXP));
        }
    }
}
