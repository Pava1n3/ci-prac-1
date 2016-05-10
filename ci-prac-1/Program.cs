using System.Collections;
using System;

namespace ci_prac_1
{
    class Program
    {
        static int[,] startdoku = new int[2, 2];
        static Stack sudoStack = new Stack();

        static void Main(string[] args)
        {
            //sudoStack.Push(startdoku);

            Console.WriteLine("0 / 3 " + 0 / 3);
            Console.WriteLine("1 / 3 " + 1 / 3);
            Console.WriteLine("2 / 3 " + 2 / 3);
            Console.WriteLine("3 / 3 " + 3 / 3);
            Console.WriteLine("4 / 3 " + 4 / 3);
            Console.WriteLine("5 / 3 " + 5 / 3);
            Console.WriteLine("6 / 3 " + 6 / 3);
            Console.WriteLine("7 / 3 " + 7 / 3);
            Console.WriteLine("8 / 3 " + 8 / 3);


            Console.ReadLine();
        }

        /*
        procedure backtrack(L) returns t:
            if empty(L) 
                then return nil
            else
                t = first(L);
                if goal(t) 
                    then return t
                else
                    while there are unexplored successors of t and not found do
                        t’ = next-successor(t);
                        L = push(L,t’);
                        backtrack(L)
                    endwhile;
                L = pop(L)
        endprocedure
        */
        //static Stack stack = new Stack();

        //static int[,] BackTrack(Stack soduStack)
        //{
        //    int[,] t = (int[,])sudoStack.Peek();
        //    if (false)  //if(goal(t))
        //        return t;
        //    else

        //        while(true) //has-successors(t)
        //        {
        //            //int[,] s = next-successor(t);
        //            int[,] s = new int[2, 2];
        //            sudoStack.Push(s);
        //            BackTrack(sudoStack);
        //        }
        //    sudoStack.Pop();
        //}

    }
}
