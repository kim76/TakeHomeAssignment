// Copyright (C) 2016 Kim Bilida All rights reserved.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GildedRose.ServiceLibrary.Model
{
   /// <summary>
   ///    Result of an item request that was processed by the Gilded Rose Service
   /// </summary>
   /// <seealso cref="GildedRose.ServiceLibrary.Model.RequestResponse" />
   /// <seealso cref="RequestResponse" />
   [DataContract(Namespace = Constants.ServiceNamespace)]
   public class ItemsRequestResponse : RequestResponse
   {
      /// <summary>
      ///    Gets or sets the items returned by the request.
      /// </summary>
      /// <value>
      ///    The items.
      /// </value>
      [DataMember]
      public IEnumerable<Item> Items { get; set; }
   }
}