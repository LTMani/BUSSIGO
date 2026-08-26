using System;
using UnityEngine;
using Bussigo.Core;
using Bussigo.Vehicle;

namespace Bussigo.World
{
    public class TerminalPlatformStation : MonoBehaviour
    {
        public string stationName = "Vijayawada PNBS Platform 4";
        public bool isOriginStation = true;
        public int waitingPassengerCount = 45;
        public bool isBusDocked = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BusPlayer"))
            {
                isBusDocked = true;
                if (!isOriginStation && BussigoGameManager.Instance != null)
                {
                    BussigoGameManager.Instance.ArriveAtDestination();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("BusPlayer"))
            {
                isBusDocked = false;
            }
        }
    }
}
