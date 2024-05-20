using System;

// Clase del cliente
public class Cliente
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    public string Genero { get; set; }
    public string MetodoPago { get; set; }
}

// Clase del producto
public class Producto
{
    public string Codigo { get; set; }
    public string Descripcion { get; set; }
    public int InventarioInicial { get; set; }
    public double PrecioUnitario { get; set; }
}

// Clase compra
public class Compra
{
    public DateTime Fecha { get; set; }
    public Producto[] Productos { get; set; }
    public double Total { get; set; }
}

// Clase farmacia
public class Farmacia
{
    private Producto[] inventario = new Producto[6]; // Arreglo para el inventario

    private int indexInventario = 0; // Indice para el inventario

    // Metodo agregar producto al inventario
    public void AgregarProducto(Producto producto)
    {
        if (indexInventario < inventario.Length)
        {
            inventario[indexInventario] = producto;
            indexInventario++;
        }
    }

    // Metodo inicializar inventario Stock Inicial
    public void InicializarInventario()
    {
        AgregarProducto(new Producto() { Codigo = "000", Descripcion = "Aspirina", InventarioInicial = 50, PrecioUnitario = 2 });
        AgregarProducto(new Producto() { Codigo = "001", Descripcion = "Diclofenaco", InventarioInicial = 45, PrecioUnitario = 5 });
        AgregarProducto(new Producto() { Codigo = "002", Descripcion = "Pasiflora", InventarioInicial = 12, PrecioUnitario = 3 });
        AgregarProducto(new Producto() { Codigo = "003", Descripcion = "Paracetamol", InventarioInicial = 20, PrecioUnitario = 7 });
        AgregarProducto(new Producto() { Codigo = "004", Descripcion = "Sucrol", InventarioInicial = 10, PrecioUnitario = 4 });
        AgregarProducto(new Producto() { Codigo = "005", Descripcion = "Jarabe para la tos", InventarioInicial = 8, PrecioUnitario = 12 });
    }

    // Metodo realizar compra
    public Compra RealizarCompra(Producto[] productosSeleccionados)
    {
        // Verificar Stock
        foreach (var productoSeleccionado in productosSeleccionados)
        {
            Producto productoExistente = null;
            for (int i = 0; i < indexInventario; i++)
            {
                if (inventario[i].Codigo == productoSeleccionado.Codigo)
                {
                    productoExistente = inventario[i];
                    break;
                }
            }

            if (productoExistente == null || productoSeleccionado.InventarioInicial > productoExistente.InventarioInicial)
            {
                Console.WriteLine($"Producto {productoSeleccionado.Descripcion} no disponible en la cantidad solicitada.");
                return null;
            }
        }

        // Calcular total de la compra
        double total = 0;
        foreach (var productoSeleccionado in productosSeleccionados)
        {
            total += productoSeleccionado.PrecioUnitario * productoSeleccionado.InventarioInicial;
        }

        // Actualizar inventario
        foreach (var productoSeleccionado in productosSeleccionados)
        {
            for (int i = 0; i < indexInventario; i++)
            {
                if (inventario[i].Codigo == productoSeleccionado.Codigo)
                {
                    inventario[i].InventarioInicial -= productoSeleccionado.InventarioInicial;
                    break;
                }
            }
        }

        // Crear registro de compra
        Compra compra = new Compra
        {
            Fecha = DateTime.Now,
            Productos = productosSeleccionados,
            Total = total
        };

        return compra;
    }

    // Metodo mostrar Stock Actualizado
    public void MostrarInventario()
    {
        Console.WriteLine("=== Inventario de Productos ===");
        Console.WriteLine("| Código | Descripción          | Precio | Disponibilidad |");
        for (int i = 0; i < indexInventario; i++)
        {
            Console.WriteLine($"| {inventario[i].Codigo}      | {inventario[i].Descripcion,-20} | {inventario[i].PrecioUnitario,5} | {inventario[i].InventarioInicial,14} |");
        }
    }

    // Metodo mostrar el historial de compras y calcular el gasto total
    public void MostrarHistorialCompras(Compra[] compras)
    {
        bool hayCompras = false;
        double gastoTotal = 0;

        if (compras != null && compras.Length > 0)
        {
            Console.WriteLine("=== Historial de Compras ===");
            foreach (var compra in compras)
            {
                if (compra != null)
                {
                    hayCompras = true;
                    Console.WriteLine($"Fecha: {compra.Fecha}, Total: {compra.Total}");
                    Console.WriteLine("Productos:");
                    foreach (var producto in compra.Productos)
                    {
                        Console.WriteLine($"- {producto.Descripcion}: {producto.InventarioInicial} unidades");
                    }
                    Console.WriteLine();
                    gastoTotal += compra.Total;
                }
            }
        }

        if (!hayCompras)
        {
            Console.WriteLine("No hay compras registradas.");
        }

        Console.WriteLine($"Gasto total en compras: {gastoTotal}");
    }



    // Metodo obtener producto por código
    public Producto ObtenerProducto(string codigo)
    {
        for (int i = 0; i < indexInventario; i++)
        {
            if (inventario[i].Codigo == codigo)
            {
                return inventario[i];
            }
        }
        return null;
    }
}

class Program
{
    static Farmacia farmacia = new Farmacia(); // Instancia de farmacia

    static Cliente cliente = new Cliente(); // Instancia del cliente

    static Compra[] historialCompras = new Compra[10]; // Arreglo para el historial de compras
    static int indexHistorialCompras = 0; // Indice para el historial de compras

    static void Main(string[] args)
    {
        farmacia.InicializarInventario(); // Inicializar el inventario con Stock Inicial

        // Registro de cliente
        Console.WriteLine("=== Registro de Cliente ===");
        Console.Write("Nombre: ");
        cliente.Nombre = LeerStringSinNumeros();
        Console.Write("Apellido: ");
        cliente.Apellido = LeerStringSinNumeros();
        Console.Write("Teléfono: ");
        cliente.Telefono = LeerTelefono();
        Console.Write("Dirección: ");
        cliente.Direccion = Console.ReadLine();
        Console.Write("Género (H/M): ");
        cliente.Genero = LeerGenero();
        Console.Write("Método de Pago (A - Efectivo / B - Tarjeta): ");
        cliente.MetodoPago = LeerMetodoPago();
        Console.WriteLine("¡Cliente registrado exitosamente!");

        // Menú principal
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Farmacias Galeno X URL ===");
            Console.WriteLine("=== Menu Principal ===");
            Console.WriteLine("1. Consulta de Productos");
            Console.WriteLine("2. Compra de Medicamentos");
            Console.WriteLine("3. Informe de Compras");
            Console.WriteLine("4. Salida");
            Console.Write("Seleccione una opcion: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    farmacia.MostrarInventario();
                    break;
                case "2":
                    RealizarCompra();
                    break;
                case "3":
                    farmacia.MostrarHistorialCompras(historialCompras);
                    break;
                case "4":
                    Console.WriteLine("¡Gracias por utilizar nuestro servicio!");
                    Console.WriteLine("Ruby Sanchez - 1347424");
                    return;
                default:
                    Console.WriteLine("Opcion invalida.");
                    break;
            }
        }
    }

    // Metodo Res. Solo letras
    static string LeerStringSinNumeros()
    {
        string input;
        do
        {
            input = Console.ReadLine();
            if (!EsSoloTexto(input))
            {
                Console.WriteLine("Por favor, ingrese solo letras.");
            }
        } while (!EsSoloTexto(input));
        return input;
    }

    // Metodo para verificar si una cadena contiene solo letras
    static bool EsSoloTexto(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
            {
                return false;
            }
        }
        return true;
    }

    // Metodo para leer el teléfono Res. Solo numeros
    static string LeerTelefono()
    {
        string telefono;
        do
        {
            telefono = Console.ReadLine();
            if (!EsNumero(telefono))
            {
                Console.WriteLine("Por favor, ingrese solo numeros para el telefono.");
            }
        } while (!EsNumero(telefono));
        return telefono;
    }

    // Metodo para verificar si una cadena contiene solo nemeros
    static bool EsNumero(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
        }
        return true;
    }

    // Método para leer el genero
    static string LeerGenero()
    {
        string genero;
        do
        {
            genero = Console.ReadLine().ToUpper();
            if (genero != "H" && genero != "M")
            {
                Console.WriteLine("Por favor, ingrese 'H' para hombre o 'M' para mujer.");
            }
        } while (genero != "H" && genero != "M");
        return genero;
    }

    // Metodo para leer el metodo de pago
    static string LeerMetodoPago()
    {
        string metodoPago;
        do
        {
            metodoPago = Console.ReadLine().ToUpper();
            if (metodoPago != "A" && metodoPago != "B")
            {
                Console.WriteLine("Por favor, ingrese 'A' para Efectivo o 'B' para Tarjeta.");
            }
        } while (metodoPago != "A" && metodoPago != "B");
        return metodoPago;
    }

    // Metodo realizar una compra
    static void RealizarCompra()
    {
        Console.WriteLine("=== Realizar Compra ===");

        // Mostrar inventario de productos
        farmacia.MostrarInventario();

        // Seleccionar numero de productos a comprar
        Console.WriteLine("¿Cuántos productos desea comprar? (1, 2 o 3)");
        string opcion = Console.ReadLine();

        int numProductos;
        bool isValid = int.TryParse(opcion, out numProductos);

        if (!isValid || numProductos < 1 || numProductos > 3)
        {
            Console.WriteLine("Opcion invalida. Por favor, ingrese 1, 2 o 3.");
            return;
        }

        List<string> codigosSeleccionados = new List<string>();
        Producto[] productosSeleccionados = new Producto[numProductos];
        int indexProductosSeleccionados = 0;

        while (indexProductosSeleccionados < numProductos)
        {
            Console.WriteLine("Ingrese el codigo del producto que desea comprar: ");
            string codigo = Console.ReadLine();

            if (codigosSeleccionados.Contains(codigo))
            {
                Console.WriteLine("¡Ya ha seleccionado este producto! Por favor, ingrese un codigo diferente.");
                continue;
            }

            Producto producto = farmacia.ObtenerProducto(codigo);

            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado. Por favor, ingrese un codigo válido.");
                continue;
            }

            Console.WriteLine($"Producto seleccionado: {producto.Descripcion}. Ingrese la cantidad que desea comprar: ");
            int cantidad;
            bool cantidadValida = int.TryParse(Console.ReadLine(), out cantidad);

            if (!cantidadValida || cantidad <= 0 || cantidad > producto.InventarioInicial)
            {
                Console.WriteLine("Cantidad invalida. Por favor, ingrese una cantidad valida.");
                continue;
            }

            codigosSeleccionados.Add(codigo);

            productosSeleccionados[indexProductosSeleccionados] = new Producto
            {
                Codigo = producto.Codigo,
                Descripcion = producto.Descripcion,
                PrecioUnitario = producto.PrecioUnitario,
                InventarioInicial = cantidad
            };
            indexProductosSeleccionados++;
        }

        // Procesar la compra
        Compra compra = farmacia.RealizarCompra(productosSeleccionados);

        if (compra != null)
        {
            // Registro de compra
            historialCompras[indexHistorialCompras] = compra;
            indexHistorialCompras++;

            Console.WriteLine("=== Factura ===");
            Console.WriteLine($"Cliente: {cliente.Nombre} {cliente.Apellido}");
            Console.WriteLine($"Fecha de compra: {compra.Fecha}");
            Console.WriteLine("Productos:");

            foreach (var item in compra.Productos)
            {
                Console.WriteLine($"- {item.Descripcion}: {item.InventarioInicial} unidades");
            }

            Console.WriteLine($"Total a pagar: {compra.Total}");
        }
    }
}
