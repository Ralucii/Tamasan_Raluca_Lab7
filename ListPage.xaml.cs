using Tamasan_Raluca_Lab7.Models;

namespace Tamasan_Raluca_Lab7;

public partial class ListPage : ContentPage
{
    private ShopList slist;

    public ListPage()
    {
        InitializeComponent();
    }

    private void OnSaveButtonClicked(object sender, EventArgs e)
    {
        OnSaveButtonClicked(sender, e, slist);
    }

    async void OnSaveButtonClicked(object sender, EventArgs e, ShopList slist)
    {
        ShopList bindingContext = (ShopList)BindingContext;
        bindingContext.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(bindingContext);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }

    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)this.BindingContext)
        {
            BindingContext = new Product()
        });
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");

        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }

    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        Product p;
        if (listView.SelectedItem != null)
        {
            p = listView.SelectedItem as Product;
            await App.Database.DeleteProductAsync(p);
            await Navigation.PopAsync();
        }
    }
}