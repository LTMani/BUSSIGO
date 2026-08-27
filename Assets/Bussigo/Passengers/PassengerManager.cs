using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;

namespace Bussigo.Passengers
{
    /// <summary>
    /// Master passenger and travel logistics orchestrator.
    /// </summary>
    public class PassengerManager : MonoBehaviour, IService
    {
        [NonSerialized]
        public BoardingManager boardingManager = new BoardingManager();

        [NonSerialized]
        public PassengerSatisfactionSystem satisfactionSystem = new PassengerSatisfactionSystem();

        public BusChassisController playerBus;

        public float AverageSatisfaction => satisfactionSystem.CalculateAggregateSatisfaction(boardingManager.onboardPassengers);
        public int OnboardPassengerCount => boardingManager.onboardPassengers.Count;

        public void Initialize()
        {
            ServiceLocator.Register<PassengerManager>(this);
            // Default queue at Vijayawada PNBS
            boardingManager.PopulateTerminalQueue("NODE_VJA_PNBS", 38);
            Debug.Log("[BUSSIGO] PassengerManager initialized.");
        }

        public void Shutdown()
        {
            // Clean shutdown
        }

        private void Update()
        {
            if (playerBus == null) return;

            float dt = Time.deltaTime;
            if (dt <= 0f || dt > 0.1f) dt = 0.02f;

            // Apply driving telemetry to passenger satisfaction
            float longAccel = playerBus.currentSpeedKmh; // approximated acceleration delta
            float latAccel = 0f; // lateral G
            satisfactionSystem.ApplyDrivingTelemetryEvent(boardingManager.onboardPassengers, longAccel, latAccel, dt);
        }

        public void PerformTerminalBoardingSequence()
        {
            while (boardingManager.terminalWaitingQueue.Count > 0)
            {
                if (!boardingManager.ProcessNextBoardingPassenger(out _))
                {
                    break; // Bus full
                }
            }
        }

        public List<PassengerProfile> PerformNodeArrivalDeboarding(string currentNodeID)
        {
            return boardingManager.ProcessDeboardingAtNode(currentNodeID);
        }
    }
}
