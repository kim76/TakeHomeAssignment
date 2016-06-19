// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.ServiceLibrary.Exceptions;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary.DataAccess
{
   /// <summary>
   ///    An in memory data repository as a sample.
   /// </summary>
   /// <seealso cref="GildedRose.ServiceLibrary.DataAccess.IDataRepository" />
   internal class InMemoryDataRepository : IDataRepository
   {
      private readonly HashSet<Item> _items;

      public InMemoryDataRepository()
      {
         //default items
         _items = new HashSet<Item>
         {
            new Item
            {
               Guid = Guid.NewGuid(),
               Name = "Vase",
               Description = "A ming dynasty vase",
               Price = 1000,
               StockCount = 3
            },
            new Item
            {
               Guid = Guid.NewGuid(),
               Name = "Butter",
               Description = "Organic butter",
               Price = 10,
               StockCount = 20
            },
            new Item
            {
               Guid = Guid.NewGuid(),
               Name = "Dirt",
               Description = "Plain old dirt",
               Price = 1,
               StockCount = 10000
            },
            new Item
            {
               Guid = Guid.NewGuid(),
               Name = "Sulfuras, Hand of Ragnaros",
               Description = "Sulfuras, Hand of Ragnaros",
               Price = 1000000,
               StockCount = 0
            }
         };
      }

      internal InMemoryDataRepository(IEnumerable<Item> items)
      {
         _items = new HashSet<Item>(items);
      }

      public IEnumerable<Item> GetAllItems()
      {
         return _items;
      }

      /// <summary>
      /// Gets the item based on its Guid.
      /// </summary>
      /// <param name="guid">The unique identifier.</param>
      /// <returns></returns>
      /// <exception cref="GildedRose.ServiceLibrary.Exceptions.UnknownItemException"></exception>
      public Item GetItem(Guid guid)
      {
         var item = _items.FirstOrDefault(i => i.Guid == guid);
         if (item == null) throw new UnknownItemException();
         return item;
      }
   }
}