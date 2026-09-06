using ProyectoBiblioteca.Modelos;

namespace ProyectoBiblioteca.Estructuras
{
    public class ArbolBPlus
    {
        // Nodo interno utilizado por el Árbol B+
        private class Nodo
        {
            public bool EsHoja;
            public int CantidadClaves;
            public int[] Claves;
            public Libro?[] Libros;
            public Nodo?[] Hijos;
            public Nodo? Siguiente;

            public Nodo(int orden, bool esHoja)
            {
                EsHoja = esHoja;
                CantidadClaves = 0;

                Claves = new int[orden];
                Libros = new Libro?[orden];
                Hijos = new Nodo?[orden + 1];

                Siguiente = null;
            }
        }

        private Nodo raiz;
        private int orden;

        public ArbolBPlus(int orden = 4)
        {
            if (orden < 3)
            {
                orden = 3;
            }

            this.orden = orden;

            // Al inicio, la raíz también es una hoja
            raiz = new Nodo(orden, true);
        }

        public void Insertar(Libro libro)
        {
            // Evitamos códigos repetidos
            if (Buscar(libro.Codigo) != null)
            {
                throw new InvalidOperationException(
                    "Ya existe un libro con ese código."
                );
            }

            // Por ahora solamente trabajamos con la raíz como hoja.
            // En el siguiente paso implementaremos la división.
            if (!raiz.EsHoja)
            {
                throw new InvalidOperationException(
                    "La inserción en nodos internos aún no está implementada."
                );
            }

            // Un árbol de orden 4 puede tener máximo 3 claves por nodo.
            if (raiz.CantidadClaves >= orden - 1)
            {
                throw new InvalidOperationException(
                    "La hoja está llena. Es necesario realizar una división."
                );
            }

            int i = raiz.CantidadClaves - 1;

            // Movemos las claves mayores hacia la derecha
            // para mantenerlas ordenadas.
            while (i >= 0 && libro.Codigo < raiz.Claves[i])
            {
                raiz.Claves[i + 1] = raiz.Claves[i];
                raiz.Libros[i + 1] = raiz.Libros[i];

                i--;
            }

            // Insertamos el nuevo libro en su posición correcta.
            raiz.Claves[i + 1] = libro.Codigo;
            raiz.Libros[i + 1] = libro;

            raiz.CantidadClaves++;
        }

        public Libro? Buscar(int codigo)
        {
            Nodo actual = raiz;

            // Cuando existan nodos internos,
            // recorreremos el árbol hasta llegar a una hoja.
            while (!actual.EsHoja)
            {
                int i = 0;

                while (
                    i < actual.CantidadClaves &&
                    codigo >= actual.Claves[i]
                )
                {
                    i++;
                }

                Nodo? siguiente = actual.Hijos[i];

                if (siguiente == null)
                {
                    return null;
                }

                actual = siguiente;
            }

            // En la hoja buscamos el código.
            for (int i = 0; i < actual.CantidadClaves; i++)
            {
                if (actual.Claves[i] == codigo)
                {
                    return actual.Libros[i];
                }
            }

            return null;
        }

        public void Imprimir()
        {
            Nodo? actual = raiz;

            // Buscamos la primera hoja del árbol.
            while (actual != null && !actual.EsHoja)
            {
                actual = actual.Hijos[0];
            }

            // Recorremos las hojas.
            while (actual != null)
            {
                for (int i = 0; i < actual.CantidadClaves; i++)
                {
                    Libro? libro = actual.Libros[i];

                    if (libro != null)
                    {
                        Console.WriteLine(
                            $"{libro.Codigo} - {libro.Titulo}"
                        );
                    }
                }

                actual = actual.Siguiente;
            }
        }
    }
}