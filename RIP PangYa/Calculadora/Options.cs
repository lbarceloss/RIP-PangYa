namespace Calculadora
{
    public class Options
    {
        public double Distance { get; set; }
        public double PercentShot { get; set; }
        public double Ground { get; set; }
        public double MiraRad { get; set; }
        public double SlopeMiraRad { get; set; }
        public double Spin { get; set; }
        public double Curve { get; set; }
        public Vector3D Position { get; set; }
        public int Shot { get; set; }
        public int Ps { get; set; }
        public Power Power { get; set; }

        public Options(double distance, double percentShot = 1.0, double ground = 0, double miraRad = 0.0, double slopeMiraRad = 0.0,
            double spin = 0.0, double curve = 0.0, Vector3D position = null, int shot = 0, int ps = 0, Power power = null)
        {
            Distance = distance;
            PercentShot = percentShot;
            Ground = ground;
            MiraRad = miraRad;
            SlopeMiraRad = slopeMiraRad;
            Spin = spin / 30;
            Curve = curve / 30;
            Position = position ?? new Vector3D(0.0, 0.0, 0.0);
            Shot = shot;
            Ps = ps;
            Power = power ?? new Power(31, new PowerOptions());
        }
    }

    public class Power
    {
        public int Pwr { get; set; }
        public PowerOptions Options { get; set; }

        public Power(int power, PowerOptions opt)
        {
            this.Pwr = power;
            this.Options = opt;
        }
    }

    public class PowerOptions
    {
        public int Auxpart { get; set; }
        public int Mascot { get; set; }
        public int Card { get; set; }
        public int PsAuxpart { get; set; }
        public int PsMascot { get; set; }
        public int PsCard { get; set; }

        public PowerOptions(int auxpart = 0, int mascot = 0, int card = 0, int psAuxpart = 0, int psMascot = 0, int psCard = 0)
        {
            Auxpart = auxpart;
            Mascot = mascot;
            Card = card;
            PsAuxpart = psAuxpart;
            PsMascot = psMascot;
            PsCard = psCard;
        }

        public int Total(int option)
        {
            int pwr = Auxpart + Mascot + Card;

            if (option == 1 || option == 2 || option == 3)
                pwr += PsAuxpart + PsMascot + PsCard;

            return pwr;
        }
    }

}
