namespace Calculadora
{

    public class Ball
    {
        public Vector3D position = new Vector3D(0.0, 0.0, 0.0);
        public Vector3D slope = new Vector3D(0.0, 1.0, 0.0);

        public int state_process = 0;
        public double max_height = 0.0;
        public int num_max_height = -1;
        public int count = 0;

        public Vector3D velocity = new Vector3D(0.0, 0.0, 0.0);

        public double ball_28 = 0.0;
        public double ball_2C = 0.0;
        public double ball_30 = 0.0;

        public double curve = 0.0;
        public double spin = 0.0;

        public double rotation_curve = 0.0;
        public double rotation_spin = 0.0;

        public int ball_44 = 0;
        public int ball_48 = 0;
        public int ball_70 = -1;
        public int ball_90 = 0;

        public int ball_BC = 0;

        public double mass = 0.045926999;
        public double diametro = 0.14698039;

        public void Copy(Ball other)
        {

            Ball cpy = this;

            cpy.position = other.position.Clone();
            cpy.slope = other.slope.Clone();
            cpy.velocity = other.velocity.Clone();
            cpy.state_process = other.state_process;
            cpy.max_height = other.max_height;
            cpy.spin = other.spin;
            cpy.curve = other.curve;
            cpy.count = other.count;
            cpy.num_max_height = other.num_max_height;
            cpy.ball_28 = other.ball_28;
            cpy.ball_2C = other.ball_2C;
            cpy.ball_30 = other.ball_30;
            cpy.ball_44 = other.ball_44;
            cpy.ball_48 = other.ball_48;
            cpy.ball_70 = other.ball_70;
            cpy.ball_90 = other.ball_90;
            cpy.ball_BC = other.ball_BC;
        }
    }
}
