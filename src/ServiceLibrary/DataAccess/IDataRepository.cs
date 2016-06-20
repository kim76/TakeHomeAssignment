// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary.DataAccess
{
   /// <summary>
   /// Provides data storage functions
   /// </summary>
   public interface IDataRepository
   {
      /// <summary>
      /// Gets all items in the inventory.
      /// </summary>
      /// <returns></returns>
      IEnumerable<Item> GetAllItems();
      
      /// <summary>
      /// Gets the item based on its Guid.
      /// </summary>
      /// <param name="guid">The unique identifier.</param>
      /// <returns></returns>
      Item GetItem(Guid guid);
   }
}