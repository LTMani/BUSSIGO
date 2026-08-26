using System;
using NUnit.Framework;
using UnityEngine;
using Bussigo.Core;

namespace Bussigo.Tests.EditMode
{
    public class DummyTestService : IService
    {
        public bool IsInitialized { get; private set; } = false;
        public bool IsShutdown { get; private set; } = false;

        public void Initialize() => IsInitialized = true;
        public void Shutdown() => IsShutdown = true;
    }

    public struct TestCustomEvent : IGameEvent
    {
        public string Message;
        public int Value;
        public TestCustomEvent(string message, int value)
        {
            Message = message;
            Value = value;
        }
    }

    [TestFixture]
    public class CoreFoundationTests
    {
        [SetUp]
        public void Setup()
        {
            ServiceLocator.Reset();
            EventBus.Clear();
        }

        [TearDown]
        public void Teardown()
        {
            ServiceLocator.Reset();
            EventBus.Clear();
        }

        [Test]
        public void ServiceLocator_RegisterAndGet_SuccessfullyResolves()
        {
            var dummyService = new DummyTestService();
            ServiceLocator.Register<DummyTestService>(dummyService);

            Assert.IsTrue(dummyService.IsInitialized);

            var resolved = ServiceLocator.Get<DummyTestService>();
            Assert.AreSame(dummyService, resolved);

            bool found = ServiceLocator.TryGet<DummyTestService>(out var tryResolved);
            Assert.IsTrue(found);
            Assert.AreSame(dummyService, tryResolved);
        }

        [Test]
        public void ServiceLocator_Unregister_CallsShutdownAndRemoves()
        {
            var dummyService = new DummyTestService();
            ServiceLocator.Register<DummyTestService>(dummyService);
            ServiceLocator.Unregister<DummyTestService>();

            Assert.IsTrue(dummyService.IsShutdown);
            Assert.IsFalse(ServiceLocator.TryGet<DummyTestService>(out _));
        }

        [Test]
        public void EventBus_SubscribeAndPublish_HandlerReceivesEvent()
        {
            bool eventReceived = false;
            string receivedMsg = "";
            int receivedVal = 0;

            Action<TestCustomEvent> handler = (e) =>
            {
                eventReceived = true;
                receivedMsg = e.Message;
                receivedVal = e.Value;
            };

            EventBus.Subscribe(handler);
            EventBus.Publish(new TestCustomEvent("VijayawadaToHyderabad", 275));

            Assert.IsTrue(eventReceived);
            Assert.AreEqual("VijayawadaToHyderabad", receivedMsg);
            Assert.AreEqual(275, receivedVal);

            // Test Unsubscribe
            eventReceived = false;
            EventBus.Unsubscribe(handler);
            EventBus.Publish(new TestCustomEvent("TestUnsub", 100));

            Assert.IsFalse(eventReceived);
        }

        [Test]
        public void GameStateMachine_StateTransitions_ExecuteCorrectly()
        {
            var sm = new GameStateMachine();
            sm.RegisterState(new MainMenuState());
            sm.RegisterState(new TerminalBoardingState());
            sm.RegisterState(new HighwayDrivingState());

            GamePhase prev = GamePhase.MainMenu;
            GamePhase next = GamePhase.MainMenu;
            bool callbackTriggered = false;

            sm.OnStateChanged += (p, n) =>
            {
                callbackTriggered = true;
                prev = p;
                next = n;
            };

            sm.ChangeState(GamePhase.MainMenu);
            Assert.AreEqual(GamePhase.MainMenu, sm.CurrentPhase);

            sm.ChangeState(GamePhase.TerminalBoarding);
            Assert.IsTrue(callbackTriggered);
            Assert.AreEqual(GamePhase.MainMenu, prev);
            Assert.AreEqual(GamePhase.TerminalBoarding, next);
            Assert.AreEqual(GamePhase.TerminalBoarding, sm.CurrentPhase);
        }
    }
}
