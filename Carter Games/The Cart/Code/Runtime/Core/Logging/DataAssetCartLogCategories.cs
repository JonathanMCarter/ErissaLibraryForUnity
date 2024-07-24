﻿/*
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

using CarterGames.Cart.Core.Data;
using UnityEngine;

namespace CarterGames.Cart.Core.Logs
{
    /// <summary>
    /// A data asset to store the states of all the log categories.
    /// </summary>
    [CreateAssetMenu(menuName = "Carter Games/The Cart/Log Categories (Data Asset)")]
    public sealed class DataAssetCartLogCategories : DataAsset
    {
        [SerializeField] private SerializableDictionary<string, bool> lookup;


        public bool IsEnabled(CartLogCategory category)
        {
            if (!lookup.TryGetValue(category.GetType().FullName, out var enabled)) return false;
            return enabled;
        }
        
        
        public bool IsEnabled<T>() where T : CartLogCategory
        {
            if (!lookup.ContainsKey(typeof(T).FullName)) return false;
            return lookup[typeof(T).FullName];
        }
    }
}