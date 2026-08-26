using System;
using UnityEngine;

namespace Bussigo.Core
{
    public class MainMenuState : IGameState
    {
        public GamePhase Phase => GamePhase.MainMenu;

        public void OnEnter()
        {
            Debug.Log("[GameState] Entered MainMenuState.");
        }

        public void OnUpdate(float deltaTime) { }

        public void OnExit()
        {
            Debug.Log("[GameState] Exited MainMenuState.");
        }
    }

    public class TerminalBoardingState : IGameState
    {
        public GamePhase Phase => GamePhase.TerminalBoarding;

        public void OnEnter()
        {
            Debug.Log("[GameState] Entered TerminalBoardingState: Waiting for passengers to board at origin platform.");
        }

        public void OnUpdate(float deltaTime) { }

        public void OnExit()
        {
            Debug.Log("[GameState] Exited TerminalBoardingState: Departure clearance granted.");
        }
    }

    public class HighwayDrivingState : IGameState
    {
        public GamePhase Phase => GamePhase.HighwayDriving;

        public void OnEnter()
        {
            Debug.Log("[GameState] Entered HighwayDrivingState: Cruising on NH65 corridor.");
        }

        public void OnUpdate(float deltaTime) { }

        public void OnExit()
        {
            Debug.Log("[GameState] Exited HighwayDrivingState.");
        }
    }

    public class DestinationArrivalState : IGameState
    {
        public GamePhase Phase => GamePhase.DestinationArrival;

        public void OnEnter()
        {
            Debug.Log("[GameState] Entered DestinationArrivalState: Docking into destination terminal.");
        }

        public void OnUpdate(float deltaTime) { }

        public void OnExit()
        {
            Debug.Log("[GameState] Exited DestinationArrivalState.");
        }
    }

    public class TripSummaryState : IGameState
    {
        public GamePhase Phase => GamePhase.TripSummary;

        public void OnEnter()
        {
            Debug.Log("[GameState] Entered TripSummaryState: Calculating revenue, fuel expenses, and driver XP.");
        }

        public void OnUpdate(float deltaTime) { }

        public void OnExit()
        {
            Debug.Log("[GameState] Exited TripSummaryState.");
        }
    }
}
