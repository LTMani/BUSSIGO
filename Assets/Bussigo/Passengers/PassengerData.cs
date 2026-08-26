using System;
using UnityEngine;

namespace Bussigo.Passengers
{
    public enum PassengerCategory
    {
        SoloTraveller = 0,
        Family = 1,
        Student = 2,
        BusinessTraveller = 3,
        ElderlyPassenger = 4,
        Tourist = 5
    }

    public enum LuggageSize
    {
        None = 0,
        SmallHandbag = 1,
        MediumSuitcase = 2,
        LargeTrunk = 3
    }

    public enum LuggageState
    {
        Waiting = 0,
        LoadedInCabin = 1,
        StoredInLuggageBay = 2,
        Unloaded = 3
    }

    public enum BoardingState
    {
        WaitingInQueue = 0,
        TicketCheck = 1,
        BoardingBus = 2,
        Seated = 3,
        Deboarding = 4,
        TripCompleted = 5,
        BoardingDenied = 6
    }

    [Serializable]
    public class PassengerProfile
    {
        public string passengerID;
        public string passengerName;
        public PassengerCategory category;
        public string originNodeID;
        public string destinationNodeID;
        public int assignedSeatNumber;
        public LuggageSize luggageSize;
        public LuggageState luggageState;
        public BoardingState boardingState;

        [Range(0f, 100f)] public float satisfactionScore = 100.0f;
        public float patienceSeconds = 120.0f;
        public float comfortSensitivity = 1.0f;

        public PassengerProfile() { }

        public PassengerProfile(string id, string name, PassengerCategory cat, string origin, string dest, LuggageSize lug)
        {
            passengerID = id;
            passengerName = name;
            category = cat;
            originNodeID = origin;
            destinationNodeID = dest;
            luggageSize = lug;
            luggageState = (lug == LuggageSize.LargeTrunk || lug == LuggageSize.MediumSuitcase) ? LuggageState.StoredInLuggageBay : LuggageState.LoadedInCabin;
            boardingState = BoardingState.WaitingInQueue;
            satisfactionScore = 100.0f;
        }
    }
}
