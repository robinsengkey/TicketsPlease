using System;
using System.Collections.Generic;
using System.IO;

namespace TicketSales_DX
{
    class Program
    {
        private List<string> tempList = new List<string>();
        string adminPassword = "";

        static void Main()
        {
            Program p = new Program();
            p.Intro();
        }

        private void Intro()
        {
            Console.WriteLine("Input \"Help\" for list of helpful help commands to help you use this program");
            Run();
        }

        private void Run()
        {
            while (true)
            {
                Admin();
                Console.WriteLine("\n<Enter command>\n");
                string input = Console.ReadLine().ToLower();
                if (input == "help") { Help(); }
                else if (input == "addshow") { AddShows(); } //Admin command
                else if (input == "removeshow") { RemoveShows(); } //Admin command
                else if (input == "buy") { Buy(); }
                else if (input == "get" || input == "list") { Get(); } //Admin command
                else if (input == "showshows") { ShowShows(); }
                else if (input == "refund") { Refund(); }
                else if (input == "clear") { Clear(); }
                else if (input == "quit") { Quit(); }

                else { Console.WriteLine("Enter a valid command please \n\nWrite \"Help\" if you need help remembering commands\n"); }

            }
        }

        private void Admin()
        {
            using (StreamWriter w = File.AppendText(@"..\AdminPassword.txt")) { }
            using (StreamReader reader = new StreamReader(@"..\AdminPassword.txt"))
            {
                tempList.Clear();
                foreach (string show in File.ReadLines(@"..\AdminPassword.txt"))
                {
                    tempList.Add(show);
                }

            }
            if (tempList.Count == 0)
            {
                Console.WriteLine("Since this is the first time this program is being run on this computer, please add an Admin password, take care as this password can only be changed by deleting the file and can be anything you want");
                adminPassword = Console.ReadLine();
                tempList.Add(adminPassword);
                using (StreamWriter sw = new StreamWriter(@"..\AdminPassword.txt"))
                {
                    sw.WriteLine(tempList[0]);
                }
            }
            {
                adminPassword = tempList[0];
            }
        }

        private void Help()
        {
            Console.WriteLine("\n <Help Menu>");
            Console.WriteLine("Help (For help, there is a slight possibility you are already aware of this one)");
            Console.WriteLine("Buy(make a new order)");
            Console.WriteLine("ShowShows(To see all shows)");
            Console.WriteLine("Refund(Refund an order)");
            Console.WriteLine("Clear (To clear all text. Does not clear data)");
            
            Console.WriteLine("Admin commands");
            Console.WriteLine("List(See all orders for, amongst other things, bokföring)");
            Console.WriteLine("AddShow(Add new shows");
            Console.WriteLine("RemoveShow(Remove old shows)");
            Console.WriteLine("Quit (To end the program)\n");
        }

        private void Buy()
        {
            int showCount = 0;
            using (StreamReader reader = new StreamReader(@"..\Shows.txt"))
            {
                tempList.Clear();
                foreach (string show in File.ReadLines(@"..\Shows.txt"))
                {
                    tempList.Add(show);
                    showCount++;
                    Console.WriteLine("\n" + showCount + "." + show);

                }
            }
            if (showCount == 0)
            {
                Console.WriteLine("There are no shows");
                return;
            }

            Console.WriteLine("\nEnter the number of the show you would like to attend");
            int showNumber = Parse(Console.ReadLine());
            if (showNumber < 1 || showNumber > showCount)
            {
                Console.WriteLine("<Please select a valid show");
                return;
            }

            string selectedShowName = tempList[showNumber - 1];
            Console.WriteLine("\nSo you have chosen death... I mean \nYou have selected: " + selectedShowName);

            Console.WriteLine("\nEnter your first name");
            string firstName = FirstUpperCase(Console.ReadLine());
            if (firstName.Contains(" "))
            {
                Console.WriteLine("<Name may not contain spaces>");
                return;
            }

            Console.WriteLine("\nEnter your last name");
            string lastName = FirstUpperCase(Console.ReadLine());
            if (lastName.Contains(" "))
            {
                Console.WriteLine("<Name may not contain spaces>");
                return;
            }

            string fullName = firstName + " " + lastName;

            if (fullName.Length == 0)
            {
                Console.WriteLine("Name entered cannot be empty");
                return;
            }

            if (fullName.Length > 40)
            {
                Console.WriteLine("Name can't exceed 40 characters");
                return;
            }

            if (fullName.Contains("Refunded") || fullName.Contains("Total Price:"))
            {
                Console.WriteLine("That won't fly here fool");
                return;
            }

            foreach (char c in fullName)
            {
                if (int.TryParse(c.ToString(), out _))
                {
                    Console.WriteLine("<Name may not contain numbers>");
                    return;
                }

            }

            string rndString = RandomNumberGen();

            Console.WriteLine("How many Adult tickets do you want to purchase? (Age: 18+)");
            int adultTickets = NegativeToPositiveNumber(Parse(Console.ReadLine()));
            Console.WriteLine("How many Elder tickets do you want to purchase? (Age: 65+)");
            int elderTickets = NegativeToPositiveNumber(Parse(Console.ReadLine()));
            Console.WriteLine("How many Child tickets do you want to purchase? (Age: Under 18)");
            int childTickets = NegativeToPositiveNumber(Parse(Console.ReadLine()));

            int totalCost = CalculatePrice(adultTickets, elderTickets, childTickets);
            if (totalCost == 0)
            {
                Console.WriteLine("You have to buy a minimum of 1 ticket to proceed");
                return;
            }
            Console.WriteLine("Total cost: " + totalCost + "\n" + "Your order number: " + rndString);


            using (StreamWriter sw = new StreamWriter(@"..\Purchases.txt", true))
            {
                sw.WriteLine("Name:" + fullName + " Show number:" + showNumber +
                              "  Order number:" + rndString + "  Show name:" + selectedShowName +
                              "  Adults:" + adultTickets + "  Elders:" + elderTickets + "  Children:" + childTickets + " Total Price:" + totalCost);
            }

        }
        private void AddShows()
        {
            Console.WriteLine("Enter Admin password to edit shows");
            string input = Console.ReadLine();
            if (input == adminPassword)
            {
                using (StreamWriter w = File.AppendText(@"..\Shows.txt")) { }
                using (StreamReader reader = new StreamReader(@"..\Shows.txt"))
                {
                    tempList.Clear();
                    foreach (string show in File.ReadLines(@"..\Shows.txt"))
                    {
                        tempList.Add(show);
                    }
                }
                Console.WriteLine("Enter the name of the show you would like to add to the list");
                string newShow = Console.ReadLine();
                tempList.Add(newShow);

                using (StreamWriter writer = new StreamWriter(@"..\Shows.txt"))
                {
                    writer.Write("");
                }
                using (StreamWriter writer = new StreamWriter(@"..\Shows.txt", true))
                {
                    foreach (string show in tempList)
                    {
                        writer.WriteLine(show);
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect");
            }
        }
        private void RemoveShows()
        {
            Console.WriteLine("Enter Admin password to edit shows");
            string input = Console.ReadLine();
            int counter = 0;
            if (input == adminPassword)
            {
                using (StreamWriter w = File.AppendText(@"..\Shows.txt")) { }
                using (StreamReader reader = new StreamReader(@"..\Shows.txt"))
                {
                    tempList.Clear();
                    foreach (string show in File.ReadLines(@"..\Shows.txt"))
                    {
                        counter++;
                        tempList.Add(show);
                        Console.WriteLine("Nr " + counter + " -" + show);
                    }
                    if (counter == 0)
                    {
                        Console.WriteLine("There are no shows to remove");
                        return;
                    }
                }
                Console.WriteLine("Select which show number to remove");
                int showToRemove = Parse(Console.ReadLine()) - 1;
                if (counter - 1 < showToRemove || showToRemove < 0)
                {
                    Console.WriteLine("Number can not be bigger than the list nor smaller than 0");
                    return;
                }

                tempList.RemoveAt(showToRemove);
                using (StreamWriter writer = new StreamWriter(@"..\Shows.txt"))
                {
                    writer.Write("");
                }
                using (StreamWriter writer = new StreamWriter(@"..\Shows.txt", true))
                {
                    foreach (string show in tempList)
                    {
                        writer.WriteLine(show);
                    }
                }
            }
            else
            {
                Console.WriteLine("Incorrect");
            }

        }
        private string RandomNumberGen()
        {
            bool Duplicate = true;
            string rndString = "";
            using (StreamReader reader = new StreamReader(@"..\Shows.txt"))
            {
                tempList.Clear();
                foreach (string show in File.ReadLines(@"..\Shows.txt"))
                {
                    tempList.Add(show);
                }

            }
            while (Duplicate) // Random generator for ordernumbers that checks for duplicate numbers
            {
                using (StreamWriter w = File.AppendText(@"..\Purchases.txt")) { }
                int counter = 0;
                using (StreamReader reader = new StreamReader(@"..\Purchases.txt"))
                {
                    tempList.Clear();
                    foreach (string order in File.ReadLines(@"..\Purchases.txt"))
                    {
                        tempList.Add(order);
                        counter++;

                    }
                }
                bool check = true;
                while (rndString.Length != 9)
                {
                    Random rnd = new Random();
                    rndString += rnd.Next(0, 10).ToString();
                }

                for (int i = counter - 1; i >= 0; i--)
                {
                    string list = tempList[i];
                    if (list.Contains(rndString))
                    {
                        check = false;
                    }
                }
                if (check)
                {
                    Duplicate = false;
                }
                else
                {
                    Duplicate = true;
                }

            }
            return rndString;
        }
        private int Parse(string enteredString)
        {
            try
            {
                int enteredNumber = int.Parse(enteredString);
                return enteredNumber;
            }
            catch (FormatException)
            {
                Console.WriteLine("Entering anything but a number will be read as 0");
                return 0;
            }
        }
        private void Get()
        {
            Console.WriteLine("Enter admin password to see all orders");
            string input = Console.ReadLine();
            if (input == adminPassword)
            {
                Console.WriteLine("\n<Orders>");
                using (StreamWriter w = File.AppendText(@"..\Purchases.txt")) { }
                using (StreamReader reader = new StreamReader(@"..\Purchases.txt"))
                {
                    int count = 0;
                    tempList.Clear();
                    foreach (string line in File.ReadLines(@"..\Purchases.txt"))
                    {
                        Console.WriteLine(line);
                        count++;
                    }
                    if (count == 0)
                    {
                        Console.WriteLine("\nThere are no orders");
                        return;
                    }

                }
            }
        }
        private int NegativeToPositiveNumber(int number)
        {
            if (number >= 0) { return number; }
            else { return number * -1; }
        }
        private string FirstUpperCase(string wholeString)
        {
            string newStringName = wholeString.Substring(0, 1).ToUpper() + wholeString.Substring(1).ToLower();
            return newStringName;
        }
        private void ShowShows()
        {
            int count = 0;
            using (StreamWriter w = File.AppendText(@"..\Shows.txt")) { }
            using (StreamReader reader = new StreamReader(@"..\Shows.txt"))
            {
                tempList.Clear();
                foreach (string show in File.ReadLines(@"..\Shows.txt"))
                {
                    count++;
                    Console.WriteLine("Nr. " + count + " " + show);
                }
                if (count == 0)
                {
                    Console.WriteLine("\nThere are no shows right now");
                }
            }
        }

        private void Refund()
        {
            Console.WriteLine("Enter your ordernumber to refund it");
            string input = Console.ReadLine();
            if (input.Length != 9)
            {
                Console.WriteLine("Ordernumbers are 9 numbers long, please retry with a valid number");
                return;
            }

            int inputInt = Parse(input); //It checks if it is parseable but since the next part doesnt use an int I just use the first input instead, if its negative that is not a problem since it wont remove anything
            using (StreamReader reader = new StreamReader(@"..\Purchases.txt"))
            {
                bool check = false;
                tempList.Clear();
                string orderString = "";
                foreach (string order in File.ReadLines(@"..\Purchases.txt"))
                {
                    if (order.Contains(input) && !order.Contains("Refunded"))
                    {
                        int indexForCost = order.IndexOf("Total Price:");
                        string totalCost = order.Substring(indexForCost + 12);
                        orderString = "Refunded---" + order + "---Refunded";
                        Console.WriteLine("Your order has been found and will be refunded for " + totalCost + " sek");
                        check = true;
                    }
                    else
                    {
                        orderString = order;
                    }

                    tempList.Add(orderString);
                }
                if (!check)
                {
                    Console.WriteLine("Your order was not found");
                    return;
                }


            }
            using (StreamWriter writer = new StreamWriter(@"..\Purchases.txt"))
            {
                writer.Write("");
            }
            using (StreamWriter writer = new StreamWriter(@"..\Purchases.txt", true))
            {
                foreach (string order in tempList)
                {
                    writer.WriteLine(order);
                }
            }
        }

        public int CalculatePrice(int adult, int elder, int child)
        {
            int fullPrice = adult * 100 + elder * 75 + child * 25;
            return fullPrice;
        }

        private void Clear()
        {
            Console.Clear();
        }

        private void Quit()
        {
            Console.WriteLine("Enter Admin password to force quit the program");
            string input = Console.ReadLine();
            if (input == adminPassword)
            {
                Environment.Exit(0);
            }
        }

    }

}
