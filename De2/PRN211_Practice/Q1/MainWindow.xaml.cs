﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Q1.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Q1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LuyenOnThiDBContext context;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            context = new LuyenOnThiDBContext();
            //loadProduct();
            loadCategory();

        }
            public void loadProduct()
            {
                //lvProduct.ItemsSource = context.Products.Include(x => x.Category).ToList();

                List<Product> list = context.Products.Include(x => x.Category).ToList();
                lvProduct.ItemsSource = list;
            }

        public void loadCategory()
        {
            cbCategory.ItemsSource = context.Categories.ToList();
            cbCategory.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Product product = new Product();
            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("Name can not empty");
                return;
            }
            product.ProductName = tbName.Text;
            product.CategoryId = (cbCategory.SelectedItem as Category).CategoryId;
            product.QuantityPerUnit = tbQuantity.Text;
            product.Discontinued = rbTrue.IsChecked == true;

            context.Products.Add(product);
            context.SaveChanges();
            loadProduct();
            MessageBox.Show("Add success");
        }
        private void lvProduct_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Product item = (sender as ListView).SelectedItem as Product;
            if (item != null)
            {
                tbId.Text = item.ProductId.ToString();
                tbName.Text = item.ProductName;
                tbQuantity.Text = item.QuantityPerUnit;
                cbCategory.SelectedItem = item.Category;
                rbTrue.IsChecked = true;
                rbFalse.IsChecked = item.Discontinued == false;
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("ID is invalid");
                return;
            }
            try
            {
                Product product = context.Products.FirstOrDefault(x => x.ProductId == int.Parse(tbId.Text));
                if (product != null)
                {
                    product.ProductName = tbName.Text;
                    product.QuantityPerUnit = tbQuantity.Text;
                    product.CategoryId = (cbCategory.SelectedItem as Category).CategoryId;
                    product.QuantityPerUnit = tbQuantity.Text;
                    product.Discontinued = rbTrue.IsChecked == true;
                    context.Products.Update(product);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Update successfully");
                        loadProduct();
                    }
                    else
                    {
                        MessageBox.Show("Cannot find product");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid");
            }



        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbName.Text))
            {
                MessageBox.Show("ID is invalid");
                return;
            }
            try
            {
                Product product = context.Products.Include(x => x.OrderDetails).FirstOrDefault(x => x.ProductId == int.Parse(tbId.Text));
                if (product != null)
                {
                    if (product.OrderDetails.Count > 0)
                    {
                        context.OrderDetails.RemoveRange(product.OrderDetails);
                    }

                    context.Products.Remove(product);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Delete successfully");
                        loadProduct();
                    }
                }
                else
                {
                    MessageBox.Show("Cannot find product/ product Empty");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid");
            }
        }

        //Save file json
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            // Tạo một hộp thoại lưu tệp
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Đặt định dạng mở rộng mặc định thành .json
            saveFileDialog.DefaultExt = ".json";

            // Đặt bộ lọc để chỉ hiển thị các tệp JSON hoặc tất cả các tệp
            saveFileDialog.Filter = "JSON FILES (*.json)|*.json|All Files (*.*)|*.*";

            // Hiển thị hộp thoại lưu tệp và kiểm tra xem người dùng đã chọn một tệp để lưu hay chưa
            if (saveFileDialog.ShowDialog() == true)
            {
                // Tùy chọn cho quá trình chuyển đổi JSON
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true, // Định dạng đẹp để dễ đọc
                };

                // Lấy danh sách sản phẩm từ nguồn dữ liệu (ở đây giả sử context là một đối tượng DbContext)
                List<Product> productsList = context.Products.ToList();

                // Loại bỏ tham chiếu đệ quy của các đối tượng liên quan
                productsList.ForEach(x =>
                {
                    x.Category = null; // Đặt tham chiếu đến Category về null để tránh vòng lặp vô hạn
                });

                // Chuyển danh sách sản phẩm thành chuỗi JSON
                string jsonContent = JsonSerializer.Serialize(productsList, jsonOptions);

                // Ghi nội dung JSON vào tệp đã chọn
                File.WriteAllText(saveFileDialog.FileName, jsonContent);

                // Hiển thị thông báo khi quá trình lưu thành công
                MessageBox.Show("Save Json successfully");
            }
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".json"; // 
            openFileDialog.Filter = "JSON FILES (*.json)|*.json|All Files (*.*)|*.*";

            if(openFileDialog.ShowDialog() == true)
            {
                string jsonContent = File.ReadAllText(openFileDialog.FileName);
                List<Product> products= JsonSerializer.Deserialize<List<Product>>(jsonContent);
                lvProduct.ItemsSource = products;
            }
        }

        private void btnLoadProduct_Click(object sender, RoutedEventArgs e)
        {
            loadProduct();
        }
    }
}
