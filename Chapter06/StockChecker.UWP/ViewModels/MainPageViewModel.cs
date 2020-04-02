﻿using StockChecker.UWP.Helpers;
using StockChecker.UWP.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockChecker.UWP.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _productId;
        private int _quantity;
        private int _originalQuantity;
        private bool _canViewQuantity;
        private bool _canUpdateQuantity;
        private string _userRole;


        public int ProductId
        {
            get => _productId;
            set
            {
                if (UpdateField(ref _productId, value))
                {
                    RefreshQuantity();
                }
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (UpdateField(ref _quantity, value))
                {
                    UpdateQuantity.RaiseCanExecuteChanged();
                }
            }
        }

        private bool UpdateField<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


        private readonly IHttpStockClientHelper _httpClientHelper;

        public RelayCommand UpdateQuantity { get; set; }

        public RelayCommand DecreaseQuantity { get; set; }

        public string UserRole
        {
            get => _userRole;
            set
            {
                if (UpdateField(ref _userRole, value))
                {
                    CanViewQuantity = UserRole == "Administrator" || UserRole == "Sales";
                    CanUpdateQuantity = UserRole == "Administrator" || UserRole == "Maintenance";
                }
            }
        }

        public MainPageViewModel(IHttpStockClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;

            UpdateQuantity = new RelayCommand(async () =>
            {
                await _httpClientHelper.UpdateQuantityAsync(                    
                    ProductId, Quantity);
                await RefreshQuantity();                
            }, 
            () => Quantity != _originalQuantity && CanUpdateQuantity);        
            
            DecreaseQuantity = new RelayCommand(async () =>
            {
                await _httpClientHelper.UpdateQuantityAsync(
                    ProductId, Quantity - 1);
                await RefreshQuantity();
            }, 
            () => Quantity > 0 && CanUpdateQuantity);
        }

        private async Task RefreshQuantity()
        {
            int? newQuantity = await _httpClientHelper.GetQuantityAsync(ProductId);
            if (!newQuantity.HasValue) return;
            Quantity = newQuantity.Value;
            _originalQuantity = Quantity;
            UpdateQuantity.RaiseCanExecuteChanged();
            DecreaseQuantity.RaiseCanExecuteChanged();
        }

        public bool CanUpdateQuantity
        {
            get => _canUpdateQuantity;
            set
            {
                UpdateField(ref _canUpdateQuantity, value);
            }            
        }

        public bool CanViewQuantity
        {
            get => _canViewQuantity;
            set
            {
                UpdateField(ref _canViewQuantity, value);
            }            
        }
    }
}
