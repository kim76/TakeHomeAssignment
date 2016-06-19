
// Copyright (C) 2016 Kim Bilida All rights reserved.

using System.Security.Authentication;
using GildedRose.ServiceLibrary.DataAccess;
using GildedRose.ServiceLibrary.Model;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;

namespace GildedRose.ServiceLibrary.Authentication
{
   [TestFixture]
   public class AuthenticatorTests
   {
      [Test]
      public void RequestToken_NullUser_ThrowsAuthenticationException()
      {
         var userRepository = Substitute.For<IUserRepository>();
         userRepository.GetUser("").ReturnsNull();
         var auth = new Authenticator(userRepository);
         Assert.Throws<AuthenticationException>(() => auth.RequestToken("xx", "yy"), "An exception should be thrown.");
      }

      [Test]
      public void RequestToken_ValidUser_TokenGiven()
      {
         var userRepository = Substitute.For<IUserRepository>();
         var username = "Bob";
         var password = "password";
         userRepository.GetUser("").ReturnsForAnyArgs(User.NewUser(username, password));
         var auth = new Authenticator(userRepository);
         var result = auth.RequestToken(username, password);
         Assert.IsNotNullOrEmpty(result, "A token was expected.");
      }

      [Test]
      public void RequestToken_WrongPassword_ThrowsAuthenticationException()
      {
         var userRepository = Substitute.For<IUserRepository>();
         var username = "Bob";
         userRepository.GetUser(username).Returns(User.NewUser(username, "password"));
         var auth = new Authenticator(userRepository);
         Assert.Throws<AuthenticationException>(() => auth.RequestToken(username, "WrongPassword"),
            "An exception should be thrown.");
      }
   }
}