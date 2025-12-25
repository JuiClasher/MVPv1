using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MVP.Core.Entities;
using MVP.Infrastructure.Data;

namespace MVP.UI.TablaProductos
{
    /// <summary>
    /// Lógica de interacción para ProductosCRUD.xaml
    /// </summary>
    public partial class ProductosCRUD : Window
    {
        private readonly ApplicationDbContext _context;
        private Producto? _selectedProducto;

        public ProductosCRUD()
        {
            InitializeComponent();

            // Construir DbContext leyendo la conexión de appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var conn = configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULTCONNECTION")
                       ?? throw new InvalidOperationException("No connection string configured.");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(conn, npgsql => npgsql.EnableRetryOnFailure())
                .Options;

            _context = new ApplicationDbContext(options);

            LoadCombos();
            LoadProductos();
        }

        private void LoadCombos()
        {
            cbCategoria.ItemsSource = _context.Categorias.AsNoTracking().OrderBy(c => c.Nombre).ToList();
            cbMarca.ItemsSource = _context.Marcas.AsNoTracking().OrderBy(m => m.Nombre).ToList();
            cbUdm.ItemsSource = _context.UnidadesDeMedida.AsNoTracking().OrderBy(u => u.Nombre).ToList();
        }

        private void LoadProductos()
        {
            var productos = _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.UnidadDeMedida)
                .AsNoTracking()
                .OrderBy(p => p.Nombre)
                .ToList();

            dgProductos.ItemsSource = productos;
            ClearForm();
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedProducto = dgProductos.SelectedItem as Producto;
            if (_selectedProducto == null) return;

            txtNombre.Text = _selectedProducto.Nombre;
            txtSKU.Text = _selectedProducto.CodigoSKU;
            txtCodigoBarras.Text = _selectedProducto.CodigoBarras ?? string.Empty;
            txtPrecioVenta.Text = _selectedProducto.PrecioVenta.ToString("0.00");
            txtPrecioCosto.Text = _selectedProducto.PrecioCosto?.ToString("0.00") ?? string.Empty;
            txtPrecioMayorista.Text = _selectedProducto.PrecioMayorista?.ToString("0.00") ?? string.Empty;
            txtStockMinimo.Text = _selectedProducto.StockMinimo?.ToString() ?? string.Empty;
            chkActivo.IsChecked = _selectedProducto.Activo;
            cbCategoria.SelectedValue = _selectedProducto.CategoriaID;
            cbMarca.SelectedValue = _selectedProducto.MarcaID;
            cbUdm.SelectedValue = _selectedProducto.UnidadDeMedidaID;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e) => ClearForm();

        private void ClearForm()
        {
            _selectedProducto = null;
            txtNombre.Clear();
            txtSKU.Clear();
            txtCodigoBarras.Clear();
            txtPrecioVenta.Clear();
            txtPrecioCosto.Clear();
            txtPrecioMayorista.Clear();
            txtStockMinimo.Clear();
            chkActivo.IsChecked = true;
            cbCategoria.SelectedIndex = -1;
            cbMarca.SelectedIndex = -1;
            cbUdm.SelectedIndex = -1;
            dgProductos.SelectedIndex = -1;
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadProductos();

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateFormOrThrow();

                var nuevo = new Producto
                {
                    EmpresaID = GetEmpresaIdFallback(),
                    CategoriaID = (Guid)cbCategoria.SelectedValue!,
                    MarcaID = (Guid)cbMarca.SelectedValue!,
                    UnidadDeMedidaID = (Guid)cbUdm.SelectedValue!,
                    Nombre = txtNombre.Text.Trim(),
                    CodigoSKU = txtSKU.Text.Trim(),
                    CodigoBarras = string.IsNullOrWhiteSpace(txtCodigoBarras.Text) ? null : txtCodigoBarras.Text.Trim(),
                    PrecioCosto = ParseDecimal(txtPrecioCosto.Text),
                    PrecioVenta = decimal.Parse(txtPrecioVenta.Text),
                    PrecioMayorista = ParseDecimal(txtPrecioMayorista.Text),
                    StockMinimo = ParseIntNullable(txtStockMinimo.Text),
                    Activo = chkActivo.IsChecked ?? true
                };

                _context.Productos.Add(nuevo);
                _context.SaveChanges();
                LoadProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProducto == null)
            {
                MessageBox.Show("Seleccioná un producto antes de modificar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                ValidateFormOrThrow();

                var prod = _context.Productos.Find(_selectedProducto.ProductoID);
                if (prod == null) throw new InvalidOperationException("Producto no encontrado en la base.");

                prod.Nombre = txtNombre.Text.Trim();
                prod.CategoriaID = (Guid)cbCategoria.SelectedValue!;
                prod.MarcaID = (Guid)cbMarca.SelectedValue!;
                prod.UnidadDeMedidaID = (Guid)cbUdm.SelectedValue!;
                prod.CodigoSKU = txtSKU.Text.Trim();
                prod.CodigoBarras = string.IsNullOrWhiteSpace(txtCodigoBarras.Text) ? null : txtCodigoBarras.Text.Trim();
                prod.PrecioCosto = ParseDecimal(txtPrecioCosto.Text);
                prod.PrecioVenta = decimal.Parse(txtPrecioVenta.Text);
                prod.PrecioMayorista = ParseDecimal(txtPrecioMayorista.Text);
                prod.StockMinimo = ParseIntNullable(txtStockMinimo.Text);
                prod.Activo = chkActivo.IsChecked ?? true;
                prod.FechaModificacion = DateTimeOffset.Now.ToOffset(TimeSpan.FromHours(-3));

                _context.SaveChanges();
                LoadProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al modificar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedProducto == null)
            {
                MessageBox.Show("Seleccioná un producto antes de eliminar.", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var res = MessageBox.Show($"Confirma eliminar '{_selectedProducto.Nombre}'?", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res != MessageBoxResult.Yes) return;

            try
            {
                var prod = _context.Productos.Find(_selectedProducto.ProductoID);
                if (prod == null) throw new InvalidOperationException("Producto no encontrado en la base.");

                _context.Productos.Remove(prod);
                _context.SaveChanges();
                LoadProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static decimal? ParseDecimal(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return decimal.TryParse(s, out var d) ? d : throw new FormatException("Formato decimal inválido.");
        }

        private static int? ParseIntNullable(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            return int.TryParse(s, out var i) ? i : throw new FormatException("Formato entero inválido.");
        }

        private static Guid GetEmpresaIdFallback()
        {
            // Si no manejás multiempresa aún, fijar el primer Empresa disponible o Guid.Empty según tu modelo.
            // Ajustá según la lógica de tu app.
            return Guid.Empty;
        }

        private void ValidateFormOrThrow()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)) throw new InvalidOperationException("Nombre requerido.");
            if (string.IsNullOrWhiteSpace(txtSKU.Text)) throw new InvalidOperationException("Codigo SKU requerido.");
            if (!decimal.TryParse(txtPrecioVenta.Text, out _)) throw new InvalidOperationException("Precio Venta inválido.");
            if (cbCategoria.SelectedValue == null) throw new InvalidOperationException("Seleccioná una categoria.");
            if (cbMarca.SelectedValue == null) throw new InvalidOperationException("Seleccioná una marca.");
            if (cbUdm.SelectedValue == null) throw new InvalidOperationException("Seleccioná una unidad de medida.");
        }
    }
}
