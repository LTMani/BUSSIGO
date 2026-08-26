using System;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;
using Bussigo.World;

namespace Bussigo.Passengers
{
    public class PassengerCrowdSimulator : MonoBehaviour
    {
        public BusChassisController playerBus;
        public TerminalPlatformStation originTerminal;
        public TerminalPlatformStation destTerminal;

        private float boardingTimer = 0f;
        private bool isBoardingFinished = false;

        private void Update()
        {
            if (playerBus == null) return;

            // Origin Platform Boarding Check
            if (originTerminal != null && originTerminal.isBusDocked && !isBoardingFinished)
            {
                if (playerBus.isDoorOpen && Mathf.Abs(playerBus.currentSpeedKmh) < 0.5f)
                {
                    boardingTimer += Time.deltaTime;
                    if (boardingTimer >= 0.15f)
                    {
                        boardingTimer = 0f;
                        if (BussigoGameManager.Instance != null && BussigoGameManager.Instance.boardedPassengers < BussigoGameManager.Instance.maxPassengerCapacity)
                        {
                            BussigoGameManager.Instance.boardedPassengers++;
                            playerBus.UpdatePayloadMass(BussigoGameManager.Instance.boardedPassengers);
                        }
                        else
                        {
                            isBoardingFinished = true;
                            if (BussigoGameManager.Instance != null)
                            {
                                BussigoGameManager.Instance.CompleteBoarding();
                            }
                        }
                    }
                }
            }

            // Destination Platform Alighting Check
            if (destTerminal != null && destTerminal.isBusDocked && isBoardingFinished)
            {
                if (playerBus.isDoorOpen && Mathf.Abs(playerBus.currentSpeedKmh) < 0.5f)
                {
                    boardingTimer += Time.deltaTime;
                    if (boardingTimer >= 0.15f)
                    {
                        boardingTimer = 0f;
                        if (BussigoGameManager.Instance != null && BussigoGameManager.Instance.boardedPassengers > 0)
                        {
                            BussigoGameManager.Instance.boardedPassengers--;
                            playerBus.UpdatePayloadMass(BussigoGameManager.Instance.boardedPassengers);
                        }
                        else
                        {
                            if (BussigoGameManager.Instance != null)
                            {
                                BussigoGameManager.Instance.CompleteTrip();
                            }
                        }
                    }
                }
            }
        }
    }
}
