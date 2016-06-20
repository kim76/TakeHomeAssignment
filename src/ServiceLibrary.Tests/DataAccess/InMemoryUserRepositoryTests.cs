using System;
using System.Collections.Generic;
using GildedRose.ServiceLibrary.Exceptions;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary.DataAccess
{
   [TestFixture]
   public class InMemoryUserRepositoryTests
   {
      [Test]
      public void GetUser_UsernameDoesnotExist_AuthenticationExceptionThrown()
      {
         var users = new InMemoryUserRepository(new Dictionary<string, string> { { "user1", "user1Password" }, { "user2", "user2Password" } });
         Assert.Throws<UnknownUserException>(() => users.GetUser("x"));
      }
      [Test]
      public void GetUser_UserIdDoesnotExist_AuthenticationExceptionThrown()
      {
         var users = new InMemoryUserRepository(new Dictionary<string, string> { { "user1", "user1Password" }, { "user2", "user2Password" } });
         Assert.Throws<UnknownUserException>(() => users.GetUser(Guid.NewGuid()));
      }

      [Test]
      public void GetUser_ValidUsername_UserReturned()
      {
         var username = "user1";
         var userList = new Dictionary<string, string> {{username, "user1Password" }, { "user2", "user2Password"}};
         var users = new InMemoryUserRepository(userList);
         var result = users.GetUser(username);
         Assert.IsNotNull(result, "User was not correctly retrieved.");
      }

      [Test]
      public void GetUser_ValidGuid_UserReturned()
      {
         var username = "user1";
         var userList = new Dictionary<string, string> { { username, "user1Password" }, { "user2", "user2Password" } };
         var users = new InMemoryUserRepository(userList);
         var result = users.GetUser(username);
         var result2 = users.GetUser(result.Id);
         Assert.IsNotNull(result2, "User was not correctly retrieved.");
      }
   }
}