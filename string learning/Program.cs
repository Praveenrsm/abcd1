
using System.Data;
using System.Reflection.Metadata.Ecma335;

public delegate int calculate(int a, int b);
public class calculation{
    public event calculate CalculationEvent;
    public int addition(int a, int b) { int c = a + b; Console.WriteLine("Addition "+c); return c; }
    public int subtractition(int a, int b) { int c = a - b; Console.WriteLine("Subtraction "+c); return c; }
    public int multiply(int a, int b) { int c = a * b; Console.WriteLine("Multiplication "+c); return c; }
    public int divide(int a, int b) { int c = a / b; Console.WriteLine("Division "+c); return c; }
    public void OnCalculate(int a,int b)
    {
        if(CalculationEvent != null) {
            CalculationEvent.Invoke(a, b);
        }
        else
        {
            CalculationEvent.Invoke(a,b);
        }
    }
}
public static class user
{

    public static string admin(this int marks, string name) { return name + marks; }
    static int marks = 90; static string output = marks.admin("Ramesh");
}
    

    class Program
{
    //int[] a = { 0, 1, 2 };
    //int[] b = new int[10];

    //public int this[int index]
    //{
    //    get { return index; }
    //    set {  a[index] = value; }
    //}



    static string lastOperator = "+"; 
    static void Main()
    {
            int marks = 90; string output = marks.admin("Ramesh");
        Console.WriteLine(output);

            double result = 0;
        bool isFirstInput = true;
            while (true)
            {
                double number;
                string input;

                Console.Write("Enter number: ");
                input = Console.ReadLine();
                if (!double.TryParse(input, out number))
                {
                    Console.WriteLine("Invalid number. Try again.");
                    continue;
                }


                if (isFirstInput)
                {
                    result = number;
                    isFirstInput = false;
                }
                else
                {
                    switch (lastOperator)
                    {
                        case "+": result += number; break;
                        case "-": result -= number; break;
                        case "*": result *= number; break;
                        case "/":
                            if (number == 0)
                            {
                                Console.WriteLine("Cannot divide by zero.");
                                continue;
                            }
                            result /= number; break;
                        case "%": result %= number; break;
                    }
                }

                Console.Write("Enter operator (+, -, *, /, %, =): ");
                input = Console.ReadLine();

                if (input == "=")
                {
                    Console.WriteLine("Result = " + result);
                    result = 0;
                }
                else if ("+-*/%".Contains(input))
                {
                    lastOperator = input;
                }
                else
                {
                    Console.WriteLine("Invalid operator. Please enter one of +, -, *, /, %, =");
                break;
                }
            }
        while (true)
        {
            Console.Write("Enter expression: ");
            string inputs = Console.ReadLine();

            var results = new DataTable().Compute(inputs, null);
            Console.WriteLine("Result: " + results);
        }



        //        Program thismethod = new Program();
        //int indexer1 = thismethod[2];
        //int indexer2 = thismethod[11];
        //Console.WriteLine("indexer1 value is :" + indexer1 + "indexer2 value is" + indexer2);
        //for(int i = 0; i < thismethod.a.Length; i++)
        //{
        //    Console.WriteLine(thismethod.a[i]);
        //}
        //Console.WriteLine("Max Generation" + GC.MaxGeneration);
        //Program pro=new Program();
        //Console.WriteLine("Proram object is in which garbage collection" + GC.GetGeneration(pro));
        //Console.WriteLine("Total Memory Allocated before GC " + GC.GetTotalMemory(false));
        //GC.Collect(0);
        //Console.WriteLine("Total Memory Allocated after GC " + GC.GetTotalMemory(false));
        //calculation calculate = new calculation();
        //////single cast delegate
        //calculate operation = new calculate(calculate.addition);
        ////multicast delegates
        //operation += calculate.subtractition;
        ////ananymous delegate
        //operation += delegate (int x, int y)
        //{
        //    int c = x % y;
        //    Console.WriteLine(c);
        //    return c;
        //};
        //operation(1, 2);

        ////Events:
        //calculate.CalculationEvent += calculate.addition;
        //calculate.CalculationEvent += calculate.subtractition;
        //calculate.OnCalculate(1, 2);
    } 
}