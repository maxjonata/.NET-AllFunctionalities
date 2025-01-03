namespace CalculatorCmd
{
    class Calculator
    {
        public static decimal Addition(params List<decimal> numbers)
        {
            decimal finalNumber = 0;

            foreach (decimal number in numbers)
            {
                finalNumber += number;
            }

            return finalNumber;
        }

        public static decimal Subtraction(params List<decimal> numbers)
        {
            decimal finalNumber = numbers[0];
            numbers.RemoveAt(0);

            foreach (decimal number in numbers)
            {
                finalNumber -= number;
            }

            return finalNumber;
        }

        public static decimal Multiplication(params List<decimal> numbers)
        {
            decimal finalNumber = numbers[0];
            numbers.RemoveAt(0);

            foreach (decimal number in numbers)
            {
                finalNumber *= number;
            }

            return finalNumber;
        }

        public static decimal Division(params List<decimal> numbers)
        {
            decimal finalNumber = numbers[0];
            numbers.RemoveAt(0);

            foreach (decimal number in numbers)
            {
                finalNumber /= number;
            }

            return finalNumber;
        }

        public static decimal GenericCalculation(
            Func<decimal, decimal, decimal> operation,
            params List<decimal> numbers
        )
        {
            decimal finalNumber;

            // Find out starting number for operation
            if (operation(1m, 1m) == 2)
            {
                finalNumber = 0;
            }
            else
            {
                finalNumber = numbers[0];
                numbers.RemoveAt(0);
            }

            foreach (decimal number in numbers)
            {
                finalNumber = operation(finalNumber, number);
            }

            return finalNumber;
        }

        // For tests
        public static void Main(string[] args)
        {
            Console.WriteLine(
                "GenericCalculation addition 2+3 = "
                    + Calculator.GenericCalculation((x, y) => x + y, [2m, 3m])
            );
            Console.WriteLine(
                "GenericCalculation subtraction 2-3 = "
                    + Calculator.GenericCalculation((x, y) => x - y, [2m, 3m])
            );
            Console.WriteLine(
                "GenericCalculation multiplication 2*3 = "
                    + Calculator.GenericCalculation((x, y) => x * y, [2m, 3m])
            );
            Console.WriteLine(
                "GenericCalculation Division 2/3 = "
                    + Calculator.GenericCalculation((x, y) => x / y, [2m, 3m])
            );
            Console.WriteLine("Addition 2+3 = " + Calculator.Addition([2m, 3m]));
            Console.WriteLine("Subtraction 2-3 = " + Calculator.Subtraction([2m, 3m]));
            Console.WriteLine("Multiplication 2*3 = " + Calculator.Multiplication([2m, 3m]));
            Console.WriteLine("Division 2/3 = " + Calculator.Division([2m, 3m]));
        }
    }
}
