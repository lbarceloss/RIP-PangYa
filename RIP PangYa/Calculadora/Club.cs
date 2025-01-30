using System;

namespace Calculadora
{


    public class Club
    {
        public ClubType type = ClubType.WOOD;
        public DistanceState distance_state = DistanceState.BIGGER_OR_EQUAL_58;

        public double rotation_spin = 0.55;
        public double rotation_curve = 1.61;
        public double power_factor = 236;
        public double degree = 10;
        public double power_base = 230;

        public void Init(ClubInfo club_info)
        {
            this.type = club_info.type;

            this.rotation_spin = club_info.rotation_spin;
            this.rotation_curve = club_info.rotation_curve;
            this.power_factor = club_info.power_factor;
            this.degree = club_info.degree;
            this.power_base = club_info.power_base;
        }

        public double GetDregRad()
        {
            return this.degree * Math.PI / 180;
        }

        public double GetPower(Options extraPower, int pwrSlot, int ps, double spin)
        {
            double pwrjard = 0.0;
            switch (this.type)
            {
                case ClubType.WOOD:
                    {
                        pwrjard = extraPower.Power.Options.Total(ps) + PowerShotFactory.GetPowerShotFactory(ps) + ((pwrSlot - 15) * 2);
                        pwrjard *= 1.5;
                        pwrjard /= this.power_base;
                        pwrjard += 1;
                        pwrjard *= this.power_factor;
                        break;
                    }
                case ClubType.IRON:
                    {
                        pwrjard = ((PowerShotFactory.GetPowerShotFactory(ps) / this.power_base + 1.0) * this.power_factor) + (extraPower.Power.Options.Total(ps) * this.power_factor * 1.3) / this.power_base;
                        break;
                    }
                case ClubType.SW: // No código original, o SW é o mesmo que o PW.
                case ClubType.PW:
                    {
                        double getPowerByDegree(double degree, int _spin)
                        {
                            return 0.5 + ((0.5 * (degree + (_spin * Constraints._00D19B98))) / (56.0 / 180 * Math.PI));
                        }

                        switch (this.distance_state)
                        {
                            case DistanceState.LESS_10:
                            case DistanceState.LESS_15:
                            case DistanceState.LESS_28:
                                pwrjard = (getPowerByDegree(this.GetDregRad(), (int)spin) * (52.0 + (ps > 0 ? 28.0 : 0))) + (extraPower.Power.Options.Total(ps) * this.power_factor) / this.power_base;
                                break;
                            case DistanceState.LESS_58:
                                pwrjard = (getPowerByDegree(this.GetDregRad(), (int)spin) * (80.0 + (ps > 0 ? 18.0 : 0))) + (extraPower.Power.Options.Total(ps) * this.power_factor) / this.power_base;
                                break;
                            case DistanceState.BIGGER_OR_EQUAL_58:
                                pwrjard = ((PowerShotFactory.GetPowerShotFactory(ps) / this.power_base + 1.0) * this.power_factor) + (extraPower.Power.Options.Total(ps) * this.power_factor) / this.power_base;
                                break;
                        }
                        break;
                    }
                case ClubType.PT:
                    {
                        pwrjard = this.power_factor;

                        break;
                    }
            }
            return pwrjard;
        }


        public double GetPower2(Options extraPower, int pwrSlot, int ps)
        {
            double pwrjard = (extraPower.Power.Options.Auxpart + extraPower.Power.Options.Mascot + extraPower.Power.Options.Card) / 2 + (pwrSlot - 15);

            if (ps >= 0)
                pwrjard += (extraPower.Power.Options.PsCard / 2);

            pwrjard /= 170;

            return pwrjard + 1.5;
        }


        public double GetRange(Options extraPower, double pwrSlot, int ps)
        {
            double pwr_range = this.power_base + extraPower.Power.Options.Total(ps) + PowerShotFactory.GetPowerShotFactory(ps);
            if (this.type == ClubType.WOOD)
            {
                pwr_range += (pwrSlot - 15) * 2;
            }

            if (this.type == ClubType.PW)
            {
                switch (this.distance_state)
                {
                    case DistanceState.LESS_10:
                    case DistanceState.LESS_15:
                    case DistanceState.LESS_28:
                        pwr_range = 30.0 + (ps != 0 ? 30.0 : 0.0) + extraPower.Power.Options.Total(ps);
                        break;
                    case DistanceState.LESS_58:
                        pwr_range = 60.0 + (ps != 0 ? 20.0 : 0.0) + extraPower.Power.Options.Total(ps);
                        break;
                    case DistanceState.BIGGER_OR_EQUAL_58:
                        pwr_range = this.power_base + extraPower.Power.Options.Total(ps) + PowerShotFactory.GetPowerShotFactory(ps);
                        break;
                }
            }
            return pwr_range;
        }
    }
}
