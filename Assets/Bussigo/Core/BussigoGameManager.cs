using System;
using UnityEngine;

namespace Bussigo.Core
{
    public enum GamePhase
    {
        MainMenu = 0,
        TerminalBoarding = 1,
        HighwayDriving = 2,
        TollPlazaApproaching = 3,
        DestinationArrival = 4,
        TripSummary = 5
    }

    public class BussigoGameManager : MonoBehaviour
    {
        public static BussigoGameManager Instance { get; private set; }

        [Header("Runtime State")]
        public GamePhase currentPhase = GamePhase.MainMenu;
        public string activeCorridorName = "NH65: Vijayawada PNBS -> Hyderabad MGBS";
        public float totalRouteDistanceKm = 275.0f;
        public float currentDistanceDrivenKm = 0.0f;

        [Header("Trip Live Telemetry")]
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
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartTrip()
        {
            currentPhase = GamePhase.TerminalBoarding;
            currentDistanceDrivenKm = 0f;
            boardedPassengers = 0;
            Debug.Log("[BUSSIGO] Trip started at Vijayawada PNBS Platform 4. Ready for passenger boarding.");
        }

        public void CompleteBoarding()
        {
            boardedPassengers = maxPassengerCapacity;
            currentPhase = GamePhase.HighwayDriving;
            earnedRevenueINR = boardedPassengers * 850; // ₹850 per ticket
            Debug.Log($"[BUSSIGO] Boarding complete: {boardedPassengers} passengers boarded. Revenue: ₹{earnedRevenueINR}.");
        }

        public void DeductFastagToll(int tollAmount)
        {
            fastagBalanceINR = Mathf.Max(0, fastagBalanceINR - tollAmount);
            Debug.Log($"[BUSSIGO] FASTag Toll Paid: ₹{tollAmount}. New Balance: ₹{fastagBalanceINR}.");
        }

        public void ArriveAtDestination()
        {
            currentPhase = GamePhase.DestinationArrival;
            Debug.Log("[BUSSIGO] Bus arrived at Hyderabad MGBS Platform 12. Ready for passenger alighting.");
        }

        public void CompleteTrip()
        {
            boardedPassengers = 0;
            currentPhase = GamePhase.TripSummary;
            Debug.Log($"[BUSSIGO] Trip completed successfully! Final Profit: ₹{earnedRevenueINR - 135}.");
        }
    }
}
