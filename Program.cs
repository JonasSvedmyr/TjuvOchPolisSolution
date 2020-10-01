using System;
using System.Collections.Generic;
using System.Threading;

namespace TjuvOchPolis
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Thief> prison = new List<Thief>();
            List<Person> people = GeneratePopulation(); //will generate all cops, citizens and thiefs
            List<string> events = new List<string>();
            char[,] GameBoard = new char[25, 100];
            bool running = true;
            GameBoard.Clear(25, 100); //Solves a bug
            while (running)
            {
                GameBoard = PopulateGameboard(people, events, GameBoard, prison);
                WriteGameboard(GameBoard);
                Console.WriteLine();
                if (events.Count != 0)
                {
                    Console.WriteLine();
                    foreach (var e in events)
                    {
                        Console.WriteLine(e);
                    }
                    if (prison.Count != 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Amount of locked up thiefs: {prison.Count}");
                        foreach (var thief in prison)
                        {
                            Console.WriteLine($"Theif has served: {30 - thief.LokeckedUp} seconds");
                        }
                        
                    }
                    Console.WriteLine();
                    Console.WriteLine($"Amount of robberys so far: {Counters.AmountOfRobberys}");
                    Console.WriteLine($"Amount of arrests so far: {Counters.AmountOfArrests}");
                    //Thread.Sleep(4000);
                    Console.Write("Press any key to continue");
                    Console.ReadKey(true);
                }
                foreach (Person person1 in people)
                {
                    person1.Move();
                }
                events.Clear();
                GameBoard.Clear(25, 100); //Extension Method
                Console.Clear();
            }
        }

        private static char[,] PopulateGameboard(List<Person> people, List<string> events, char[,] gameBoard, List<Thief> prison) 
        {
            foreach (Person person in people)
            {
                char c = person switch
                {
                    Cop cop => 'P',
                    Thief thief => 'T',
                    Citizen citizen => 'M',
                    _ => 'N'
                };
                if(person is Thief thief1 && thief1.LokeckedUp > 0)
                {
                    thief1.LokeckedUp--;
                    if(thief1.LokeckedUp == 0)
                    {
                        prison.Remove(thief1);
                        events.Add("Theif is out of prison");
                    }
                }
                else if (char.IsWhiteSpace(gameBoard[person.PosY, person.PosX])) //to see if the position is empty
                {
                    gameBoard[person.PosY, person.PosX] = c;
                }
                else //If there is already someone in that position
                {
                    gameBoard[person.PosY, person.PosX] = 'X';

                    // this is not gonna work fully if 3 or more people are on the same position
                    foreach (var person2 in people) //Trying to find which person is alreday in that position
                    {
                        if(person2 is Thief thief2 && thief2.LokeckedUp > 0)//To make sure it is not a thief already in prison
                        {

                        }
                        else if (person.PosX == person2.PosX && person.PosY == person2.PosY)
                        {
                            if (person is Cop && person  is Citizen || person2 is Cop && person is Citizen)
                            {
                                //events.Add("Cop meets a citizen");
                                break;

                            }
                            else if (person is Cop && person2 is Thief)
                            {
                                CopConfiscatesItems((Thief)person2, (Cop)person, events, prison);
                                break;
                            }
                            else if (person2 is Cop && person is Thief)
                            {
                                CopConfiscatesItems((Thief)person, (Cop)person2, events, prison);
                                break;
                            }
                            else if (person is Citizen && person2 is Thief)
                            {
                                ThiefRobbsCitizen((Thief)person2, (Citizen)person, events);
                                Counters.AmountOfRobberys++;
                                break;
                            }
                            else if (person2 is Citizen && person is Thief)
                            {
                                ThiefRobbsCitizen((Thief)person, (Citizen)person2, events);
                                Counters.AmountOfRobberys++;
                                break;
                            }
                        }
                    }
                }

            }
            return gameBoard;
        }
        public static void CopConfiscatesItems(Thief thief, Cop cop, List<string> events, List<Thief> prison)
        {
            if (thief.StolenGoods.Money == 0 && thief.StolenGoods.Keys == 0 && thief.StolenGoods.MobliePhones == 0 && thief.StolenGoods.Clocks == 0)
            {
                events.Add($"Cop tried arrest the thief and to confiscate stolen goods but the thief has not stolen anything");
            }
            else
            {
                cop.ConfiscatedItems.Keys += thief.StolenGoods.Keys;
                thief.StolenGoods.Keys = 0;
                cop.ConfiscatedItems.MobliePhones += thief.StolenGoods.MobliePhones;
                thief.StolenGoods.MobliePhones = 0;
                cop.ConfiscatedItems.Money += thief.StolenGoods.Money;
                thief.StolenGoods.Money = 0;
                cop.ConfiscatedItems.Clocks += thief.StolenGoods.Clocks;
                thief.StolenGoods.Clocks = 0;
                events.Add($"The cop arrested a thief and confiscated all stolen goods from a thief. Cop now has confiscated {cop.ConfiscatedItems.Clocks} clocks, {cop.ConfiscatedItems.Keys} keys, {cop.ConfiscatedItems.MobliePhones} mobilephones and {cop.ConfiscatedItems.Money} money");
                thief.LokeckedUp = 30;
                Counters.AmountOfArrests++;
                prison.Add(thief);
            }
        }
        public static void ThiefRobbsCitizen(Thief thief, Citizen citizen, List<string> events)
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 4);
            while (true)
            {
                if (citizen.HasClock == false && citizen.HasKeys == false && citizen.HasMoblephone == false && citizen.HasMoney == false)
                {
                    events.Add("Thief was unable to robb citizens becouse the citizen has nothing left");
                    break;
                }
                else if (randomNumber == 0 && citizen.HasMoblephone)
                {
                    citizen.HasMoblephone = false;
                    thief.StolenGoods.MobliePhones++;
                    events.Add($"Thief robbed a citizen and took the citizens mobilephone. Thief has now stolen {thief.StolenGoods.Clocks} clocks, {thief.StolenGoods.Keys} keys, {thief.StolenGoods.MobliePhones} mobilephones and {thief.StolenGoods.Money} money");
                    break;
                }
                else if (randomNumber == 1 && citizen.HasClock)
                {
                    citizen.HasClock = false;
                    thief.StolenGoods.Clocks++;
                    events.Add($"Thief robbed a citizen and took the citizens clock. Thief has now stolen {thief.StolenGoods.Clocks} clocks, {thief.StolenGoods.Keys} keys, {thief.StolenGoods.MobliePhones} mobilephones and {thief.StolenGoods.Money} money");
                    break;
                }
                else if (randomNumber == 2 && citizen.HasKeys)
                {
                    citizen.HasKeys = false;
                    thief.StolenGoods.Keys++;
                    events.Add($"Thief robbed a citizen and took the citizens keys. Thief has now stolen {thief.StolenGoods.Clocks} clocks, {thief.StolenGoods.Keys} keys, {thief.StolenGoods.MobliePhones} mobilephones and {thief.StolenGoods.Money} money");
                    break;
                }
                else if (randomNumber == 3 && citizen.HasMoney)
                {
                    citizen.HasMoney = false;
                    thief.StolenGoods.Money++;
                    events.Add($"Thief robbed a citizen and took some money. Thief has now stolen {thief.StolenGoods.Clocks} clocks, {thief.StolenGoods.Keys} keys, {thief.StolenGoods.MobliePhones} mobilephones and {thief.StolenGoods.Money} money");
                    break;
                }
                randomNumber = random.Next(0, 4);
            }
        }
        private static List<Person> GeneratePopulation()
        {
            List<Person> people = new List<Person>();
            for (int i = 0; i < 30; i++)
            {
                people.Add(new Cop());
            }
            for (int i = 0; i < 30; i++)
            {
                people.Add(new Citizen());
            }
            for (int i = 0; i < 10; i++)
            {
                people.Add(new Thief());
            }

            return people;
        }

        private static void WriteGameboard(char[,] GameBoard)
        {
            for (int y = 0; y < 25; y++)
            {
                Console.WriteLine();
                for (int x = 0; x < 100; x++)
                {
                        Console.Write(GameBoard[y, x]);
                }
            }
        }
    }
}
