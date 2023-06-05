namespace CalculateServices.Types
{
    /// <summary>
    /// Выходные данные расчёта градиента давления одиночной трубы.
    /// </summary>
    public class OutPutSinglePipePressureGradientCalculation
    {
        /// <summary>
        /// давление, Па
        /// </summary>
        public double POut { get; set; }
        /// <summary>
        /// скорость, м/с
        /// </summary>
        public double v { get; set; }
        /// <summary>
        /// Число Re
        /// </summary>
        public double Re { get; set; }
    }
}
