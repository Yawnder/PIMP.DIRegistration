using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PIMP.DIRegistration.Core;
using PIMP.DIRegistration.CoreTest.TestImplementation;

namespace PIMP.DIRegistration.CoreTest
{
    [TestClass]
    public class RegistrationCensorTest
    {

        public bool ExecuteSlowTests { get; } = false;
        public bool ExecuteOrderTests { get; } = false;

        private static object dependancyOrderLock = new object();

        [TestMethod]
        public void RegistrationCensor_RegisterAll_NoContextFilters_Implicit()
        {
            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object);

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_NoContextFiltersExplicit()
        {
            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object, "");

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_OneContextFilterRight()
        {
            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = "";
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object, "");

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_OneContextFilterWrong()
        {
            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = "Bar";
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object, "");

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_TwoContextFilterWrong()
        {
            // Arrange
            RegistratorTest1.ValidContext = "Foo";
            RegistratorTest2.ValidContext = "Bar";
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object, "");

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAllAsync_Fast()
        {
            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>())).Callback(() => Thread.Sleep(1));

            // Act
            var registrationTask = RegistrationCensor<IContainer>.RegisterAllAsync(mockContainer.Object, "");
            var executed = registrationTask.Wait(500);

            // Assert
            Assert.IsTrue(executed);
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAllAsync_Slow()
        {
            if (!this.ExecuteSlowTests)
                Assert.Inconclusive("!this.ExecuteSlowTests");

            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = null;

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>())).Callback(() => Thread.Sleep(250));

            // Act
            var registrationTask = RegistrationCensor<IContainer>.RegisterAllAsync(mockContainer.Object, "");
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(0));
            Thread.Sleep(250);
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(1));
            var executed = registrationTask.Wait(500);

            // Assert
            Assert.IsTrue(executed);
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_DependancyOrder1()
        {
            if (!this.ExecuteOrderTests)
                Assert.Inconclusive("!this.ExecuteOrderTests");

            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = null;
            RegistratorTest2.Dependancy = nameof(RegistratorTest1);

            string expectedCallOrder = "RegistratorTest1RegistratorTest2";
            string actualCallOrder = "";

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()))
                .Callback<string>((n) => { actualCallOrder += n; });

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object);

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(expectedCallOrder, actualCallOrder);
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_DependancyOrder2()
        {
            if (!this.ExecuteOrderTests)
                Assert.Inconclusive("!this.ExecuteOrderTests");

            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = nameof(RegistratorTest2);
            RegistratorTest2.Dependancy = null;

            string expectedCallOrder = "RegistratorTest2RegistratorTest1";
            string actualCallOrder = "";

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()))
                .Callback<string>((n) => { actualCallOrder += n; });

            // Act
            RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object);

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(2));
            Assert.AreEqual(expectedCallOrder, actualCallOrder);
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_Dependancy_Recursive()
        {
            if (!this.ExecuteOrderTests)
                Assert.Inconclusive("!this.ExecuteOrderTests");

            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = nameof(RegistratorTest2);
            RegistratorTest2.Dependancy = nameof(RegistratorTest1);

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            try
            {
                RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object);
                Assert.Fail("Shouldn't reach this point.");
            }
            catch (MissingDependencyException)
            { }
            catch
            {
                Assert.Fail("Shouldn't reach this point.");
            }

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(0));
        }

        [TestMethod]
        public void RegistrationCensor_RegisterAll_Dependancy_Missing()
        {
            if (!this.ExecuteOrderTests)
                Assert.Inconclusive("!this.ExecuteOrderTests");

            // Arrange
            RegistratorTest1.ValidContext = null;
            RegistratorTest2.ValidContext = null;
            RegistratorTest1.Dependancy = "MissingDependancy";
            RegistratorTest2.Dependancy = nameof(RegistratorTest1);

            var mockContainer = new Mock<IContainer>(MockBehavior.Strict);
            mockContainer.Setup(m => m.Register(It.IsAny<string>()));

            // Act
            try
            {
                RegistrationCensor<IContainer>.RegisterAll(mockContainer.Object);
                Assert.Fail("Shouldn't reach this point.");
            }
            catch (MissingDependencyException)
            { }
            catch
            {
                Assert.Fail("Shouldn't reach this point.");
            }

            // Assert
            mockContainer.Verify(m => m.Register(It.IsAny<string>()), Times.Exactly(0));
        }
    }
}