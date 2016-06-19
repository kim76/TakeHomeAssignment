
// Copyright (C) 2016 Kim Bilida All rights reserved.
using System.Runtime.Serialization;

namespace GildedRose.ServiceLibrary.Model
{
   /// <summary>
   ///    Result of a request for a token that was processed by the Gilded Rose Service
   /// </summary>
   /// <seealso cref="GildedRose.ServiceLibrary.Model.RequestResponse" />
   [DataContract(Namespace = Constants.ServiceNamespace)]
   public class TokenRequestResponse : RequestResponse
   {
      /// <summary>
      /// Gets or sets the token.
      /// </summary>
      /// <value>
      /// The token.
      /// </value>
      [DataMember]
      public string Token { get; internal set; }
   }
}