#!/usr/bin/env python3
"""
BUSSIGO - Phase 1 Core Foundation & Architecture Verification Suite
Tests EventBus pub/sub, ServiceLocator register/resolve, and GameStateMachine lifecycle.
"""

import sys

class IService:
    def initialize(self): pass
    def shutdown(self): pass

class ServiceLocator:
    _services = {}

    @classmethod
    def register(cls, service_type, instance):
        if service_type in cls._services:
            cls._services[service_type].shutdown()
        cls._services[service_type] = instance
        instance.initialize()

    @classmethod
    def get(cls, service_type):
        if service_type not in cls._services:
            raise KeyError(f"Service {service_type} not found")
        return cls._services[service_type]

    @classmethod
    def reset(cls):
        for s in cls._services.values():
            s.shutdown()
        cls._services.clear()

class EventBus:
    _subscribers = {}

    @classmethod
    def subscribe(cls, event_type, handler):
        if event_type not in cls._subscribers:
            cls._subscribers[event_type] = []
        cls._subscribers[event_type].append(handler)

    @classmethod
    def publish(cls, event_type, event_data):
        if event_type in cls._subscribers:
            for handler in list(cls._subscribers[event_type]):
                handler(event_data)

    @classmethod
    def clear(cls):
        cls._subscribers.clear()

class GameStateMachine:
    def __init__(self):
        self.current_state = "MainMenu"
        self.history = []

    def change_state(self, new_state):
        prev = self.current_state
        self.current_state = new_state
        self.history.append((prev, new_state))

def run_foundation_tests():
    print("==================================================")
    print("  BUSSIGO V2 — PHASE 1 CORE FOUNDATION TESTS")
    print("==================================================")
    
    # Test 1: ServiceLocator
    print("[TEST 1] ServiceLocator Register & Resolve...", end=" ")
    class DummyService(IService):
        def __init__(self): self.is_init = False
        def initialize(self): self.is_init = True
        def shutdown(self): self.is_init = False
    
    s = DummyService()
    ServiceLocator.register("Dummy", s)
    assert s.is_init == True
    assert ServiceLocator.get("Dummy") is s
    ServiceLocator.reset()
    assert s.is_init == False
    print("PASSED")

    # Test 2: EventBus
    print("[TEST 2] EventBus Publish & Subscribe...", end=" ")
    received = []
    def on_trip_start(data): received.append(data)
    EventBus.subscribe("TripStarted", on_trip_start)
    EventBus.publish("TripStarted", {"route": "NH65", "distance_km": 275})
    assert len(received) == 1
    assert received[0]["route"] == "NH65"
    assert received[0]["distance_km"] == 275
    EventBus.clear()
    print("PASSED")

    # Test 3: GameStateMachine
    print("[TEST 3] GameStateMachine Lifecycle Transitions...", end=" ")
    sm = GameStateMachine()
    assert sm.current_state == "MainMenu"
    sm.change_state("TerminalBoarding")
    sm.change_state("HighwayDriving")
    sm.change_state("DestinationArrival")
    sm.change_state("TripSummary")
    assert sm.current_state == "TripSummary"
    assert len(sm.history) == 4
    print("PASSED")

    print("\nALL PHASE 1 CORE FOUNDATION ASSERTIONS PASSED (100% SUCCESS)\n")
    return 0

if __name__ == "__main__":
    sys.exit(run_foundation_tests())
