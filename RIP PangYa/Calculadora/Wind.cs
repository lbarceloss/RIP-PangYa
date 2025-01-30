using System;

namespace Calculadora
{
    public class Wind
    {
        public double wind = 0;
        public double degree = 0;

        public Vector3D GetWind()
        {
            return new Vector3D(this.wind * Math.Sin(this.degree * Math.PI / 180) * -1, 0, this.wind * Math.Cos(this.degree * Math.PI / 180));
        }
    }
}
