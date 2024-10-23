﻿#if CARTERGAMES_CART_MODULE_LOCALIZATION

/*
 * Copyright (c) 2024 Carter Games
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Cart.Modules.Localization
{
	/// <summary>
	/// Defines a copy entry that is localized into 1 or multiple languages.
	/// </summary>
	[Serializable]
	public sealed class LocalizationData
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		[SerializeField] private string id;
		[SerializeField] private LocalizationEntry[] entries;
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// The id for the data to use.
		/// </summary>
		public string LocId => id;
		
		
		/// <summary>
		/// The entries for the data to use.
		/// </summary>
		public LocalizationEntry[] Entries => entries;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Constructors
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

		// Limited to editor and only used if Notion Data module is active.
#if UNITY_EDITOR && CARTERGAMES_CART_MODULE_NOTIONDATA
		
		/// <summary>
		/// Blank constructor needed for generic new T() to compile mainly.
		/// </summary>
		public LocalizationData() {}
		
		
		/// <summary>
		/// Creates a new data with the entered values.
		/// </summary>
		/// <remarks>Used in Notion Data module.</remarks>
		/// <param name="id">The id for the data.</param>
		/// <param name="entries">The entries for this id.</param>
		public LocalizationData(string id, IEnumerable<LocalizationEntry> entries)
		{
			this.id = id;
			this.entries = entries.ToArray();
		}
#endif
	}
}

#endif