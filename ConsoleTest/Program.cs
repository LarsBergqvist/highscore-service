﻿using Core;
using Core.Repositories;
using Infrastructure.Repositories;
using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = new FakeHighScoreRepository();
        }
    }
}
