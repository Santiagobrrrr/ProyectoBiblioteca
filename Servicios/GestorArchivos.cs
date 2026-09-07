using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Servicios
{
    public class GestorArchivos
    {
        public void CargarLibros(
            string rutaArchivo,
            Biblioteca biblioteca)
        {
            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine(
                    "No se encontró el archivo de libros."
                );

                return;
            }

            string[] lineas =
                File.ReadAllLines(rutaArchivo);

            for (int i = 1; i < lineas.Length; i++)
            {
                string linea = lineas[i];

                if (string.IsNullOrWhiteSpace(linea))
                {
                    continue;
                }

                string[] datos = linea.Split(',');

                if (datos.Length != 6)
                {
                    Console.WriteLine(
                        $"Línea {i + 1} inválida."
                    );

                    continue;
                }

                bool codigoValido =
                    int.TryParse(datos[0], out int codigo);

                bool copiasValidas =
                    int.TryParse(
                        datos[4],
                        out int copiasDisponibles
                    );

                bool prestamosValidos =
                    int.TryParse(
                        datos[5],
                        out int vecesPrestado
                    );

                if (
                    !codigoValido ||
                    !copiasValidas ||
                    !prestamosValidos
                )
                {
                    Console.WriteLine(
                        $"Datos numéricos inválidos en línea {i + 1}."
                    );

                    continue;
                }

                Libro libro = new Libro(
                    codigo,
                    datos[1],
                    datos[2],
                    datos[3],
                    copiasDisponibles,
                    vecesPrestado
                );

                bool registrado =
                    biblioteca.RegistrarLibro(libro);

                if (!registrado)
                {
                    Console.WriteLine(
                        $"Código duplicado: {codigo}"
                    );
                }
            }
        }
    }
}