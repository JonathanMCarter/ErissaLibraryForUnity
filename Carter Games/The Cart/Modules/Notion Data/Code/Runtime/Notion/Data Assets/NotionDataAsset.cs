﻿#if CARTERGAMES_CART_MODULE_NOTIONDATA

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
using CarterGames.Cart.Core.Data;
using UnityEngine;

namespace CarterGames.Cart.Modules.NotionData
{   
    /// <summary>
    /// A base class for any Notion database data assets.
    /// </summary>
    /// <typeparam name="T">The type the data is storing.</typeparam>
    [Serializable]
    public abstract class NotionDataAsset<T> : DataAsset where T : new()
    {
	    [SerializeField] private List<T> data;

	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Properties
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

	    /// <summary>
        /// The data stored on the asset.
        /// </summary>
        public List<T> Data => data;


	    /// <summary>
        /// Defines the parser user to apply the data to the asset from Notion.
        /// </summary>
        protected virtual INotionDatabaseProcessor<T> DatabaseProcessor => new NotionDatabaseProcessorStandard<T>();

	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Methods
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

	    /// <summary>
        /// Applies the data found to the asset.
        /// </summary>
        /// <param name="result">The resulting data downloaded to try and apply.</param>
        private void Apply(NotionDatabaseQueryResult result)
        {
            data = DatabaseProcessor.Process(result);
            PostDataDownloaded();
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }


	    /// <summary>
        /// Override to run logic post download such as making edits to some data values or assigning others etc.
        /// </summary>
        protected virtual void PostDataDownloaded()
        { }
	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Fields
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

#pragma warning disable
	    [SerializeField, HideInInspector] private string linkToDatabase;     // Is used in editor space, so ignore the not used warning.
	    [SerializeField, HideInInspector] private string databaseApiKey;     // Is used in editor space, so ignore the not used warning.
	    [SerializeField] private List<NotionSortProperty> sortProperties;
#pragma warning restore
    }
}

#endif