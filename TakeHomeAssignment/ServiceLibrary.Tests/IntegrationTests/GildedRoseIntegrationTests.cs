// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GildedRose.ServiceLibrary.Authentication;
using GildedRose.ServiceLibrary.DataAccess;
using GildedRose.ServiceLibrary.Model;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary.IntegrationTests
{
   /// <summary>
   ///    Tests some integration and multithreading of the service
   /// </summary>
   [TestFixture]
   public class GildedRoseIntegrationTests
   {
      private static GildedRoseService _client;

      /// <summary>
      ///    Send the user on a shopping spree to buy as much of the items as possible.  
      /// </summary>
      /// <param name="username">The username.</param>
      /// <param name="password">The password.</param>
      /// <param name="itemId">The item identifier.</param>
      /// <returns></returns>
      private static int ShoppingSpree(string username, string password, Guid itemId)
      {
         var token = _client.RequestToken(username, password).Token;
         var totalPurchased = 0;
         while (_client.BuyItem(token, itemId.ToString(), 1).Status == ResponseStatus.Success)
         {
            totalPurchased++;
         }
         return totalPurchased;
      }

      /// <summary>
      /// Sends a group of users to buy up all available stock on an item. Ensure that the total number purchased is equal to the number that was in stock.
      /// </summary>
      [Test]
      [System.ComponentModel.Category("Integration")]
      public void BuyAllMultithreaded()
      {
         //Set up some sample users
         var userDictionary = new Dictionary<string, string>
         {
            {"Anna", "xxx"},
            {"Pui Yee", "yyy"},
            {"Affandy", "ppp"}
         };
         var users = new InMemoryUserRepository(userDictionary);

         //Set up a popular item
         var item = new Item
         {
            Description = "great planting soil",
            Guid = Guid.NewGuid(),
            Name = "Dirt",
            Price = 1,
            StockCount = 10000000
         };
         var items = new List<Item> {item};

         //prepare the Service with our sample data
         _client = new GildedRoseService(new InMemoryDataRepository(items), new Authenticator(users));

         var originalStockCount = item.StockCount;
         var totalPurchased = 0;

         //Send our users on a shopping spree
         Parallel.ForEach(userDictionary, u =>
         {
            var amountPurchased = ShoppingSpree(u.Key, u.Value, item.Guid);
            Console.WriteLine("{0} bought {1:N0} items.", u.Key, amountPurchased);
            Interlocked.Add(ref totalPurchased, amountPurchased);
         });

         //Ensure the correct total number of items was purchased.
         Assert.AreEqual(originalStockCount, totalPurchased, "The total number of items purchased is incorrect.");
      }
   }
}