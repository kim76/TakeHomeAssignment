// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;

namespace GildedRose.ServiceLibrary.Exceptions
{
   /// <summary>
   /// Thrown when there is insufficient stock to complete the request.
   /// </summary>
   /// <seealso cref="System.Exception" />
   public class InsufficientStockException : Exception
   {
      /// <summary>
      /// The quantity that was requested.
      /// </summary>
      /// <value>
      /// The quantity requested.
      /// </value>
      public int QuantityRequested { get; private set; }

      /// <summary>
      /// Gets the number of items in stock.
      /// </summary>
      /// <value>
      /// The items in stock.
      /// </value>
      public int ItemsInStock { get; private set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="InsufficientStockException"/> class.
      /// </summary>
      /// <param name="quantityRequested">The quantity requested.</param>
      /// <param name="itemsInStock">The items in stock.</param>
      public InsufficientStockException(int quantityRequested, int itemsInStock)
      {
         QuantityRequested = quantityRequested;
         ItemsInStock = itemsInStock;
      }
   }
}