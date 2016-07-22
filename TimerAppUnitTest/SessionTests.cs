using NUnit.Framework;
using System;
using System.Collections.Generic;
using TimerApp.Model;

namespace TimerAppUnitTest
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public void LoadPrevTest()
        {
            var session = new Session(new Recipe { Steps = new List<Step> { new Step(), new Step(), new Step() } });
            Assert.False(session.CanLoadPreviousStep, $"can load previous step at index {session.CurrentStepIndex}, expected index 0");
        }

        [Test]
        public void LoadNextTest()
        {
            var session = new Session(new Recipe { Steps = new List<Step> { new Step(), new Step(), new Step() } });
            Assert.True(session.CanLoadNextStep, $"can't load next step at index {session.CurrentStepIndex}, expected index 0");
        }

        [Test]
        public void StartCurrentTest()
        {
            var session = new Session(new Recipe { Steps = new List<Step> { new Step(), new Step(), new Step() } });
            session.StartCurrentStep();
            var time = DateTime.UtcNow;
            Assert.True(session.CurrentStepStart < time, $"start time wrong {session.CurrentStepStart}, expected before {time}");
        }

        [Test]
        public void LoadPrevFinalTest()
        {
            var session = new Session(new Recipe { Steps = new List<Step> { new Step(), new Step(), new Step() } });
            session.StartNextStep();
            session.StartNextStep();
            Assert.True(session.CanLoadPreviousStep, $"can't load prev step at index {session.CurrentStepIndex}, expected index 2, last step");
        }

        [Test]
        public void LoadNextFinalTest()
        {
            var session = new Session(new Recipe { Steps = new List<Step> { new Step(), new Step(), new Step() } });
            session.StartNextStep();
            session.StartNextStep();
            Assert.False(session.CanLoadNextStep, $"can load next step at index {session.CurrentStepIndex}, expected index 2, last step");
        }
    }
}