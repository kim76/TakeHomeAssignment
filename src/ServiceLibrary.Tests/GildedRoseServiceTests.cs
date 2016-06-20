// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using GildedRose.ServiceLibrary.Authentication;
using GildedRose.ServiceLibrary.DataAccess;
using GildedRose.ServiceLibrary.Model;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary
{
   [TestFixture]
   public class GildedRoseServiceTests
   {
      [Test]
      public void BuyItem_AllItemsPurchased_StockSetToZero()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var stockCount = 5;
         var item = new Item {StockCount = stockCount};
         var guid = Guid.NewGuid();
         dataRepository.GetItem(guid).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var response = service.BuyItem("valid", guid.ToString(), stockCount);
         Assert.AreEqual(0, item.StockCount, "The stock count of the item must decrease when purchased. " + response.Message);
      }

      [Test]
      public void BuyItem_InsufficientStock_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var stockCount = 5;
         var item = new Item {StockCount = stockCount};
         dataRepository.GetItem(new Guid()).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var result = service.BuyItem("valid", "", stockCount + 1);
         Assert.AreEqual(ResponseStatus.Failed, result.Status,
            "Status must be set to failed when there is insufficient stock.");
      }

      [Test]
      public void BuyItem_ItemDoesnotExist_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         dataRepository.GetItem(new Guid()).ReturnsForAnyArgs((Item)null);
         var service = new GildedRoseService(dataRepository, authenticator);
         var result = service.BuyItem("valid", "", 1);
         Assert.AreEqual(ResponseStatus.Failed, result.Status,
            "Status must be set to failed when the item doesn't exist.");
      }

      [Test]
      public void BuyItem_InvalidItemId_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(false);
         var service = new GildedRoseService(Substitute.For<IDataRepository>(), authenticator);
         var result = service.BuyItem("invalid", "", 1);
         Assert.AreEqual(ResponseStatus.Failed, result.Status,
            "Status must be set to failed when the item does not exist.");
      }

      [Test]
      public void BuyItem_InvalidToken_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(false);
         var service = new GildedRoseService(Substitute.For<IDataRepository>(), authenticator);
         var result = service.BuyItem("invalid token", Guid.NewGuid().ToString(), 1);
         Assert.AreEqual(ResponseStatus.Unauthorized, result.Status, "Status must be set to Unauthorized when the token is invalid.");
      }

      [Test]
      public void BuyItem_TokenThrowsException_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ThrowsForAnyArgs<Exception>();
         var service = new GildedRoseService(Substitute.For<IDataRepository>(), authenticator);
         var result = service.BuyItem("invalid token", Guid.NewGuid().ToString(), 1);
         Assert.AreEqual(ResponseStatus.Unauthorized, result.Status, "Status must be set to Unauthorized when the token is invalid.");
      }

      [Test]
      public void BuyItem_OneItemPurchased_StockDecreasedByOne()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var stockCount = 5;
         var item = new Item {StockCount = stockCount};
         var guid = Guid.NewGuid();
         dataRepository.GetItem(guid).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var quantityPurchased = 1;
         var response = service.BuyItem("valid", guid.ToString(), quantityPurchased);
         Assert.AreEqual(stockCount - quantityPurchased, item.StockCount,
            "The stock count of the item must decrease when purchased. " + response.Message);
      }

      [Test]
      public void BuyItem_ValidRequest_RequestSucceeds()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var item = new Item {StockCount = 5};
         var guid = Guid.NewGuid();
         dataRepository.GetItem(guid).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var result = service.BuyItem("valid", guid.ToString(), 1);
         Assert.AreEqual(ResponseStatus.Success, result.Status,
            "Purchase should have succeeded. " + result.Message);
      }

      [Test]
      public void BuyItem_ZeroInStock_OutOfStockMessage()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var item = new Item {StockCount = 0, Guid = Guid.NewGuid()};
         dataRepository.GetItem(new Guid()).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var quantity = 1;
         var result = service.BuyItem("valid", item.Guid.ToString(), quantity);
         Assert.AreEqual(string.Format("There is insufficient stock to purchase {0} items.", quantity), result.Message,
            "Must receive an out of stock message");
      }

      [Test]
      public void BuyItem_ZeroInStock_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.IsTokenValid("").ReturnsForAnyArgs(true);
         var dataRepository = Substitute.For<IDataRepository>();
         var item = new Item {StockCount = 0};
         dataRepository.GetItem(new Guid()).ReturnsForAnyArgs(item);
         var service = new GildedRoseService(dataRepository, authenticator);
         var result = service.BuyItem("valid", "", 1);
         Assert.AreEqual(ResponseStatus.Failed, result.Status, "Status must be set to failed when the token is invalid.");
      }

      [Test]
      public void GetInventory_EmptyList_RequestSucceeds()
      {
         var repo = Substitute.For<IDataRepository>();
         repo.GetAllItems().Returns(new List<Item>());
         var service = new GildedRoseService(repo, Substitute.For<IAuthenticator>());
         var result = service.GetAllItems();
         Assert.AreEqual(ResponseStatus.Success, result.Status,
            "Status must be set to successful when the operation succeeds.");
      }

      [Test]
      public void GetInventory_EmptyList_ReturnToken()
      {
         var repo = Substitute.For<IDataRepository>();
         repo.GetAllItems().Returns(new List<Item>());
         var service = new GildedRoseService(repo, Substitute.For<IAuthenticator>());
         var result = service.GetAllItems();
         Assert.AreEqual(ResponseStatus.Success, result.Status,
            "Status must be set to successful when the operation succeeds.");
      }

      [Test]
      public void GetInventory_ExceptionThrown_ResultMessageProvided()
      {
         var repo = Substitute.For<IDataRepository>();
         repo.GetAllItems().Throws(new FileNotFoundException());
         var service = new GildedRoseService(repo, Substitute.For<IAuthenticator>());
         var result = service.GetAllItems();
         Assert.IsNotNullOrEmpty(result.Message, "A message must be provided in the response when the operation fails.");
      }

      [Test]
      public void GetInventory_ExceptionThrown_ResultStatusFailed()
      {
         var repo = Substitute.For<IDataRepository>();
         repo.GetAllItems().Throws(new ArgumentException());
         var service = new GildedRoseService(repo, Substitute.For<IAuthenticator>());
         var result = service.GetAllItems();
         Assert.AreEqual(ResponseStatus.Failed, result.Status,
            "The result status should be failed when the repository throws an exception.");
      }

      [Test]
      public void RequestToken_InvalidUsername_RequestFails()
      {
         var authenticator = Substitute.For<IAuthenticator>();
         authenticator.RequestToken("", "").ThrowsForAnyArgs<AuthenticationException>();
         var service = new GildedRoseService(Substitute.For<IDataRepository>(), authenticator);
         var result = service.RequestToken("invalid", "invalid");
         Assert.AreEqual(ResponseStatus.Failed, result.Status,
            "Status must be set to failed when the user does not exist.");
      }
   }
}