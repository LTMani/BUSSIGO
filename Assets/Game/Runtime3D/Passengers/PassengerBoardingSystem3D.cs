using System;
using System.Collections.Generic;
using UnityEngine;
using Bussigo.Game.Runtime3D.Vehicle;
using Bussigo.Game.Runtime3D.Environment;

namespace Bussigo.Game.Runtime3D.Passengers
{
    public class PassengerBoardingSystem3D : MonoBehaviour
    {
        public UnityBusController3D playerBus;
        public BusTerminalStation3D originStation;
        public BusTerminalStation3D destinationStation;

        [Header("Passenger State")]
        public int totalWaitingPassengers = 45;
        public int currentBoardedPassengers = 0;
        public int maxSeatingCapacity = 49;
        public bool isBoardingComplete = false;
        public bool isAlightingComplete = false;

        private List<GameObject> passengerEntities = new List<GameObject>();
        private float boardingTimer = 0f;

        public void InitializeStationCrowd(Transform stationPlatform)
        {
            Color[] clothes = new Color[] {
                new Color(0.85f, 0.2f, 0.2f),
                new Color(0.2f, 0.45f, 0.85f),
                new Color(0.9f, 0.8f, 0.15f),
                new Color(0.2f, 0.75f, 0.35f),
                new Color(0.95f, 0.95f, 0.95f)
            };

            for (int i = 0; i < totalWaitingPassengers; i++)
            {
                Vector3 spawnPos = stationPlatform.position + new Vector3(
                    UnityEngine.Random.Range(-2.5f, 2.5f),
                    0.6f,
                    UnityEngine.Random.Range(-8f, 8f)
                );

                GameObject pax = ProceduralPassengerMeshBuilder.CreatePassengerCharacter(
                    stationPlatform,
                    spawnPos,
                    clothes[i % clothes.Length]
                );
                passengerEntities.Add(pax);
            }
        }

        private void Update()
        {
            if (playerBus == null) return;

            // Check Origin Boarding
            if (originStation != null && originStation.isBusDockedInPlatform && !isBoardingComplete)
            {
                if (playerBus.isDoorOpen && playerBus.currentSpeedKmh < 1.0f)
                {
                    boardingTimer += Time.deltaTime;
                    if (boardingTimer >= 0.25f && currentBoardedPassengers < totalWaitingPassengers)
                    {
                        boardingTimer = 0f;
                        currentBoardedPassengers++;

                        // Fade out / remove boarded passenger from platform
                        if (passengerEntities.Count > 0)
                        {
                            GameObject p = passengerEntities[passengerEntities.Count - 1];
                            passengerEntities.RemoveAt(passengerEntities.Count - 1);
                            Destroy(p);
                        }

                        if (currentBoardedPassengers >= totalWaitingPassengers)
                        {
                            isBoardingComplete = true;
                            originStation.arePassengersBoarded = true;
                            Debug.Log($"[Boarding] All {currentBoardedPassengers} passengers boarded. Ready for departure to Hyderabad!");
                        }
                    }
                }
            }

            // Check Destination Alighting
            if (destinationStation != null && destinationStation.isBusDockedInPlatform && isBoardingComplete && !isAlightingComplete)
            {
                if (playerBus.isDoorOpen && playerBus.currentSpeedKmh < 1.0f)
                {
                    boardingTimer += Time.deltaTime;
                    if (boardingTimer >= 0.20f && currentBoardedPassengers > 0)
                    {
                        boardingTimer = 0f;
                        currentBoardedPassengers--;

                        if (currentBoardedPassengers <= 0)
                        {
                            isAlightingComplete = true;
                            destinationStation.arePassengersDroppedOff = true;
                            Debug.Log("[Alighting] All passengers alighted at Hyderabad MGBS! Trip complete.");
                        }
                    }
                }
            }
        }
    }
}
