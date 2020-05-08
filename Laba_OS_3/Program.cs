using System;
using System.Collections.Generic;

namespace Laba_OS_3
{
    enum Menu { EnterData = 1, Info, Exit}
    class Program
    {
        static void Main(string[] args)
        {
            PrintInfo();
            while (true)
            {
                Console.WriteLine($"1 - Enter data and start{Environment.NewLine}2 - Info{Environment.NewLine}3 - Exit");
                Console.Write("menu:");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case (int)Menu.EnterData:
                            {
                                int blockSize = GetBlockSize();
                                List<int> pages = GetPages(blockSize);
                                Queue<int> queue = new Queue<int>();
                                for (int i = 0; i != blockSize; ++i)
                                {
                                    queue.Enqueue(pages[i]);
                                }
                                for (int i = 0; i != blockSize; ++i)
                                {
                                    pages.RemoveAt(0);
                                }
                                Compare(ref queue, ref pages);
                                break;
                            }
                        case (int)Menu.Info:
                            PrintInfo();
                            break;
                        case (int)Menu.Exit:
                            return;
                    }
                }
            }
        }
        static void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine("Laboratory work number 3 of OS");
            Console.WriteLine("Author: Osipov Igor, 484 group");
            Console.WriteLine($"{Environment.NewLine}This program illustrate work of FIFO and LRU algorithms of memorypage organization");
            Console.WriteLine();
        }

        static int GetBlockSize()
        {
            Console.WriteLine("Enter amount of page blocks");
            Console.Write("amount:");
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Bad input, try again");
                Console.Write("amount:");
            }  
            return value;
        }

        static List<int> GetPages(int size)
        {
            List<int> arr = null;
            while (arr == null)
            {
                Console.WriteLine($"Enter separated by space numbers of pages and press enter, minamount is { size + 1 }");
                Console.Write("numbers:");

                string inputLine = Console.ReadLine();
                string[] inputArr = inputLine.Split(' ');
                arr = new List<int>();
                int value;

                if (inputArr.Length < size + 1)
                {
                    Console.WriteLine($"There is {inputArr.Length} pages, but should be more than {size}");
                    arr = null;
                }                    
                else
                {
                    foreach (var el in inputArr)
                    {
                        if (int.TryParse(el, out value))
                            arr.Add(value);
                        else
                        {
                            Console.WriteLine("Bad input, try again");
                            Console.Write("numbers:");
                            arr = null;
                            break;
                        }
                    }
                }
            }
            return arr;
        }

        static void Compare(ref Queue<int> queue, ref List<int> pages)
        {
            Console.WriteLine();
            Queue<int> queueCopy1 = new Queue<int>(queue);
            Queue<int> queueCopy2 = new Queue<int>(queue);
            int breaksFIFO = FIFO(ref queueCopy2, ref pages);
            int breaksLRU = LRU(ref queueCopy1, ref pages);

            if (breaksLRU < breaksFIFO)
                Console.WriteLine("LRU is optimal algorithm for this set of pages");
            else if (breaksLRU > breaksFIFO)
                Console.WriteLine("FIFO is optimal algorithm for this set of pages");
            else
                Console.WriteLine("Both algorithms are equal for this set of pages");
            
            Console.WriteLine($"Page breaks in FIFO: {breaksFIFO}");
            Console.WriteLine($"Page breaks in LRU: {breaksLRU}");
            Console.WriteLine();
        }

        static int FIFO(ref Queue<int> queue, ref List<int> pages)
        {
            Console.Write($"FIFO{Environment.NewLine}Current pages:");
            int breakCounter = 0;
            foreach (var el in queue)
                Console.Write(el.ToString() + " ");
            
            Console.Write(Environment.NewLine);
            foreach (var page in pages)
            {
                if (AddPage(ref queue, page))
                {
                    foreach (var el in queue)
                        Console.Write(el.ToString() + " ");
                    Console.WriteLine($" <- {page}; First out, {page} in");
                    ++breakCounter;
                }
                else
                {
                    foreach (var el in queue)
                        Console.Write(el.ToString() + " ");                  
                    Console.WriteLine($" <- {page}");
                }
            }
            return breakCounter;
        }

        static bool AddPage(ref Queue<int> queue, int page)
        {
            if (!queue.Contains(page))
            {
                queue.Dequeue();
                queue.Enqueue(page);
                return true;
            }
            return false;
        }

        static int LRU(ref Queue<int> queue, ref List<int> pages)
        {
            Console.Write($"LRU{Environment.NewLine}Current pages:");
           
            int breakCounter = 0;
            foreach (var el in queue)
                Console.Write(el.ToString() + " ");

            Console.Write(Environment.NewLine);
            foreach (var page in pages)
            {
                if (queue.Contains(page))
                {
                    Queue<int> tempQ = new Queue<int>();
                    int size = queue.Count;
                    bool notFirst = false;
                    for (var i = 0; i != size; ++i)
                    {
                        var q = queue.Dequeue();
                        if (q != page || notFirst)
                            tempQ.Enqueue(q);
                        else if (q == page)
                            notFirst = true;
                    }
                    tempQ.Enqueue(page);

                    foreach (var el in tempQ)
                        queue.Enqueue(el);

                    foreach (var el in queue)
                        Console.Write(el.ToString() + " ");

                    Console.WriteLine($" <- {page}");
                }
                else
                {
                    queue.Dequeue();
                    queue.Enqueue(page);
                    ++breakCounter;
                    foreach (var el in queue)
                        Console.Write(el.ToString() + " ");

                    Console.WriteLine($" <- {page}; First out, {page} in");
                }
            }
            return breakCounter;
        }
    }
}