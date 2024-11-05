using System;
using System.Data.SqlClient;

class Program
{
    private static readonly string connectionString = "Server=DESKTOP-GC5LB1L\\SQLEXPRESS;Database=Clientes;Trusted_Connection=True;";

    static void Main()
    {
        Console.WriteLine("Escolha uma opção:");
        Console.WriteLine("1 - Cadastrar novo cliente");
        Console.WriteLine("2 - Atualizar cadastro de cliente");
        Console.WriteLine("3 - Listar clientes");
        Console.WriteLine("0 - Sair");

        

        int opcao = int.Parse(Console.ReadLine());
        Console.Clear();
        Console.Write("Processando Informação...");
        Thread.Sleep(1000);
        Console.Clear();

        switch (opcao)
        {
            case 1:
                CadastrarCliente();
                break;
            case 2:
                AtualizarCliente();
                break;
            case 3:
                ListarClientes();
                break;
            case 0:
                return; // Sai do programa
            default:
                Console.WriteLine("Opção inválida.");
                break;
        }
    }

    // Método para cadastrar um novo cliente
    public static void CadastrarCliente()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Telefone: ");
        string telefone = Console.ReadLine();

        string query = "INSERT INTO Clientes (Nome, Email, Telefone) VALUES (@Nome, @Email, @Telefone)";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Nome", nome);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Telefone", telefone);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        Console.WriteLine("Cliente cadastrado com sucesso!");
    }

    // Método para atualizar um cliente
    public static void AtualizarCliente()
    {
        Console.Write("Insira o ID do cliente: ");
        int id = int.Parse(Console.ReadLine());

        // Verifica se o cliente existe antes de tentar atualizar
        if (!ClienteExiste(id))
        {
            Console.WriteLine("Cliente não encontrado. Atualização não realizada.");
            return;
        }

        Console.Write("Novo Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Novo Email: ");
        string email = Console.ReadLine();
        Console.Write("Novo Telefone: ");
        string telefone = Console.ReadLine();

        string query = "UPDATE Clientes SET Nome = @Nome, Email = @Email, Telefone = @Telefone WHERE Id = @Id";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Nome", nome);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Telefone", telefone);
            conn.Open();
            int rowsAffected = cmd.ExecuteNonQuery();

            Console.WriteLine(rowsAffected > 0 ? "Cliente atualizado com sucesso!" : "Erro ao atualizar cliente.");
        }
    }

    // Verifica se o cliente existe
    public static bool ClienteExiste(int id)
    {
        string query = "SELECT COUNT(*) FROM Clientes WHERE Id = @Id";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }
    }

    // Método para listar todos os clientes
    public static void ListarClientes()
    {
        string query = "SELECT * FROM Clientes";
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader["ID"]}, Nome: {reader["Nome"]}, Email: {reader["Email"]}, Telefone: {reader["Telefone"]}");
                }
            }
        }
    }
}
