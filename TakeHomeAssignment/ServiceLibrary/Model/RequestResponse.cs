// Copyright (C) 2016 Kim Bilida All rights reserved.

using System.Runtime.Serialization;

namespace GildedRose.ServiceLibrary.Model
{
   /// <summary>
   ///    Result of a request that was processed by the Gilded Rose Service
   /// </summary>
   [DataContract(Namespace = Constants.ServiceNamespace)]
   public class RequestResponse
   {
      /// <summary>
      ///    Gets or sets the status of the response.
      /// </summary>
      /// <value>
      ///    The status.
      /// </value>
      [DataMember]
      public ResponseStatus Status { get; set; }
      
      /// <summary>
      ///    Gets or sets the message providing additional information about the response.
      /// </summary>
      /// <value>
      ///    The message.
      /// </value>
      [DataMember]
      public string Message { get; set; }
   }
   
   public enum ResponseStatus
   {
      Undefined = 0,
      Success,
      Unauthorized,
      Failed,
   }
}