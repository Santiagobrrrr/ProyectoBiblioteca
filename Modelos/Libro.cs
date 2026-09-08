namespace ProyectoBiblioteca.Modelos;

public class Libro
{
    // Datos principales del libro
    public int Codigo { get; set; }
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string Categoria { get; set; }

    // Datos utilizados para préstamos y disponibilidad
    public int CopiasDisponibles { get; set; }
    public int VecesPrestado { get; set; }
    public int CopiasTotales { get; private set; }

    // Inicializa los datos de un nuevo libro
    public Libro(
        int codigo,
        string titulo,
        string autor,
        string categoria,
        int copiasDisponibles,
        int vecesPrestado)
    {
        Codigo = codigo;
        Titulo = titulo;
        Autor = autor;
        Categoria = categoria;
        CopiasDisponibles = copiasDisponibles;
        VecesPrestado = vecesPrestado;
        // Se conserva para evitar devoluciones mayores a la cantidad registrada inicialmente
        CopiasTotales = copiasDisponibles;
    }
}