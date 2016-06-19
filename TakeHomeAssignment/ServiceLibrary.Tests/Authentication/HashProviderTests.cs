
// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary.Authentication
{
   [TestFixture]
   public class HashProviderTests
   {
      [Test]
      public void HashPassword_SamePassword_DifferentHash()
      {
         var guid = Guid.NewGuid();
         var password = "BadPass0rd";
         string hash = HashProvider.Hash(guid.ToString(), password);
         var guid2 = Guid.NewGuid();
         string hash2 = HashProvider.Hash(guid2.ToString(), password);
         Assert.AreNotEqual(hash,hash2,"The same password must have a different hash given a different salt.");
      }

      [Test]
      public void VerifyPassword_ValidPassword_SucessfulMatch()
      {
         var guid = Guid.NewGuid();
         var password = "BadPass0rd";
         string hash = HashProvider.Hash(guid.ToString(), password); 
         bool matched = HashProvider.IsMatch(hash, guid.ToString(), password);
         Assert.IsTrue(matched, "The same password must have a different hash given a different salt.");
      }
   }
}
