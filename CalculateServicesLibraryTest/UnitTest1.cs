using CalculateServices;
using CalculateServices.Types;

namespace CalculateServicesLibraryTest
{
    [TestClass]
    public class UnitTest1
    {
        private const double Tolerance = 0.001;
        private const double POutValueTest = 0.001;
        private const double SpeedValueTest = 4.998459931479837888991505693418;
        private const double ReValueTest = 17.183778537681991463955982101964;

        private InPutSinglePipePressureGradientCalculation TestInputData = new InPutSinglePipePressureGradientCalculation()
        {
            Q = 4.6296296296296296296296296296296e-4,
            D = 0.1143,
            alpha = 90.00,
            PIn = 1500000,
            L = 900,
            dL = 300
        };

        [TestMethod]
        public void TestPipeCalculationModule()
        {
            var POutFact = CalculateServicesLibrary.PipeCalculationModule(TestInputData);
            var condition = MyTestComparison(POutValueTest, POutFact);
            Assert.IsTrue(condition, GetMessage(POutFact, POutValueTest, Tolerance));
        }

        [TestMethod]
        public void TestSpeed()
        {
            var speedFact = CalculateServicesLibrary.Speed(TestInputData);
            var condition = MyTestComparison(SpeedValueTest, speedFact);
            Assert.IsTrue(condition, GetMessage(speedFact, ReValueTest, Tolerance));
        }

        [TestMethod]
        public void TestReynoldsNumber()
        {
            var reFact = CalculateServicesLibrary.ReynoldsNumber(TestInputData);
            var condition = MyTestComparison(ReValueTest, reFact);
            Assert.IsTrue(condition, GetMessage(reFact, ReValueTest, Tolerance));
        }

        #region MyTestTools
        private bool MyTestComparison(double value1, double value2)
        {
            var delta = Math.Abs(value1 - value2);
            return Tolerance >= delta;
        }

        private string GetMessage(double factValue, double testValue, double tolerance)
        {
            return string.Format("Фактически расчитанное значение: '{0}', тестовое значение: {1}, погрешность: {2}", factValue, testValue, tolerance);
        }
        #endregion
    }
}