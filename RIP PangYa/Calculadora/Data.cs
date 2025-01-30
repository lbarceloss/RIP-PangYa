namespace Calculadora
{
    public class Data
    {
        public double power;
        public double desvio;
        public double power_range;
        public SmartData smart_data;

        public Data(double power, double desvio, double power_range, SmartData smart_data)
        {
            this.power = power;
            this.desvio = desvio;
            this.power_range = power_range;
            this.smart_data = smart_data;
        }
    }

}
