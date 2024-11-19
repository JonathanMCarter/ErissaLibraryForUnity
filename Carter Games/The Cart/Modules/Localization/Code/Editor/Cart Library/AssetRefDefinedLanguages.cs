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
using CarterGames.Cart.Core.Management.Editor;
using CarterGames.Cart.Modules.Settings;
using UnityEditor;

namespace CarterGames.Cart.Modules.Localization.Editor
{
	public class AssetRefDefinedLanguages : IScriptableAssetDef<DataAssetDefinedLanguages>
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

		private static DataAssetDefinedLanguages cache;
		private static SerializedObject objCache;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   IScriptableAssetDef Implementation
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

		public Type AssetType => typeof(DataAssetDefinedLanguages);
		public string DataAssetFileName => "[Cart] [Localization] Defined Languages Data Asset.asset";
		public string DataAssetFilter => $"t:{typeof(DataAssetDefinedLanguages).FullName}";
		public string DataAssetPath => $"{ScriptableRef.FullPathData}/Modules/{DataAssetFileName}";


		public DataAssetDefinedLanguages AssetRef => ScriptableRef.GetOrCreateAsset(this, ref cache);
		public SerializedObject ObjectRef => ScriptableRef.GetOrCreateAssetObject(this, ref objCache);


		public void TryCreate()
		{
			ScriptableRef.GetOrCreateAsset(this, ref cache);
		}

		public void OnCreated() { }
	}
}

#endif