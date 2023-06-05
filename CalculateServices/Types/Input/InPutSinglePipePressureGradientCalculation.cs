namespace CalculateServices.Types
{
    /// <summary>
    /// Входные данные расчёта градиента давления одиночной трубы
    /// </summary>
    public class InPutSinglePipePressureGradientCalculation
    {
        /// <summary>
        /// Расход жидкости, м3/с
        /// </summary>
        public double Q { get; set; }
        /// <summary>
        /// Внутренний диаметр трубы, м
        /// </summary>
        public double D { get; set; }
        /// <summary>
        /// Угол наклона трубы
        /// </summary>
        public double alpha { get; set; }
        /// <summary>
        /// Даление на входе трубы, Па;
        /// </summary>
        public double PIn { get; set; }
        /// <summary>
        /// Длина трубы, м
        /// </summary>
        public double L { get; set; }
        /// <summary>
        /// Максимальная длина сегмента, м
        /// </summary>
        public double dL { get; set; }

    }
}
