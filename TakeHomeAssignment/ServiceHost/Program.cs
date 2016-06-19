// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using GildedRose.ServiceLibrary;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceHost
{
   internal class Program
   {
      static string url = "http://localhost:8000/";

      private static void Main(string[] args)
      {
         WebServiceHost host = new WebServiceHost(typeof (GildedRoseService), new Uri(url));
         try
         {
            try
            {
               ServiceEndpoint ep = host.AddServiceEndpoint(typeof (IGildedRoseService), new WebHttpBinding(), "");
               host.Open();
            }
            catch (AddressAccessDeniedException)
            {
               Console.WriteLine(
                  "The application must be run as an administrator. Or use the cmd 'netsh http add urlacl url={0} user=everyone' to configure HTTP settings",
                  url.Replace("localhost", "+"));
               Console.ReadKey();
               return;
            }
            Item loadTestItem;
            using (var cf = new ChannelFactory<IGildedRoseService>(new WebHttpBinding(), url))
            {
               cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

               var channel = cf.CreateChannel();


               Console.WriteLine("Calling GetAllItems via HTTP GET: ");
               var s = channel.GetAllItems();
               Console.WriteLine("   Output: {0}", s.ToStringXml());

               Console.WriteLine("");
               Console.WriteLine("This can also be accomplished by navigating to");
               Console.WriteLine("http://localhost:8000/GetAllItems");
               Console.WriteLine("in a web browser while this sample is running.");

               Console.WriteLine("");

               Console.WriteLine("Calling GetAllItems via HTTP POST: ");
               s = channel.GetAllItems();
               loadTestItem = s.Items.First(i => i.Name == "Dirt");
               Console.WriteLine("   Output: {0}", s.ToStringXml());
               Console.WriteLine("");
            }
            LoadTest(loadTestItem.Guid);
            Console.WriteLine("Press <ENTER> to terminate");
            Console.ReadLine();

            host.Close();
         }
         catch (CommunicationException cex)
         {
            Console.WriteLine("An exception occurred: {0}", cex.Message);
            Console.ReadLine();
            host.Abort();
         }
      }

      /// <summary>
      /// Performs a quick load test.
      /// </summary>
      /// <param name="item">The item.</param>
      public static void LoadTest(Guid item)
      {
         Console.WriteLine("Running load test...");
         //Send our users on a shopping spree
         var userDictionary = new Dictionary<string, string>
         {
            {"Anna", "Anna"},
            {"Alex", "Alex"},
            {"Fong", "Fong"}
         };
         int totalPurchased = 0;
         Stopwatch sw = Stopwatch.StartNew();
         Parallel.ForEach(userDictionary, u =>
         {
            var amountPurchased = ShoppingSpree(u.Key, u.Value, item);
            Console.WriteLine("{0} bought {1:N0} items.", u.Key, amountPurchased);
            Interlocked.Add(ref totalPurchased, amountPurchased);
         });
         Console.WriteLine("Total purchased: {0} in {1} s", totalPurchased, sw.Elapsed.TotalSeconds);
      }

      /// <summary>
      ///    Send the user on a shopping spree to buy as much of the items as possible.  
      /// </summary>
      /// <param name="username">The username.</param>
      /// <param name="password">The password.</param>
      /// <param name="itemId">The item identifier.</param>
      /// <returns></returns>
      private static int ShoppingSpree(string username, string password, Guid itemId)
      {
         var totalPurchased = 0;
         using (var cf = new ChannelFactory<IGildedRoseService>(new WebHttpBinding(),url))
         {
            cf.Endpoint.Behaviors.Add(new WebHttpBehavior());

            var channel = cf.CreateChannel();
            var token = channel.RequestToken(username, password).Token;
            while (channel.BuyItem(token, itemId, 1).Status == ResponseStatus.Success)
            {
               totalPurchased++;
            }
         }
         return totalPurchased;
      }
   }

   static class DataContractExtentions
   {

      /// <summary>
      /// Converts the object to an XML string.
      /// </summary>
      /// <param name="obj">The object.</param>
      /// <returns></returns>
      public static string ToStringXml(this object obj)
      {
         var dcs = new DataContractSerializer(obj.GetType());
         var sb = new StringBuilder();

         using (var writer = XmlWriter.Create(sb, new XmlWriterSettings { Indent = true }))
         {
            dcs.WriteObject(writer, obj);
         }
         return (sb.ToString());
      }
   }
}