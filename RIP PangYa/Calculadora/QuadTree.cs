using System;
using System.Collections.Generic;

namespace Calculadora
{
    public class QuadTree
    {
        Ball ball = null;
        Club club = null;
        Wind wind = null;

        int gravity_factor = 1;
        double gravity = 34.295295715332; // gravity in Yards(scale pangya)

        Vector3D _21D8_vect = new Vector3D(0.0, 0.0, 0.0);

        Vector3D ball_position_init = new Vector3D(0.0, 0.0, 0.0);
        double power_range_shot = 0.0;

        int shot = PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.DUNK);

        double power_factor_shot = 0.0;
        double percentShot_sqrt = 0.0;
        double spike_init = -1;
        double spike_med = -1;
        double power_factor = 0.0;
        double cobra_init = -1;


        public QuadTree()
        {

            this.club = new Club();
            this.ball = new Ball();
            this.wind = new Wind();
        }

        public double GetGravity()
        {
            return this.gravity * this.gravity_factor;
        }

        public void InitShot(Ball ball, Club club, Wind wind, Options options)
        {
            this.ball = ball;
            this.club = club;
            this.wind = wind;

            this.shot = options.Shot;
            this.spike_init = -1;
            this.spike_med = -1;
            this.cobra_init = -1;

            this.ball.position = options.Position.Clone();
            this.ball_position_init = options.Position.Clone();

            this.club.distance_state = TypeDistance.CalculateTypeDistance(options.Distance);

            this.ball.max_height = this.ball.position.y;

            this.ball.count = 0;
            this.ball.num_max_height = -1;

            double pwr = this.club.GetPower(options, options.Power.Pwr, options.Ps, options.Spin);

            this.power_range_shot = this.club.GetRange(options, options.Power.Pwr, options.Ps);

            this.power_factor = pwr;

            pwr *= Math.Sqrt(options.PercentShot);
            if (
                options.Shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.TOMAHAWK) ||
                options.Shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE)
            )
                pwr *= 1.3;
            else
                pwr *= 1.0;

            pwr *= Math.Sqrt(options.Ground * 0.01);

            this.power_factor_shot = pwr;
            this.percentShot_sqrt = Math.Sqrt(options.PercentShot);


            this.ball.curve = options.Curve;
            this.ball.spin = options.Spin;

            Dictionary<string, double> value1 = this.GetValuesDegree(options.MiraRad + (0 - (this.ball.curve * Constraints._00D17908)), 1);
            Dictionary<string, double> value2 = this.GetValuesDegree(this.club.distance_state == DistanceState.BIGGER_OR_EQUAL_58 ? this.club.GetDregRad() : this.club.GetDregRad() + (this.ball.spin * Constraints._00D19B98), 0);

            Random r = new Random();
            this.ball.curve -= this.GetSlope(options.MiraRad - options.SlopeMiraRad, r.NextDouble());

            pwr *= ((Math.Abs(this.ball.curve) * 0.1) + 1);

            Vector3D vectA = new Vector3D(value2["neg_sin"], value2["neg_rad"], value2["cos2"]);

            vectA.MultiplyScalar(pwr);

            Vector3D v1 = new Vector3D(value1["cos"], value1["rad"], value1["sin"]);
            Vector3D v2 = new Vector3D(value1["_C"], value1["_10"], value1["_14"]);
            Vector3D v3 = new Vector3D(value1["neg_sin"], value1["neg_rad"], value1["cos2"]);
            Vector3D v4 = new Vector3D(value1["_24"], value1["_28"], value1["_2C"]);

            this.ball.velocity.x = v2.x * vectA.y + vectA.x * v1.x + v3.x * vectA.z + v4.x;
            this.ball.velocity.y = v1.y * vectA.x + v2.y * vectA.y + v3.y * vectA.z + v4.y;
            this.ball.velocity.z = v1.z * vectA.x + v2.z * vectA.y + v3.z * vectA.z + v4.z;

            // Rotação eixo X, Z
            this.ball.rotation_curve = this.ball.curve * options.PercentShot;
            this.ball.rotation_spin = this.club.distance_state == DistanceState.BIGGER_OR_EQUAL_58
                ? (this.club.GetPower2(options, options.Power.Pwr, options.Ps) * options.PercentShot) * options.PercentShot
                : 0.0;

            this.ball.ball_48 = this.ball.ball_44;

        }

        public Dictionary<string, double> GetValuesDegree(double degree, int option = 0)
        {
            Dictionary<string, double> obj = new Dictionary<string, double>();

            if (option == 0)
            {

                obj["cos"] = 1.0;
                obj["rad"] = 0.0;
                obj["sin"] = 0.0;
                obj["_C"] = 0.0;
                obj["_10"] = Math.Cos(degree);
                obj["_14"] = Math.Sin(degree) * -1;
                obj["neg_sin"] = 0.0;
                obj["neg_rad"] = Math.Sin(degree);
                obj["cos2"] = obj["_10"];
                obj["_24"] = 0.0;
                obj["_28"] = 0.0;
                obj["_2C"] = 0.0;

            }
            else if (option == 1)
            {
                obj["cos"] = Math.Cos(degree);
                obj["rad"] = 0.0;
                obj["sin"] = Math.Sin(degree);
                obj["_C"] = 0.0;
                obj["_10"] = 1.0;
                obj["_14"] = 0.0;
                obj["neg_sin"] = obj["sin"] * -1;
                obj["neg_rad"] = 0.0;
                obj["cos2"] = obj["cos"];
                obj["_24"] = 0.0;
                obj["_28"] = 0.0;
                obj["_2C"] = 0.0;
            }

            return obj;
        }

        public double GetSlope(double aim, double lineBall)
        {

            Vector3D ball_slope_cross_const_vect = this.ball.slope.Clone().Normalize();

            Dictionary<string, Vector3D> slope_matrix = new Dictionary<string, Vector3D>();

            slope_matrix["v1"] = ball_slope_cross_const_vect.Clone().Normalize();
            slope_matrix["v2"] = this.ball.slope.Clone();
            slope_matrix["v3"] = ball_slope_cross_const_vect.Clone().Cross(this.ball.slope).Normalize();
            slope_matrix["v4"] = new Vector3D(0.0, 0.0, 0.0);


            Dictionary<string, double> value1 = this.GetValuesDegree(aim * -1, 1);
            Dictionary<string, double> value2 = this.GetValuesDegree(lineBall * -2.0, 1);

            Dictionary<string, Vector3D> m1 = this.ApplyMatrix(this.ValuesDegreesToMatrix(value2), slope_matrix);
            Dictionary<string, Vector3D> m2 = this.ApplyMatrix(m1, this.ValuesDegreesToMatrix(value1));

            return m2["v2"].x * Constraints._00D66CA0;
        }

        public Dictionary<string, Vector3D> ValuesDegreesToMatrix(Dictionary<string, double> dictionary)
        {
            Dictionary<string, Vector3D> obj = new Dictionary<string, Vector3D>();

            obj["v1"] = new Vector3D(dictionary["cos"], dictionary["rad"], dictionary["sin"]);
            obj["v2"] = new Vector3D(dictionary["_C"], dictionary["_10"], dictionary["_14"]);
            obj["v3"] = new Vector3D(dictionary["neg_sin"], dictionary["neg_rad"], dictionary["cos2"]);
            obj["v4"] = new Vector3D(dictionary["_24"], dictionary["_28"], dictionary["_2C"]);

            return obj;
        }

        public Dictionary<string, Vector3D> ApplyMatrix(Dictionary<string, Vector3D> m1, Dictionary<string, Vector3D> m2)
        {

            Dictionary<string, Vector3D> obj = new Dictionary<string, Vector3D>();

            obj["v1"] = new Vector3D(
                m1["v1"].x * m2["v1"].x + m1["v1"].y * m2["v2"].x + m1["v1"].z * m2["v3"].x,
                m1["v1"].x * m2["v1"].y + m1["v1"].y * m2["v2"].y + m1["v1"].z * m2["v3"].y,
                m1["v1"].x * m2["v1"].z + m1["v1"].y * m2["v2"].z + m1["v1"].z * m2["v3"].z
            );

            obj["v2"] = new Vector3D(
                m1["v2"].x * m2["v1"].x + m1["v2"].y * m2["v2"].x + m1["v2"].z * m2["v3"].x,
                m1["v2"].x * m2["v1"].y + m1["v2"].y * m2["v2"].y + m1["v2"].z * m2["v3"].y,
                m1["v2"].x * m2["v1"].z + m1["v2"].y * m2["v2"].z + m1["v2"].z * m2["v3"].z
            );

            obj["v3"] = new Vector3D(
                m1["v3"].x * m2["v1"].x + m1["v3"].y * m2["v2"].x + m1["v3"].z * m2["v3"].x,
                m1["v3"].x * m2["v1"].y + m1["v3"].y * m2["v2"].y + m1["v3"].z * m2["v3"].y,
                m1["v3"].x * m2["v1"].z + m1["v3"].y * m2["v2"].z + m1["v3"].z * m2["v3"].z
            );

            obj["v4"] = new Vector3D(
                m1["v4"].x * m2["v1"].x + m1["v4"].y * m2["v2"].x + m1["v4"].z * m2["v3"].x,
                m1["v4"].x * m2["v1"].y + m1["v4"].y * m2["v2"].y + m1["v4"].z * m2["v3"].y,
                m1["v4"].x * m2["v1"].z + m1["v4"].y * m2["v2"].z + m1["v4"].z * m2["v3"].z
            );

            return obj;
        }

        public void BallProcess(double steptime, double? final = null)
        {

            this.BounceProcess(steptime, final);

            if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.COBRA) && this.cobra_init < 0)
            {
                if (this.percentShot_sqrt < Math.Sqrt(0.8))
                {
                    this.percentShot_sqrt = Math.Sqrt(0.8);
                }
                if (this.ball.count == 0)
                {
                    this.ball.velocity.y = 0.0;
                    this.ball.velocity.Normalize().MultiplyScalar(this.power_factor_shot);
                }

                double diff = this.ball.position.Clone().Sub(this.ball_position_init).Length();
                double cobra_init_up = ((this.power_range_shot * this.percentShot_sqrt) - 100.0) * 3.2;

                if (diff >= cobra_init_up)
                {
                    double power_multiply = 0.0;

                    if (this.club.type == ClubType.WOOD)
                    {
                        switch (this.club.power_base)
                        {
                            case 230.0:
                                power_multiply = 74.0;
                                break;
                            case 210.0:
                                power_multiply = 76.0;
                                break;
                            case 190.0:
                                power_multiply = 80.0;
                                break;
                        }
                    }
                    this.cobra_init = this.ball.count;
                    this.ball.velocity.Normalize().MultiplyScalar(power_multiply).MultiplyScalar(this.percentShot_sqrt);
                    this.ball.rotation_spin = 2.5;

                }
            }
            else
            {
                if (this.spike_init < 0 && this.cobra_init < 0 && this.club.distance_state == DistanceState.BIGGER_OR_EQUAL_58)
                {

                    this.ball.rotation_spin -= (Constraints._00D66CA0 - (this.ball.spin * Constraints._00CFF040)) * Constraints._00D083A0;

                }
                else if ((this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) && this.spike_init >= 0) || (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.COBRA) && this.cobra_init >= 0))
                {
                    this.ball.rotation_spin -= Constraints._00D083A0;
                }


                if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) && this.ball.count == 0)
                {

                    this.ball.velocity.y = 0.0;
                    this.ball.velocity.Normalize().MultiplyScalar(this.power_factor_shot);

                    // check se a bola andou já
                    this.ball.velocity.Normalize().MultiplyScalar(72.5).MultiplyScalar(this.percentShot_sqrt * 2);

                    this.ball.rotation_spin = 3.1;

                    this.spike_init = this.ball.count;

                }

                if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) && this.ball.num_max_height >= 0 && (this.ball.num_max_height + 0x3C) < this.ball.count && this.spike_med < 0)
                {

                    this.spike_med = this.ball.count;

                    if (this.club.type == ClubType.WOOD)
                    {

                        double new_power;

                        switch (this.club.power_base)
                        {
                            case 230.0:

                                new_power = 344.0;

                                if ((this.power_factor * this.percentShot_sqrt) < 344.0)
                                    new_power -= (this.power_factor * this.percentShot_sqrt);
                                else
                                    new_power = 0.0;

                                new_power = new_power / 112.0 * 21.5;

                                new_power = -8 - new_power;

                                this.ball.velocity.y = new_power;

                                break;
                            case 210.0:

                                new_power = 306.0;

                                if ((this.power_factor * this.percentShot_sqrt) < 306.0)
                                    new_power -= (this.power_factor * this.percentShot_sqrt);
                                else
                                    new_power = 0.0;

                                new_power = new_power / 105.0 * 20.5;

                                new_power = -10.3 - new_power;

                                this.ball.velocity.y = new_power;

                                break;
                            case 190.0:

                                new_power = 273.0;

                                if ((this.power_factor * this.percentShot_sqrt) < 273.0)
                                    new_power -= (this.power_factor * this.percentShot_sqrt);
                                else
                                    new_power = 0.0;

                                new_power = new_power / 100 * 20.2;

                                new_power = -10.8 - new_power;

                                this.ball.velocity.y = new_power;

                                break;
                        }
                    }

                    this.ball.velocity.MultiplyScalar(this.percentShot_sqrt * 7);

                    this.ball.rotation_spin = this.ball.spin;

                }
            }

            if (this.ball.velocity.y < 0 && this.ball.num_max_height < 0)
            {

                this.ball.max_height = this.ball.position.y;
                this.ball.num_max_height = this.ball.count;
            }

            this.ball.count++;
        }

        public void BounceProcess(double steptime, double? final)
        {
            if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) && this.ball.num_max_height >= 0 && (this.ball.num_max_height + 0x3C) > this.ball.count)
                return;

            Vector3D accellVect = this.ApplyForce();

            Vector3D otherVect = accellVect.Clone();

            otherVect.DivideScalar(this.ball.mass).MultiplyScalar(steptime);

            this.ball.velocity.Add(otherVect);

            if (this.ball.num_max_height == -1)
            {

                Vector3D tmpVect = this._21D8_vect.Clone().DivideScalar(this.ball.mass).MultiplyScalar(steptime);

                this.ball.velocity.Add(tmpVect);
            }

            this.ball.ball_2C += (this.ball.rotation_curve * Constraints._00D1A888 * steptime);

            this.ball.ball_30 += (this.ball.rotation_spin * Constraints._00D3D210 * steptime);

            this.ball.position.Add(this.ball.velocity.Clone().MultiplyScalar((final != null ? (double)final : steptime)));
        }

        public Vector3D ApplyForce()
        {
            Vector3D retVect = new Vector3D(0.0, 0.0, 0.0);

            if (this.ball.rotation_curve != 0)
            {

                Vector3D vectorb = new Vector3D(this.ball.velocity.z * Constraints._00D046A8, 0, this.ball.velocity.x);

                vectorb.Normalize();

                if (this.cobra_init < 0 || this.spike_init < 0)
                    vectorb.MultiplyScalar(Constraints._00D00190 * this.ball.rotation_curve * this.club.rotation_curve);

                retVect.Add(vectorb);
            }

            if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) && this.spike_init < 0)
                return new Vector3D(0.0, 0.0, 0.0);
            else if (this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.COBRA) && this.cobra_init < 0)
                return retVect;

            Vector3D windVect = this.wind.GetWind();

            windVect.MultiplyScalar(this.shot == PowerShotFactory.GetIntValueFromShotType(ShotTypeEnum.SPIKE) ? Constraints._00D16758 : Constraints._00D083A0);

            retVect.Add(windVect);

            retVect.y = retVect.y - (this.GetGravity() * this.ball.mass);

            if (this.ball.rotation_spin != 0)
                retVect.y = retVect.y + (this.club.rotation_spin * Constraints._00D66CF8 * this.ball.rotation_spin);

            Vector3D velVect = this.ball.velocity.Clone();

            velVect.MultiplyScalar(velVect.Length() * Constraints._00D3D028);

            retVect.Sub(velVect);

            return retVect;
        }

    }

}
