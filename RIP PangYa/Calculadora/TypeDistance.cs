namespace Calculadora
{

    public enum DistanceState
    {
        LESS_10,
        LESS_15,
        LESS_28,
        LESS_58,
        BIGGER_OR_EQUAL_58
    }

    public static class TypeDistance
    {
        public static DistanceState CalculateTypeDistance(double distance)
        {
            DistanceState current_state = DistanceState.LESS_10;
            if (distance >= 58.0f)
            {
                return DistanceState.BIGGER_OR_EQUAL_58;

            }
            else if (distance < 10.0f)
            {
                return DistanceState.LESS_10;

            }
            else if (distance < 15.0f)
            {
                return DistanceState.LESS_15;

            }
            else if (distance < 28.0f)
            {
                return DistanceState.LESS_28;

            }
            else if (distance < 58.0f)
            {
                return DistanceState.LESS_58;
            }
            return current_state;
        }
    }
}
