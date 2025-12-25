using System;
using System.Windows;

namespace MVP.UI.Login
{
    public partial class Login : Window
    {
        // Credenciales de prueba (cámbialas para producción)
        private const string TEST_USER = "admin";
        private const string TEST_PASS = "1234";

        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = txtUsername.Text?.Trim() ?? string.Empty;
            var pass = pwdPassword.Password ?? string.Empty;

            if (user == TEST_USER && pass == TEST_PASS)
            {
                // Navegar a ProductosCRUD (se asume que es una Window en el namespace indicado)
                var productosWindow = new MVP.UI.TablaProductos.ProductosCRUD();
                productosWindow.Show();

                // Cerrar ventana de login
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.", "Acceso denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
