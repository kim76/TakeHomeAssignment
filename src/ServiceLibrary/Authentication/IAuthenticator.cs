
// Copyright (C) 2016 Kim Bilida All rights reserved.

namespace GildedRose.ServiceLibrary.Authentication
{
   /// <summary>
   /// Provides Authentication functionality
   /// </summary>
   public interface IAuthenticator
   {
      /// <summary>
      /// Requests a token based on the username and password.
      /// </summary>
      /// <param name="username">The username.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      string RequestToken(string username, string password);

      /// <summary>
      /// Determines whether the token valid.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <returns></returns>
      bool IsTokenValid(string token);
   }
}