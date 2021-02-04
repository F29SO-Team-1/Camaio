using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Login.Models;
using Login.Data;

namespace Login.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ThreadContext context)
        {
            context.Database.EnsureCreated();

            //if there is already data in the Database so its never empty
            if (context.Threads.Any()) return;

            var threads = new Thread[]
            {
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    Votes=50, Title="Run", Image="",
                    Description="Morning Run", CreateDate=DateTime.Parse("2021-12-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                     Votes=10, Title="Tall Trees",  Image="", 
                    Description="Famous Tall trees", CreateDate=DateTime.Parse("2018-02-25")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                     Votes=120, Title="Vacation",  Image="",
                    Description="Morning Sun in Hawaii", CreateDate=DateTime.Parse("2021-12-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                     Votes=5, Title="Lockdown",  Image="",
                    Description="Lockdown mood", CreateDate=DateTime.Parse("2021-01-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                     Votes=40, Title="Biking",  Image="",
                    Description="Arthurs Seat bike", CreateDate=DateTime.Parse("2020-07-24")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                     Votes=350, Title="StoneHedge With amazing Sunrise",  Image="",
                    Description="StoneHedge with a sunrise", CreateDate=DateTime.Parse("2015-03-12")}
            };
            foreach (Thread t in threads)
            {
                context.Threads.Add(t);
            }
            context.SaveChanges();
        }
    }
}
