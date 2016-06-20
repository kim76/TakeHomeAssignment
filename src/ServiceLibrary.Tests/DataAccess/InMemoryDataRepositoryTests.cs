using System;
using GildedRose.ServiceLibrary.Exceptions;
using GildedRose.ServiceLibrary.Model;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary.DataAccess
{
   [TestFixture]
   public class InMemoryDataRepositoryTests
   {
      [Test]
      public void GetItem_unknownId_ThrowsUnknownItemException()
      {
         var item = new Item {Guid = Guid.NewGuid()};
         var repo = new InMemoryDataRepository(new[] {item});
         Assert.Throws<UnknownItemException>(() => repo.GetItem(Guid.NewGuid()),
            "Excpeption must be thrown when an unknown item is requested.");
      }
      [Test]
      public void GetItem_validId_ReturnsItem()
      {
         var item = new Item { Guid = Guid.NewGuid() };
         var repo = new InMemoryDataRepository(new[] { item });
         var returnedItem = repo.GetItem(item.Guid);
         Assert.IsNotNull(returnedItem);
      }

      [Test]
      public void GetItems_defaultCtor_ReturnsItems()
      {
         var item = new Item { Guid = Guid.NewGuid() };
         var repo = new InMemoryDataRepository();
         var items = repo.GetAllItems();
         Assert.IsNotEmpty(items);
      }
   }
}
