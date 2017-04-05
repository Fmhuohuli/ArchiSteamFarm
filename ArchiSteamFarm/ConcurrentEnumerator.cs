﻿/*
    _                _      _  ____   _                           _____
   / \    _ __  ___ | |__  (_)/ ___| | |_  ___   __ _  _ __ ___  |  ___|__ _  _ __  _ __ ___
  / _ \  | '__|/ __|| '_ \ | |\___ \ | __|/ _ \ / _` || '_ ` _ \ | |_  / _` || '__|| '_ ` _ \
 / ___ \ | |  | (__ | | | || | ___) || |_|  __/| (_| || | | | | ||  _|| (_| || |   | | | | | |
/_/   \_\|_|   \___||_| |_||_||____/  \__|\___| \__,_||_| |_| |_||_|   \__,_||_|   |_| |_| |_|

 Copyright 2015-2017 Łukasz "JustArchi" Domeradzki
 Contact: JustArchi@JustArchi.net

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0
					
 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

*/

using System;
using System.Collections;
using System.Collections.Generic;
using Nito.AsyncEx;

namespace ArchiSteamFarm {
	internal sealed class ConcurrentEnumerator<T> : IEnumerator<T> {
		public T Current => Enumerator.Current;

		private readonly IEnumerator<T> Enumerator;
		private readonly IDisposable Lock;

		object IEnumerator.Current => Current;

		internal ConcurrentEnumerator(ICollection<T> collection, AsyncReaderWriterLock rwLock) {
			if ((collection == null) || (rwLock == null)) {
				throw new ArgumentNullException(nameof(collection) + " || " + nameof(rwLock));
			}

			Lock = rwLock.ReaderLock();
			Enumerator = collection.GetEnumerator();
		}

		public void Dispose() {
			Lock.Dispose();
		}

		public bool MoveNext() {
			return Enumerator.MoveNext();
		}

		public void Reset() {
			Enumerator.Reset();
		}
	}
}