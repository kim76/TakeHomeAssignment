
// Copyright (C) 2016 Kim Bilida All rights reserved.

using System;
using System.Security.Cryptography;
using System.Text;

namespace GildedRose.ServiceLibrary.Authentication
{
   /// <summary>
   /// An internal class to provide hashing functions
   /// </summary>
   class HashProvider
   {
      /// <summary>
      /// Creates a hash using the salt and password 
      /// </summary>
      /// <param name="salt">The unique identifier.</param>
      /// <param name="password">The password.</param>
      /// <returns></returns>
      internal static string Hash(string salt, string password)
      {
         //from http://stackoverflow.com/questions/3063116/how-to-easily-salt-a-password-in-a-c-sharp-windows-form-application
         //Note: I know very little about security.  I do not recommend this code. 
         byte[] pwdBytes = Encoding.UTF8.GetBytes(password);
         byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
         byte[] saltedPassword = new byte[pwdBytes.Length + saltBytes.Length];

         Buffer.BlockCopy(pwdBytes, 0, saltedPassword, 0, pwdBytes.Length);
         Buffer.BlockCopy(saltBytes, 0, saltedPassword, pwdBytes.Length, saltBytes.Length);

         SHA256 sha = SHA256.Create();

         byte[] hash = sha.ComputeHash(saltedPassword);
         return Encoding.UTF8.GetString(hash, 0, hash.Length); 
      }

      /// <summary>
      /// Determines whether the specified hash is valid for the given salt and password.
      /// </summary>
      /// <param name="hash">The hashed password of the user.</param>
      /// <param name="salt">The salt used when creating the hash.</param>
      /// <param name="password">The password to test.</param>
      /// <returns></returns>
      internal static bool IsMatch(string hash, string salt, string password)
      {
         return hash.Equals(Hash(salt, password));
      }
   }
}