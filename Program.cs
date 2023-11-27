using BlogsApp.Helpers;
using BlogsApp.Models;
using BlogsApp.Sevices;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace BlogsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            char option;
            while (true)
            {
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("0. Exit from app");
                option = Console.ReadKey(intercept: true).KeyChar;
                Console.WriteLine();
                switch (option)
                {
                    case '1':
                        Console.Write("Enter username: ");
                        string username = Console.ReadLine();
                        Console.Write("Enter name: ");
                        string name = Console.ReadLine();
                        Console.Write("Enter surname: ");
                        string surname = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string password = Console.ReadLine();

                        UserService.Register(new User
                        {
                            Username = username,
                            Name = name,
                            Surname = surname,
                            Password = password
                        });
                        break;
                    case '2':
                        Console.Write("Enter username: ");
                        string usernameToLog = Console.ReadLine();
                        Console.Write("Enter password: ");
                        string passwordToLog = Console.ReadLine();
                        if (UserService.Login(usernameToLog, passwordToLog))
                        {
                            Console.WriteLine("Login successful\n");
                        }
                        else
                        {
                            Console.WriteLine("Login unsuccessful, check your input\n");
                        }
                        break;
                    case '0':
                        Console.WriteLine("Exiting");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("No such option\n");
                        break;
                }
            }
        }
    }
}