namespace VacationRental.Application.Common.Helpers
{
    public static class ArithmeticHelpers
    {
        public static int ConvertToPositive(double number)
        {
            return ConvertToPositive((int)number);
        }

        public static int ConvertToPositive(int number)
        {
            return Math.Abs(number);
        }
    }
}
