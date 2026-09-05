namespace ProyectoBiblioteca.Modelos;

    public class Libro
    {
        public int Codigo { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public int CopiasDisponibles { get; set; }
        public int VecesPrestado { get; set; }

        public Libro( // Constructor 
            int codigo,
            string titulo,
            string autor,
            string categoria,
            int copiasDisponibles,
            int vecesPrestado)
        {
            Codigo = codigo; // propiedad del objeto (Codigo) y parametro que llego al constructor (codigo)
            Titulo = titulo;
            Autor = autor;
            Categoria = categoria;
            CopiasDisponibles = copiasDisponibles;
            VecesPrestado = vecesPrestado;
        }
    }
