namespace praveen {
    public class praveen
    {
        public static void Main(String[] args)
        {
            string pra = "hello praveen here";
            string[] veen=pra.Split(" ");
            string output = "";
            string reverse = string.Join(" ", pra.Split(' ').Reverse());
            Console.WriteLine(reverse);
            for(int i=veen.Length-1; i>=0; i--)
            {
                output = output +veen[i] +" ";
            }
            Console.WriteLine(output);

            int index = 5;
            for(int i = 0; i < index; i++)
            {
                for(int j = index-1; j >= i; j--)
                {
                    Console.Write(i);
                }
                Console.WriteLine();
            }

            //palindrome 
            Console.WriteLine("Enter a value");
            string value=Console.ReadLine();
            bool isPalindrome = true;

            for(int i = 0,j=value.Length-1; i < j; i++,j--)
            {
                if (value[i] != value[j])
                {
                    isPalindrome = false;
                    break;
                }
            }
            if(isPalindrome)
            {
                Console.WriteLine("Its a palindrome");
            }
            else
            {
                Console.WriteLine("Not a palindrome");
            }

            int input = int.Parse(Console.ReadLine());
            int a = 0;
            int b = 1;
            for(int i=0;i< input; i++)
            {
                Console.WriteLine(a);
                int temp = a+b;
                a = b;
                b=temp;
            }
        }
    }
}