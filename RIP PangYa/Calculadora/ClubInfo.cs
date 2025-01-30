namespace Calculadora
{

    public enum ClubType
    {
        WOOD,
        IRON,
        SW,
        PW,
        PT
    }

    public class ClubInfo
    {
        public ClubType type { get; set; }
        public DistanceState type_distance = DistanceState.BIGGER_OR_EQUAL_58;
        public double rotation_spin { get; set; }
        public double rotation_curve { get; set; }
        public double power_factor { get; set; }
        public double degree { get; set; }
        public double power_base { get; set; }


        public ClubInfo(ClubType type, double rotation_spin, double rotation_curve, double power_factor, double degree, double power_base)
        {
            this.type = type;
            this.rotation_spin = rotation_spin;
            this.rotation_curve = rotation_curve;
            this.power_factor = power_factor;
            this.degree = degree;
            this.power_base = power_base;
        }
    }

}
