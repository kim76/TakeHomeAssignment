// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Runtime.Serialization;
using GildedRose.ServiceLibrary.Exceptions;

namespace GildedRose.ServiceLibrary.Model
{
   [DataContract(Namespace = Constants.ServiceNamespace)]
   public class Item
   {
      private readonly object _stockLock = new object();

      /// <summary>
      ///    Gets or sets the description of the item.
      /// </summary>
      /// <value>
      ///    The description.
      /// </value>
      [DataMember]
      public string Description { get; set; }

      /// <summary>
      ///    Gets or sets the name of the item.
      /// </summary>
      /// <value>
      ///    The name.
      /// </value>
      [DataMember]
      public string Name { get; set; }

      /// <summary>
      ///    Gets or sets the price of the item.
      /// </summary>
      /// <value>
      ///    The price.
      /// </value>
      [DataMember]
      public int Price { get; set; }

      /// <summary>
      ///    Gets or sets the number of items in stock.
      /// </summary>
      /// <value>
      ///    The stock count.
      /// </value>
      [DataMember]
      public int StockCount { get; set; }

      /// <summary>
      /// Gets or sets the unique identifier of the user.
      /// </summary>
      /// <value>
      /// The unique identifier.
      /// </value>
      [DataMember]
      public Guid Guid { get; set; }

      /// <summary>
      ///    Reduces the stock count by the given quantity.
      /// </summary>
      /// <param name="quantity">The quantity.</param>
      public void ReduceStock(int quantity)
      {
         lock (_stockLock)
         {
            if (StockCount < quantity)
            {
               throw new InsufficientStockException(quantity, StockCount);
            }
            StockCount -= quantity;
         }
      }
   }
}