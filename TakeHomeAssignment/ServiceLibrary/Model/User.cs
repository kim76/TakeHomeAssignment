// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using GildedRose.ServiceLibrary.Authentication;

namespace GildedRose.ServiceLibrary.Model
{
   /// <summary>
   /// Represents a user of the system
   /// </summary>
   public class User
   {
      /// <summary>
      ///    Prevents a default instance of the <see cref="User" /> class from being created.  Use <see cref="NewUser" />
      /// </summary>
      private User()
      {
      }

      /// <summary>
      /// Gets the user identifier.
      /// </summary>
      /// <value>
      /// The identifier.
      /// </value>
      public Guid Id { get; private set; }

      /// <summary>
      /// Gets or sets the username.
      /// </summary>
      /// <value>
      /// The username.
      /// </value>
      public string Username { get; set; }
     
      /// <summary>
      /// Gets the hashed password.
      /// </summary>
      /// <value>
      /// The password hash.
      /// </value>
      public string PasswordHash { get; private set; }

      /// <summary>
      ///    Creates a new user, provides a unique id and sets the hashed password.
      /// </summary>
      /// <param name="username">The username.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      public static User NewUser(string username, string password)
      {
         var id = Guid.NewGuid();
         return new User {Id = id, Username = username, PasswordHash = HashProvider.Hash(id.ToString(), password)};
      }
   }
}