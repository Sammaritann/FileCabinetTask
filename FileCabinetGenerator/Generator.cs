using FileCabinetApp;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetGenerator
{
    internal class Generator
    {
        public static IEnumerable<FileCabinetRecord> Generate(int starId, int count)
        {
            Random random = new Random();

            for (int i = starId; i < starId + count; i++)
            {
                FileCabinetRecord record = new FileCabinetRecord
                {
                    FirstName = StringGenerate(random),
                    LastName = StringGenerate(random),
                    Department = (short)random.Next(0, 32766),
                    Salary = (decimal)(random.NextDouble() * 1000),
                    Class = CharGenerate(random),
                    DateOfBirth = DateTimeGenerate(random),
                    Id = i
                };
                yield return record;
            }
        }

        private static string StringGenerate(Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(chars.Select(c => chars[random.Next(chars.Length)]).Take(random.Next(2, 60)).ToArray());
        }

        private static char CharGenerate(Random random)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return chars[random.Next(chars.Length)];
        }

        private static DateTime DateTimeGenerate(Random random)
        {
            DateTime start = new DateTime(1950, 1, 1);
            long diff = DateTime.Now.Ticks - start.Ticks;
            DateTime bufferData = start.AddTicks((long)(random.NextDouble() * diff));

            return new DateTime(bufferData.Year,bufferData.Month,bufferData.Day);
        }
    }
}