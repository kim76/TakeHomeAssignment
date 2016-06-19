using System;

namespace GildedRose.ServiceLibrary.Exceptions
{
   /// <summary>
   /// Indicates the item is not recognized in the system.
   /// </summary>
   /// <seealso cref="System.Exception" />
   public class UnknownItemException: Exception

   {
      public string ItemName { get; private set; }

      /// <summary>
      /// Initializes a new instance of the <see cref="UnknownItemException"/> class.
      /// </summary>
      /// <param name="itemName">Name of the item.</param>
      public UnknownItemException(string itemName)
      {
         ItemName = itemName;
      }

      public UnknownItemException()
      {
      }
   }
}
