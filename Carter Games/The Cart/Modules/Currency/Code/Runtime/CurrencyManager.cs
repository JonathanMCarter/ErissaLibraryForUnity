﻿#if CARTERGAMES_CART_MODULE_CURRENCY

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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Cart.Core;
using CarterGames.Cart.Core.Data;
using CarterGames.Cart.Core.Events;
using CarterGames.Cart.Core.Save;
using CarterGames.Cart.ThirdParty;
using UnityEngine;

namespace CarterGames.Cart.Modules.Currency
{
    public static class CurrencyManager
    {
        private static readonly Dictionary<string, CurrencyAccount> Accounts = new Dictionary<string, CurrencyAccount>();
        private const string AccountsSaveKey = "CartSave_Modules_Currency_Accounts";
        
        public static bool HasLoadedAccounts { get; private set; }

        public static List<string> AllAccountIds
        {
            get
            {
                var keys = new List<string>();
                var data = CartSaveHandler.Get<string>(AccountsSaveKey);
            
                if (!string.IsNullOrEmpty(data))
                {
                    var jsonData = JSONNode.Parse(data);

                    if (jsonData.AsArray.Count > 0)
                    {
                        foreach (var account in jsonData)
                        {
                            keys.Add(account.Value["key"]);
                        }
                    }
                }
                
                foreach (var defAccount in DataAccess.GetAsset<DataAssetDefaultAccounts>().DefaultAccounts)
                {
                    if (keys.Contains(defAccount.Key)) continue;
                    keys.Add(defAccount.Key);
                }

                return keys;
            }
        }
        

        public static readonly Evt AccountsLoaded = new Evt();
        public static readonly Evt AccountOpened = new Evt();
        public static readonly Evt<CurrencyAccount> AccountBalanceChanged = new Evt<CurrencyAccount>();
        public static readonly Evt AccountClosed = new Evt();


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeAccounts()
        {
            Application.focusChanged -= OnFocusChange;
            Application.focusChanged += OnFocusChange;
            
            LoadAccounts();

            void OnFocusChange(bool isFocused)
            {
                if (isFocused) return;
                SaveAccounts();
            }
        }
        

        public static void AddAccount(string accountId, double startingBalance = 0d)
        {
            if (Accounts.ContainsKey(accountId)) return;
            Accounts.Add(accountId, new CurrencyAccount(startingBalance));

            Accounts[accountId].Adjusted.Add(OnAccountAdjusted);
            AccountOpened.Raise();
            return;
            
            
            void OnAccountAdjusted()
            {
                AccountBalanceChanged.Raise(Accounts[accountId]);
            }
        }

        
        public static bool TryGetBalance(string accountId, out double balance)
        {
            balance = GetBalance(accountId);
            return balance > -1;
        }

        public static double GetBalance(string accountId)
        {
            if (!Accounts.TryGetValue(accountId, out var account)) return -1;
            return account.Balance;
        }
        
        
        public static bool TryGetAccount(string accountId, out CurrencyAccount account)
        {
            account = GetAccount(accountId);
            return account != null;
        }
        
        
        public static CurrencyAccount GetAccount(string accountId)
        {
            if (!Accounts.TryGetValue(accountId, out var account)) return null;
            return account;
        }

        
        public static void CloseAccount(string accountId)
        {
            if (!Accounts.ContainsKey(accountId)) return;
            Accounts.Remove(accountId);
            AccountClosed.Raise();
        }

        private static void LoadAccounts()
        {
            var data = CartSaveHandler.Get<string>(AccountsSaveKey);
            Accounts.Clear();
            
            if (!string.IsNullOrEmpty(data))
            {
                var jsonData = JSONNode.Parse(data);

                if (jsonData.AsArray.Count > 0)
                {
                    foreach (var account in jsonData)
                    {
                        Accounts.Add(account.Value["key"], new CurrencyAccount(account.Value["value"].AsDouble.Round()));
                    }
                }
            }

            foreach (var defAccount in DataAccess.GetAsset<DataAssetDefaultAccounts>().DefaultAccounts)
            {
                if (Accounts.ContainsKey(defAccount.Key)) continue;
                Accounts.Add(defAccount.Key, new CurrencyAccount(defAccount.Value.Round()));
            }
            
            HasLoadedAccounts = true;
            AccountsLoaded.Raise();
        }
        
        
        public static void SaveAccounts()
        {
            var list = new JSONArray();
            
            foreach (var account in Accounts)
            {
                if (string.IsNullOrEmpty(account.Key)) continue;
                
                if (list.HasKey(account.Key))
                {
                    list[account.Key]["value"] = account.Value.Balance.Round();
                }
                else
                {
                    var obj = new JSONObject
                    {
                        ["key"] = account.Key,
                        ["value"] = account.Value.Balance.Round()
                    };

                    list.Add(account.Key, obj);
                }
            }
            
            CartSaveHandler.Set(AccountsSaveKey, list.ToString());
        }
    }
}

#endif