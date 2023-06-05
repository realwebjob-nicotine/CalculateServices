using CalculateServices.Types;

namespace CalculateServices
{
    public static class CalculateServicesLibrary
    {
        /// <summary>
        /// Расчёта градиента давления одиночной трубы.
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns></returns>
        public static OutPutSinglePipePressureGradientCalculation SinglePipePressureGradient(InPutSinglePipePressureGradientCalculation InPutData)
        {
            var outPutData = new OutPutSinglePipePressureGradientCalculation();

            outPutData.POut = PipeCalculationModule(InPutData);
            outPutData.v = Speed(InPutData);
            outPutData.Re = ReynoldsNumber(InPutData);

            return outPutData;
        }

        #region Расчет давления

        /// <summary>
        /// Расчет сопротивления
        /// </summary>
        /// <param name="Re">Число Рейнольдса</param>
        /// <returns>Значение сопротивления</returns>
        private static double Resistance (double Re)
        {
            if (Re == 0)
            {
                throw new ArgumentException("Число Рейнольдса равно нулю.");
            }

            return 64 / Re;
        }

        /// <summary>
        /// Расчет потери на сопротивление
        /// </summary>
        /// <param name="f">Сопротивление</param>
        /// <param name="ro">Плотность потока, кг/м3</param>
        /// <param name="v">Скорость, м/с</param>
        /// <param name="D">Внутренний диаметр трубы, м</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static double ResistanceLoss(double f, double ro, double v, double D)
        {
            if (D <= 0)
            {
                throw new ArgumentException("Величина внутреннего диаметра трубы должна быть положительным числом, отличным от нуля.");
            }

            return (-0.5 * f * ro * Math.Pow(v, 2)) / D;
        }

        /// <summary>
        /// Расчет потери на сопротивление
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Значение потери на сопротивление</returns>
        public static double ResistanceLoss(InPutSinglePipePressureGradientCalculation InPutData)
        {
            var re = ReynoldsNumber(InPutData);
            var f = Resistance(re);
            var ro = DensityFlow(InPutData.PIn);
            var v = Speed(InPutData);

            return ResistanceLoss(f, ro, v, InPutData.D);
        }

        /// <summary>
        /// Расчет изменения давления за счет гравитации.
        /// </summary>
        /// <param name="ro">Плотность потока, кг/м3</param>
        /// <param name="alpha">угол наклона трубы</param>
        /// <returns>Величина значения изменения давления за счет гравитации</returns>
        private static  double ChangeDueToGravity(double ro, double alpha)
        {
            return - ro * 9.80665 * Math.Sin(Math.PI * alpha / 180.0);
        }

        /// <summary>
        /// Расчет изменения давления за счет гравитации.
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Величина значения изменения давления за счет гравитации</returns>
        public static double ChangeDueToGravity(InPutSinglePipePressureGradientCalculation InPutData)
        {
            var ro = DensityFlow(InPutData.PIn);

            return ChangeDueToGravity(ro, InPutData.alpha);
        }

        /// <summary>
        /// Расчёта градиента давления одиночной трубы
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Значение градиента давления</returns>
        public static double PressureGradient(InPutSinglePipePressureGradientCalculation InPutData)
        {
            return ChangeDueToGravity(InPutData) + ResistanceLoss(InPutData);
        }

        /// <summary>
        /// Модуль расчёта трубы
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Значение давления на выходе трубы, Па</returns>
        public static double PipeCalculationModule(InPutSinglePipePressureGradientCalculation InPutData)
        {
            if (InPutData.dL <= 0)
            {
                throw new ArgumentException("Максимальная длина сегмента трубы должна быть положительным числом, отличным от нуля.");
            }

            if (InPutData.L <= 0)
            {
                throw new ArgumentException("Длина трубы должна быть положительным числом, отличным от нуля.");
            }

            if (InPutData.dL > InPutData.L)
            {
                throw new ArgumentException("Длина сегмента трубы не может превышать длины трубы.");
            }

            var POut = 0.00;
            var L = InPutData.L;

            while (L != 0)      // цикл по сегментам трубы
            {
                POut = InPutData.PIn + PressureGradient(InPutData) * InPutData.dL;
                InPutData.PIn = POut;

                if (L >= InPutData.dL)
                {
                    L -= InPutData.dL;
                }
                else
                {
                    InPutData.dL = L;
                    L = 0;
                }
            }

            return POut;
        }

        #endregion

        #region Расчет скорости
        /// <summary>
        /// Расчет скорости
        /// </summary>
        /// <param name="Q">расход жидкости, м3/с.</param>
        /// <param name="D">внутренний диаметр трубы, м</param>
        /// <returns>Величина скорости (м/с)</returns>
        private static double Speed(double Q, double D)
        {
            if (D <= 0)
            {
                throw new ArgumentException("Величина внутреннего диаметра трубы должна быть положительным числом, отличным от нуля.");
            }
            if (Q <= 0)
            {
                throw new ArgumentException("Величина расход жидкости должна быть положительным числом, отличным от нуля.");
            }

            return Q / (0.25 * Math.PI * Math.Pow(D, 2));
        }
        /// <summary>
        /// Расчет скорости
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Величина скорости (м/с)</returns>
        public static double Speed(InPutSinglePipePressureGradientCalculation InPutData)
        {
            return Speed(InPutData.Q, InPutData.D);
        }
        #endregion

        #region расчет числа Рейнольдса
        /// <summary>
        /// Расчет плотности потока
        /// </summary>
        /// <param name="P">Давление (Па)</param>
        /// <returns>Величина плотности потока (кг/м3)</returns>
        private static double DensityFlow(double P)
        {
            if (P <= 9400000)
            {
                return (-57.8 * (P / 100000 - 1)) / 93 + 823.4;
            }
            else
            {
                return 765.57;
            }
        }

        /// <summary>
        /// Расчет вязкости потока
        /// </summary>
        /// <param name="P">Давление (Па)</param>
        /// <returns>Величина вязкости потока</returns>
        private static double ViscosityFlow(double P)
        {
            if (P <= 7700000)
            {
                return (-5.76 * (P / 100000 - 1)) / 76 + 7.6;
            }
            else
            {
                return 1.854;
            }
        }

        /// <summary>
        /// Расчет числа Рейнольдса
        /// </summary>
        /// <param name="P">Давление, Па</param>
        /// <param name="D">Ввнутренний диаметр трубы, м</param>
        /// <param name="v">Скорость, м/с</param>
        /// <returns>Значение числа Рейнольдса</returns>
        /// <exception cref="ArgumentException"></exception>
        private static double ReynoldsNumber(double P, double D, double v)
        {
            var u = ViscosityFlow(P);

            if (u == 0)
            {
                throw new ArgumentException("Вязкость потока равна нулю.");
            }

            return (DensityFlow(P) * v * D) / u;
        }

        /// <summary>
        /// Расчет числа Рейнольдса
        /// </summary>
        /// <param name="InPutData">Входные данные расчёта градиента давления одиночной трубы</param>
        /// <returns>Значение числа Рейнольдса</returns>
        /// <exception cref="ArgumentException"></exception>
        public static double ReynoldsNumber(InPutSinglePipePressureGradientCalculation InPutData)
        {
            var u = ViscosityFlow(InPutData.PIn);

            if (u == 0)
            {
                throw new ArgumentException("Вязкость потока равна нулю.");
            }

            var ro = DensityFlow(InPutData.PIn);
            var v = Speed(InPutData);

            return (ro * v * InPutData.D) / u;
        }
        #endregion
    }
}