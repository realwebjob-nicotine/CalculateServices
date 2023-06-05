using CalculateServices;
using CalculateServices.Types;
using Newtonsoft.Json;
using System.Formats.Asn1;

namespace CalculateServicesConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        var path = "InPutData.json";

        if (File.Exists(path))
        {
            try
            {
                var jsonString = File.ReadAllText(path);
                var inPutData = JsonConvert.DeserializeObject<InPutSinglePipePressureGradientCalculation>(jsonString);
                var outPutData = CalculateServicesLibrary.SinglePipePressureGradient(inPutData);
                File.WriteAllText(@"OutPutData.json", JsonConvert.SerializeObject(outPutData));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(string.Format("Ошибка входных данных ({0})", ex.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Отсутствует файл входных данных!");
        }
    }
}