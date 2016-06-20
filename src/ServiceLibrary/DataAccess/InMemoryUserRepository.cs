
// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using GildedRose.ServiceLibrary.Exceptions;
using GildedRose.ServiceLibrary.Model;

namespace GildedRose.ServiceLibrary.DataAccess
{
   /// <summary>
   /// An in-memory store of Users
   /// </summary>
   /// <seealso cref="IUserRepository" />
   public class InMemoryUserRepository : IUserRepository
   {
      private readonly HashSet<User> _users;

      /// <summary>
      /// Initializes a new instance of the <see cref="InMemoryUserRepository"/> class with some default users
      /// </summary>
      public InMemoryUserRepository()
      {
         _users =new HashSet<User>
         {
            User.NewUser("Anna",  "Anna"),
            User.NewUser("Alex",  "Alex"),
            User.NewUser("Fong",  "Fong"),
            User.NewUser("Muhammed",  "Muhammed"),
            User.NewUser("Jachintha",  "Jachintha"),
            User.NewUser("Pui Yee",  "Pui Yee"),
         };
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="InMemoryUserRepository"/> class using the provided dictionary as the userlist.
      /// </summary>
      /// <param name="usernamePasswordDictionary">A dictionary containing a list of usernames as the key with the password as the value.</param>
      internal InMemoryUserRepository(Dictionary<string, string> usernamePasswordDictionary)
      {
         _users = new HashSet<User>();
         foreach (var kvp in usernamePasswordDictionary)
         {
            _users.Add(User.NewUser(kvp.Key, kvp.Value));
         }
      }

      /// <summary>
      /// Gets the user that matches the username.
      /// </summary>
      /// <param name="username">The username.</param>
      /// <returns></returns>
      /// <exception cref="UnknownUserException"></exception>
      public User GetUser(string username)
      {
         var user = _users.FirstOrDefault(u=> u.Username == username);
         if(user == null) throw new UnknownUserException();
         return user;
      }

      /// <summary>
      /// Gets the user based on the id.
      /// </summary>
      /// <param name="id">The identifier.</param>
      /// <returns></returns>
      public User GetUser(Guid id)
      {
         var user = _users.FirstOrDefault(u => u.Id == id);
         if (user == null) throw new UnknownUserException();
         return user;
      }
   }
}