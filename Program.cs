﻿using System;
using System.Linq;
using System.Reflection;
using RedBlueGame.Enums;
using RedBlueGame.Interfaces;

namespace RedBlueGame
{
    static class Program
    {
        public static void Main()
        {
            var test = @"I:\Projects\RedBluePlayer\bin\Debug\netcoreapp2.0\RedBluePlayer.dll";
            try
            {
                var game = new RedBlueGame(getPlayers(test, test), 2);
                play(game);
            }
            catch (System.Reflection.ReflectionTypeLoadException exception)
            {
                Console.WriteLine("Assembly Load Failure!");
                throw exception;
            }
        }

        private static void play(RedBlueGame game)
        {
            Console.WriteLine($"1 2");
            while (game.State != Game.Over)
            {
                game.Play();
                var action = game.GetLastAction();
                var playerOne = action[0] ? 'X' : 'O';
                var playerTwo = action[1] ? 'X' : 'O';
                Console.WriteLine($"{playerOne} {playerTwo}");
            }
            var score = game.Score();
            Console.WriteLine($"{score[0]} {score[1]}");
        }

        private static Player getPlayers(string playerOneFilePath, string playerTwoFilePath) =>
            new Player( 
                Activator.CreateInstance(Assembly.LoadFile(playerOneFilePath).DefinedTypes.First(
                    type => type.ImplementedInterfaces.Any(
                        interfaceType => interfaceType.FullName == typeof(IPlayer).FullName))) as IPlayer,
                Activator.CreateInstance(Assembly.LoadFile(playerTwoFilePath).DefinedTypes.First(
                    type => type.ImplementedInterfaces.Any(
                        interfaceType => interfaceType.FullName == typeof(IPlayer).FullName))) as IPlayer
            );
    }
}
