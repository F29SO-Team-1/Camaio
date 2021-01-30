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
                    PostedIn=2, Location ="Scotland", Votes=50, Content="Sport", Title="Run",
                    Description="Morning Run", Date=DateTime.Parse("2021-12-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    PostedIn=3, Location ="England", Votes=10, Content="View", Title="Tall Trees",
                    Description="Famous Tall trees", Date=DateTime.Parse("2018-02-25")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    PostedIn=4, Location ="Hawaii", Votes=120, Content="Island", Title="Vacation",
                    Description="Morning Sun in Hawaii", Date=DateTime.Parse("2021-12-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    PostedIn=5, Location ="Scotland", Votes=5, Content="Home", Title="Lockdown",
                    Description="Lockdown mood", Date=DateTime.Parse("2021-01-29")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    PostedIn=2, Location ="Scotland", Votes=40, Content="Sport", Title="Biking",
                    Description="Arthurs Seat bike", Date=DateTime.Parse("2020-07-24")},
                new Thread{UserID="50b0be43-354c-4c4b-9ef7-6ebe41fb45d1",
                    PostedIn=3, Location ="England", Votes=350, Content="View", Title="StoneHedge With amazing Sunrise",
                    Description="StoneHedge with a sunrise", Date=DateTime.Parse("2015-03-12")}
            };
            foreach (Thread t in threads)
            {
                context.Threads.Add(t);
            }
            context.SaveChanges();
        }
    }
}
