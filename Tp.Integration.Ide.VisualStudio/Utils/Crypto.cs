using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace Tp.Integration.Ide.VisualStudio.Utils {
	/// <summary>
	/// Encrypts/decrypts a string using symmetric key.
	/// </summary>
	internal sealed class Crypto {
		private readonly SymmetricAlgorithm _algorithm = new DESCryptoServiceProvider();

		private readonly Encoding _encoding = new UTF8Encoding();

		private readonly byte[] _keyBytes;

		private readonly byte[] _ivBytes;

		/// <summary>
		/// Creates new crypto helper using current machine and user name as the symmetric key.
		/// </summary>
		public Crypto()
			: this(Environment.MachineName + "/" + Environment.UserName) {}

		/// <summary>
		/// Creates new crypto helper using the specified symmetric key.
		/// </summary>
		/// <param name="key">Symmetric encryption key.</param>
		/// <exception cref="ArgumentNullException">If <see cref="key"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">If <see cref="key"/> is empty string.</exception>
		public Crypto(string key) {
			if (key == null) {
				throw new ArgumentNullException("key");
			}
			if (key == "") {
				throw new ArgumentException("Key cannot be empty", "key");
			}

			_algorithm.BlockSize = 8*8;
			_algorithm.Padding = PaddingMode.Zeros;

			var sha = new SHA1CryptoServiceProvider();
			byte[] hashBytes = sha.ComputeHash(_encoding.GetBytes(key));
			_keyBytes = new byte[8];
			for (int i = 0; i < 8; i++) {
				_keyBytes[i] = hashBytes[i];
			}
			_ivBytes = new byte[8];
			for (int i = 0; i < 8; i++) {
				_ivBytes[i] = hashBytes[i + 8];
			}
		}

		/// <summary>
		/// Encrypts the specified source string.
		/// </summary>
		/// <param name="source">The string to encrypt.</param>
		/// <returns>Encrypted string.</returns>
		/// <exception cref="ArgumentNullException">If <see cref="source"/> is <c>null</c>.</exception>
		public string Encrypt(string source) {
			if (source == null) {
				throw new ArgumentNullException("source");
			}
			if (source.Length > 50) {
				throw new ArgumentOutOfRangeException("source", "String too long");
			}
			if (source == "") {
				return "";
			}
			byte[] sourceBytes = _encoding.GetBytes(source);
			var memoryStream = new MemoryStream();
			using (var cryptoTransform = _algorithm.CreateEncryptor(_keyBytes, _ivBytes)) {
				using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write)) {
					cryptoStream.WriteByte((byte) sourceBytes.Length); // write data length
					cryptoStream.Write(sourceBytes, 0, sourceBytes.Length); // write data
				}
			}
			byte[] resultBytes = memoryStream.GetBuffer();
			return Convert.ToBase64String(resultBytes, 0, resultBytes.Length);
		}

		/// <summary>
		/// Decrypts the specified encrypted string.
		/// </summary>
		/// <param name="source">Encrypted string to decrypt.</param>
		/// <returns>Decrypted string.</returns>
		/// <exception cref="ArgumentNullException">If <see cref="source"/> is <c>null</c>.</exception>
		public string Decrypt(string source) {
			if (source == null) {
				throw new ArgumentNullException("source");
			}
			if (source == "") {
				return "";
			}
			byte[] sourceBytes = Convert.FromBase64String(source);
			var memoryStream = new MemoryStream();
			using (var cryptoTransform = _algorithm.CreateDecryptor(_keyBytes, _ivBytes)) {
				using (var cryptoStream = new CryptoStream(new MemoryStream(sourceBytes, 0, sourceBytes.Length), cryptoTransform, CryptoStreamMode.Read)) {
					int l = cryptoStream.ReadByte(); // read data length
					if (l != -1) {
						while (l > 0) {
							int b = cryptoStream.ReadByte(); // read data up to length
							if (b == -1) {
								break;
							}
							memoryStream.WriteByte((byte) b);
							l--;
						}
					}
				}
			}
			memoryStream.Position = 0;
			var streamReader = new StreamReader(memoryStream);
			return streamReader.ReadToEnd();
		}
	}
}