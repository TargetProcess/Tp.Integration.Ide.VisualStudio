using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

namespace Tp.Integration.Ide.VisualStudio.Utils {
	[TestFixture]
	public class CryptoTest {
		private readonly Random _random = new Random();

		[Test]
		public void EmptyString() {
			Crypto c = new Crypto();
			string s = c.Encrypt("");
			Assert.AreEqual("", s);
			Assert.AreEqual("", c.Decrypt(s));
		}

		[Test]
		public void UseDefaultKey() {
			Crypto c = new Crypto();
			string s = c.Encrypt("hello world");
			Assert.AreNotEqual("hello world", s);
			Assert.AreEqual("hello world", c.Decrypt(s));
		}

		[Test]
		public void UseCustomKey() {
			const string S = "WqJCvfqa6JKiSXFm6t9MSAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";

			Crypto c = new Crypto("my secret key");
			string s = c.Encrypt("hello world");
			Assert.AreNotEqual("hello world", s);
			Assert.AreEqual("hello world", c.Decrypt(s));
			Assert.AreEqual("hello world", c.Decrypt(S));
		}

		[Test]
		public void FunkyPasswords() {
			Crypto c = new Crypto();
			{
				const string source = "antonida";
				string s = c.Encrypt(source);
				Assert.AreNotEqual(source, s);
				Assert.AreEqual(source, c.Decrypt(s));
			}
			{
				const string source = "привет мир";
				string s = c.Encrypt(source);
				Assert.AreNotEqual(source, s);
				Assert.AreEqual(source, c.Decrypt(s));
			}
			{
				const string source = @">rL`Fpbgr>_1j^?];cK5U>/!fm;&736puCLZeql=b-,-}rOdeR";
				string s = c.Encrypt(source);
				Assert.AreNotEqual(source, s);
				Assert.AreEqual(source, c.Decrypt(s));
			}
			{
				for (int i = 0; i < 1000; i++) {
					string source = RandomString(Math.Min(i + 1, 50));
					string s = c.Encrypt(source);
					Assert.AreNotEqual(source, s);
					Assert.AreEqual(source, c.Decrypt(s));
				}
			}
		}

		[Test]
		[ExpectedException(typeof (FormatException))]
		public void Base64FormatException() {
			Crypto c = new Crypto("my secret key");
			c.Decrypt("hello world");
		}

		[Test]
		[ExpectedException(typeof (CryptographicException))]
		public void AlgorithmCryptographicException() {
			Crypto c = new Crypto("my secret key");
			c.Decrypt(Convert.ToBase64String(Encoding.UTF8.GetBytes("hello world")));
		}

		/// <summary>
		/// Generates a random string with the given length.
		/// </summary>
		/// <param name="length">Length of the string.</param>
		/// <returns>Random string</returns>
		private string RandomString(int length) {
			var builder = new StringBuilder();
			for (int i = 0; i < length; i++) {
				char ch = Convert.ToChar(32 + Convert.ToInt32(Math.Floor((127 - 32) * _random.NextDouble())));
				builder.Append(ch);
			}
			return builder.ToString();
		}
	}
}